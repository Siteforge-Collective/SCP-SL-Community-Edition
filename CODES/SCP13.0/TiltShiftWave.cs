using CustomPlayerEffects;
using CustomRendering;
using UnityEngine.Rendering;

public class TiltShiftWave : PostProcessEffectWave
{
    private TiltShift _tiltShift;

    protected override float EffectValue
    {
        get
        {
            if (_tiltShift != null)
            {
                return _tiltShift.Amount.value;
            }
            return 0f;
        }
        set
        {
            if (_tiltShift != null)
            {
                _tiltShift.Amount.value = value;
            }
        }
    }

    protected override void SetEffectType(VolumeProfile profile)
    {
        if (profile != null && profile.TryGet<CustomRendering.TiltShift>(out var tiltShift))
        {
            _tiltShift = tiltShift;
        }
    }
}