[global::UnityEngine.ExecuteInEditMode]
[global::UnityEngine.AddComponentMenu("Camera Filter Pack/Weather/New Rain FX")]
public class CameraFilterPack_Rain_RainFX : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Shader SCShader;

	private float TimeX = 1f;

	private global::UnityEngine.Material SCMaterial;

	[global::UnityEngine.Range(-8f, 8f)]
	public float Speed = 1f;

	[global::UnityEngine.Range(0f, 1f)]
	public float Fade = 1f;

	[global::UnityEngine.HideInInspector]
	public int Count;

	private readonly global::UnityEngine.Vector4[] Coord = new global::UnityEngine.Vector4[4];

	public static global::UnityEngine.Color ChangeColorRGB;

	private global::UnityEngine.Texture2D Texture2;

	private global::UnityEngine.Texture2D Texture3;

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
		Texture2 = global::UnityEngine.Resources.Load("CameraFilterPack_RainFX_Anm2") as global::UnityEngine.Texture2D;
		Texture3 = global::UnityEngine.Resources.Load("CameraFilterPack_RainFX_Anm") as global::UnityEngine.Texture2D;
		SCShader = global::UnityEngine.Shader.Find("CameraFilterPack/RainFX");
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
			material.SetFloat("_Speed", Speed);
			material.SetVector("_ScreenResolution", new global::UnityEngine.Vector4(sourceTexture.width, sourceTexture.height, 0f, 0f));
			GetComponent<global::UnityEngine.Camera>().depthTextureMode = global::UnityEngine.DepthTextureMode.Depth;
			global::UnityEngine.AnimationCurve animationCurve = new global::UnityEngine.AnimationCurve();
			animationCurve = new global::UnityEngine.AnimationCurve();
			animationCurve.AddKey(0f, 0.01f);
			animationCurve.AddKey(64f, 5f);
			animationCurve.AddKey(128f, 80f);
			animationCurve.AddKey(255f, 255f);
			animationCurve.AddKey(300f, 255f);
			for (int i = 0; i < 4; i++)
			{
				Coord[i].z += 0.5f;
				if (Coord[i].w == -1f)
				{
					Coord[i].x = -5f;
				}
				if (Coord[i].z > 254f)
				{
					Coord[i] = new global::UnityEngine.Vector4(global::UnityEngine.Random.Range(0f, 0.9f), global::UnityEngine.Random.Range(0.2f, 1.1f), 0f, global::UnityEngine.Random.Range(0, 3));
				}
				material.SetVector("Coord" + (i + 1), new global::UnityEngine.Vector4(Coord[i].x, Coord[i].y, (int)animationCurve.Evaluate(Coord[i].z), Coord[i].w));
			}
			material.SetTexture("Texture2", Texture2);
			material.SetTexture("Texture3", Texture3);
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
