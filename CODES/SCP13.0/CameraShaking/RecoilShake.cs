using System.Diagnostics;
using UnityEngine;

namespace CameraShaking
{
    public class RecoilShake : IShakeEffect
    {
        private readonly Stopwatch _removeStopwatch;
        private readonly RecoilSettings _settings;
        private readonly Quaternion _startQuaternion;
        private bool _firstFrame = true;

        public RecoilShake(RecoilSettings settings)
        {
            _settings = settings;
            _startQuaternion = Quaternion.Euler(0f, 0f, settings.ZAxis * (Random.value - 0.5f));
            _removeStopwatch = Stopwatch.StartNew();
        }

        public bool GetEffect(ReferenceHub ply, out ShakeEffectValues shakeValues)
        {
            float progress = Mathf.Clamp01((float)_removeStopwatch.Elapsed.TotalSeconds / _settings.AnimationTime);

            float vLook = _firstFrame ? _settings.UpKick : 0f;
            float hLook = _firstFrame ? _settings.SideKick : 0f;
            _firstFrame = false;

            Quaternion currentRot = Quaternion.Slerp(_startQuaternion, Quaternion.identity, progress);
            float currentFov = Mathf.Lerp(_settings.FovKick, 1f, progress);

            shakeValues = new ShakeEffectValues(currentRot, null, null, currentFov, vLook, hLook);

            return progress < 1f;
        }
    }
}