using System;
using System.Collections.Generic;
using MEC;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public CanvasGroup selfGroup;
    public GameObject root;
    public Image image;
    public Image loadingCircle;
    public Image oldImage;
    public TextMeshProUGUI hints;
    public TextMeshProUGUI progress;

    public int framesToNext = 600;
    public Sprite[] backgrounds;
    public string[] hintsText;
    public string description;

    private CoroutineHandle _checkLoadingScreen;
    private int _currentFrames;
    private float _lastProgress;
    private string _currentLoadedScene;

    private static bool _noFade;
    private static bool _loaded;

    private const string _facilitySceneName = "Assets/_Scenes/Facility.unity";

    private void Start()
    {
        TranslationReader.OnTranslationsRefreshed += RefreshHints; 

        hintsText = TranslationReader.GetKeys("LoadingHints");
        description = TranslationReader.Get("Connection_Info", 1, "NO_TRANSLATION");

        if (progress != null)
        {
            progress.text = description;
        }

        Next();
    }

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        TranslationReader.OnTranslationsRefreshed -= RefreshHints;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void RefreshHints()
    {
        hintsText = TranslationReader.GetKeys("LoadingHints");
        description = TranslationReader.Get("Connection_Info", 1, "NO_TRANSLATION");

        if (progress != null)
        {
            progress.text = description;
        }

        Next();
    }

    private void OnEnable()
    {
        if (loadingCircle != null)
            loadingCircle.fillAmount = 0f;

        _lastProgress = 0;

        if (progress != null)
            progress.text = $"{description} 0%";
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _currentLoadedScene = scene.name.ToLower();

        if (scene.name.Equals("Facility", StringComparison.OrdinalIgnoreCase))
        {
            string networkSceneName = NetworkManager.networkSceneName ?? "";
            if (!networkSceneName.Equals(_facilitySceneName, StringComparison.OrdinalIgnoreCase))
            {
                ResetToDefault();
            }
            else
            {
                if (loadingCircle != null)
                    loadingCircle.fillAmount = 0f;

                _lastProgress = 0;
                _loaded = false;
                _noFade = false;

                if (root != null)
                {
                    root.SetActive(true);
                    CheckIfStuck();
                }
            }
        }
        else
        {
            ResetToDefault();
        }

        GameCore.Console.AddLog($"Scene Loaded: {_currentLoadedScene}", Color.green);
    }

    private void CheckIfStuck()
    {
        if (!NetworkClient.active && _currentFrames > 120)
        {
            Debug.LogError("Client is inactive");
            if (_currentLoadedScene.Contains("menu") && root.activeSelf)
            {
                ResetToDefault();
            }
            else if (string.Equals(_currentLoadedScene, "facility", StringComparison.OrdinalIgnoreCase))
            {
                SceneManager.LoadSceneAsync("NewMainMenu");
            }
        }
        else
        {
            GameCore.Console.AddLog("LoadingScreen: Client is active, not stuck.", Color.green);
        }
    }

    private void ResetToDefault()
    {
        _currentFrames = 0;
        _loaded = true;
        _noFade = false;

        if (root != null)
            root.SetActive(false);

        if (_checkLoadingScreen.IsRunning)
        {
            Timing.KillCoroutines(_checkLoadingScreen);
        }
    }

    private IEnumerator<float> _CheckLoadingScreenStuck()
    {
        while (true)
        {
            CheckIfStuck();
            yield return Timing.WaitForSeconds(3f);
        }
    }

    private void FixedUpdate()
    {
        if (!_checkLoadingScreen.IsRunning && root.activeSelf)
        {
            _checkLoadingScreen = Timing.RunCoroutine(_CheckLoadingScreenStuck());
        }

        if (!root.activeSelf)
        {
            if (selfGroup != null) selfGroup.alpha = 0f;
            _currentFrames = 0;
            return;
        }

        if (selfGroup != null && selfGroup.alpha < 1f)
        {
            selfGroup.alpha += Time.deltaTime;
            if (selfGroup.alpha > 1f) selfGroup.alpha = 1f;
        }

        _currentFrames++;
        if (_currentFrames > framesToNext)
        {
            _currentFrames = 0;
            Next();
        }

        if (NetworkManager.networkSceneName != null &&
            NetworkManager.networkSceneName.Equals(_facilitySceneName, StringComparison.OrdinalIgnoreCase))
        {
            var asyncOp = NetworkManager.loadingSceneAsync;
            if (asyncOp != null)
            {
                float currentProgress = asyncOp.progress;

                if (loadingCircle != null)
                    loadingCircle.fillAmount = currentProgress;

                if (Math.Abs(_lastProgress - currentProgress) > 0.01f)
                {
                    if (progress != null)
                    {
                        float percent = currentProgress * 100f;
                        progress.text = $"{description} {percent:F0}%";
                    }
                    _lastProgress = currentProgress;
                }
            }
        }

        if ((loadingCircle != null && loadingCircle.fillAmount >= 1f) || _loaded)
        {
            _loaded = true;

            if (loadingCircle != null)
                loadingCircle.fillAmount = 0f;

            if (progress != null)
                progress.text = $"{description} 0%";

            root?.SetActive(false);

            if (_checkLoadingScreen.IsRunning)
            {
                Timing.KillCoroutines(_checkLoadingScreen);
            }
        }
    }

    private void Next()
    {
        if (oldImage != null && image != null)
        {
            oldImage.sprite = image.sprite;
            oldImage.CrossFadeAlpha(1f, 0f, true); 
            oldImage.CrossFadeAlpha(0f, 0.5f, true);
        }

        if (backgrounds != null && backgrounds.Length > 0 && image != null)
        {
            int bgIndex = UnityEngine.Random.Range(0, backgrounds.Length);
            image.sprite = backgrounds[bgIndex];
            image.CrossFadeAlpha(1f, 0.5f, true);
        }

        if (hintsText != null && hintsText.Length > 0 && hints != null)
        {
            int hintIndex = UnityEngine.Random.Range(0, hintsText.Length);
            hints.text = hintsText[hintIndex];
        }
        else if (hints != null)
        {
            hints.text = "";
        }
    }

    internal static void FinishedLoading()
    {
        _loaded = true;
    }
}