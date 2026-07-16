using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Cryptography; 
using Discord;
using GameCore;
using Mirror;
using Org.BouncyCastle.Crypto;
using Steamworks;
using UnityEngine;
using UserSettings.OtherSettings;
using static MainThreadDispatcher;
using Debug = UnityEngine.Debug;

namespace CentralAuth
{
    public static class CentralAuthManager
    {
        private static bool _initialized;
        private static bool _authDebugEnabled;
        public const int TokenVersion = 2;

        private static readonly Regex _challengeRegex = new Regex("^[a-zA-Z0-9]{32,128}$", RegexOptions.Compiled);
        private static readonly Stopwatch _timeCounter = new Stopwatch();

        private static bool _interacted; 

        internal static string TokenChallenge;
        internal static string ServerEcdhPublicKey;
        internal static bool ForceRenew;
        internal static bool RequestAuthToken;

        internal static string ApiToken;
        internal static string Nonce;
        internal static bool NoWatermarking;
        internal static AuthStatusType AuthStatusType = AuthStatusType.Connecting;

        private static ushort _lifetime;
        private static bool _abort;
        private static Thread _authThread;
        private static string DiscordOAuth2Token;

        private const string SteamStartupArg = "-steam";
        private const string SteamDistributionStartupArg = "-distrib-steam";
        private const string DiscordStartupArg = "-discord";
        private const string ForceDiscordStartupArg = "-fdiscord";
        private const string NoAuthStartupArg = "-noauth";
        private const string AuthDebugArg = "-authdebug";
        private const string AuthDebugFileName = "EnableDebug.txt";
        public static DistributionPlatform Platform { get; private set; }
        internal static bool Authenticated { get; private set; }
        internal static string GlobalBanReason { get; private set; }
        internal static CentralAuthPreauthToken PreauthToken { get; private set; }
        internal static string DiscordState { get; private set; } = "Not initialized!";
        internal static global::Discord.Discord Discord { get; private set; }
        internal static AsymmetricCipherKeyPair SessionKeys { get; private set; }

        public static void InitAuth()
        {
            if (_initialized) return;
            _initialized = true;

            CentralServer.Init();
            SessionKeys = ECDSA.GenerateKeys();
            AuthDebug("Session keys generated (ECDSA)!", "gray");

            var args = StartupArgs.Args;

            string debugPath = Path.Combine(FileManager.GetAppFolder(true, false), AuthDebugFileName);
            if (File.Exists(debugPath) || args.Any(a => a.Equals(AuthDebugArg, StringComparison.OrdinalIgnoreCase)))
            {
                _authDebugEnabled = true;
                GameCore.Console.AddLog("Auth debug enabled.", Color.green);
            }

            if (args.Any(a => a.Equals(NoAuthStartupArg, StringComparison.OrdinalIgnoreCase)))
            {
                AuthDebug("NoAuth mode detected.");
                return;
            }

            if (args.Any(a => a.Equals(SteamStartupArg, StringComparison.OrdinalIgnoreCase)) ||
                args.Any(a => a.Equals(SteamDistributionStartupArg, StringComparison.OrdinalIgnoreCase)))
            {
                Platform = DistributionPlatform.Steam;
                GameCore.Console.AddLog("Detected distribution platform: Steam", Color.gray);
                SteamManager.StartClient();
                SteamManager.RefreshToken();
            }
            else if (args.Any(a => a.Equals(DiscordStartupArg, StringComparison.OrdinalIgnoreCase)) ||
                     args.Any(a => a.Equals(ForceDiscordStartupArg, StringComparison.OrdinalIgnoreCase)))
            {
                Platform = DistributionPlatform.Discord;
                GameCore.Console.AddLog("Detected distribution platform: Discord", Color.gray);

                try
                {
                    Discord = new global::Discord.Discord(673060799787597824, (ulong)CreateFlags.Default);
                    DiscordState = "Loaded!";

                    Discord.SetLogHook(global::Discord.LogLevel.Debug, (level, message) =>
                    {
                        if (level == global::Discord.LogLevel.Error) Debug.LogError(message);
                        else if (level == global::Discord.LogLevel.Warn) Debug.LogWarning(message);
                        else Debug.Log(message);
                    });

                    var appManager = Discord.GetApplicationManager();
                    appManager.GetOAuth2Token((global::Discord.Result result, ref OAuth2Token token) =>
                    {
                        if (result == global::Discord.Result.Ok)
                        {
                            DiscordOAuth2Token = token.AccessToken;
                            AuthDebug("Discord OAuth2 token obtained.", "green");
                            Authenticate();
                        }
                        else
                        {
                            DiscordState = $"Failed {result}";
                            AuthDebug(DiscordState, "red");
                        }
                    });
                }
                catch (Exception e)
                {
                    AuthDebug($"Discord init failed: {e}", "red");
                    Platform = DistributionPlatform.Dedicated; 
                }
            }


            Platform = DistributionPlatform.Steam;
            GameCore.Console.AddLog("Detected distribution platform: Steam", Color.gray);
            SteamManager.StartClient();
            SteamManager.RefreshToken();

            /*
            else
            {
                Platform = DistributionPlatform.Dedicated;
                GameCore.Console.AddLog("Running as dedicated server.", Color.gray);
            }
            */


            if (GameCore.Version.PrivateBeta && Platform == DistributionPlatform.Steam)
            {
                if (!SteamApps.IsDlcInstalled((AppId)859210))
                {
                    WindowsMessageBox.Show("You are not authorized to run private beta of SCP: Secret Laboratory.", WindowsMessageBox.MessageBoxFlags.Info, "SCP: Secret Laboratory");
                    Shutdown.Quit();
                    return;
                }
            }

            if (Platform != DistributionPlatform.Discord)
            {
                Authenticate();
            }
        }

        private static void Authenticate()
        {
            if (_authThread != null && _authThread.IsAlive) return;

            _abort = false;

            AuthDebug("Started authentication thread.", "gray");
            Authenticated = false;

            _authThread = new Thread(Authentication)
            {
                Name = "Authentication Thread",
                Priority = System.Threading.ThreadPriority.BelowNormal,
                IsBackground = true
            };
            _authThread.Start();
        }

        private static void Authentication()
        {
            Thread.Sleep(25);
            AuthDebug("Authentication initialized.", "gray");

            while (!_abort)
            {
                try
                {
                    if (!Authenticated)
                    {
                        string ticket = string.Empty;
                        string urlEndpoint = "";

                        if (Platform == DistributionPlatform.Steam)
                        {
                            if (!SteamManager.IsSteamReady())
                            {
                                AuthDebug("Steam not initialized.", "red");
                                Thread.Sleep(1000);
                                continue;
                            }

                            AuthDebug("Revoking previous Steam authentication ticket...", "gray");
                            SteamManager.CancelTicket();

                            AuthDebug("Obtaining authentication ticket from Steam...", "blue");
                            var authTicket = SteamManager.GetAuthSessionTicket();
                            if (authTicket != null)
                            {
                                ticket = BitConverter.ToString(authTicket.Data).Replace("-", string.Empty);
                                AuthDebug("Authentication Ticket obtained from Steam.", "blue");
                            }
                            urlEndpoint = "v5/steam/authenticate.php";
                        }
                        else if (Platform == DistributionPlatform.Discord)
                        {
                            ticket = DiscordOAuth2Token;
                            urlEndpoint = "v5/discord/authenticate.php";
                        }

                        if (string.IsNullOrEmpty(ticket))
                        {
                            Thread.Sleep(2000);
                            continue;
                        }

                        string signature = Sign(ticket);

                        string publicKeyStr = ECDSA.KeyToString(SessionKeys.Public);
                        string publicKeyHash = Sha.HashToString(Sha.Sha256(publicKeyStr));

                        List<string> postData = new List<string>
                        {
                            "ticket=" + ticket,
                            "publickey=" + publicKeyHash,
                            "signature=" + signature,
                            "version=2"
                        };

                        if (UserSettings.UserSetting<bool>.Get(MiscPrivacySetting.DoNotTrack)) postData.Add("DNT=true");
                        if (UserSettings.UserSetting<bool>.Get(MiscPrivacySetting.HideSteamProfile)) postData.Add("DisplayProfile=true");
                        if (GameCore.Version.PrivateBeta) postData.Add("privatebeta=true");

                        string url = CentralServer.StandardUrl + urlEndpoint;
                        string rawResponse = HttpQuery.Post(url, HttpQuery.ToPostArgs(postData));

                        AuthDebug($"[AUTH] Response received.", "cyan");

                        var response = JsonSerialize.FromJson<AuthenticateResponse>(rawResponse);

                        if (response != null && !string.IsNullOrEmpty(response.token))
                        {
                            ApiToken = response.token;
                            Nonce = response.nonce;
                            _lifetime = response.lifetime;

                            PreauthToken = new CentralAuthPreauthToken(
                                response.id,
                                response.flags,
                                response.country,
                                response.expiration,
                                response.token
                            );

                            Authenticated = true;
                            AuthStatusType = AuthStatusType.Success;
                            AuthDebug("Authentication session established.", "green");
                        }
                        else
                        {
                            AuthDebug("Authentication error (authenticate - response)", "red");
                            Authenticated = false;
                            Thread.Sleep(5000); 
                        }
                    }
                    else
                    {
                        Thread.Sleep(1000);
                        _lifetime--; 

                        if (_lifetime < 30 || ForceRenew) 
                        {
                            AuthDebug("Renewing authentication session...", "gray");

                            List<string> renewData = new List<string>
                            {
                                "token=" + ApiToken,
                                "nonce=" + Nonce
                            };

                            string renewSig = Sign(Nonce);

                            string renewUrl = CentralServer.StandardUrl + "v5/renew.php";
                            string renewResponseRaw = HttpQuery.Post(renewUrl, HttpQuery.ToPostArgs(renewData));

                            var renewResponse = JsonSerialize.FromJson<AuthenticateResponse>(renewResponseRaw);

                            if (renewResponse != null && renewResponse.success)
                            {
                                ApiToken = renewResponse.token;
                                _lifetime = renewResponse.lifetime;
                                Nonce = renewResponse.nonce; 
                                ForceRenew = false;
                                AuthDebug("Authentication session renewed.", "green");
                            }
                            else
                            {
                                AuthDebug("Authentication error (renewal - response)", "red");
                                Authenticated = false; 
                            }
                        }
                    }

                    if (RequestAuthToken)
                    {
                        RequestAuthToken = false;
                        RequestToken();
                    }
                }
                catch (Exception e)
                {
                    AuthDebug($"Auth thread exception: {e.Message}", "red");
                    Thread.Sleep(5000);
                }
            }
        }

        private static void RequestToken()
        {
            if (!NetworkClient.active)
            {
                AuthDebug("Request signature called without active NetworkClient.", "gray");
                return;
            }

            AuthDebug("Requesting signature from central servers...", "gray");

            string signature = Sign(Nonce);

            var postData = new List<string>
            {
                "token=" + ApiToken,
                "nonce=" + Nonce,
                "signature=" + signature,
                "format=json"
			};

            if (!_challengeRegex.IsMatch(TokenChallenge ?? ""))
            {
                AuthDebug("Invalid Challenge from server", "red");
                return;
            }
            postData.Add("Challenge=" + TokenChallenge);

            if (UserSettings.UserSetting<bool>.Get(MiscPrivacySetting.DoNotTrack)) postData.Add("DNT=true");
            if (UserSettings.UserSetting<bool>.Get(MiscPrivacySetting.HideSteamProfile)) postData.Add("DisplayProfile=true");
            if (GameCore.Version.PrivateBeta) postData.Add("privatebeta=true");

            string url = CentralServer.StandardUrl + "v5/requestsignature.php";
            string rawResponse = HttpQuery.Post(url, HttpQuery.ToPostArgs(postData));

            var webRes = JsonSerialize.FromJson<RequestSignatureResponse>(rawResponse);

            if (webRes != null && webRes.success)
            {
                Nonce = webRes.nonce;
                AuthDebug("Signature obtained from central servers", "green");

                MainThreadDispatcher.Dispatch(() =>
                {
                    SignedToken signedAuth = new SignedToken(webRes.authToken.token, webRes.authToken.signature);
                    SignedToken signedBadge = new SignedToken(webRes.badgeToken.token, webRes.badgeToken.signature);

                    string myPublicKeyStr = ECDSA.KeyToString(SessionKeys.Public);

                    byte[] signedChallenge = ECDSA.SignBytes(TokenChallenge, SessionKeys.Private);

                    var authResponseMsg = new AuthenticationResponse(
                        signedAuth,                 // SignedToken авторизации
                        signedBadge,                // SignedToken бейджа
                        myPublicKeyStr,             // Наш публичный ключ (строкой)
                        null,                       // ECDH ключ (обычно null в этой фазе или обрабатывается отдельно)
                        signedChallenge,            // Подпись челленджа (byte[])
                        UserSettings.UserSetting<bool>.Get(MiscPrivacySetting.DoNotTrack), // DoNotTrack
                        false                       // HideBadge
                    );

                    FinalizeServerAuthentication(authResponseMsg);
                }, DispatchTime.FixedUpdate);
            }
            else
            {
                AuthDebug("Authentication error (request signature - response)", "red");
            }
        }

        internal static void Abort()
        {
            if (_authThread != null && _authThread.IsAlive)
            {
                UnityEngine.Debug.Log("Stopping authentication thread...");
                _abort = true;
            }
        }

        private static void AuthDebug(string msg) => AuthDebug(msg, "gray");

        private static void AuthDebug(string msg, string color)
        {
            if (!_authDebugEnabled && color == "gray") return;
            GameCore.Console.AddDebugLog("SDAUTH", $"<color={color}>{msg}</color>", MessageImportance.Normal);
        }

        private static string Sign(string ticket)
        {
            if (string.IsNullOrEmpty(ticket)) return null;
            AuthDebug("Signing auth ticket...", "blue");
            try
            {
                string result = LauncherCommunicator.Send(ticket);
                AuthDebug("Auth ticket signed!", "blue");
                return result;
            }
            catch
            {
                Debug.LogError("Launcher signer error");
                return "ERROR";
            }
        }

        static void FinalizeServerAuthentication(AuthenticationResponse msg)
        {
            if (NetworkClient.active)
            {
                NetworkClient.Send(msg);
            }
        }
    }
}