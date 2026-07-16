public class SoundPlayer : global::UnityEngine.MonoBehaviour
{
	protected global::UnityEngine.AudioSource _source;

	public CurvePreset[] Curves;

	private readonly global::System.Collections.Generic.Dictionary<FalloffType, CurvePreset> _curves = new global::System.Collections.Generic.Dictionary<FalloffType, CurvePreset>();

	private void Start()
	{
		_source = GetComponent<global::UnityEngine.AudioSource>();
		LoadInspectorCurves();
	}

	protected void LoadInspectorCurves()
	{
		CurvePreset[] curves = Curves;
		foreach (CurvePreset curvePreset in curves)
		{
			if (!_curves.ContainsKey(curvePreset.Type))
			{
				_curves.Add(curvePreset.Type, curvePreset);
			}
		}
	}

	public void Play(global::UnityEngine.AudioClip clip, FalloffType falloff = FalloffType.Linear, float maxDistance = -1f)
	{
		if (_curves.TryGetValue(falloff, out var value))
		{
			_source.SetCustomCurve(global::UnityEngine.AudioSourceCurveType.CustomRolloff, value.FalloffCurve);
			if (maxDistance > 0f)
			{
				_source.maxDistance = maxDistance;
			}
			_source.clip = clip;
			_source.Play();
		}
	}

	public void Stop()
	{
		if (_source.isPlaying)
		{
			_source.Stop();
		}
		_source.clip = null;
	}
}
