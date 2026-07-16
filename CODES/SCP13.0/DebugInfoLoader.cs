using System;
using System.Diagnostics;
using GameCore;
using NorthwoodLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugInfoLoader : MonoBehaviour
{
    public Text Audio;
    public Text Cpu;
    public Text CpuThreadsAndFrequency;
    public Text Gpu;
    public Text DriverVersion;
    public Text GraphicApi;
    public Text Os;
    public Text Ram;
    public Text Resolution;
    public Text Fullscreen;
    public Text ShaderLevel;
    public Text Steam;
    public Text UnityVersion;
    public Text GameVersion;
    public Text Build;
    public Text GameLanguage;
    public Text GameScene;
    public Text Errors;
    public Text CentralServerText;

    private string _centralserver = "";
    private bool _updateOnNextFrame;
    private readonly Stopwatch _stopwatch = new Stopwatch();

    private void Awake()
    {
        // Подписка на события изменения настроек и загрузки драйверов
        PlayerPrefsSl.SettingChanged += (s, s1) => this._updateOnNextFrame = true;
        PlayerPrefsSl.SettingRemoved += (s) => this._updateOnNextFrame = true;
        PlayerPrefsSl.SettingsRefreshed += () => this._updateOnNextFrame = true;

        // Подписка на загрузку сцены и драйвера GPU
        SceneManager.sceneLoaded += (arg0, mode) => this._updateOnNextFrame = true;
        GpuDriver.DriverLoaded += (s) => this._updateOnNextFrame = true;
    }

    private void OnEnable()
    {
        this._stopwatch.Restart();
        this.UpdateData();
    }

    private void UpdateData()
    {
        if (!this.enabled) return;

        this.Gpu.text = "GPU: " + SystemInfo.graphicsDeviceName;
        this.DriverVersion.text = "GPU Driver: " + GpuDriver.DriverVersion;

        // VRAM и Shader Level
        this.ShaderLevel.text = string.Format("VRAM: {0}MB ShaderLevel: {1}",
            SystemInfo.graphicsMemorySize,
            SystemInfo.graphicsShaderLevel.ToString().Insert(1, "."));

        this.GraphicApi.text = "Graphic API: " + SystemInfo.graphicsDeviceType.ToString() + " " + SystemInfo.graphicsDeviceType.ToString();

        // Формирование строки разрешения и VSync
        string[] resParts = new string[6];
        resParts[0] = "Resolution: ";
        resParts[1] = Screen.width.ToString();
        resParts[2] = "x";
        resParts[3] = Screen.height.ToString();
        resParts[4] = "  ";
        resParts[5] = string.Format("VSync: {0}", QualitySettings.vSyncCount == 0 ? Application.targetFrameRate : QualitySettings.vSyncCount);
        this.Resolution.text = string.Concat(resParts);

        this.Fullscreen.text = "Fullscreen: " + Screen.fullScreenMode.ToString();
        this.Cpu.text = "CPU: " + SystemInfo.processorType;

        // Потоки и частота CPU
        this.CpuThreadsAndFrequency.text = string.Concat("Threads: ", SystemInfo.processorCount.ToString(), "   ", SystemInfo.processorFrequency.ToString(), "MHz");

        this.Ram.text = "RAM: " + SystemInfo.systemMemorySize.ToString() + "MB";
        this.Audio.text = "Audio Supported: " + SystemInfo.supportsAudio.ToString();
        this.Os.text = "OS: " + NorthwoodLib.OperatingSystem.VersionString;

        // Определение платформы авторизации
        var platform = CentralAuth.CentralAuthManager.Platform;
        if (platform == DistributionPlatform.Steam)
            this.Steam.text = "Steam: " + SteamManager.GetApiState();
        else if (platform == DistributionPlatform.Discord)
            this.Steam.text = "Discord: " + platform.ToString();
        else
            this.Steam.text = "No auth platform!";

        this.UnityVersion.text = "Unity " + Application.unityVersion;
        this.GameVersion.text = "Version: " + GameCore.Version.VersionString;
        this.Build.text = "Build: " + PlatformInfo.singleton.BuildGuid;

        // Центральный сервер и язык
        this._centralserver = "Central Server: " + CentralServer.SelectedServer;
        this.CentralServerText.text = this._centralserver;
        this.GameLanguage.text = "Language: " + TranslationReader.TranslationDirectoryName;
        this.GameScene.text = "Scene: " + SceneManager.GetActiveScene().name;

        // Счетчик ошибок из DebugScreenController
        string[] errorParts = new string[6];
        errorParts[0] = "Asserts: ";
        errorParts[1] = DebugScreenController.Asserts.ToString();
        errorParts[2] = " Errors: ";
        errorParts[3] = DebugScreenController.Errors.ToString();
        errorParts[4] = " Exceptions: ";
        errorParts[5] = DebugScreenController.Exceptions.ToString();
        this.Errors.text = string.Concat(errorParts);
    }

    private void Update()
    {
        this._stopwatch.Restart();

        if (this._updateOnNextFrame)
        {
            this._updateOnNextFrame = false;
            this.UpdateData();
        }
    }

    private void FixedUpdate()
    {
        string selected = CentralServer.SelectedServer;
        if (!string.IsNullOrEmpty(selected))
        {
            if (!this._centralserver.Contains(selected))
            {
                this._centralserver = "Central Server: " + selected;
                this.CentralServerText.text = this._centralserver;
            }
        }
    }
}