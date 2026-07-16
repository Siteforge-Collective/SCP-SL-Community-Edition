[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/AAA/Super Computer")]
public class CameraFilterPack_AAA_SuperComputer : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	[global::UnityEngine.Range(0f, 1f)]
	public float _AlphaHexa = 1f;

	private float TimeX = 1f;

	private global::UnityEngine.Material SCMaterial;

	[global::UnityEngine.Range(-20f, 20f)]
	public float ShapeFormula = 10f;

	[global::UnityEngine.Range(0f, 6f)]
	public float Shape = 1f;

	[global::UnityEngine.Range(-4f, 4f)]
	public float _BorderSize = 1f;

	public global::UnityEngine.Color _BorderColor = new global::UnityEngine.Color(0f, 0.2f, 1f, 1f);

	public float _SpotSize = 2.5f;

	public global::UnityEngine.Vector2 center = new global::UnityEngine.Vector2(0f, 0f);

	public float Radius = 0.77f;

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
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/AAA_Super_Computer");
		if (!global::UnityEngine.SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
		}
	}

	private void OnRenderImage(global::UnityEngine.RenderTexture sourceTexture, global::UnityEngine.RenderTexture destTexture)
	{
		if (SCShader != null)
		{
			TimeX += global::UnityEngine.Time.deltaTime / 4f;
			if (TimeX > 100f)
			{
				TimeX = 0f;
			}
			material.SetFloat("_TimeX", TimeX);
			material.SetFloat("_Value", ShapeFormula);
			material.SetFloat("_Value2", Shape);
			material.SetFloat("_PositionX", center.x);
			material.SetFloat("_PositionY", center.y);
			material.SetFloat("_Radius", Radius);
			material.SetFloat("_BorderSize", _BorderSize);
			material.SetColor("_BorderColor", _BorderColor);
			material.SetFloat("_AlphaHexa", _AlphaHexa);
			material.SetFloat("_SpotSize", _SpotSize);
			material.SetVector("_ScreenResolution", new global::UnityEngine.Vector4(sourceTexture.width, sourceTexture.height, 0f, 0f));
			global::UnityEngine.Graphics.Blit(sourceTexture, destTexture, material);
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
