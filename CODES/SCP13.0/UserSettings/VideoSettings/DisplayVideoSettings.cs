using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NorthwoodLib.Pools;
using UnityEngine;

namespace UserSettings.VideoSettings
{
    public static class DisplayVideoSettings
    {
        private const string PrefsKey = "UnitySelectMonitor";

        private static Resolution[] _supportedResolutions;

        private static bool _isChangingDisplay;

        public static readonly AspectRatio[] SupportedRatios = new AspectRatio[4]
        {
            new AspectRatio
            {
                Horizontal = 4f,
                Vertical = 3f,
                RatioMinMax = new Vector2(1.28f, 1.38f)
            },
            new AspectRatio
            {
                Horizontal = 16f,
                Vertical = 10f,
                RatioMinMax = new Vector2(1.59f, 1.61f)
            },
            new AspectRatio
            {
                Horizontal = 16f,
                Vertical = 9f,
                RatioMinMax = new Vector2(1.75f, 1.78f)
            },
            new AspectRatio
            {
                Horizontal = 21f,
                Vertical = 9f,
                RatioMinMax = new Vector2(2.27f, 2.39f)
            }
        };

        private static int _desiredWidth;

        private static int _desiredHeight;

        private static bool _alreadyRunning;

        public static int CurrentDisplayIndex { get; private set; }

        public static event Action OnDisplayChanged;

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            SetupDisplayChangeDetector();

            SetDefaultValues();

            UpdateSettings(true);

            Display.onDisplaysUpdated += delegate { UpdateSettings(false); };

            UserSetting<int>.AddListener(DisplayVideoSetting.VSyncCount, delegate (int _)
            {
                UpdateSettings(false);
            });

            UserSetting<bool>.AddListener(DisplayVideoSetting.FpsLimiter, delegate (bool _)
            {
                UpdateSettings(false);
            });

            UserSetting<float>.AddListener(DisplayVideoSetting.FpsLimiter, delegate (float _)
            {
                UpdateSettings(false);
            });

            UserSetting<int>.AddListener(DisplayVideoSetting.FullscreenMode, delegate (int _)
            {
                UpdateSettings(false);
            });

            UserSetting<int>.AddListener(DisplayVideoSetting.Resolution, delegate (int _)
            {
                UpdateSettings(false);
            });

            UserSetting<int>.AddListener(DisplayVideoSetting.AspectRatio, OnAspectRatioChanged);
        }

        private static void SetupDisplayChangeDetector()
        {
            CurrentDisplayIndex = -1;
            StaticUnityMethods.OnUpdate += delegate
            {
                int num = PlayerPrefs.GetInt(PrefsKey);
                if (num != CurrentDisplayIndex && !_isChangingDisplay)
                {
                    CurrentDisplayIndex = num;
                    DisplayVideoSettings.OnDisplayChanged?.Invoke();
                    UpdateSettings(false);
                }
            };
        }

        private static void SetDefaultValues()
        {
            UserSetting<float>.SetDefaultValue(DisplayVideoSetting.FpsLimiter, 60f);
        }

        private static void OnAspectRatioChanged(int newRatio)
        {
            Resolution[] selectedAspectResolutions = GetSelectedAspectResolutions(newRatio);
            int value = selectedAspectResolutions.Length - 1;
            int num = int.MaxValue;
            int num2 = int.MaxValue;
            for (int i = 0; i < selectedAspectResolutions.Length; i++)
            {
                Resolution resolution = selectedAspectResolutions[i];
                int num3 = Mathf.Abs(resolution.height - Screen.height);
                if (num3 <= num)
                {
                    int num4 = Mathf.Abs(resolution.width - Screen.width);
                    if (num3 != num || num4 <= num2)
                    {
                        value = i;
                        num = num3;
                        num2 = num4;
                    }
                }
            }

            UserSetting<int>.Set(DisplayVideoSetting.Resolution, value);
        }

        private static void UpdateSettings(bool onLoad = false)
        {
            HashSet<uint> hashSet = HashSetPool<uint>.Shared.Rent();
            List<Resolution> list = ListPool<Resolution>.Shared.Rent();
            Resolution[] resolutions = Screen.resolutions;
            for (int i = 0; i < resolutions.Length; i++)
            {
                Resolution item = resolutions[i];
                uint item2 = (uint)((ushort)item.width | (item.height << 16));
                if (hashSet.Add(item2))
                {
                    list.Add(item);
                }
            }
            _supportedResolutions = list.ToArray();
            ListPool<Resolution>.Shared.Return(list);
            HashSetPool<uint>.Shared.Return(hashSet);

            QualitySettings.vSyncCount = UserSetting<int>.Get(DisplayVideoSetting.VSyncCount);
            Application.targetFrameRate = (UserSetting<bool>.Get(DisplayVideoSetting.FpsLimiter) ? Mathf.RoundToInt(UserSetting<float>.Get(DisplayVideoSetting.FpsLimiter)) : 0);

            FullScreenMode fullScreenMode = (FullScreenMode)UserSetting<int>.Get(DisplayVideoSetting.FullscreenMode);
            Resolution[] selectedAspectResolutions = GetSelectedAspectResolutions(UserSetting<int>.Get(DisplayVideoSetting.AspectRatio));
            int num = ((selectedAspectResolutions != null) ? selectedAspectResolutions.Length : 0);
            if (num == 0)
            {
                Screen.fullScreenMode = fullScreenMode;
                return;
            }

            int value = UserSetting<int>.Get(DisplayVideoSetting.Resolution, num - 1, setAsDefault: true);
            Resolution resolution = selectedAspectResolutions[Mathf.Clamp(value, 0, num - 1)];

            Screen.SetResolution(resolution.width, resolution.height, fullScreenMode);

            if (onLoad && fullScreenMode == FullScreenMode.ExclusiveFullScreen && !_alreadyRunning)
            {
                _alreadyRunning = true;
                _desiredWidth = resolution.width;
                _desiredHeight = resolution.height;
                StaticUnityMethods.OnUpdate += UpdateExclusiveFullScreen;
            }
        }

        private static Resolution[] GetUnsupportedResolutions()
        {
            List<Resolution> list = ListPool<Resolution>.Shared.Rent();
            Resolution[] supportedResolutions = _supportedResolutions;
            foreach (Resolution resolution in supportedResolutions)
            {
                if (!IsSupportedRatio(resolution))
                {
                    list.Add(resolution);
                }
            }
            Resolution[] result = list.ToArray();
            ListPool<Resolution>.Shared.Return(list);
            return result;
        }

        public static Resolution[] GetSelectedAspectResolutions(int filterId)
        {
            if (filterId <= 0)
            {
                return _supportedResolutions;
            }
            if (filterId > SupportedRatios.Length)
            {
                return GetUnsupportedResolutions();
            }
            AspectRatio aspectRatio = SupportedRatios[filterId - 1];
            List<Resolution> list = ListPool<Resolution>.Shared.Rent();
            Resolution[] supportedResolutions = _supportedResolutions;
            foreach (Resolution resolution in supportedResolutions)
            {
                if (aspectRatio.CheckRes(resolution))
                {
                    list.Add(resolution);
                }
            }
            Resolution[] result = list.ToArray();
            ListPool<Resolution>.Shared.Return(list);
            return result;
        }

        public static bool IsSupportedRatio(Resolution res)
        {
            AspectRatio[] supportedRatios = SupportedRatios;
            foreach (AspectRatio aspectRatio in supportedRatios)
            {
                if (aspectRatio.CheckRes(res))
                {
                    return true;
                }
            }
            return false;
        }

        public static async void ChangeDisplay(int displayIndex)
        {
            if (!_isChangingDisplay)
            {
                _isChangingDisplay = true;
                List<DisplayInfo> list = ListPool<DisplayInfo>.Shared.Rent();
                Screen.GetDisplayLayout(list);
                if (displayIndex >= 0 && displayIndex < list.Count)
                {
                    DisplayInfo display = list[displayIndex];
                    AsyncOperation async = Screen.MoveMainWindowTo(display, default(Vector2Int));
                    while (!async.isDone)
                    {
                        await Task.Delay(Mathf.CeilToInt(Time.deltaTime * 1000f));
                    }
                    UserSetting<int>.Set(DisplayVideoSetting.AspectRatio, 0);
                    OnAspectRatioChanged(0);
                }
                ListPool<DisplayInfo>.Shared.Return(list);
                _isChangingDisplay = false;
            }
        }

        private static void UpdateExclusiveFullScreen()
        {
            if (Input.anyKeyDown && Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
            {
                Screen.SetResolution(_desiredWidth, _desiredHeight, FullScreenMode.ExclusiveFullScreen);
                StaticUnityMethods.OnUpdate -= UpdateExclusiveFullScreen;
                _alreadyRunning = false;
            }
        }
    }
}