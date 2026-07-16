namespace PlayerRoles.PlayableScps.Scp939.Ripples
{
	public class FirearmRippleTrigger : global::PlayerRoles.PlayableScps.Scp939.Ripples.RippleTriggerBase
	{
		private global::PlayerRoles.PlayableScps.Scp939.Scp939FocusAbility _focus;

		private global::RelativePositioning.RelativePosition _syncRipplePos;

		private global::PlayerRoles.RoleTypeId _syncRoleColor;

		public override void SpawnObject()
		{
			base.SpawnObject();
			global::InventorySystem.Items.Firearms.FirearmExtensions.ServerSoundPlayed = (global::System.Action<global::InventorySystem.Items.Firearms.Firearm, byte, float>)global::System.Delegate.Combine(global::InventorySystem.Items.Firearms.FirearmExtensions.ServerSoundPlayed, new global::System.Action<global::InventorySystem.Items.Firearms.Firearm, byte, float>(OnServerSoundPlayed));
		}

		public override void ResetObject()
		{
			base.ResetObject();
			global::InventorySystem.Items.Firearms.FirearmExtensions.ServerSoundPlayed = (global::System.Action<global::InventorySystem.Items.Firearms.Firearm, byte, float>)global::System.Delegate.Remove(global::InventorySystem.Items.Firearms.FirearmExtensions.ServerSoundPlayed, new global::System.Action<global::InventorySystem.Items.Firearms.Firearm, byte, float>(OnServerSoundPlayed));
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			global::RelativePositioning.RelativePositionSerialization.WriteRelativePosition(writer, _syncRipplePos);
			if (!(_focus.State < 1f))
			{
				global::Mirror.NetworkWriterExtensions.WriteSByte(writer, (sbyte)_syncRoleColor);
			}
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			base.Player.Play(global::RelativePositioning.RelativePositionSerialization.ReadRelativePosition(reader).Position, DecodeColor(reader));
		}

		protected override void Awake()
		{
			base.Awake();
			GetSubroutine<global::PlayerRoles.PlayableScps.Scp939.Scp939FocusAbility>(out _focus);
		}

		private global::UnityEngine.Color DecodeColor(global::Mirror.NetworkReader reader)
		{
			if (reader.Position >= reader.Length)
			{
				return global::UnityEngine.Color.red;
			}
			if (!global::PlayerRoles.PlayerRoleLoader.TryGetRoleTemplate<global::PlayerRoles.HumanRole>((global::PlayerRoles.RoleTypeId)global::Mirror.NetworkReaderExtensions.ReadSByte(reader), out var result))
			{
				return global::UnityEngine.Color.red;
			}
			return result.RoleColor;
		}

		private void OnServerSoundPlayed(global::InventorySystem.Items.Firearms.Firearm firearm, byte audioId, float dis)
		{
			if (global::Mirror.NetworkServer.active && firearm.Owner.roleManager.CurrentRole is global::PlayerRoles.HumanRole humanRole && !((humanRole.FpcModule.Position - base.ScpRole.FpcModule.Position).sqrMagnitude > dis * dis))
			{
				_syncRipplePos = new global::RelativePositioning.RelativePosition(humanRole.FpcModule.Position);
				_syncRoleColor = humanRole.RoleTypeId;
				ServerSendRpc((ReferenceHub x) => x == base.Owner || global::PlayerRoles.Spectating.SpectatorNetworking.IsSpectatedBy(base.Owner, x));
			}
		}
	}
}
