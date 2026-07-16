namespace Knife.Tools
{
	public static class GPURaycast
	{
		private static global::UnityEngine.RenderTexture worldPosBuffer;

		private static global::UnityEngine.RenderTexture worldNormBuffer;

		private static global::UnityEngine.RenderTexture idBuffer;

		private static global::UnityEngine.RenderBuffer[] mrtBuffers;

		private static global::UnityEngine.Camera gpuRaycastCamera;

		private static global::UnityEngine.Plane[] cameraPlanes;

		private static global::UnityEngine.Renderer[] allSceneRenderers;

		private static global::UnityEngine.Renderer[] visibleRenderers;

		private static global::System.Collections.Generic.Dictionary<global::UnityEngine.Renderer, global::Knife.Tools.DecalsTarget> targets = new global::System.Collections.Generic.Dictionary<global::UnityEngine.Renderer, global::Knife.Tools.DecalsTarget>();

		private static global::System.Collections.Generic.List<global::UnityEngine.Renderer> allRegisteredRenderers = new global::System.Collections.Generic.List<global::UnityEngine.Renderer>();

		private static global::System.Collections.Generic.List<global::UnityEngine.Renderer> visibleRegisteredRenderers = new global::System.Collections.Generic.List<global::UnityEngine.Renderer>();

		private static global::UnityEngine.MaterialPropertyBlock propertyBlock;

		private static global::UnityEngine.Texture2D posBufferCopy;

		private static global::UnityEngine.Texture2D normBufferCopy;

		private static global::UnityEngine.Texture2D idBufferCopy;

		private static int lastPixelWidth;

		private static int lastPixelHeight;

		private static global::UnityEngine.Camera lastCamera;

		private static global::UnityEngine.Shader gpuRaycastShader;

		private static global::UnityEngine.Shader gpuPackedRaycastShader;

		private static global::UnityEngine.ComputeShader gpuRaycastCopyDataShader;

		private static global::UnityEngine.Color positionRead;

		private static global::UnityEngine.Color normalRead;

		private static global::UnityEngine.Color idRead;

		private static global::UnityEngine.Vector4 _ZBufferParams = default(global::UnityEngine.Vector4);

		private static int LastPixelWidth
		{
			get
			{
				return lastPixelWidth;
			}
			set
			{
				lastPixelWidth = value;
			}
		}

		private static int LastPixelHeight
		{
			get
			{
				return lastPixelHeight;
			}
			set
			{
				lastPixelHeight = value;
			}
		}

		public static bool Raycast(global::UnityEngine.Camera camera, global::UnityEngine.Vector2 pixelPosition, bool renderersHitInfo, out global::Knife.Tools.GPURaycastInfo hitInfo)
		{
			InitRes();
			if (renderersHitInfo)
			{
				return Raycast(camera, pixelPosition, out hitInfo);
			}
			return RaycastWithoutRenderer(camera, pixelPosition, out hitInfo);
		}

		private static void InitRes()
		{
			if (posBufferCopy == null)
			{
				posBufferCopy = new global::UnityEngine.Texture2D(1, 1, global::UnityEngine.TextureFormat.RGBAFloat, mipChain: false);
			}
			if (normBufferCopy == null)
			{
				normBufferCopy = new global::UnityEngine.Texture2D(1, 1, global::UnityEngine.TextureFormat.RGBAFloat, mipChain: false);
			}
			if (idBufferCopy == null)
			{
				idBufferCopy = new global::UnityEngine.Texture2D(1, 1, global::UnityEngine.TextureFormat.RGBAFloat, mipChain: false);
			}
			if (propertyBlock == null)
			{
				propertyBlock = new global::UnityEngine.MaterialPropertyBlock();
			}
			if (gpuRaycastShader == null)
			{
				gpuRaycastShader = global::UnityEngine.Resources.Load<global::UnityEngine.Shader>("Knife/GPURaycast/GPURaycastInfoShader");
			}
			if (gpuPackedRaycastShader == null)
			{
				gpuPackedRaycastShader = global::UnityEngine.Resources.Load<global::UnityEngine.Shader>("Knife/GPURaycast/GPURaycastPackedInfoShader");
			}
			if (gpuRaycastCopyDataShader == null)
			{
				gpuRaycastCopyDataShader = global::UnityEngine.Resources.Load<global::UnityEngine.ComputeShader>("Knife/GPURaycast/RenderTextureToBuffer");
			}
		}

		public static void AddDecalsTarget(global::Knife.Tools.DecalsTarget target)
		{
			if (target == null)
			{
				throw new global::System.ArgumentNullException("Decals Target cannot be null");
			}
			if (targets.ContainsValue(target))
			{
				RemoveDecalsTarget(target);
			}
			for (int i = 0; i < target.Renderers.Length; i++)
			{
				targets.Add(target.Renderers[i], target);
				allRegisteredRenderers.Remove(target.Renderers[i]);
			}
			allRegisteredRenderers.AddRange(target.Renderers);
		}

		public static void RemoveDecalsTarget(global::Knife.Tools.DecalsTarget target)
		{
			if (target == null)
			{
				throw new global::System.ArgumentNullException("Decals Target cannot be null");
			}
			for (int i = 0; i < target.Renderers.Length; i++)
			{
				targets.Remove(target.Renderers[i]);
				allRegisteredRenderers.Remove(target.Renderers[i]);
			}
		}

		private static float LinearEyeDepth(float z)
		{
			return 1f / (_ZBufferParams.z * z + _ZBufferParams.w);
		}

		private static float Unlinear01Depth(float linear01depth)
		{
			return (1f / linear01depth - _ZBufferParams.w) / _ZBufferParams.z;
		}

		private static void PackZBufferParams(global::UnityEngine.Camera camera)
		{
			_ZBufferParams.x = 1f - camera.farClipPlane / camera.nearClipPlane;
			_ZBufferParams.y = camera.farClipPlane / camera.nearClipPlane;
			_ZBufferParams.z = _ZBufferParams.x / camera.farClipPlane;
			_ZBufferParams.w = _ZBufferParams.y / camera.farClipPlane;
		}

		private static global::UnityEngine.Color UnpackData(float packedData)
		{
			global::UnityEngine.Color clear = global::UnityEngine.Color.clear;
			clear.r = global::UnityEngine.Mathf.Floor(packedData / 1000000f);
			clear.r *= 1000000f;
			clear.g = global::UnityEngine.Mathf.Floor((packedData - clear.r) / 1000f);
			clear.b = global::UnityEngine.Mathf.Floor(packedData - clear.r - clear.g * 1000f) / 256f;
			clear.r /= 256000000f;
			clear.g /= 256f;
			return clear;
		}

		private static void SetupHitInfo(out global::Knife.Tools.GPURaycastDecalsTargetInfo hitInfo)
		{
			hitInfo.position = global::UnityEngine.Vector3.zero;
			hitInfo.normal = global::UnityEngine.Vector3.zero;
			hitInfo.hittedRenderer = null;
			hitInfo.IsHitted = false;
			hitInfo.hittedTarget = null;
			hitInfo.VertexIndex = -1;
		}

		public static bool RaycastToRegisteredTargets(global::UnityEngine.Camera camera, float pixelX, float pixelY, out global::Knife.Tools.GPURaycastDecalsTargetInfo hitInfo)
		{
			global::UnityEngine.Vector2 zero = global::UnityEngine.Vector2.zero;
			zero.x = pixelX / (float)camera.pixelWidth;
			zero.y = pixelY / (float)camera.pixelHeight;
			return RaycastToRegisteredTargets(camera, zero, out hitInfo);
		}

		public static bool RaycastToRegisteredTargetsFromMousePosition(global::UnityEngine.Camera camera, out global::Knife.Tools.GPURaycastDecalsTargetInfo hitInfo)
		{
			return RaycastToRegisteredTargets(camera, global::UnityEngine.Input.mousePosition.x, global::UnityEngine.Input.mousePosition.y, out hitInfo);
		}

		public static bool RaycastToRegisteredTargets(global::UnityEngine.Camera camera, global::UnityEngine.Vector2 uv, out global::Knife.Tools.GPURaycastDecalsTargetInfo hitInfo)
		{
			InitRes();
			uv.y = 1f - uv.y;
			SetupHitInfo(out hitInfo);
			int num = ((camera.targetTexture == null) ? camera.pixelWidth : camera.targetTexture.width);
			int num2 = ((camera.targetTexture == null) ? camera.pixelHeight : camera.targetTexture.height);
			float num3 = 0.2f;
			num = (int)((float)num * num3);
			num2 = (int)((float)num2 * num3);
			if (lastCamera != camera || gpuRaycastCamera == null)
			{
				gpuRaycastCamera = new global::UnityEngine.GameObject("GPURaycastCamera", typeof(global::UnityEngine.Camera)).GetComponent<global::UnityEngine.Camera>();
				gpuRaycastCamera.gameObject.SetActive(value: false);
				gpuRaycastCamera.enabled = false;
				gpuRaycastCamera.gameObject.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
			}
			gpuRaycastCamera.cameraType = global::UnityEngine.CameraType.Game;
			gpuRaycastCamera.targetTexture = null;
			gpuRaycastCamera.transform.position = camera.transform.position;
			gpuRaycastCamera.transform.rotation = camera.transform.rotation;
			gpuRaycastCamera.clearFlags = global::UnityEngine.CameraClearFlags.Color;
			gpuRaycastCamera.backgroundColor = global::UnityEngine.Color.clear;
			gpuRaycastCamera.renderingPath = global::UnityEngine.RenderingPath.Forward;
			gpuRaycastCamera.rect = camera.rect;
			lastCamera = camera;
			cameraPlanes = global::UnityEngine.GeometryUtility.CalculateFrustumPlanes(gpuRaycastCamera);
			visibleRegisteredRenderers.Clear();
			visibleRegisteredRenderers.AddRange(allRegisteredRenderers.FindAll((global::UnityEngine.Renderer r) => global::UnityEngine.GeometryUtility.TestPlanesAABB(cameraPlanes, r.bounds)));
			for (int num4 = 0; num4 < visibleRegisteredRenderers.Count; num4++)
			{
				visibleRegisteredRenderers[num4].GetPropertyBlock(propertyBlock);
				propertyBlock.SetInt("ObjectID", num4 + 1);
				visibleRegisteredRenderers[num4].SetPropertyBlock(propertyBlock);
			}
			if (LastPixelWidth != num || LastPixelHeight != num2 || worldPosBuffer == null)
			{
				if (worldPosBuffer != null)
				{
					worldPosBuffer.Release();
					global::UnityEngine.Object.DestroyImmediate(worldPosBuffer, allowDestroyingAssets: true);
				}
				worldPosBuffer = new global::UnityEngine.RenderTexture(num, num2, 16, global::UnityEngine.RenderTextureFormat.ARGBFloat);
				worldPosBuffer.Create();
				LastPixelWidth = num;
				LastPixelHeight = num2;
			}
			gpuRaycastCamera.targetTexture = worldPosBuffer;
			gpuRaycastCamera.RenderWithShader(gpuPackedRaycastShader, "");
			if (uv.x < 0f || uv.x > 1f || uv.y < 0f || uv.y > 1f)
			{
				return false;
			}
			int num5 = (int)(uv.x * (float)num);
			int num6 = (int)(uv.y * (float)num2);
			global::UnityEngine.RenderTexture active = global::UnityEngine.RenderTexture.active;
			global::UnityEngine.RenderTexture.active = worldPosBuffer;
			posBufferCopy.ReadPixels(new global::UnityEngine.Rect(num5, num6, 1f, 1f), 0, 0);
			global::UnityEngine.RenderTexture.active = active;
			posBufferCopy.Apply();
			global::UnityEngine.Color pixel = posBufferCopy.GetPixel(0, 0);
			global::UnityEngine.Color clear = global::UnityEngine.Color.clear;
			clear.r = uv.x;
			clear.g = 1f - uv.y;
			clear.b = pixel.g;
			global::UnityEngine.Vector2 vector = new global::UnityEngine.Vector2(clear.r, clear.g);
			float b = clear.b;
			global::UnityEngine.Matrix4x4 projectionMatrix = camera.projectionMatrix;
			global::UnityEngine.Matrix4x4 cameraToWorldMatrix = camera.cameraToWorldMatrix;
			global::UnityEngine.Vector2 vector2 = new global::UnityEngine.Vector2(projectionMatrix.m00, projectionMatrix.m11);
			global::UnityEngine.Vector2 vector3 = new global::UnityEngine.Vector2(projectionMatrix.m02, projectionMatrix.m12);
			global::UnityEngine.Vector2 vector4 = vector * 2f - global::UnityEngine.Vector2.one - vector3;
			vector4.x /= vector2.x;
			vector4.y /= vector2.y;
			global::UnityEngine.Vector3 vector5 = new global::UnityEngine.Vector3(vector4.x, vector4.y, 1f) * b;
			vector5.z *= -1f;
			global::UnityEngine.Vector3 position = cameraToWorldMatrix.MultiplyVector(vector5) + camera.transform.position;
			normalRead = UnpackData(pixel.b) * 2f - global::UnityEngine.Color.white;
			global::UnityEngine.Vector3 normal = new global::UnityEngine.Vector3(normalRead.r, normalRead.g, normalRead.b);
			normal.Normalize();
			int num7 = global::UnityEngine.Mathf.RoundToInt(pixel.a) - 1 - 1;
			hitInfo.position = position;
			hitInfo.normal = normal;
			if (num7 >= 0 && num7 < visibleRegisteredRenderers.Count)
			{
				hitInfo.hittedRenderer = visibleRegisteredRenderers[num7];
				hitInfo.hittedTarget = targets[hitInfo.hittedRenderer];
			}
			hitInfo.VertexIndex = (int)pixel.r;
			hitInfo.IsHitted = pixel.a > 1f;
			return hitInfo.IsHitted;
		}

		private static bool Raycast(global::UnityEngine.Camera camera, global::UnityEngine.Vector2 pixelPosition, out global::Knife.Tools.GPURaycastInfo hitInfo)
		{
			hitInfo.position = global::UnityEngine.Vector3.zero;
			hitInfo.normal = global::UnityEngine.Vector3.zero;
			hitInfo.hittedRenderer = null;
			hitInfo.IsHitted = false;
			int num = ((camera.targetTexture == null) ? camera.pixelWidth : camera.targetTexture.width);
			int num2 = ((camera.targetTexture == null) ? camera.pixelHeight : camera.targetTexture.height);
			if (lastCamera != camera || gpuRaycastCamera == null)
			{
				gpuRaycastCamera = new global::UnityEngine.GameObject("GPURaycastCamera", typeof(global::UnityEngine.Camera)).GetComponent<global::UnityEngine.Camera>();
				gpuRaycastCamera.gameObject.SetActive(value: false);
				gpuRaycastCamera.enabled = false;
				gpuRaycastCamera.gameObject.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
			}
			gpuRaycastCamera.cameraType = global::UnityEngine.CameraType.Game;
			gpuRaycastCamera.targetTexture = null;
			gpuRaycastCamera.transform.position = camera.transform.position;
			gpuRaycastCamera.transform.rotation = camera.transform.rotation;
			gpuRaycastCamera.clearFlags = global::UnityEngine.CameraClearFlags.Color;
			gpuRaycastCamera.backgroundColor = global::UnityEngine.Color.clear;
			gpuRaycastCamera.renderingPath = global::UnityEngine.RenderingPath.Forward;
			gpuRaycastCamera.rect = camera.rect;
			lastCamera = camera;
			cameraPlanes = global::UnityEngine.GeometryUtility.CalculateFrustumPlanes(gpuRaycastCamera);
			allSceneRenderers = global::UnityEngine.Object.FindObjectsOfType<global::UnityEngine.Renderer>();
			visibleRenderers = global::System.Linq.Enumerable.ToList(allSceneRenderers).FindAll((global::UnityEngine.Renderer r) => global::UnityEngine.GeometryUtility.TestPlanesAABB(cameraPlanes, r.bounds)).ToArray();
			for (int num3 = 0; num3 < visibleRenderers.Length; num3++)
			{
				visibleRenderers[num3].GetPropertyBlock(propertyBlock);
				propertyBlock.SetInt("ObjectID", num3);
				visibleRenderers[num3].SetPropertyBlock(propertyBlock);
			}
			if (LastPixelWidth != num || LastPixelHeight != num2 || worldPosBuffer == null || worldNormBuffer == null || idBuffer == null)
			{
				if (worldPosBuffer != null)
				{
					worldPosBuffer.Release();
					global::UnityEngine.Object.DestroyImmediate(worldPosBuffer, allowDestroyingAssets: true);
				}
				if (worldNormBuffer != null)
				{
					worldNormBuffer.Release();
					global::UnityEngine.Object.DestroyImmediate(worldNormBuffer, allowDestroyingAssets: true);
				}
				if (idBuffer != null)
				{
					idBuffer.Release();
					global::UnityEngine.Object.DestroyImmediate(idBuffer, allowDestroyingAssets: true);
				}
				worldPosBuffer = new global::UnityEngine.RenderTexture(num, num2, 16, global::UnityEngine.RenderTextureFormat.ARGBFloat);
				worldNormBuffer = new global::UnityEngine.RenderTexture(num, num2, 0, global::UnityEngine.RenderTextureFormat.ARGBFloat);
				idBuffer = new global::UnityEngine.RenderTexture(num, num2, 0, global::UnityEngine.RenderTextureFormat.ARGBFloat);
				worldPosBuffer.Create();
				worldNormBuffer.Create();
				idBuffer.Create();
				mrtBuffers = new global::UnityEngine.RenderBuffer[3];
				mrtBuffers[0] = worldPosBuffer.colorBuffer;
				mrtBuffers[1] = worldNormBuffer.colorBuffer;
				mrtBuffers[2] = idBuffer.colorBuffer;
				LastPixelWidth = num;
				LastPixelHeight = num2;
				gpuRaycastCamera.SetTargetBuffers(mrtBuffers, worldPosBuffer.depthBuffer);
			}
			gpuRaycastCamera.SetTargetBuffers(mrtBuffers, worldPosBuffer.depthBuffer);
			gpuRaycastCamera.RenderWithShader(gpuRaycastShader, "");
			global::UnityEngine.Vector2 vector = new global::UnityEngine.Vector2(pixelPosition.x / (float)camera.pixelWidth, pixelPosition.y / (float)camera.pixelHeight);
			if (vector.x < 0f || vector.x > 1f || vector.y < 0f || vector.y > 1f)
			{
				return false;
			}
			int num4 = (int)(vector.x * (float)num);
			int num5 = (int)(vector.y * (float)num2);
			global::UnityEngine.RenderTexture active = global::UnityEngine.RenderTexture.active;
			global::UnityEngine.RenderTexture.active = worldPosBuffer;
			posBufferCopy.ReadPixels(new global::UnityEngine.Rect(num4, num5, 1f, 1f), 0, 0);
			global::UnityEngine.RenderTexture.active = worldNormBuffer;
			normBufferCopy.ReadPixels(new global::UnityEngine.Rect(num4, num5, 1f, 1f), 0, 0);
			global::UnityEngine.RenderTexture.active = idBuffer;
			idBufferCopy.ReadPixels(new global::UnityEngine.Rect(num4, num5, 1f, 1f), 0, 0);
			global::UnityEngine.RenderTexture.active = active;
			posBufferCopy.Apply();
			normBufferCopy.Apply();
			idBufferCopy.Apply();
			positionRead = posBufferCopy.GetPixel(0, 0);
			normalRead = normBufferCopy.GetPixel(0, 0);
			idRead = idBufferCopy.GetPixel(0, 0);
			int num6 = global::UnityEngine.Mathf.RoundToInt(idRead.r) - 1;
			hitInfo.position = new global::UnityEngine.Vector3(positionRead.r, positionRead.g, positionRead.b);
			hitInfo.normal = new global::UnityEngine.Vector3(normalRead.r, normalRead.g, normalRead.b);
			if (num6 >= 0 && num6 < visibleRenderers.Length)
			{
				hitInfo.hittedRenderer = visibleRenderers[num6];
			}
			hitInfo.IsHitted = positionRead.a > 0f;
			return hitInfo.IsHitted;
		}

		private static bool RaycastWithoutRenderer(global::UnityEngine.Camera camera, global::UnityEngine.Vector2 pixelPosition, out global::Knife.Tools.GPURaycastInfo hitInfo)
		{
			hitInfo.position = global::UnityEngine.Vector3.zero;
			hitInfo.normal = global::UnityEngine.Vector3.zero;
			hitInfo.hittedRenderer = null;
			hitInfo.IsHitted = false;
			int num = ((camera.targetTexture == null) ? camera.pixelWidth : camera.targetTexture.width);
			int num2 = ((camera.targetTexture == null) ? camera.pixelHeight : camera.targetTexture.height);
			if (lastCamera != camera || gpuRaycastCamera == null)
			{
				gpuRaycastCamera = new global::UnityEngine.GameObject("GPURaycastCamera", typeof(global::UnityEngine.Camera)).GetComponent<global::UnityEngine.Camera>();
				gpuRaycastCamera.gameObject.SetActive(value: false);
				gpuRaycastCamera.enabled = false;
				gpuRaycastCamera.gameObject.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
			}
			gpuRaycastCamera.cameraType = global::UnityEngine.CameraType.Game;
			gpuRaycastCamera.targetTexture = null;
			gpuRaycastCamera.transform.position = camera.transform.position;
			gpuRaycastCamera.transform.rotation = camera.transform.rotation;
			gpuRaycastCamera.clearFlags = global::UnityEngine.CameraClearFlags.Color;
			gpuRaycastCamera.backgroundColor = global::UnityEngine.Color.clear;
			gpuRaycastCamera.renderingPath = global::UnityEngine.RenderingPath.Forward;
			gpuRaycastCamera.rect = camera.rect;
			gpuRaycastCamera.fieldOfView = camera.fieldOfView;
			gpuRaycastCamera.aspect = camera.aspect;
			lastCamera = camera;
			global::UnityEngine.Vector2 vector = new global::UnityEngine.Vector2(pixelPosition.x / (float)camera.pixelWidth, pixelPosition.y / (float)camera.pixelHeight);
			if (vector.x < 0f || vector.x > 1f || vector.y < 0f || vector.y > 1f)
			{
				return false;
			}
			if (LastPixelWidth != num || LastPixelHeight != num2 || worldPosBuffer == null || worldNormBuffer == null || idBuffer == null)
			{
				if (worldPosBuffer != null)
				{
					worldPosBuffer.Release();
					global::UnityEngine.Object.DestroyImmediate(worldPosBuffer, allowDestroyingAssets: true);
				}
				if (worldNormBuffer != null)
				{
					worldNormBuffer.Release();
					global::UnityEngine.Object.DestroyImmediate(worldNormBuffer, allowDestroyingAssets: true);
				}
				if (idBuffer != null)
				{
					idBuffer.Release();
					global::UnityEngine.Object.DestroyImmediate(idBuffer, allowDestroyingAssets: true);
				}
				worldPosBuffer = new global::UnityEngine.RenderTexture(num, num2, 16, global::UnityEngine.RenderTextureFormat.ARGBFloat);
				worldNormBuffer = new global::UnityEngine.RenderTexture(num, num2, 0, global::UnityEngine.RenderTextureFormat.ARGBFloat);
				idBuffer = new global::UnityEngine.RenderTexture(num, num2, 0, global::UnityEngine.RenderTextureFormat.ARGBFloat);
				worldPosBuffer.Create();
				worldNormBuffer.Create();
				idBuffer.Create();
				mrtBuffers = new global::UnityEngine.RenderBuffer[3];
				mrtBuffers[0] = worldPosBuffer.colorBuffer;
				mrtBuffers[1] = worldNormBuffer.colorBuffer;
				mrtBuffers[2] = idBuffer.colorBuffer;
				LastPixelWidth = num;
				LastPixelHeight = num2;
				gpuRaycastCamera.SetTargetBuffers(mrtBuffers, worldPosBuffer.depthBuffer);
			}
			gpuRaycastCamera.SetTargetBuffers(mrtBuffers, worldPosBuffer.depthBuffer);
			gpuRaycastCamera.RenderWithShader(gpuRaycastShader, "");
			int num3 = (int)(vector.x * (float)num);
			int num4 = (int)(vector.y * (float)num2);
			global::UnityEngine.RenderTexture active = global::UnityEngine.RenderTexture.active;
			global::UnityEngine.RenderTexture.active = worldPosBuffer;
			posBufferCopy.ReadPixels(new global::UnityEngine.Rect(num3, num4, 1f, 1f), 0, 0);
			global::UnityEngine.RenderTexture.active = worldNormBuffer;
			normBufferCopy.ReadPixels(new global::UnityEngine.Rect(num3, num4, 1f, 1f), 0, 0);
			global::UnityEngine.RenderTexture.active = idBuffer;
			idBufferCopy.ReadPixels(new global::UnityEngine.Rect(num3, num4, 1f, 1f), 0, 0);
			global::UnityEngine.RenderTexture.active = active;
			posBufferCopy.Apply();
			normBufferCopy.Apply();
			idBufferCopy.Apply();
			positionRead = posBufferCopy.GetPixel(0, 0);
			normalRead = normBufferCopy.GetPixel(0, 0);
			idRead = idBufferCopy.GetPixel(0, 0);
			hitInfo.position = new global::UnityEngine.Vector3(positionRead.r, positionRead.g, positionRead.b);
			hitInfo.normal = new global::UnityEngine.Vector3(normalRead.r, normalRead.g, normalRead.b);
			hitInfo.IsHitted = positionRead.a > 0f;
			return hitInfo.IsHitted;
		}

		public static void FullscreenRaycastAsync(global::UnityEngine.Camera camera, float resolutionFactor, out int width, out int height, global::System.Action<global::Knife.Tools.GPURaycastInfo[]> callback)
		{
			InitRes();
			int num = ((camera.targetTexture == null) ? camera.pixelWidth : camera.targetTexture.width);
			int num2 = ((camera.targetTexture == null) ? camera.pixelHeight : camera.targetTexture.height);
			num = (int)((float)num * resolutionFactor);
			num2 = (int)((float)num2 * resolutionFactor);
			width = num;
			height = num2;
			if (lastCamera != camera || gpuRaycastCamera == null)
			{
				gpuRaycastCamera = new global::UnityEngine.GameObject("GPURaycastCamera", typeof(global::UnityEngine.Camera)).GetComponent<global::UnityEngine.Camera>();
				gpuRaycastCamera.gameObject.SetActive(value: false);
				gpuRaycastCamera.enabled = false;
				gpuRaycastCamera.gameObject.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
			}
			gpuRaycastCamera.cameraType = global::UnityEngine.CameraType.Game;
			gpuRaycastCamera.targetTexture = null;
			gpuRaycastCamera.transform.position = camera.transform.position;
			gpuRaycastCamera.transform.rotation = camera.transform.rotation;
			gpuRaycastCamera.clearFlags = global::UnityEngine.CameraClearFlags.Color;
			gpuRaycastCamera.backgroundColor = global::UnityEngine.Color.clear;
			gpuRaycastCamera.renderingPath = global::UnityEngine.RenderingPath.Forward;
			gpuRaycastCamera.rect = camera.rect;
			gpuRaycastCamera.fieldOfView = camera.fieldOfView;
			gpuRaycastCamera.aspect = camera.aspect;
			lastCamera = camera;
			if (LastPixelWidth != width || LastPixelHeight != height || worldPosBuffer == null || worldNormBuffer == null || idBuffer == null)
			{
				if (worldPosBuffer != null)
				{
					worldPosBuffer.Release();
					global::UnityEngine.Object.DestroyImmediate(worldPosBuffer, allowDestroyingAssets: true);
				}
				if (worldNormBuffer != null)
				{
					worldNormBuffer.Release();
					global::UnityEngine.Object.DestroyImmediate(worldNormBuffer, allowDestroyingAssets: true);
				}
				if (idBuffer != null)
				{
					idBuffer.Release();
					global::UnityEngine.Object.DestroyImmediate(idBuffer, allowDestroyingAssets: true);
				}
				worldPosBuffer = new global::UnityEngine.RenderTexture(width, height, 16, global::UnityEngine.RenderTextureFormat.ARGBFloat);
				worldNormBuffer = new global::UnityEngine.RenderTexture(width, height, 0, global::UnityEngine.RenderTextureFormat.ARGBFloat);
				idBuffer = new global::UnityEngine.RenderTexture(width, height, 0, global::UnityEngine.RenderTextureFormat.ARGBFloat);
				worldPosBuffer.Create();
				worldNormBuffer.Create();
				idBuffer.Create();
				mrtBuffers = new global::UnityEngine.RenderBuffer[3];
				mrtBuffers[0] = worldPosBuffer.colorBuffer;
				mrtBuffers[1] = worldNormBuffer.colorBuffer;
				mrtBuffers[2] = idBuffer.colorBuffer;
				LastPixelWidth = width;
				LastPixelHeight = height;
				gpuRaycastCamera.SetTargetBuffers(mrtBuffers, worldPosBuffer.depthBuffer);
			}
			gpuRaycastCamera.SetTargetBuffers(mrtBuffers, worldPosBuffer.depthBuffer);
			gpuRaycastCamera.RenderWithShader(gpuRaycastShader, "");
			global::UnityEngine.ComputeBuffer buffer = new global::UnityEngine.ComputeBuffer(width * height, 16);
			global::UnityEngine.ComputeBuffer buffer2 = new global::UnityEngine.ComputeBuffer(width * height, 16);
			gpuRaycastCopyDataShader.GetKernelThreadGroupSizes(0, out var x, out var y, out var _);
			gpuRaycastCopyDataShader.SetInt("buffersWidth", width);
			gpuRaycastCopyDataShader.SetTexture(0, "posTex", worldPosBuffer);
			gpuRaycastCopyDataShader.SetTexture(0, "normalTex", worldNormBuffer);
			gpuRaycastCopyDataShader.SetBuffer(0, "positions", buffer);
			gpuRaycastCopyDataShader.SetBuffer(0, "normals", buffer2);
			gpuRaycastCopyDataShader.Dispatch(0, (int)(width / x), (int)(height / y), 1);
			_ = new global::UnityEngine.Vector4[width * height];
			_ = new global::UnityEngine.Vector4[width * height];
		}

		public static global::Knife.Tools.GPURaycastInfo[] FullscreenRaycast(global::UnityEngine.Camera camera, float resolutionFactor, out int width, out int height)
		{
			InitRes();
			int num = ((camera.targetTexture == null) ? camera.pixelWidth : camera.targetTexture.width);
			int num2 = ((camera.targetTexture == null) ? camera.pixelHeight : camera.targetTexture.height);
			num = (int)((float)num * resolutionFactor);
			num2 = (int)((float)num2 * resolutionFactor);
			width = num;
			height = num2;
			if (lastCamera != camera || gpuRaycastCamera == null)
			{
				gpuRaycastCamera = new global::UnityEngine.GameObject("GPURaycastCamera", typeof(global::UnityEngine.Camera)).GetComponent<global::UnityEngine.Camera>();
				gpuRaycastCamera.gameObject.SetActive(value: false);
				gpuRaycastCamera.enabled = false;
				gpuRaycastCamera.gameObject.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
			}
			gpuRaycastCamera.cameraType = global::UnityEngine.CameraType.Game;
			gpuRaycastCamera.targetTexture = null;
			gpuRaycastCamera.transform.position = camera.transform.position;
			gpuRaycastCamera.transform.rotation = camera.transform.rotation;
			gpuRaycastCamera.clearFlags = global::UnityEngine.CameraClearFlags.Color;
			gpuRaycastCamera.backgroundColor = global::UnityEngine.Color.clear;
			gpuRaycastCamera.renderingPath = global::UnityEngine.RenderingPath.Forward;
			gpuRaycastCamera.rect = camera.rect;
			gpuRaycastCamera.fieldOfView = camera.fieldOfView;
			gpuRaycastCamera.aspect = camera.aspect;
			lastCamera = camera;
			if (LastPixelWidth != width || LastPixelHeight != height || worldPosBuffer == null || worldNormBuffer == null || idBuffer == null)
			{
				if (worldPosBuffer != null)
				{
					worldPosBuffer.Release();
					global::UnityEngine.Object.DestroyImmediate(worldPosBuffer, allowDestroyingAssets: true);
				}
				if (worldNormBuffer != null)
				{
					worldNormBuffer.Release();
					global::UnityEngine.Object.DestroyImmediate(worldNormBuffer, allowDestroyingAssets: true);
				}
				if (idBuffer != null)
				{
					idBuffer.Release();
					global::UnityEngine.Object.DestroyImmediate(idBuffer, allowDestroyingAssets: true);
				}
				worldPosBuffer = new global::UnityEngine.RenderTexture(width, height, 16, global::UnityEngine.RenderTextureFormat.ARGBFloat);
				worldNormBuffer = new global::UnityEngine.RenderTexture(width, height, 0, global::UnityEngine.RenderTextureFormat.ARGBFloat);
				idBuffer = new global::UnityEngine.RenderTexture(width, height, 0, global::UnityEngine.RenderTextureFormat.ARGBFloat);
				worldPosBuffer.Create();
				worldNormBuffer.Create();
				idBuffer.Create();
				mrtBuffers = new global::UnityEngine.RenderBuffer[3];
				mrtBuffers[0] = worldPosBuffer.colorBuffer;
				mrtBuffers[1] = worldNormBuffer.colorBuffer;
				mrtBuffers[2] = idBuffer.colorBuffer;
				LastPixelWidth = width;
				LastPixelHeight = height;
				gpuRaycastCamera.SetTargetBuffers(mrtBuffers, worldPosBuffer.depthBuffer);
			}
			gpuRaycastCamera.SetTargetBuffers(mrtBuffers, worldPosBuffer.depthBuffer);
			gpuRaycastCamera.RenderWithShader(gpuRaycastShader, "");
			global::UnityEngine.ComputeBuffer computeBuffer = new global::UnityEngine.ComputeBuffer(width * height, 16);
			global::UnityEngine.ComputeBuffer computeBuffer2 = new global::UnityEngine.ComputeBuffer(width * height, 16);
			gpuRaycastCopyDataShader.GetKernelThreadGroupSizes(0, out var x, out var y, out var _);
			gpuRaycastCopyDataShader.SetInt("buffersWidth", width);
			gpuRaycastCopyDataShader.SetTexture(0, "posTex", worldPosBuffer);
			gpuRaycastCopyDataShader.SetTexture(0, "normalTex", worldNormBuffer);
			gpuRaycastCopyDataShader.SetBuffer(0, "positions", computeBuffer);
			gpuRaycastCopyDataShader.SetBuffer(0, "normals", computeBuffer2);
			gpuRaycastCopyDataShader.Dispatch(0, (int)(width / x), (int)(height / y), 1);
			global::UnityEngine.Vector4[] array = new global::UnityEngine.Vector4[width * height];
			global::UnityEngine.Vector4[] array2 = new global::UnityEngine.Vector4[width * height];
			computeBuffer.GetData(array);
			computeBuffer2.GetData(array2);
			global::Knife.Tools.GPURaycastInfo[] array3 = new global::Knife.Tools.GPURaycastInfo[width * height];
			global::Knife.Tools.GPURaycastInfo gPURaycastInfo = default(global::Knife.Tools.GPURaycastInfo);
			for (int i = 0; i < array3.Length; i++)
			{
				gPURaycastInfo.position = array[i];
				gPURaycastInfo.normal = array2[i];
				gPURaycastInfo.IsHitted = array[i].w > 0f;
				array3[i] = gPURaycastInfo;
			}
			computeBuffer.Dispose();
			computeBuffer2.Dispose();
			return array3;
		}

		public static int MultiRaycastWithoutRenderer(global::UnityEngine.Camera camera, global::UnityEngine.Vector2[] pixelPositions, global::System.Collections.Generic.List<global::Knife.Tools.GPURaycastInfo> hitsInfo)
		{
			hitsInfo.Clear();
			if (pixelPositions.Length == 0)
			{
				return 0;
			}
			float num = 0.4f;
			int num2 = ((camera.targetTexture == null) ? camera.pixelWidth : camera.targetTexture.width);
			int num3 = ((camera.targetTexture == null) ? camera.pixelHeight : camera.targetTexture.height);
			num2 = (int)((float)num2 * num);
			num3 = (int)((float)num3 * num);
			for (int i = 0; i < pixelPositions.Length; i++)
			{
				pixelPositions[i] *= num;
			}
			if (lastCamera != camera || gpuRaycastCamera == null)
			{
				gpuRaycastCamera = new global::UnityEngine.GameObject("GPURaycastCamera", typeof(global::UnityEngine.Camera)).GetComponent<global::UnityEngine.Camera>();
				gpuRaycastCamera.gameObject.SetActive(value: false);
				gpuRaycastCamera.enabled = false;
				gpuRaycastCamera.gameObject.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
			}
			gpuRaycastCamera.cameraType = global::UnityEngine.CameraType.Game;
			gpuRaycastCamera.targetTexture = null;
			gpuRaycastCamera.transform.position = camera.transform.position;
			gpuRaycastCamera.transform.rotation = camera.transform.rotation;
			gpuRaycastCamera.clearFlags = global::UnityEngine.CameraClearFlags.Color;
			gpuRaycastCamera.backgroundColor = global::UnityEngine.Color.clear;
			gpuRaycastCamera.renderingPath = global::UnityEngine.RenderingPath.Forward;
			gpuRaycastCamera.rect = camera.rect;
			gpuRaycastCamera.fieldOfView = camera.fieldOfView;
			gpuRaycastCamera.aspect = camera.aspect;
			lastCamera = camera;
			if (LastPixelWidth != num2 || LastPixelHeight != num3 || worldPosBuffer == null || worldNormBuffer == null || idBuffer == null)
			{
				if (worldPosBuffer != null)
				{
					worldPosBuffer.Release();
					global::UnityEngine.Object.DestroyImmediate(worldPosBuffer, allowDestroyingAssets: true);
				}
				if (worldNormBuffer != null)
				{
					worldNormBuffer.Release();
					global::UnityEngine.Object.DestroyImmediate(worldNormBuffer, allowDestroyingAssets: true);
				}
				if (idBuffer != null)
				{
					idBuffer.Release();
					global::UnityEngine.Object.DestroyImmediate(idBuffer, allowDestroyingAssets: true);
				}
				worldPosBuffer = new global::UnityEngine.RenderTexture(num2, num3, 16, global::UnityEngine.RenderTextureFormat.ARGBFloat);
				worldNormBuffer = new global::UnityEngine.RenderTexture(num2, num3, 0, global::UnityEngine.RenderTextureFormat.ARGBFloat);
				idBuffer = new global::UnityEngine.RenderTexture(num2, num3, 0, global::UnityEngine.RenderTextureFormat.ARGBFloat);
				worldPosBuffer.Create();
				worldNormBuffer.Create();
				idBuffer.Create();
				mrtBuffers = new global::UnityEngine.RenderBuffer[3];
				mrtBuffers[0] = worldPosBuffer.colorBuffer;
				mrtBuffers[1] = worldNormBuffer.colorBuffer;
				mrtBuffers[2] = idBuffer.colorBuffer;
				LastPixelWidth = num2;
				LastPixelHeight = num3;
				gpuRaycastCamera.SetTargetBuffers(mrtBuffers, worldPosBuffer.depthBuffer);
			}
			gpuRaycastCamera.SetTargetBuffers(mrtBuffers, worldPosBuffer.depthBuffer);
			gpuRaycastCamera.RenderWithShader(gpuRaycastShader, "");
			int num4 = num2;
			int num5 = num3;
			int num6 = 0;
			int num7 = 0;
			for (int j = 0; j < pixelPositions.Length; j++)
			{
				if (pixelPositions[j].x < (float)num4)
				{
					num4 = (int)pixelPositions[j].x;
				}
				if (pixelPositions[j].x > (float)num6)
				{
					num6 = (int)pixelPositions[j].x;
				}
				if (pixelPositions[j].y < (float)num5)
				{
					num5 = (int)pixelPositions[j].y;
				}
				if (pixelPositions[j].y > (float)num7)
				{
					num7 = (int)pixelPositions[j].y;
				}
			}
			int num8 = global::UnityEngine.Mathf.Clamp(num6 - num4, 1, num2);
			int num9 = global::UnityEngine.Mathf.Clamp(num7 - num5, 1, num3);
			global::UnityEngine.Texture2D texture2D = (texture2D = new global::UnityEngine.Texture2D(num8, num9, global::UnityEngine.TextureFormat.RGBAFloat, mipChain: false));
			global::UnityEngine.Texture2D texture2D2 = (texture2D2 = new global::UnityEngine.Texture2D(num8, num9, global::UnityEngine.TextureFormat.RGBAFloat, mipChain: false));
			global::UnityEngine.RenderTexture active = global::UnityEngine.RenderTexture.active;
			global::UnityEngine.RenderTexture.active = worldPosBuffer;
			texture2D.ReadPixels(new global::UnityEngine.Rect(num4, num5, num8, num9), 0, 0);
			global::UnityEngine.RenderTexture.active = worldNormBuffer;
			texture2D2.ReadPixels(new global::UnityEngine.Rect(num4, num5, num8, num9), 0, 0);
			global::UnityEngine.RenderTexture.active = active;
			texture2D.Apply();
			texture2D2.Apply();
			global::UnityEngine.Color[] pixels = texture2D.GetPixels();
			global::UnityEngine.Color[] pixels2 = texture2D2.GetPixels();
			for (int k = 0; k < pixelPositions.Length; k++)
			{
				global::UnityEngine.Vector2 vector = pixelPositions[k] - new global::UnityEngine.Vector2(num4, num5) - global::UnityEngine.Vector2.one;
				vector.y = (float)num9 - vector.y;
				vector.x = global::UnityEngine.Mathf.Clamp(vector.x, 0f, num8 - 1);
				vector.y = global::UnityEngine.Mathf.Clamp(vector.y, 0f, num9 - 1);
				int num10 = (int)vector.x;
				int num11 = (int)vector.y;
				positionRead = pixels[num11 * num8 + num10];
				normalRead = pixels2[num11 * num8 + num10];
				if (positionRead.a > 0f)
				{
					hitsInfo.Add(new global::Knife.Tools.GPURaycastInfo
					{
						position = new global::UnityEngine.Vector3(positionRead.r, positionRead.g, positionRead.b),
						normal = new global::UnityEngine.Vector3(normalRead.r, normalRead.g, normalRead.b)
					});
				}
			}
			global::UnityEngine.Object.DestroyImmediate(texture2D, allowDestroyingAssets: true);
			global::UnityEngine.Object.DestroyImmediate(texture2D2, allowDestroyingAssets: true);
			return hitsInfo.Count;
		}
	}
}
