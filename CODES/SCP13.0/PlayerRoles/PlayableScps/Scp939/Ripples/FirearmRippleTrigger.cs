using InventorySystem.Items.Firearms;
using Mirror;
using RelativePositioning;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp939.Ripples
{
	public class FirearmRippleTrigger : RippleTriggerBase
	{
		private Scp939FocusAbility _focus;

		private RelativePosition _syncRipplePos;

		private RoleTypeId _syncRoleColor;

		private ReferenceHub _syncPlayer;

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		protected override void Awake()
		{
		}

		private Color DecodeColor(NetworkReader reader)
		{
			return default(Color);
		}

		private void OnServerSoundPlayed(Firearm firearm, byte audioId, float dis)
		{
		}
	}
}
