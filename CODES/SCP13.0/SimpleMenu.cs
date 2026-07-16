using CentralAuth;
using MEC;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleMenu : MonoBehaviour
{
    public bool isPreloader;

    private static bool _server;
    private static bool _forceSettings;
    private static string _targetSceneName;

    private const float minLoadingTime = 3f;

    internal static readonly string[] MenuSceneNames = new string[3]
    {
        "MainMenuRemastered",
        "NewMainMenu",
        "FastMenu"
    };

    public float Progress => 0f;

    private void Awake()
    {
        if (isPreloader)
        {
            return;
        }

        CentralAuthManager.InitAuth();

        string[] args = StartupArgs.Args;
        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "-fastmenu":
                    PlayerPrefsSl.Set("fastmenu", true);
                    PlayerPrefsSl.Set("menumode", 2);
                    break;
                case "-newmenu":
                    PlayerPrefsSl.Set("menumode", 1);
                    break;
                case "-nographics":
                    _server = true;
                    break;
                case "-forcemenu":
                    _forceSettings = true;
                    break;
            }
        }
        Refresh();
    }

    private void Start()
    {
        Timing.RunCoroutine(StartLoad());
    }

    private IEnumerator<float> StartLoad()
    {
        yield return float.NegativeInfinity;

        if (isPreloader)
        {
            float startTime = Time.time;
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Loader", LoadSceneMode.Single);

            while (!asyncOperation.isDone)
            {
                yield return minLoadingTime;
            }

            float remainingTime = minLoadingTime - (Time.time - startTime);
            if (remainingTime > 0f)
            {
                yield return Timing.WaitForSeconds(remainingTime);
            }

            asyncOperation.allowSceneActivation = true;
        }
    }

    public void ChangeMode()
    {
        PlayerPrefsSl.Set("fastmenu", false);
        PlayerPrefsSl.Set("menumode", 1);
        Refresh();
        LoadCorrectScene();
    }

    public static void ChangeMode(int id)
    {
        PlayerPrefsSl.Set("menumode", id);
        Refresh();
        LoadCorrectScene();
    }

    private static void Refresh()
    {
        _targetSceneName = _server ? "FastMenu" : MenuSceneNames[(!_forceSettings) ? 1 : Mathf.Clamp(PlayerPrefsSl.Get("menumode", 1), 0, 2)];

        var networkManager = Object.FindFirstObjectByType<CustomNetworkManager>();
        if (networkManager != null)
        {
            networkManager.offlineScene = _targetSceneName;
        }
    }

    public static void LoadCorrectScene()
    {
        SceneManager.LoadScene(_targetSceneName);
    }
}