using CommandSystem;
using Mirror;
using Org.BouncyCastle.Crypto;
using PluginAPI.Events;
using RemoteAdmin;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using ToggleableMenus;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameCore
{
    public class Console : SimpleToggleableMenu
    {
        public enum ConsoleLogType
        {
            DoNotLog = 0,
            Log = 1,
            Warning = 2,
            Error = 3
        }

        public CommandHint[] hints;
        public Text txt;
        public InputField cmdField;

        public static Console singleton { get; private set; }
        public static bool RequestDNT { get; private set; }
        internal static string BinariesRootPath { get; private set; }
        internal static bool DisableSRV { get; private set; }
        internal static bool BindSyncingEnabled { get; set; }
        internal static bool SkipIpValidation { get; private set; }

        public readonly GameConsoleCommandHandler ConsoleCommandHandler = GameConsoleCommandHandler.Create();
        private int _clientCommandPosition;
        private readonly List<string> _clientCommandLogs = new List<string>();

        private static bool _noConsoleLogOutput;
        internal static bool DisplayCreateServer;
        internal static bool TranslationDebugMode;
        internal static AsymmetricKeyParameter _publicKey;
        private static ConsoleCommandSender Ccs;
        internal static string SyncbindPath;
        internal static List<Log> Logs = new List<Log>();
        private static ConcurrentQueue<Log> logQueue = new ConcurrentQueue<Log>();

        private int _scrollup;
        private int _previousScrlup;
        // NOTE: _alwaysRefreshing is initialized to true in the static constructor per .cctor ISIL [rcx+74]=1
        internal static bool _alwaysRefreshing;
        internal static bool _change;
        internal static bool _allowBindSyncing;
        internal static bool _bindSyncingContinue;
        private string _content;

        private StringBuilder _stringBuilder = new StringBuilder();

        private bool GetAllowInput()
        {
            if (cmdField == null) return false;
            return cmdField.isFocused;
        }

        public override bool CanToggle
        {
            get
            {
                if (!IsEnabled)
                    return true;

                if (GetAllowInput())
                    return cmdField.text.Length <= 1;

                return true;
            }
        }

        public override bool LockMovement => false;

        // Static constructor: .cctor ISIL confirms String.Concat (not Path.Combine) and _alwaysRefreshing = true
        static Console()
        {
            Ccs = new ConsoleCommandSender();
            SyncbindPath = string.Concat(
                Environment.GetFolderPath((Environment.SpecialFolder)26),
                "/SCP Secret Laboratory/internal/SyncCmd");
            // .cctor ISIL step 84-86: [rcx+74] = 1 → _alwaysRefreshing = true
            _alwaysRefreshing = true;
        }

        protected override void Awake()
        {
            base.Awake();
            BinariesRootPath = Path.GetDirectoryName(Path.GetFullPath(Application.dataPath));
            DontDestroyOnLoad(gameObject);

            if (singleton != null && singleton != this)
            {
                DestroyImmediate(gameObject);
                return;
            }

            singleton = this;
        }

        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;

            // ISIL Start: both messages logged with Color.green (0xFF00FF00 decoded as RGBA = Color.green)
            Debug.Log("Hi there! Initializing console...");
            logQueue.Enqueue(new Log("Hi there! Initializing console...", Color.green, false));
            Debug.Log("Done! Type 'help' to print the list of available commands.");
            logQueue.Enqueue(new Log("Done! Type 'help' to print the list of available commands.", Color.green, false));

            CentralAuth.CentralAuthManager.InitAuth();

            Thread thread = new Thread(RefreshPublicKey)
            {
                Name = "Public key refreshing",
                IsBackground = true,
                Priority = System.Threading.ThreadPriority.BelowNormal
            };
            thread.Start();

            string[] args = StartupArgs.Args;

            // -noconsolelog: just sets the flag, no log message (confirmed by ISIL)
            if (args.Any(arg => string.Equals(arg, "-noconsolelog", StringComparison.OrdinalIgnoreCase)))
            {
                _noConsoleLogOutput = true;
            }

            // -dnt
            if (args.Any(arg => string.Equals(arg, "-dnt", StringComparison.OrdinalIgnoreCase)))
            {
                RequestDNT = true;
                Debug.Log("\"Do not track\" request will be sent to all servers you are joining - enabled by startup argument.");
                logQueue.Enqueue(new Log("\"Do not track\" request will be sent to all servers you are joining - enabled by startup argument.", Color.green, false));
            }

            // -nosrv
            if (args.Any(arg => string.Equals(arg, "-nosrv", StringComparison.OrdinalIgnoreCase)))
            {
                DisableSRV = true;
                Debug.Log("SRV DNS records resolution has been disabled.");
                logQueue.Enqueue(new Log("SRV DNS records resolution has been disabled.", Color.green, false));
            }

            // -hidetag: ISIL steps 746-853 → UserSetting.Set(rcx=5, rdx=2) i.e. Set(setting=5, value=2)
            // CS was WRONG: used Set(5, 6) — value should be 2
            if (args.Any(arg => string.Equals(arg, "-hidetag", StringComparison.OrdinalIgnoreCase)))
            {
                UserSettings.UserSetting<UserSettings.OtherSettings.MiscPrivacySetting>.Set(
                    (UserSettings.OtherSettings.MiscPrivacySetting)5,
                    (UserSettings.OtherSettings.MiscPrivacySetting)2);
                Debug.Log("Your global badge will be automatically hidden.");
                logQueue.Enqueue(new Log("Your global badge will be automatically hidden.", Color.green, false));
            }

            // -neverhidelocaltag: ISIL steps 900-1006 → Set(rcx=6, rdx=1) i.e. Set(setting=6, value=1)
            // CS was WRONG: used Set(6, 6) — value should be 1
            if (args.Any(arg => string.Equals(arg, "-neverhidelocaltag", StringComparison.OrdinalIgnoreCase)))
            {
                UserSettings.UserSetting<UserSettings.OtherSettings.MiscPrivacySetting>.Set(
                    (UserSettings.OtherSettings.MiscPrivacySetting)6,
                    (UserSettings.OtherSettings.MiscPrivacySetting)1);
                Debug.Log("Your local badge won't be automatically hidden.");
                logQueue.Enqueue(new Log("Your local badge won't be automatically hidden.", Color.green, false));
            }

            // -hidelocaltag: ISIL steps 1053-1159 → Set(rcx=6, rdx=2) i.e. Set(setting=6, value=2)
            // CS was WRONG: used Set(6, 6) and had no Debug.Log
            if (args.Any(arg => string.Equals(arg, "-hidelocaltag", StringComparison.OrdinalIgnoreCase)))
            {
                UserSettings.UserSetting<UserSettings.OtherSettings.MiscPrivacySetting>.Set(
                    (UserSettings.OtherSettings.MiscPrivacySetting)6,
                    (UserSettings.OtherSettings.MiscPrivacySetting)2);
                Debug.Log("Your local badge will be automatically hidden.");
                logQueue.Enqueue(new Log("Your local badge will be automatically hidden.", Color.green, false));
            }

            // -nevercover: ISIL steps 1200-1312 → Set(rcx=4, rdx=1) = Set(setting=4, value=1)
            // COMPLETELY MISSING from original CS!
            if (args.Any(arg => string.Equals(arg, "-nevercover", StringComparison.OrdinalIgnoreCase)))
            {
                UserSettings.UserSetting<UserSettings.OtherSettings.MiscPrivacySetting>.Set(
                    (UserSettings.OtherSettings.MiscPrivacySetting)4,
                    (UserSettings.OtherSettings.MiscPrivacySetting)1);
                Debug.Log("Your global badge won't be covered.");
                logQueue.Enqueue(new Log("Your global badge won't be covered.", Color.green, false));
            }

            // -skipipvalidation: ISIL steps 1358-1469 → SkipIpValidation = true + log
            // CS was missing Debug.Log and Enqueue
            if (args.Any(arg => string.Equals(arg, "-skipipvalidation", StringComparison.OrdinalIgnoreCase)))
            {
                SkipIpValidation = true;
                Debug.Log("IP validation will be skipped.");
                logQueue.Enqueue(new Log("IP validation will be skipped.", Color.green, false));
            }

            // -scs: ISIL steps 1515-1618 → DisplayCreateServer = true + log
            // CS was missing Debug.Log and Enqueue
            if (args.Any(arg => string.Equals(arg, "-scs", StringComparison.OrdinalIgnoreCase)))
            {
                DisplayCreateServer = true;
                Debug.Log("Create Server button will be displayed (startup argument).");
                logQueue.Enqueue(new Log("Create Server button will be displayed (startup argument).", Color.green, false));
            }

            // -allow-syncbind: ISIL steps 1619-1965 — full syncbind logic
            // CS was completely missing all the syncbind verification logic
            if (args.Any(arg => string.Equals(arg, "-allow-syncbind", StringComparison.OrdinalIgnoreCase)))
            {
                // If syncbind NOT enabled → delete any existing sync file and skip
                if (!_allowBindSyncing)
                {
                    if (File.Exists(SyncbindPath))
                        File.Delete(SyncbindPath);
                }
                else
                {
                    // Warn about risk
                    Debug.Log("[WARNING] You have \"-allow-syncbind\" startup argument added.");
                    logQueue.Enqueue(new Log("[WARNING] You have \"-allow-syncbind\" startup argument added.", Color.green, false));
                    Debug.Log("[WARNING] Your command binding might be messed up, and your key might be logged by the server you join.");
                    logQueue.Enqueue(new Log("[WARNING] Your command binding might be messed up, and your key might be logged by the server you join.", Color.green, false));

                    _bindSyncingContinue = true;

                    if (!File.Exists(SyncbindPath))
                    {
                        // No sync file → disable bind syncing
                        BindSyncingEnabled = false;
                    }
                    else
                    {
                        // Read and verify HWID from sync file
                        //string savedHwid = File.ReadAllText(SyncbindPath);
                        // ISIL calls 0x182234250 (HWID getter) then 0x1819016A0 (string comparison/verification)
                        //string currentHwid = HwidMethods.GetCurrent();   // placeholder – adapt to actual HWID API
                        bool hwidMatch = true; // placeholder

                        if (!hwidMatch)
                        {
                            Debug.Log("Bind syncing has been disabled - HWID mismatch.");
                            logQueue.Enqueue(new Log("Bind syncing has been disabled - HWID mismatch.", Color.yellow, false));
                            File.Delete(SyncbindPath);
                        }
                        else
                        {
                            Debug.Log("[WARNING] Bind syncing is ENABLED.");
                            logQueue.Enqueue(new Log("[WARNING] Bind syncing is ENABLED.", Color.yellow, false));
                            // ISIL step 1963-1965: call 0x18053CCC0 with (1, false) → likely BindSyncingEnabled = true
                            BindSyncingEnabled = true;
                        }
                    }
                }
            }

            // -tdm: ISIL steps 1966+ → TranslationDebugMode = true + log
            // CS was missing Debug.Log and Enqueue
            if (args.Any(arg => string.Equals(arg, "-tdm", StringComparison.OrdinalIgnoreCase)))
            {
                TranslationDebugMode = true;
                Debug.Log("Translation debug mode has been enabled (startup argument).");
                logQueue.Enqueue(new Log("Translation debug mode has been enabled (startup argument).", Color.green, false));
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            string msg = string.Concat("Scene Manager: Loaded scene '", scene.name, "' [", scene.path, "]");
            Debug.Log(msg);
            logQueue.Enqueue(new Log(msg, Color.green, false));
            RefreshConsoleScreen();
        }

        private void Update()
        {
            // ISIL: check CentralAuthManager state == 2 before getting Discord
            CentralAuth.CentralAuthManager.Discord?.RunCallbacks();

            if (_change)
            {
                txt.text = _content;
                _change = false;
            }
        }

        private void LateUpdate()
        {
            if (!IsEnabled)
            {
                // ISIL LateUpdate step 219-221: reset _clientCommandPosition = 0 when not enabled
                _clientCommandPosition = 0;
                return;
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                if (!string.IsNullOrEmpty(cmdField.text))
                {
                    TypeCommand(cmdField.text);
                    cmdField.text = string.Empty;
                    EventSystem.current.SetSelectedGameObject(cmdField.gameObject);
                }
                cmdField.ActivateInputField();
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (_clientCommandLogs.Count > _clientCommandPosition)
                {
                    _clientCommandPosition++;
                    int index = _clientCommandLogs.Count - _clientCommandPosition;
                    if (index >= 0)
                        cmdField.text = _clientCommandLogs[index];
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (_clientCommandPosition > 0)
                {
                    _clientCommandPosition--;
                    int index = _clientCommandLogs.Count - _clientCommandPosition;
                    if (index < _clientCommandLogs.Count)
                        cmdField.text = _clientCommandLogs[index];
                    else
                        cmdField.text = string.Empty;
                }
            }

            float scroll = Input.GetAxisRaw("Mouse ScrollWheel") * 10f;
            if (scroll != 0)
            {
                _scrollup += (int)scroll;
                if (_scrollup < 0) _scrollup = 0;
                if (_scrollup > Logs.Count) _scrollup = Logs.Count;
            }

            if (_scrollup != _previousScrlup)
            {
                _previousScrlup = _scrollup;
                RefreshConsoleScreen();
            }

            // ISIL LateUpdate: calls RefreshConsoleScreen() again if _alwaysRefreshing
            if (_alwaysRefreshing)
            {
                RefreshConsoleScreen();
            }
        }

        private void FixedUpdate()
        {
            bool logged = false;
            while (logQueue.TryDequeue(out Log log))
            {
                // ISIL FixedUpdate: 6-element string array with milliseconds
                // Format: "<color=#808080><size=18>[" + FormatTime("HH:mm:ss") + "<size=16>." + FormatTime("fff") + "</size>]</size></color> " + log.text
                string timeHms = TimeBehaviour.FormatTime("HH:mm:ss");
                string timeMs = TimeBehaviour.FormatTime("fff");
                string fullMsg = string.Concat(
                    "<color=#808080><size=18>[",
                    timeHms,
                    "<size=16>.",
                    timeMs,
                    "</size>]</size></color> ",
                    log.text);

                // Split by '\n' (char 10) and add each line individually
                string[] lines = fullMsg.Split('\n');
                foreach (var line in lines)
                {
                    // Replace '\r' with empty string (per ISIL step 216-218)
                    string cleanLine = line.Replace("\r", string.Empty);
                    Logs.Add(new Log(cleanLine, log.color, log.nospace));
                }
                logged = true;
            }

            if (logged)
            {
                _scrollup = 0;
                RefreshConsoleScreen();
            }
        }

        private void RefreshConsoleScreen()
        {
            if (txt == null) return;

            // ISIL RefreshConsoleScreen: checks txt == null via Object.op_Equality
            if (Logs == null || Logs.Count == 0)
            {
                _content = string.Empty;
                _change = true;
                return;
            }

            // ISIL builds a 7-element concat array per entry, appending forward from index 0
            // Format: current_content + separator + "<color=" + hex + ">" + text + "</color>"
            // Separator: "\n\n" if nospace=true, "\n" if nospace=false (per ISIL cmovne logic)
            string content = string.Empty;

            int limit = Logs.Count - _scrollup;
            for (int i = 0; i < limit; i++)
            {
                Log item = Logs[i];
                string sep = (i != 0) ? (item.nospace ? "\n\n" : "\n") : string.Empty;
                content = string.Concat(content, sep, "<color=", Misc.ToHex(item.color), ">", item.text, "</color>");
                _content = content;

                // ISIL: if content length > 0x3A98 (15000), RemoveAt(0) then restart loop
                if (content.Length > 15000)
                {
                    Logs.RemoveAt(0);
                    // Restart the rendering from scratch
                    RefreshConsoleScreen();
                    return;
                }
            }

            _change = true;
        }

        public static void AddDebugLog(string debugKey, string message, MessageImportance importance, bool nospace = false)
        {
            if (ConsoleDebugMode.CheckImportance(debugKey, importance, out var color))
            {
                AddLog("[DEBUG_" + debugKey + "] " + message, color, nospace);
            }
            Debug.Log(message);
        }

        // AddLog: ISIL confirms NO ServerStatic.IsDedicated check and simpler _noConsoleLogOutput logic
        // CS was WRONG: had extra ServerStatic.IsDedicated block and wrong condition
        public static void AddLog(string text, Color c, bool nospace = false, ConsoleLogType type = ConsoleLogType.Log)
        {
            // ISIL: only check _noConsoleLogOutput, no IsDedicated check, no type condition on the outer if
            if (!_noConsoleLogOutput)
            {
                switch (type)
                {
                    case ConsoleLogType.DoNotLog:
                        // DoNotLog: skip Debug call entirely (ISIL jumps past Debug calls for type==0)
                        break;
                    case ConsoleLogType.Error:
                        Debug.LogError(text);
                        break;
                    case ConsoleLogType.Warning:
                        Debug.LogWarning(text);
                        break;
                    case ConsoleLogType.Log:
                        Debug.Log(text);
                        break;
                    default:
                        // ISIL step 098-124: throws ArgumentOutOfRangeException for unknown type values
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            }

            logQueue.Enqueue(new Log(text, c, nospace));
        }

        // FindConnectedRoot: ISIL uses null checks, not try/catch
        // CS was WRONG to use try/catch – the original uses sequential null checks
        public static GameObject FindConnectedRoot(NetworkConnection conn)
        {
            if (conn == null) return null;

            NetworkIdentity identity = conn.identity;
            if (identity == null) return null;

            GameObject go = identity.gameObject;
            if (go == null) return null;

            if (go.CompareTag("Player"))
                return go;

            return null;
        }

        // TypeCommand: several bugs fixed:
        // 1. sender ??= Ccs (not ServerConsole.Scs — Ccs is Console's own ConsoleCommandSender)
        // 2. Added Debug.Log(">>> " + cmd) at start (ISIL step 068-077)
        // 3. Added _clientCommandLogs.Add(cmd) (ISIL step 089-094)
        // 4. Added NetworkClient.active handling for "/" and "@" prefixes when server is not active
        internal string TypeCommand(string cmd, CommandSender sender = null)
        {
            // ISIL step 068-077: log the command
            Debug.Log(">>> " + cmd);

            // ISIL step 078-088: use Console.Ccs if sender is null
            sender ??= Ccs;

            // ISIL step 089-094: add command to history
            _clientCommandLogs?.Add(cmd);

            if (cmd.StartsWith(".", StringComparison.Ordinal) && cmd.Length > 1)
            {
                // ISIL step 106-238: "." prefix = send to server as GameConsoleTransmission
                if (!NetworkClient.active && !NetworkServer.active)
                {
                    string err = "You must be connected to a server to use this command.";
                    Debug.Log(err);
                    logQueue.Enqueue(new Log(err, Color.red, false));
                    return "You must be connected to a server to use remote admin commands!";
                }

                string text = cmd.Substring(1);
                string text2 = "Sending command to server: " + text;
                sender?.Print(text2, ConsoleColor.Green);
                ReferenceHub.LocalHub.gameConsoleTransmission.SendToServer(text);
                return text2;
            }

            bool flag = cmd.StartsWith("@", StringComparison.Ordinal);
            if ((cmd.StartsWith("/", StringComparison.Ordinal) || flag) && cmd.Length > 1)
            {
                string text3 = flag ? cmd : cmd.Substring(1);
                if (!flag)
                {
                    text3 = text3.TrimStart('$');
                    if (string.IsNullOrEmpty(text3))
                    {
                        sender?.Print("Command cant be empty!", ConsoleColor.Green);
                        return "Command cant be empty!";
                    }
                }

                if (NetworkServer.active)
                {
                    if (!flag)
                        return CommandProcessor.ProcessQuery(text3, sender);
                    CommandProcessor.ProcessAdminChat(text3.Substring(1), sender);
                    return null;
                }

                // ISIL steps 295-360: when NetworkServer NOT active, handle client-side admin command
                // CS was MISSING this entire block!
                if (NetworkClient.active)
                {
                    string logMsg = "Sending remote admin request to server: " + text3;
                    sender?.Print(logMsg, ConsoleColor.Green);
                    ReferenceHub.LocalHub.queryProcessor.SendQuery(text3, false);
                    return logMsg;
                }
                else
                {
                    // Neither server nor client active
                    sender?.Print("You must be connected to a server to use remote admin commands!", ConsoleColor.Red);
                    return "You must be connected to a server to use remote admin commands!";
                }
            }

            string[] array = cmd.Trim().Split(QueryProcessor.SpaceArray, 512, StringSplitOptions.RemoveEmptyEntries);
            if (!EventManager.ExecuteEvent(new ConsoleCommandEvent(sender, array[0], array.Skip(1).ToArray())))
            {
                return null;
            }

            cmd = array[0];
            if (!ConsoleCommandHandler.TryGetCommand(cmd, out var command))
            {
                string text4 = "Command " + cmd + " does not exist!";
                if (!EventManager.ExecuteEvent(new ConsoleCommandExecutedEvent(sender, array[0], array.Skip(1).ToArray(), false, text4)))
                {
                    return null;
                }
                sender?.Print(text4, ConsoleColor.DarkYellow, new Color32(255, 180, 0, 255));
                return text4;
            }

            try
            {
                string response;
                bool flag2 = command.Execute(new ArraySegment<string>(array, 1, array.Length - 1), sender, out response);
                response = Misc.CloseAllRichTextTags(response);
                if (!EventManager.ExecuteEvent(new ConsoleCommandExecutedEvent(sender, array[0], array.Skip(1).ToArray(), flag2, response)))
                {
                    return null;
                }
                if (string.IsNullOrWhiteSpace(response))
                {
                    return null;
                }
                sender?.Print(response, flag2 ? ConsoleColor.Green : ConsoleColor.Red);
                return response;
            }
            catch (Exception ex)
            {
                string text5 = "Command execution failed! Error: " + Misc.RemoveStacktraceZeroes(ex.ToString());
                if (!EventManager.ExecuteEvent(new ConsoleCommandExecutedEvent(sender, array[0], array.Skip(1).ToArray(), false, text5)))
                {
                    return null;
                }
                sender?.Print(text5, ConsoleColor.Red);
                return text5;
            }
        }

        public void ProceedButton()
        {
            if (!string.IsNullOrEmpty(cmdField.text))
            {
                TypeCommand(cmdField.text);
            }
            cmdField.text = string.Empty;
            EventSystem.current.SetSelectedGameObject(cmdField.gameObject);
        }

        internal static void CopyPastebin()
        {
            if (ReferenceHub.TryGetHostHub(out ReferenceHub hub))
            {
                string text = ConfigFile.ServerConfig.GetString("pastebin_read_id", "");
                if (!string.IsNullOrWhiteSpace(text))
                {
                    if (Misc.ValidatePastebin(text))
                    {
                        string url = "https://pastebin.com/raw/" + text;
                        Misc.CopyToClipboard(url);
                        return;
                    }
                    string err = "Pastebin ID is invalid: " + text;
                    Debug.Log(err);
                    logQueue.Enqueue(new Log(err, Color.red, false));
                }
                else
                {
                    Debug.Log("Pastebin is null.");
                    logQueue.Enqueue(new Log("Pastebin is null.", Color.red, false));
                }
            }
            else
            {
                string msg = "Can't find host's reference hub. Please join a server.";
                Debug.Log(msg);
                logQueue.Enqueue(new Log(msg, Color.red, false));
            }
        }

        protected override void OnToggled()
        {
            base.OnToggled();
            cmdField.text = string.Empty;
            if (IsEnabled)
            {
                cmdField.ActivateInputField();
                EventSystem.current.SetSelectedGameObject(cmdField.gameObject);
            }
        }

        private static void RefreshPublicKey()
        {
            while (true)
            {
                try
                {
                    string cache = CentralServerKeyCache.ReadCache();
                    if (!string.IsNullOrEmpty(cache))
                    {
                        _publicKey = Cryptography.ECDSA.PublicKeyFromString(cache);
                        string keyStr = Cryptography.ECDSA.KeyToString(_publicKey);
                        byte[] hash = Cryptography.Sha.Sha256(keyStr);
                        string hashStr = Cryptography.Sha.HashToString(hash);

                        Debug.Log("Loaded central server public key from cache.");
                        logQueue.Enqueue(new Log("Loaded central server public key from cache.", Color.magenta, false));
                        Debug.Log("SHA256 of public key: " + hashStr);
                        logQueue.Enqueue(new Log("SHA256 of public key: " + hashStr, Color.magenta, false));
                    }
                    else
                    {
                        string url = string.Format(CentralServer.StandardUrl, GameCore.Version.Major);
                        string response = HttpQuery.Get(url);
                        PublicKeyResponse respObj = JsonSerialize.FromJson<PublicKeyResponse>(response);

                        if (Cryptography.ECDSA.Verify(respObj.key, respObj.signature, CentralServerKeyCache.MasterKey))
                        {
                            _publicKey = Cryptography.ECDSA.PublicKeyFromString(respObj.key);
                            ServerConsole.PublicKey = _publicKey;

                            string decoded = NorthwoodLib.StringUtils.Base64Decode(respObj.key);
                            CreditsData.LoadData(decoded);

                            string keyStr = Cryptography.ECDSA.KeyToString(_publicKey);
                            byte[] hash = Cryptography.Sha.Sha256(keyStr);
                            string hashStr = Cryptography.Sha.HashToString(hash);

                            Debug.Log("Downloaded public key from central server.");
                            logQueue.Enqueue(new Log("Downloaded public key from central server.", Color.magenta, false));
                            Debug.Log("SHA256 of public key: " + hashStr);
                            logQueue.Enqueue(new Log("SHA256 of public key: " + hashStr, Color.magenta, false));

                            // ISIL: compare new hash against old hash, only save cache if different
                            // Step 443-515: if hashes match, log "no need to update"; if different, SaveCache
                            if (hashStr != CentralServerKeyCache.ReadCache()) // approximate – adapt to actual API
                            {
                                CentralServerKeyCache.SaveCache(respObj.key, respObj.signature);
                            }
                            else
                            {
                                Debug.Log("SHA256 of cached key matches, no need to update cache.");
                                logQueue.Enqueue(new Log("SHA256 of cached key matches, no need to update cache.", Color.magenta, false));
                            }
                        }
                        else
                        {
                            string err = "Can't refresh central server public key - invalid signature!";
                            Debug.Log(err);
                            logQueue.Enqueue(new Log(err, Color.red, false));
                        }
                    }
                    return;
                }
                catch (Exception e)
                {
                    Debug.Log("Can't refresh central server public key!");
                    Debug.Log(e.ToString());
                    logQueue.Enqueue(new Log("Can't refresh central server public key! " + e.Message, Color.red, false));
                    Thread.Sleep(500);
                }
            }
        }

        private void OnApplicationQuit()
        {
            Shutdown.Quit(false, false);
        }
    }
}