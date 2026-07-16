using System;
using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerRoles.Filmmaker
{
    public class FilmmakerUI : MonoBehaviour
    {
        private enum FilmmakerKey
        {
            UpdatePlayback = 32,    // Space
            JumpToBeginning = 278,  // Home
            JumpToEnd = 279,        // End
            RemoveKeyframe = 127,   // Delete
            JumpForward = 275,      // Right Arrow
            JumpBackward = 276      // Left Arrow
        }

        [SerializeField]
        private Toggle _automaticPreview;

        [SerializeField]
        private Slider _playbackSpeedPercent;

        [SerializeField]
        private TMP_Text _playbackSpeedText;

        [SerializeField]
        private TMP_Dropdown _blendModeDropdown;

        private const float ClearAllTime = 5f;

        private float _playbackSpeed = 1f; 

        private Canvas _canvas;

        private readonly Stopwatch _clearStopwatch = new Stopwatch();

        private Transform CurCam => MainCameraController.CurrentCamera;

        private FilmmakerBlendPreset BlendPreset => (FilmmakerBlendPreset)_blendModeDropdown.value;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _canvas.enabled = HideHUDController.IsHUDVisible;

            HideHUDController.ToggleHUD += OnHideHudChanged;
            FilmmakerKeyframesRenderer.OnTimeSet += UpdatePreviewConditionally;

            _automaticPreview.onValueChanged.AddListener(val =>
            {
                if (val) FilmmakerTimelineManager.UpdatePlayback();
            });

            _playbackSpeedPercent.onValueChanged.AddListener(newVal =>
            {
                _playbackSpeed = newVal / 100f;
                _playbackSpeedText.text = string.Format("Playback speed: {0}%", newVal);
            });
        }

        private void OnDestroy()
        {
            HideHUDController.ToggleHUD -= OnHideHudChanged;
            FilmmakerKeyframesRenderer.OnTimeSet -= UpdatePreviewConditionally;
        }

        private void OnHideHudChanged(bool state)
        {
            if (_canvas != null)
                _canvas.enabled = state;
        }

        private void UpdatePreviewConditionally()
        {
            if (_automaticPreview.isOn)
            {
                FilmmakerTimelineManager.UpdatePlayback();
            }
        }

        private void Update()
        {
            if (FilmmakerTimelineManager.PlaybackActive) 
            {
                FilmmakerTimelineManager.TimeSeconds += Time.deltaTime * _playbackSpeed;
            }

            if (GetKeyDown(FilmmakerKey.UpdatePlayback))
            {
                FilmmakerTimelineManager.PlaybackActive = !FilmmakerTimelineManager.PlaybackActive;
                FilmmakerTimelineManager.UpdatePlayback();
            }

            if (GetKey(FilmmakerKey.RemoveKeyframe))
            {
                if (!_clearStopwatch.IsRunning)
                    _clearStopwatch.Restart();

                if (_clearStopwatch.Elapsed.TotalSeconds > ClearAllTime)
                {
                    foreach (var track in FilmmakerTimelineManager.AllTracks)
                    {
                        track.ClearAll();
                    }
                    _clearStopwatch.Reset();
                }
            }
            else
            {
                _clearStopwatch.Reset();
            }

            int newFrame = UpdateTimeFrames();
            if (newFrame != -1)
            {
                FilmmakerTimelineManager.TimeFrames = newFrame;

                if (!_automaticPreview.isOn)
                {
                    FilmmakerTimelineManager.UpdatePlayback();
                }
            }
        }

        private int UpdateTimeFrames()
        {
            if (GetKeyDown(FilmmakerKey.JumpToBeginning))
                return 0;

            if (GetKeyDown(FilmmakerKey.JumpToEnd))
            {
                var tracks = FilmmakerTimelineManager.AllTracks;
                if (tracks.Length == 0) return 0;
                return tracks.Max(t => t.LastFrame);
            }

            if (GetKeyDown(FilmmakerKey.JumpForward))
            {
                var tracks = FilmmakerTimelineManager.AllTracks;
                int current = FilmmakerTimelineManager.TimeFrames;
                var nextFrames = tracks.Select(t =>
                {
                    if (t.TryGetNextFrame(current, out int next))
                        return (int?)next;
                    return null;
                }).Where(n => n.HasValue).Select(n => n.Value);

                if (nextFrames.Any())
                    return nextFrames.Min();
                return current;
            }

            if (GetKeyDown(FilmmakerKey.JumpBackward))
            {
                var tracks = FilmmakerTimelineManager.AllTracks;
                int current = FilmmakerTimelineManager.TimeFrames;
                var prevFrames = tracks.Select(t =>
                {
                    if (t.TryGetPrevFrame(current, out int prev))
                        return (int?)prev;
                    return null;
                }).Where(p => p.HasValue).Select(p => p.Value);

                if (prevFrames.Any())
                    return prevFrames.Max();
                return current;
            }

            if (GetKeyDown(FilmmakerKey.RemoveKeyframe)) 
            {
                foreach (var track in FilmmakerTimelineManager.AllTracks)
                {
                    track.ClearFrame(FilmmakerTimelineManager.TimeFrames);
                }
            }

            return -1;
        }

        public void UpdatePlayback()
        {
            FilmmakerTimelineManager.UpdatePlayback();
        }

        public void SamplePosition()
        {
            var positionTrack = FilmmakerTimelineManager.PositionTrack;
            if (positionTrack != null && CurCam != null)
            {
                positionTrack.AddOrReplace(CurCam.position, BlendPreset);
            }
        }

        public void SampleRotation()
        {
            var rotationTrack = FilmmakerTimelineManager.RotationTrack;
            if (rotationTrack != null && CurCam != null)
            {
                rotationTrack.AddOrReplace(CurCam.rotation, BlendPreset);
            }
        }

        public void SampleZoom()
        {
            var zoomTrack = FilmmakerTimelineManager.ZoomTrack;
            if (zoomTrack != null)
            {
                zoomTrack.AddOrReplace(FilmmakerRole.ZoomScale, BlendPreset);
            }
        }

        private static bool GetKey(FilmmakerKey key)
        {
            return Input.GetKey((KeyCode)key);
        }

        private static bool GetKeyDown(FilmmakerKey key)
        {
            return Input.GetKeyDown((KeyCode)key);
        }
    }
}