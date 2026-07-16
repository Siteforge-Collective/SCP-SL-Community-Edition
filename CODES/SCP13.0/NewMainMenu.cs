using MEC;
using Mirror.LiteNetLib4Mirror;
using Steamworks;
using Steamworks.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Misc;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;
using UserSettings;
using Color = UnityEngine.Color;

public class NewMainMenu : MonoBehaviour
{
    public GameObject[] submenus;
    public AudioMixer mainMixer;
    public GameObject DirectConnectPopup;
    public TMP_InputField DirectConnectTextField;
    public Text Version;
    public GameObject Tabs;
    public Text RejoinText;
    public ButtonAudio RejoinButtonAudio;
    public RawImage RejoinIcon;

    private CustomNetworkManager _mng;

    private void Start()
    {
        /*
        if (mainMixer != null)
        {
            mainMixer.SetFloat("MasterVolumeLowpassFreq", 22000f);
            mainMixer.SetFloat("MasterVolumeHighpassWet", -80f);
        }
        */
        _mng = UnityEngine.Object.FindObjectOfType<CustomNetworkManager>();

        if (submenus != null)
        {
            for (int i = 0; i < submenus.Length; i++)
            {
                submenus[i].SetActive(i == 0);
            }
        }

        if (Version != null)
        {
            Version.text = GameCore.Version.VersionString;
        }

        if (Tabs != null)
        {
            Tabs.SetActive(true);
        }

        SteamMatchmaking.OnLobbyGameCreated += OnLobbyGameCreated;

        string lastIp = FavoriteAndHistory.IPHistory.LastOrDefault();
        CustomNetworkManager.LastIp = lastIp;
    }

    private void Update()
    {
        if (RejoinText != null && RejoinButtonAudio != null)
        {
            List<string> ipHistory = FavoriteAndHistory.IPHistory;
            string lastIp = ipHistory.LastOrDefault();
            bool hasHistory = !string.IsNullOrEmpty(lastIp);

            RejoinButtonAudio.enabled = hasHistory;

            if (RejoinIcon != null)
            {
                RejoinIcon.canvasRenderer.SetAlpha(hasHistory ? 1f : 0.5f);
            }
        }
    }

    public void Refresh() => SimpleMenu.LoadCorrectScene();

    public void QuitGame() => Shutdown.Quit(true, false);

    public void ChangeMenu(int id)
    {
        if (submenus != null)
        {
            for (int i = 0; i < submenus.Length; i++)
            {
                submenus[i].SetActive(i == id);
            }
        }
    }

    public void DirectConnectClick()
    {
        if (DirectConnectPopup != null)
            DirectConnectPopup.SetActive(!DirectConnectPopup.activeSelf);
    }

    public void StartServer() => CustomNetworkManager.StartNondedicated(false);

    public void ReJoin()
    {
        string lastIp = CustomNetworkManager.LastIp;
        if (!string.IsNullOrEmpty(lastIp))
        {
            Connect(lastIp, false);
            FavoriteAndHistory.ResetServerID();
        }
    }

    public void Connect()
    {
        if (DirectConnectTextField != null)
        {
            Connect(DirectConnectTextField.text, false);
            FavoriteAndHistory.ResetServerID();
        }
    }

    public void Connect(string ip, bool skipValidation = false)
    {
        if (!CrashDetector.singleton.Show())
        {
            Timing.RunCoroutine(SetIPOrHost(ip, skipValidation));
        }
    }

    private void OnLobbyGameCreated(Lobby lobby, uint ip, ushort port, SteamId serverId)
    {
        string ipStr = new System.Net.IPAddress(ip).ToString();
        string fullAddress = string.Concat(ipStr, ":", port);
        Connect(fullAddress, false);
    }

    private IEnumerator<float> SetIPOrHost(string ip, bool skipValidation)
    {
        CustomNetworkManager.LastIp = ip;

        FavoriteAndHistory.Modify(FavoriteAndHistory.StorageLocation.History, ip, false);

        if (string.IsNullOrEmpty(ip))
        {
            if (string.IsNullOrEmpty(_mng.networkAddress))
            {
                _mng.networkAddress = "localhost";
                LiteNetLib4MirrorTransport.Singleton.port = 7777;
            }
        }
        else
        {
            if (ip.Contains(":"))
            {
                int colonCount = ip.Count(s => s == ':');
                if (colonCount == 1)
                {
                    string[] parts = ip.Split(':');
                    _mng.networkAddress = parts[0];
                    if (ushort.TryParse(parts[1], out ushort portResult))
                        LiteNetLib4MirrorTransport.Singleton.port = portResult;
                }
                else if (ip.StartsWith("[") && ip.Contains("]:"))
                {
                    int endBracketIndex = ip.IndexOf("]:");
                    _mng.networkAddress = ip.Substring(1, endBracketIndex - 1);
                    string portPart = ip.Substring(endBracketIndex + 2);
                    if (ushort.TryParse(portPart, out ushort portResult))
                        LiteNetLib4MirrorTransport.Singleton.port = portResult;
                }
            }
            else
            {
                _mng.networkAddress = ip;
                LiteNetLib4MirrorTransport.Singleton.port = 7777;
            }

            IPAddressType detectedType;
            bool isValid = Misc.ValidateIpOrHostname(_mng.networkAddress, out detectedType, true, true);

            if (!isValid)
            {
                if (skipValidation)
                {
                    GameCore.Console.AddLog("Possible invalid IP detected, skipping validation.", Color.gray, false, GameCore.Console.ConsoleLogType.Log);
                }
                else
                {
                    if (!System.Net.IPAddress.TryParse(_mng.networkAddress, out _))
                    {
                        GameCore.Console.AddLog($"Resolving SRV records of {_mng.networkAddress}...", Color.white, false, GameCore.Console.ConsoleLogType.Log);
                        var task = SrvResolving.ResolveSRV(_mng.networkAddress);
                        while (!task.IsCompleted) yield return Timing.WaitForSeconds(0.05f);

                        var result = task.Result;
                        if (result != null)
                        {
                            _mng.networkAddress = result.Target;
                            LiteNetLib4MirrorTransport.Singleton.port = (ushort)result.Port;
                        }

                        if (!Misc.ValidateIpOrHostname(_mng.networkAddress, out detectedType, true, true))
                        {
                            ShowInvalidIpError();
                            yield break;
                        }
                    }
                    else
                    {
                        ShowInvalidIpError();
                        yield break;
                    }
                }
            }
        }

        CustomNetworkManager.ConnectionIp = _mng.networkAddress;
 
        if (UserSetting<bool>.Get(UserSettings.UserInterfaceSettings.UISetting.HideIP))
        {
            GameCore.Console.AddLog($"Connection IP set to {_mng.networkAddress}, port: {LiteNetLib4MirrorTransport.Singleton.port}", Color.green, false, GameCore.Console.ConsoleLogType.Log);
        }

        if (SteamLobby.singleton.Lobby.Id.IsValid) SteamLobby.singleton.LeaveLobby();

        CustomLiteNetLib4MirrorTransport.SetReconnectionParameters(false);
        _mng.StartClient();
    }

    public void SetMusicVolume(float volume)
    {
        float safeVolume = Mathf.Clamp(volume, 0.0001f, 1f);
        float dbValue = Mathf.Log10(safeVolume) * 20;

        mainMixer.SetFloat("MusicVolume", dbValue);
    }

    private void ShowInvalidIpError()
    {
        GameCore.Console.AddLog("Invalid IP or Hostname.", Color.red, true, GameCore.Console.ConsoleLogType.Error);
        _mng.ShowLog(11, "", "", "", null);
    }
}