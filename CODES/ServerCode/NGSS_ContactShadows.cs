[global::UnityEngine.ImageEffectAllowedInSceneView]
[global::UnityEngine.ExecuteInEditMode]
public class NGSS_ContactShadows : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Light mainDirectionalLight;

	public global::UnityEngine.Shader contactShadowsShader;

	public bool noiseFilter;

	[global::UnityEngine.Range(0f, 3f)]
	public float shadowsSoftness = 1f;

	[global::UnityEngine.Range(1f, 4f)]
	public float shadowsDistance = 2f;

	[global::UnityEngine.Range(0.1f, 4f)]
	public float shadowsFade = 1f;

	[global::UnityEngine.Range(0f, 0.02f)]
	public float shadowsBias = 0.0065f;

	[global::UnityEngine.Range(0f, 1f)]
	public float rayWidth = 0.1f;

	[global::UnityEngine.Range(16f, 128f)]
	public int raySamples = 64;

	private global::UnityEngine.Rendering.CommandBuffer blendShadowsCB;

	private global::UnityEngine.Rendering.CommandBuffer computeShadowsCB;

	private bool isInitialized;

	private global::UnityEngine.Camera _mCamera;

	private global::UnityEngine.Material _mMaterial;

	private global::UnityEngine.Camera mCamera
	{
		get
		{
			if (_mCamera == null)
			{
				_mCamera = GetComponent<global::UnityEngine.Camera>();
				if (_mCamera == null)
				{
					_mCamera = global::UnityEngine.Camera.main;
				}
				if (_mCamera == null)
				{
					global::UnityEngine.Debug.LogError("NGSS Error: No MainCamera found, please provide one.", this);
				}
				else
				{
					_mCamera.depthTextureMode |= global::UnityEngine.DepthTextureMode.Depth;
				}
			}
			return _mCamera;
		}
	}

	private global::UnityEngine.Material mMaterial
	{
		get
		{
			if (_mMaterial == null)
			{
				if (contactShadowsShader == null)
				{
					global::UnityEngine.Shader.Find("Hidden/NGSS_ContactShadows");
				}
				_mMaterial = new global::UnityEngine.Material(contactShadowsShader);
				if (_mMaterial == null)
				{
					global::UnityEngine.Debug.LogWarning("NGSS Warning: can't find NGSS_ContactShadows shader, make sure it's on your project.", this);
					base.enabled = false;
					return null;
				}
			}
			return _mMaterial;
		}
	}

	private void AddCommandBuffers()
	{
		computeShadowsCB = new global::UnityEngine.Rendering.CommandBuffer
		{
			name = "NGSS ContactShadows: Compute"
		};
		blendShadowsCB = new global::UnityEngine.Rendering.CommandBuffer
		{
			name = "NGSS ContactShadows: Mix"
		};
		bool flag = mCamera.actualRenderingPath == global::UnityEngine.RenderingPath.Forward;
		global::UnityEngine.Rendering.CommandBuffer[] commandBuffers;
		if ((bool)mCamera)
		{
			commandBuffers = mCamera.GetCommandBuffers(flag ? global::UnityEngine.Rendering.CameraEvent.AfterDepthTexture : global::UnityEngine.Rendering.CameraEvent.BeforeLighting);
			for (int i = 0; i < commandBuffers.Length; i++)
			{
				if (commandBuffers[i].name == computeShadowsCB.name)
				{
					return;
				}
			}
			mCamera.AddCommandBuffer(flag ? global::UnityEngine.Rendering.CameraEvent.AfterDepthTexture : global::UnityEngine.Rendering.CameraEvent.BeforeLighting, computeShadowsCB);
		}
		if (!mainDirectionalLight)
		{
			return;
		}
		commandBuffers = mainDirectionalLight.GetCommandBuffers(global::UnityEngine.Rendering.LightEvent.AfterScreenspaceMask);
		for (int i = 0; i < commandBuffers.Length; i++)
		{
			if (commandBuffers[i].name == blendShadowsCB.name)
			{
				return;
			}
		}
		mainDirectionalLight.AddCommandBuffer(global::UnityEngine.Rendering.LightEvent.AfterScreenspaceMask, blendShadowsCB);
	}

	private void RemoveCommandBuffers()
	{
		_mMaterial = null;
		bool flag = mCamera.actualRenderingPath == global::UnityEngine.RenderingPath.Forward;
		if ((bool)mCamera)
		{
			mCamera.RemoveCommandBuffer(flag ? global::UnityEngine.Rendering.CameraEvent.AfterDepthTexture : global::UnityEngine.Rendering.CameraEvent.BeforeLighting, computeShadowsCB);
		}
		if ((bool)mainDirectionalLight)
		{
			mainDirectionalLight.RemoveCommandBuffer(global::UnityEngine.Rendering.LightEvent.AfterScreenspaceMask, blendShadowsCB);
		}
		isInitialized = false;
	}

	private void Init()
	{
		if (!isInitialized && !(mainDirectionalLight == null))
		{
			if (mCamera.renderingPath == global::UnityEngine.RenderingPath.UsePlayerSettings || mCamera.renderingPath == global::UnityEngine.RenderingPath.VertexLit)
			{
				global::UnityEngine.Debug.LogWarning("Please set your camera rendering path to either Forward or Deferred and re-enable this component.", this);
				base.enabled = false;
				return;
			}
			AddCommandBuffers();
			int num = global::UnityEngine.Shader.PropertyToID("NGSS_ContactShadowRT");
			int num2 = global::UnityEngine.Shader.PropertyToID("NGSS_DepthSourceRT");
			computeShadowsCB.GetTemporaryRT(num, -1, -1, 0, global::UnityEngine.FilterMode.Bilinear, global::UnityEngine.RenderTextureFormat.R8);
			computeShadowsCB.GetTemporaryRT(num2, -1, -1, 0, global::UnityEngine.FilterMode.Point, global::UnityEngine.RenderTextureFormat.RFloat);
			computeShadowsCB.Blit(num, num2, mMaterial, 0);
			computeShadowsCB.Blit(num2, num, mMaterial, 1);
			computeShadowsCB.Blit(num, num2, mMaterial, 2);
			blendShadowsCB.Blit(global::UnityEngine.Rendering.BuiltinRenderTextureType.None, global::UnityEngine.Rendering.BuiltinRenderTextureType.CurrentActive, mMaterial, 3);
			computeShadowsCB.SetGlobalTexture("NGSS_ContactShadowsTexture", num2);
			isInitialized = true;
		}
	}

	private void OnEnable()
	{
		Init();
	}

	private void OnDisable()
	{
		if (isInitialized)
		{
			RemoveCommandBuffers();
		}
	}

	private void OnApplicationQuit()
	{
		if (isInitialized)
		{
			RemoveCommandBuffers();
		}
	}

	private void OnPreRender()
	{
		Init();
		if (isInitialized && !(mainDirectionalLight == null))
		{
			mMaterial.SetVector("LightDir", mCamera.transform.InverseTransformDirection(mainDirectionalLight.transform.forward));
			mMaterial.SetFloat("ShadowsSoftness", shadowsSoftness);
			mMaterial.SetFloat("ShadowsDistance", shadowsDistance);
			mMaterial.SetFloat("ShadowsFade", shadowsFade);
			mMaterial.SetFloat("ShadowsBias", shadowsBias);
			mMaterial.SetFloat("RayWidth", rayWidth);
			mMaterial.SetInt("RaySamples", raySamples);
			if (noiseFilter)
			{
				mMaterial.EnableKeyword("NGSS_CONTACT_SHADOWS_USE_NOISE");
			}
			else
			{
				mMaterial.DisableKeyword("NGSS_CONTACT_SHADOWS_USE_NOISE");
			}
		}
	}
}
