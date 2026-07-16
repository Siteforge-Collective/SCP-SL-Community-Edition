[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/Weather/Rain_Pro_3D")]
public class CameraFilterPack_Atmosphere_Rain_Pro_3D : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	public bool _Visualize;

	private float TimeX = 1f;

	private global::UnityEngine.Material SCMaterial;

	[global::UnityEngine.Range(0f, 100f)]
	public float _FixDistance = 3f;

	[global::UnityEngine.Range(0f, 1f)]
	public float Fade = 1f;

	[global::UnityEngine.Range(0f, 2f)]
	public float Intensity = 0.5f;

	public bool DirectionFollowCameraZ;

	[global::UnityEngine.Range(-0.45f, 0.45f)]
	public float DirectionX = 0.12f;

	[global::UnityEngine.Range(0.4f, 2f)]
	public float Size = 1.5f;

	[global::UnityEngine.Range(0f, 0.5f)]
	public float Speed = 0.275f;

	[global::UnityEngine.Range(0f, 0.5f)]
	public float Distortion = 0.025f;

	[global::UnityEngine.Range(0f, 1f)]
	public float StormFlashOnOff = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float DropOnOff = 1f;

	[global::UnityEngine.Range(-0.5f, 0.99f)]
	public float Drop_Near;

	[global::UnityEngine.Range(0f, 1f)]
	public float Drop_Far = 0.5f;

	[global::UnityEngine.Range(0f, 1f)]
	public float Drop_With_Obj = 0.2f;

	[global::UnityEngine.Range(0f, 1f)]
	public float Myst = 0.1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float Drop_Floor_Fluid;

	public global::UnityEngine.Color Myst_Color = new global::UnityEngine.Color(0.5f, 0.5f, 0.5f, 1f);

	private global::UnityEngine.Texture2D Texture2;

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
		Texture2 = global::UnityEngine.Resources.Load("CameraFilterPack_Atmosphere_Rain_FX") as global::UnityEngine.Texture2D;
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/Atmosphere_Rain_Pro_3D");
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
			material.SetFloat("_Value", Fade);
			material.SetFloat("_Value2", Intensity);
			if (DirectionFollowCameraZ)
			{
				float z = GetComponent<global::UnityEngine.Camera>().transform.rotation.z;
				if (z > 0f && z < 360f)
				{
					material.SetFloat("_Value3", z);
				}
				if (z < 0f)
				{
					material.SetFloat("_Value3", z);
				}
			}
			else
			{
				material.SetFloat("_Value3", DirectionX);
			}
			material.SetFloat("_Value4", Speed);
			material.SetFloat("_Value5", Size);
			material.SetFloat("_Value6", Distortion);
			material.SetFloat("_Value7", StormFlashOnOff);
			material.SetFloat("_Value8", DropOnOff);
			material.SetFloat("_FixDistance", _FixDistance);
			material.SetFloat("_Visualize", _Visualize ? 1 : 0);
			material.SetFloat("Drop_Near", Drop_Near);
			material.SetFloat("Drop_Far", Drop_Far);
			material.SetFloat("Drop_With_Obj", 1f - Drop_With_Obj);
			material.SetFloat("Myst", Myst);
			material.SetColor("Myst_Color", Myst_Color);
			material.SetFloat("Drop_Floor_Fluid", Drop_Floor_Fluid);
			material.SetVector("_ScreenResolution", new global::UnityEngine.Vector4(sourceTexture.width, sourceTexture.height, 0f, 0f));
			material.SetTexture("Texture2", Texture2);
			GetComponent<global::UnityEngine.Camera>().depthTextureMode = global::UnityEngine.DepthTextureMode.Depth;
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
