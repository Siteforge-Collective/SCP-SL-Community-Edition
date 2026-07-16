[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/VHS/Real VHS HQ")]
public class CameraFilterPack_Real_VHS : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	private global::UnityEngine.Material SCMaterial;

	private global::UnityEngine.Texture2D VHS;

	private global::UnityEngine.Texture2D VHS2;

	[global::UnityEngine.Range(0f, 1f)]
	public float TRACKING = 0.212f;

	[global::UnityEngine.Range(0f, 1f)]
	public float JITTER = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float GLITCH = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float NOISE = 1f;

	[global::UnityEngine.Range(-1f, 1f)]
	public float Brightness;

	[global::UnityEngine.Range(0f, 1.5f)]
	public float Constrast = 1f;

	private global::UnityEngine.Material material
	{
		get
		{
			if (SCMaterial == null)
			{
				SCMaterial = new global::UnityEngine.Material(SCShader);
				SCMaterial.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
			}
			return SCMaterial;
		}
	}

	private void Start()
	{
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/Real_VHS");
		VHS = global::UnityEngine.Resources.Load("CameraFilterPack_VHS1") as global::UnityEngine.Texture2D;
		VHS2 = global::UnityEngine.Resources.Load("CameraFilterPack_VHS2") as global::UnityEngine.Texture2D;
		if (!global::UnityEngine.SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
		}
	}

	public static global::UnityEngine.Texture2D GetRTPixels(global::UnityEngine.Texture2D t, global::UnityEngine.RenderTexture rt, int sx, int sy)
	{
		global::UnityEngine.RenderTexture active = global::UnityEngine.RenderTexture.active;
		global::UnityEngine.RenderTexture.active = rt;
		t.ReadPixels(new global::UnityEngine.Rect(0f, 0f, t.width, t.height), 0, 0);
		global::UnityEngine.RenderTexture.active = active;
		return t;
	}

	private void OnRenderImage(global::UnityEngine.RenderTexture sourceTexture, global::UnityEngine.RenderTexture destTexture)
	{
		if (SCShader != null)
		{
			material.SetTexture("VHS", VHS);
			material.SetTexture("VHS2", VHS2);
			material.SetFloat("TRACKING", TRACKING);
			material.SetFloat("JITTER", JITTER);
			material.SetFloat("GLITCH", GLITCH);
			material.SetFloat("NOISE", NOISE);
			material.SetFloat("Brightness", Brightness);
			material.SetFloat("CONTRAST", 1f - Constrast);
			int height = 576;
			global::UnityEngine.RenderTexture temporary = global::UnityEngine.RenderTexture.GetTemporary(382, height, 0);
			temporary.filterMode = global::UnityEngine.FilterMode.Trilinear;
			global::UnityEngine.Graphics.Blit(sourceTexture, temporary, material);
			global::UnityEngine.Graphics.Blit(temporary, destTexture);
			global::UnityEngine.RenderTexture.ReleaseTemporary(temporary);
		}
		else
		{
			global::UnityEngine.Graphics.Blit(sourceTexture, destTexture);
		}
	}

	private void Update()
	{
	}

	private void OnDisable()
	{
		if ((bool)SCMaterial)
		{
			global::UnityEngine.Object.DestroyImmediate(SCMaterial);
		}
	}
}
