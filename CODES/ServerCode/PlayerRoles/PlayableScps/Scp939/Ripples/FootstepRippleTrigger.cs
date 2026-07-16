namespace PlayerRoles.PlayableScps.Scp939.Ripples
{
	public class FootstepRippleTrigger : global::PlayerRoles.PlayableScps.Scp939.Ripples.RippleTriggerBase
	{
		private ReferenceHub _syncPlayer;

		private global::RelativePositioning.RelativePosition _syncPos;

		private byte _syncDistance;

		public override void SpawnObject()
		{
			base.SpawnObject();
			global::PlayerRoles.FirstPersonControl.Thirdperson.AnimatedCharacterModel.OnFootstepPlayed = (global::System.Action<global::PlayerRoles.FirstPersonControl.Thirdperson.AnimatedCharacterModel, float>)global::System.Delegate.Combine(global::PlayerRoles.FirstPersonControl.Thirdperson.AnimatedCharacterModel.OnFootstepPlayed, new global::System.Action<global::PlayerRoles.FirstPersonControl.Thirdperson.AnimatedCharacterModel, float>(OnFootstepPlayed));
		}

		public override void ResetObject()
		{
			base.ResetObject();
			global::PlayerRoles.FirstPersonControl.Thirdperson.AnimatedCharacterModel.OnFootstepPlayed = (global::System.Action<global::PlayerRoles.FirstPersonControl.Thirdperson.AnimatedCharacterModel, float>)global::System.Delegate.Remove(global::PlayerRoles.FirstPersonControl.Thirdperson.AnimatedCharacterModel.OnFootstepPlayed, new global::System.Action<global::PlayerRoles.FirstPersonControl.Thirdperson.AnimatedCharacterModel, float>(OnFootstepPlayed));
		}

		private void OnFootstepPlayed(global::PlayerRoles.FirstPersonControl.Thirdperson.AnimatedCharacterModel model, float dis)
		{
			if (!(model.OwnerHub.roleManager.CurrentRole is global::PlayerRoles.HumanRole humanRole))
			{
				return;
			}
			global::UnityEngine.Vector3 position = base.ScpRole.FpcModule.Position;
			global::UnityEngine.Vector3 position2 = humanRole.FpcModule.Position;
			if (!((position - position2).sqrMagnitude > dis * dis))
			{
				if (base.IsLocalOrSpectated && !humanRole.FpcModule.Motor.IsInvisible)
				{
					base.Player.Play(humanRole);
				}
				if (global::Mirror.NetworkServer.active)
				{
					_syncPlayer = model.OwnerHub;
					_syncPos = new global::RelativePositioning.RelativePosition(position2);
					_syncDistance = (byte)global::UnityEngine.Mathf.Min(dis, 255f);
					ServerSendRpc(toAll: false);
				}
			}
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			global::Utils.Networking.ReferenceHubReaderWriter.WriteReferenceHub(writer, _syncPlayer);
			global::RelativePositioning.RelativePositionSerialization.WriteRelativePosition(writer, _syncPos);
			writer.WriteByte(_syncDistance);
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			if (global::Utils.Networking.ReferenceHubReaderWriter.TryReadReferenceHub(reader, out _syncPlayer) && _syncPlayer.roleManager.CurrentRole is global::PlayerRoles.HumanRole humanRole && humanRole.FpcModule.Motor.IsInvisible && humanRole.FpcModule.CharacterModelInstance is global::PlayerRoles.FirstPersonControl.Thirdperson.AnimatedCharacterModel animatedCharacterModel)
			{
				_syncPos = global::RelativePositioning.RelativePositionSerialization.ReadRelativePosition(reader);
				_syncDistance = reader.ReadByte();
				base.Player.Play(_syncPos.Position, humanRole.RoleColor);
				global::AudioPooling.AudioSourcePoolManager.PlaySound(animatedCharacterModel.RandomFootstep, _syncPos.Position, (int)_syncDistance);
			}
		}
	}
}
