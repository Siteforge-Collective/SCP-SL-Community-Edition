using System.Collections.Generic;
using InventorySystem.Hotkeys.Customization;
using InventorySystem.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem.Hotkeys
{
    public abstract class HotkeyIconBase : MonoBehaviour
    {
        private class HighlightHandler
        {
            public bool IsPlaying;
            public float Timer;
        }

        [SerializeField] protected Image Outline;
        [SerializeField] protected Image HighlightImage;
        [SerializeField] protected TextMeshProUGUI KeycodeText;

        private KeyCode _prevKeyCode;
        private Color _highlightTransparentColor;

        private static readonly AnimationCurve HighlightAnimation;
        private static readonly Dictionary<KeyCode, HighlightHandler> ActiveHighlights;
        static HotkeyIconBase()
        {
            HighlightAnimation = new AnimationCurve(new Keyframe[]
            {
                new Keyframe(0f, 0f),
                new Keyframe(0.2f, 1f),
                new Keyframe(1f, 0f)
            });

            ActiveHighlights = new Dictionary<KeyCode, HighlightHandler>();
        }

        public KeyCode Key
        {
            get => _prevKeyCode;
            set
            {
                if (value != _prevKeyCode)
                {
                    _prevKeyCode = value;
                    if (KeycodeText != null)
                        KeycodeText.text = value.ToString();
                }
            }
        }

        public abstract void SetItem(ItemBase item);
        public abstract bool CheckCompatibility(ItemBase item, HotkeyApperance apperance);

        public void UpdateAnimations()
        {
            if (!ActiveHighlights.TryGetValue(Key, out HighlightHandler handler))
                return;

            if (handler.IsPlaying)
            {
                if (!InventorySystem.GUI.InventoryGuiController.InventoryVisible)
                {
                    handler.Timer += Time.deltaTime;
                }

                HighlightImage.enabled = true;
                float intensity = HighlightAnimation.Evaluate(handler.Timer);

                Color c = HighlightImage.color;
                c.a = intensity;
                HighlightImage.color = c;

                if (handler.Timer >= 1f)
                {
                    HighlightImage.enabled = false;
                    handler.IsPlaying = false;
                    handler.Timer = 0f;
                }
            }
        }

        public void PlayHighlightAnimation()
        {
            if (!ActiveHighlights.TryGetValue(Key, out HighlightHandler handler))
            {
                handler = new HighlightHandler();
                ActiveHighlights.Add(Key, handler);
            }

            handler.Timer = 0f;
            handler.IsPlaying = true;
        }

        public void CancelHighlightAnimation()
        {
            HighlightImage.color = _highlightTransparentColor;
            HighlightImage.enabled = false;

            if (ActiveHighlights.TryGetValue(Key, out HighlightHandler handler))
            {
                handler.IsPlaying = false;
                handler.Timer = 0f;
            }
        }

        public virtual void SetColors(Color color)
        {
            if (Outline != null)
                Outline.color = color;

            if (KeycodeText != null)
                KeycodeText.color = color;

            Color transparent = color;
            transparent.a = 0f;
            _highlightTransparentColor = transparent;
        }
    }
}