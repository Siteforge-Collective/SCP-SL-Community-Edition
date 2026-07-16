public class ControlPanelLights : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Texture[] emissions;

	public global::UnityEngine.Material targetMat;

	private static readonly int _emissionMap = global::UnityEngine.Shader.PropertyToID("_EmissionMap");

	private void Start()
	{
		global::MEC.Timing.RunCoroutine(_Animate(), global::MEC.Segment.FixedUpdate);
	}

	private global::System.Collections.Generic.IEnumerator<float> _Animate()
	{
		int l = emissions.Length;
		while (this != null)
		{
			if (targetMat != null)
			{
				targetMat.SetTexture(_emissionMap, emissions[global::UnityEngine.Random.Range(0, l)]);
			}
			yield return global::MEC.Timing.WaitForSeconds(global::UnityEngine.Random.Range(0.2f, 0.8f));
		}
	}
}
