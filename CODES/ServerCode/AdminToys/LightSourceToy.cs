namespace AdminToys
{
	public class LightSourceToy : global::AdminToys.AdminToyBase
	{
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Light _light;

		[global::Mirror.SyncVar]
		public float LightIntensity;

		[global::Mirror.SyncVar]
		public float LightRange;

		[global::Mirror.SyncVar]
		public global::UnityEngine.Color LightColor;

		[global::Mirror.SyncVar]
		public bool LightShadows;

		public override string CommandName => "LightSource";

		public float NetworkLightIntensity
		{
			get
			{
				return LightIntensity;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref LightIntensity))
				{
					float lightIntensity = LightIntensity;
					SetSyncVar(value, ref LightIntensity, 16uL);
				}
			}
		}

		public float NetworkLightRange
		{
			get
			{
				return LightRange;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref LightRange))
				{
					float lightRange = LightRange;
					SetSyncVar(value, ref LightRange, 32uL);
				}
			}
		}

		public global::UnityEngine.Color NetworkLightColor
		{
			get
			{
				return LightColor;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref LightColor))
				{
					global::UnityEngine.Color lightColor = LightColor;
					SetSyncVar(value, ref LightColor, 64uL);
				}
			}
		}

		public bool NetworkLightShadows
		{
			get
			{
				return LightShadows;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref LightShadows))
				{
					bool lightShadows = LightShadows;
					SetSyncVar(value, ref LightShadows, 128uL);
				}
			}
		}

		public override void OnSpawned(ReferenceHub admin, global::System.ArraySegment<string> arguments)
		{
			base.OnSpawned(admin, arguments);
			base.transform.position = admin.PlayerCameraReference.position;
			base.transform.localScale = global::UnityEngine.Vector3.one;
		}

		private void Update()
		{
			_light.intensity = LightIntensity;
			_light.range = LightRange;
			_light.color = LightColor;
			_light.shadows = (LightShadows ? global::UnityEngine.LightShadows.Soft : global::UnityEngine.LightShadows.None);
		}

		private void MirrorProcessed()
		{
		}

		public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
		{
			bool result = base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				global::Mirror.NetworkWriterExtensions.WriteSingle(writer, LightIntensity);
				global::Mirror.NetworkWriterExtensions.WriteSingle(writer, LightRange);
				global::Mirror.NetworkWriterExtensions.WriteColor(writer, LightColor);
				global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, LightShadows);
				return true;
			}
			global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 0x10L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteSingle(writer, LightIntensity);
				result = true;
			}
			if ((base.syncVarDirtyBits & 0x20L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteSingle(writer, LightRange);
				result = true;
			}
			if ((base.syncVarDirtyBits & 0x40L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteColor(writer, LightColor);
				result = true;
			}
			if ((base.syncVarDirtyBits & 0x80L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, LightShadows);
				result = true;
			}
			return result;
		}

		public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				float lightIntensity = LightIntensity;
				NetworkLightIntensity = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
				float lightRange = LightRange;
				NetworkLightRange = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
				global::UnityEngine.Color lightColor = LightColor;
				NetworkLightColor = global::Mirror.NetworkReaderExtensions.ReadColor(reader);
				bool lightShadows = LightShadows;
				NetworkLightShadows = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
				return;
			}
			long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
			if ((num & 0x10L) != 0L)
			{
				float lightIntensity2 = LightIntensity;
				NetworkLightIntensity = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
			}
			if ((num & 0x20L) != 0L)
			{
				float lightRange2 = LightRange;
				NetworkLightRange = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
			}
			if ((num & 0x40L) != 0L)
			{
				global::UnityEngine.Color lightColor2 = LightColor;
				NetworkLightColor = global::Mirror.NetworkReaderExtensions.ReadColor(reader);
			}
			if ((num & 0x80L) != 0L)
			{
				bool lightShadows2 = LightShadows;
				NetworkLightShadows = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
			}
		}
	}
}
