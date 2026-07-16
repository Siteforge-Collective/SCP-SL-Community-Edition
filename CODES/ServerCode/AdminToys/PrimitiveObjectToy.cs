namespace AdminToys
{
	public class PrimitiveObjectToy : global::AdminToys.AdminToyBase
	{
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Material _regularMatTemplate;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Material _transparentMatTemplate;

		private global::UnityEngine.GameObject _spawnedPrimitve;

		private global::UnityEngine.MeshRenderer _renderer;

		private global::UnityEngine.Material _sharedRegular;

		private global::UnityEngine.Material _sharedTransparent;

		private bool _materialsSet;

		[global::Mirror.SyncVar(hook = "SetPrimitive")]
		public global::UnityEngine.PrimitiveType PrimitiveType;

		[global::Mirror.SyncVar(hook = "SetColor")]
		public global::UnityEngine.Color MaterialColor;

		public override string CommandName => "PrimitiveObject";

		public global::UnityEngine.PrimitiveType NetworkPrimitiveType
		{
			get
			{
				return PrimitiveType;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref PrimitiveType))
				{
					global::UnityEngine.PrimitiveType primitiveType = PrimitiveType;
					SetSyncVar(value, ref PrimitiveType, 16uL);
					if (global::Mirror.NetworkServer.localClientActive && !getSyncVarHookGuard(16uL))
					{
						setSyncVarHookGuard(16uL, value: true);
						SetPrimitive(primitiveType, value);
						setSyncVarHookGuard(16uL, value: false);
					}
				}
			}
		}

		public global::UnityEngine.Color NetworkMaterialColor
		{
			get
			{
				return MaterialColor;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref MaterialColor))
				{
					global::UnityEngine.Color materialColor = MaterialColor;
					SetSyncVar(value, ref MaterialColor, 32uL);
					if (global::Mirror.NetworkServer.localClientActive && !getSyncVarHookGuard(32uL))
					{
						setSyncVarHookGuard(32uL, value: true);
						SetColor(materialColor, value);
						setSyncVarHookGuard(32uL, value: false);
					}
				}
			}
		}

		public override void OnSpawned(ReferenceHub admin, global::System.ArraySegment<string> arguments)
		{
			base.OnSpawned(admin, arguments);
			string[] array = arguments.Array;
			NetworkPrimitiveType = ((array.Length > 2 && global::System.Enum.TryParse<global::UnityEngine.PrimitiveType>(array[2], ignoreCase: true, out var result)) ? result : global::UnityEngine.PrimitiveType.Sphere);
			NetworkMaterialColor = ((array.Length > 3 && global::UnityEngine.ColorUtility.TryParseHtmlString(array[3], out var color)) ? color : global::UnityEngine.Color.gray);
			float result2;
			float num = ((array.Length > 4 && float.TryParse(array[4], out result2)) ? result2 : 1f);
			base.transform.position = admin.PlayerCameraReference.position;
			base.transform.rotation = admin.PlayerCameraReference.rotation;
			base.transform.localScale = global::UnityEngine.Vector3.one * num;
			base.NetworkScale = base.transform.localScale;
		}

		private void Start()
		{
			SetPrimitive(global::UnityEngine.PrimitiveType.Sphere, PrimitiveType);
		}

		private void SetColor(global::UnityEngine.Color oldColor, global::UnityEngine.Color newColor)
		{
			if (!(_spawnedPrimitve == null))
			{
				if (!_materialsSet)
				{
					_sharedRegular = new global::UnityEngine.Material(_regularMatTemplate);
					_sharedTransparent = new global::UnityEngine.Material(_transparentMatTemplate);
					_materialsSet = true;
				}
				_renderer.sharedMaterial = ((newColor.a >= 1f) ? _sharedRegular : _sharedTransparent);
				_renderer.sharedMaterial.SetColor("_Color", newColor);
			}
		}

		private void SetPrimitive(global::UnityEngine.PrimitiveType oldPrim, global::UnityEngine.PrimitiveType newPrim)
		{
			if (_spawnedPrimitve != null)
			{
				global::UnityEngine.Object.Destroy(_spawnedPrimitve);
			}
			_spawnedPrimitve = global::UnityEngine.GameObject.CreatePrimitive(newPrim);
			_renderer = _spawnedPrimitve.GetComponent<global::UnityEngine.MeshRenderer>();
			_spawnedPrimitve.transform.SetParent(base.transform);
			_spawnedPrimitve.transform.localPosition = global::UnityEngine.Vector3.zero;
			_spawnedPrimitve.transform.localRotation = global::UnityEngine.Quaternion.identity;
			_spawnedPrimitve.transform.localScale = global::UnityEngine.Vector3.one;
			global::UnityEngine.Object.Destroy(_spawnedPrimitve.GetComponent<global::UnityEngine.Collider>());
			if (Scale.x > 0f || Scale.y > 0f || Scale.z > 0f)
			{
				bool convex = newPrim != global::UnityEngine.PrimitiveType.Plane && newPrim != global::UnityEngine.PrimitiveType.Quad;
				_spawnedPrimitve.AddComponent<global::UnityEngine.MeshCollider>().convex = convex;
			}
			SetColor(global::UnityEngine.Color.clear, MaterialColor);
		}

		private void MirrorProcessed()
		{
		}

		public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
		{
			bool result = base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				global::Mirror.GeneratedNetworkCode._Write_UnityEngine_002EPrimitiveType(writer, PrimitiveType);
				global::Mirror.NetworkWriterExtensions.WriteColor(writer, MaterialColor);
				return true;
			}
			global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 0x10L) != 0L)
			{
				global::Mirror.GeneratedNetworkCode._Write_UnityEngine_002EPrimitiveType(writer, PrimitiveType);
				result = true;
			}
			if ((base.syncVarDirtyBits & 0x20L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteColor(writer, MaterialColor);
				result = true;
			}
			return result;
		}

		public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				global::UnityEngine.PrimitiveType primitiveType = PrimitiveType;
				NetworkPrimitiveType = global::Mirror.GeneratedNetworkCode._Read_UnityEngine_002EPrimitiveType(reader);
				if (!SyncVarEqual(primitiveType, ref PrimitiveType))
				{
					SetPrimitive(primitiveType, PrimitiveType);
				}
				global::UnityEngine.Color materialColor = MaterialColor;
				NetworkMaterialColor = global::Mirror.NetworkReaderExtensions.ReadColor(reader);
				if (!SyncVarEqual(materialColor, ref MaterialColor))
				{
					SetColor(materialColor, MaterialColor);
				}
				return;
			}
			long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
			if ((num & 0x10L) != 0L)
			{
				global::UnityEngine.PrimitiveType primitiveType2 = PrimitiveType;
				NetworkPrimitiveType = global::Mirror.GeneratedNetworkCode._Read_UnityEngine_002EPrimitiveType(reader);
				if (!SyncVarEqual(primitiveType2, ref PrimitiveType))
				{
					SetPrimitive(primitiveType2, PrimitiveType);
				}
			}
			if ((num & 0x20L) != 0L)
			{
				global::UnityEngine.Color materialColor2 = MaterialColor;
				NetworkMaterialColor = global::Mirror.NetworkReaderExtensions.ReadColor(reader);
				if (!SyncVarEqual(materialColor2, ref MaterialColor))
				{
					SetColor(materialColor2, MaterialColor);
				}
			}
		}
	}
}
