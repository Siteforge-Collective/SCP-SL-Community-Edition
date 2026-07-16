namespace InventorySystem.Items.Firearms.BasicMessages
{
	public static class GunHitMessageExtensions
	{
		private const float HoleRayLength = 1.1f;

		private const float BloodRayLength = 5f;

		private static BloodDrawer _bloodDrawer;

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			CustomNetworkManager.OnClientReady += RegisterHandlers;
		}

		private static void RegisterHandlers()
		{
			global::Mirror.NetworkClient.ReplaceHandler<global::InventorySystem.Items.Firearms.BasicMessages.GunHitMessage>(ClientMessageReceived);
		}

		private static void ClientMessageReceived(global::InventorySystem.Items.Firearms.BasicMessages.GunHitMessage msg)
		{
		}

		public static void Serialize(this global::Mirror.NetworkWriter writer, global::InventorySystem.Items.Firearms.BasicMessages.GunHitMessage value)
		{
			int num = value.ReceivedDamage + 1;
			global::Mirror.NetworkWriterExtensions.WriteSByte(writer, (sbyte)(value.DrawBlood ? (-num) : num));
			if (num == 1)
			{
				global::Mirror.NetworkWriterExtensions.WriteVector3(writer, value.BulletholeOrigin);
				global::Mirror.NetworkWriterExtensions.WriteVector3(writer, value.BulletholeDirection);
			}
			else
			{
				global::Mirror.NetworkWriterExtensions.WriteVector3(writer, value.DamagePosition);
			}
		}

		public static global::InventorySystem.Items.Firearms.BasicMessages.GunHitMessage Deserialize(this global::Mirror.NetworkReader reader)
		{
			sbyte num = global::Mirror.NetworkReaderExtensions.ReadSByte(reader);
			bool flag = num < 0;
			int num2 = global::UnityEngine.Mathf.Abs(num) - 1;
			if (num2 != 0)
			{
				return new global::InventorySystem.Items.Firearms.BasicMessages.GunHitMessage(flag, num2, global::Mirror.NetworkReaderExtensions.ReadVector3(reader));
			}
			return new global::InventorySystem.Items.Firearms.BasicMessages.GunHitMessage(global::Mirror.NetworkReaderExtensions.ReadVector3(reader), global::Mirror.NetworkReaderExtensions.ReadVector3(reader), flag);
		}
	}
}
