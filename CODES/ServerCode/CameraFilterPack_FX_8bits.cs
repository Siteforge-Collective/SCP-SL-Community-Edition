[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/Pixel/8bits")]
public class CameraFilterPack_FX_8bits : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	private float TimeX = 1f;

	private global::UnityEngine.Material SCMaterial;

	[global::UnityEngine.Range(-1f, 1f)]
	public float Brightness;

	[global::UnityEngine.Range(80f, 640f)]
	public int ResolutionX = 160;

	[global::UnityEngine.Range(60f, 480f)]
	public int ResolutionY = 240;

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
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/FX_8bits");
		if (!global::UnityEngine.SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
		}
	}

	private void OnRenderImage(global::UnityEngine.RenderTexture sourceTexture, global::UnityEngine.RenderTexture destTexture)
	{
		if (SCShader != null)
		{
			TimeX += global::UnityEngine.Time.deltaTime;
			if (TimeX > 100f)
			{
				TimeX = 0f;
			}
			material.SetFloat("_TimeX", TimeX);
			if (Brightness == 0f)
			{
				Brightness = 0.001f;
			}
			material.SetFloat("_Distortion", Brightness);
			global::UnityEngine.RenderTexture temporary = global::UnityEngine.RenderTexture.GetTemporary(ResolutionX, ResolutionY, 0);
			global::UnityEngine.Graphics.Blit(sourceTexture, temporary, material);
			temporary.filterMode = global::UnityEngine.FilterMode.Point;
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
