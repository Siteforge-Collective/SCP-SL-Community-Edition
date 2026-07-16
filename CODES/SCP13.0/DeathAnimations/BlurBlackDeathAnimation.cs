using CustomRendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace DeathAnimations
{
    public class BlurBlackDeathAnimation : FirstpersonDeathAnimation
    {
        private const float InstantDarknessSpeed = 0.25f;

        public float delayTillDark;

        private bool _instantDarkness;

        private Volume _tempVolume;

        private BlurEffect _blur;

        private Darken _darken;

        private float _fadeTime;

        private void Update()
        {
            _fadeTime += Time.deltaTime;

            if (_instantDarkness)
            {
                _darken.Intensity.value = Mathf.Clamp01(_darken.Intensity.value + (InstantDarknessSpeed * Time.deltaTime));
                if (_darken.Intensity.value >= 1f)
                {
                    Destroy(this);
                }
                return;
            }

            if (_fadeTime < delayTillDark)
            {
                _blur.Amount.value = Mathf.Clamp01(_blur.Amount.value + Time.deltaTime);
            }
            else
            {
                _blur.Amount.value = Mathf.Clamp01(_blur.Iterations.value - Time.deltaTime);
                _darken.Intensity.value = Mathf.Clamp01(_darken.Intensity.value + Time.deltaTime);
                if (_darken.Intensity.value >= 1f)
                {
                    _instantDarkness = true;
                }
            }
        }

        protected override void OnAnimationStarted()
        {
            base.OnAnimationStarted();
            _fadeTime = 0f;
            _instantDarkness = false;

            _tempVolume = PostProcessingVolumes.SafeGetVolume(LayerMask.NameToLayer("PostProcessing"), 0f, new BlurEffect(), new Darken());
            _blur = _tempVolume.GetComponent<BlurEffect>();
            _darken = _tempVolume.GetComponent<Darken>();
            _tempVolume.weight = 1f;
        }

        protected override void OnAnimationEnded()
        {
            EventAssigned = false;
            PostProcessingVolumes.DestroyVolume(_tempVolume);
            Destroy(this);
        }
    }
}
