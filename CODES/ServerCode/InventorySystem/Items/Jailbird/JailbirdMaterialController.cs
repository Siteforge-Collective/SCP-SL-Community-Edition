namespace InventorySystem.Items.Jailbird
{
	public class JailbirdMaterialController : global::UnityEngine.MonoBehaviour
	{
		private static readonly global::System.Collections.Generic.HashSet<ushort> AlmostDepletedJailbirds = new global::System.Collections.Generic.HashSet<ushort>();

		private ushort _serial;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Material _almostDepletedMat;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Material _normalMat;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Renderer _emissionRend;

		private void Update()
		{
			bool flag = AlmostDepletedJailbirds.Contains(_serial);
			_emissionRend.sharedMaterial = (flag ? _almostDepletedMat : _normalMat);
		}

		public void SetSerial(ushort serial)
		{
			_serial = serial;
		}

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			global::InventorySystem.Items.Jailbird.JailbirdItem.OnRpcReceived += delegate(ushort serial, global::InventorySystem.Items.Jailbird.JailbirdMessageType rpc)
			{
				if (rpc == global::InventorySystem.Items.Jailbird.JailbirdMessageType.AlmostDepleted)
				{
					AlmostDepletedJailbirds.Add(serial);
				}
			};
			CustomNetworkManager.OnClientReady += AlmostDepletedJailbirds.Clear;
		}
	}
}
