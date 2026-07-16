public class ScopeShaderReplacement : global::UnityEngine.MonoBehaviour
{
	private static global::UnityEngine.Shader _replacementShader;

	private void Start()
	{
		if (_replacementShader == null)
		{
			_replacementShader = global::UnityEngine.Shader.Find("Hidden/UnlitShaderReplacement");
		}
		GetComponent<global::UnityEngine.Camera>().SetReplacementShader(_replacementShader, "RenderType");
	}
}
