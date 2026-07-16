[global::UnityEngine.AddComponentMenu("UBER/Apply Light for Deferred")]
[global::UnityEngine.ExecuteInEditMode]
public class UBER_applyLightForDeferred : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Light lightForSelfShadowing;

	private global::UnityEngine.Renderer _renderer;

	private void Start()
	{
		Reset();
	}

	private void Reset()
	{
		if ((bool)GetComponent<global::UnityEngine.Light>() && lightForSelfShadowing == null)
		{
			lightForSelfShadowing = GetComponent<global::UnityEngine.Light>();
		}
		if ((bool)GetComponent<global::UnityEngine.Renderer>() && _renderer == null)
		{
			_renderer = GetComponent<global::UnityEngine.Renderer>();
		}
	}

	private void Update()
	{
		if (!lightForSelfShadowing)
		{
			return;
		}
		if ((bool)_renderer)
		{
			if (lightForSelfShadowing.type == global::UnityEngine.LightType.Directional)
			{
				for (int i = 0; i < _renderer.sharedMaterials.Length; i++)
				{
					_renderer.sharedMaterials[i].SetVector("_WorldSpaceLightPosCustom", -lightForSelfShadowing.transform.forward);
				}
			}
			else
			{
				for (int j = 0; j < _renderer.materials.Length; j++)
				{
					_renderer.sharedMaterials[j].SetVector("_WorldSpaceLightPosCustom", new global::UnityEngine.Vector4(lightForSelfShadowing.transform.position.x, lightForSelfShadowing.transform.position.y, lightForSelfShadowing.transform.position.z, 1f));
				}
			}
		}
		else if (lightForSelfShadowing.type == global::UnityEngine.LightType.Directional)
		{
			global::UnityEngine.Shader.SetGlobalVector("_WorldSpaceLightPosCustom", -lightForSelfShadowing.transform.forward);
		}
		else
		{
			global::UnityEngine.Shader.SetGlobalVector("_WorldSpaceLightPosCustom", new global::UnityEngine.Vector4(lightForSelfShadowing.transform.position.x, lightForSelfShadowing.transform.position.y, lightForSelfShadowing.transform.position.z, 1f));
		}
	}
}
