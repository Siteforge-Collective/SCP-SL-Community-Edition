using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UserSettings;
using UserSettings.GUIElements;

public class MainMenuSoundtrackController : MonoBehaviour
{
    public enum MenuSoundtrackState
    {
        MenuJustLoaded = 0,
        MenuLoop = 1,
        MenuIntensive = 2,
        Muted = 3,
        Retro = 4,
        PregameLobby = 5
    }

    public enum MenuThemeSetting
    {
        RegularTheme = 0,
        EventTheme = 1
    }

    [Serializable]
    public struct MenuAltTheme
    {
        public string Name;
        public AudioClip Theme;
    }

    [Header("Topos (основная тема)")]
    [SerializeField] private AudioSource ToposThemeSource;
    [SerializeField] private AudioClip IntroClip;
    [SerializeField] private AudioClip ToposClip;
    [SerializeField][Range(0.1f, 20f)] private float ToposMetric = 5.1f;
    public float ToposLength { get; private set; }

    [Header("Intense (боевая)")]
    [SerializeField] private AudioSource IntenseThemeSource;
    [SerializeField][Range(0f, 300f)] private float IntenseDropTime = 60f;
    [SerializeField][Range(0, 10)] private int IntenseSequencesToDrop = 2;

    [Header("Retro / Альтернативные")]
    [SerializeField] private AudioSource RetroThemeSource;
    [SerializeField] private List<MenuAltTheme> Themes = new List<MenuAltTheme>();

    private MenuSoundtrackState _currentState = MenuSoundtrackState.MenuJustLoaded;
    private static MainMenuSoundtrackController _instance;

    public static bool DontPlayIntensive { get; private set; }
    private static bool _unlockedThemeSet;

    public bool DebugMode;
    public bool DebugTrigger;
    public MenuSoundtrackState DebugOverride;

    private static bool HasEventMusic => MenuThemeReward.IsHolidayThemeActive;

    public static readonly LinkableEnum ThemePrefsKey = LinkableEnum.ForType<MenuThemeSetting>();

    public static string[] GetThemeNames
    {
        get
        {
            if (_instance == null) return new string[] { "Regular" };

            var list = new List<string> { "Regular" };

            foreach (var theme in _instance.Themes)
            {
                if (theme.Theme != null && !string.IsNullOrEmpty(theme.Name))
                    list.Add(theme.Name);
            }

            return list.ToArray();
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        ToposLength = ToposClip != null ? ToposClip.length : 0f;

        SceneManager.sceneLoaded += OnSceneLoaded;

        Invoke(nameof(ApplySavedTheme), 0.1f);
    }

    private void Start()
    {
        if (DebugMode && DebugTrigger)
            _currentState = DebugOverride;
    }

    private void ApplySavedTheme()
    {
        SetPreferredTheme(false);
    }

    public static void AddTheme(MenuAltTheme theme)
    {
        if (_instance != null && theme.Theme != null)
        {
            _instance.Themes.Add(theme);
        }
    }

    public static void InsertTheme(int index, MenuAltTheme theme)
    {
        if (_instance != null && theme.Theme != null)
        {
            _instance.Themes.Insert(Mathf.Clamp(index, 0, _instance.Themes.Count), theme);
        }
    }

    private void PlayTheme(AudioClip clip)
    {
        if (clip == null || ToposThemeSource == null) return;

        ToposThemeSource.Stop();
        ToposThemeSource.clip = clip;
        ToposThemeSource.loop = true;
        ToposThemeSource.Play();
    }

    private void SetPreferredTheme(bool forcePlay)
    {
        int savedValue = UserSetting<int>.Load(GetPrefsKey(), 0);

        MenuThemeSetting setting = (MenuThemeSetting)Mathf.Clamp(savedValue, 0, 1);

        AudioClip targetClip = ToposClip;

        if (HasEventMusic && setting == MenuThemeSetting.EventTheme && Themes.Count > 0)
        {
            targetClip = Themes[0].Theme ?? ToposClip;
        }
        else if (setting == MenuThemeSetting.EventTheme && Themes.Count > 1)
        {
            targetClip = Themes[1].Theme ?? ToposClip;
        }

        if (IntroClip != null && _currentState == MenuSoundtrackState.MenuJustLoaded)
        {
            ToposThemeSource.clip = IntroClip;
            ToposThemeSource.loop = false;
            ToposThemeSource.Play();

            Invoke(nameof(PlayLoopAfterIntro), IntroClip.length);
        }
        else
        {
            PlayTheme(targetClip);
        }

        _currentState = MenuSoundtrackState.MenuLoop;
    }

    private void PlayLoopAfterIntro()
    {
        SetPreferredTheme(true);
    }

    private void MuteTheme()
    {
        ToposThemeSource?.Stop();
        IntenseThemeSource?.Stop();
        RetroThemeSource?.Stop();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool isLobby = scene.name.Contains("Lobby") || scene.name.Contains("Pregame");

        DontPlayIntensive = isLobby;

        if (isLobby)
        {
            IntenseThemeSource?.Stop();
            _currentState = MenuSoundtrackState.PregameLobby;
        }
        else
        {
            _currentState = MenuSoundtrackState.MenuLoop;
            SetPreferredTheme(false);
        }
    }

    private void LateUpdate()
    {
        if (DebugMode) return;

        // Управление интенсивной музыкой
        if (!DontPlayIntensive && IntenseThemeSource != null && IntenseThemeSource.clip != null)
        {
            if (ToposThemeSource.isPlaying && ToposThemeSource.time >= IntenseDropTime)
            {
                if (!IntenseThemeSource.isPlaying)
                {
                    IntenseThemeSource.Play();
                }
            }
            else
            {
                IntenseThemeSource?.Stop();
            }
        }

        // Ретро-тема (если включена вручную или по событию)
        if (RetroThemeSource != null && RetroThemeSource.isPlaying)
        {
            IntenseThemeSource?.Stop();
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            _instance = null;
        }
    }

    [ContextMenu("Force Regular Theme")]
    private void ForceRegular()
    {
        string key = GetPrefsKey();
        UserSetting<int>.Save(key, 0);
        ApplySavedTheme();
    }

    [ContextMenu("Force Event Theme")]
    private void ForceEvent()
    {
        string key = GetPrefsKey();
        UserSetting<int>.Save(key, 1);
        ApplySavedTheme();
    }

    private string GetPrefsKey()
    {
        return SettingsKeyGenerator.TypeValueToKey(ThemePrefsKey.TypeHash, 0);
    }
}