namespace PlayerRoles.PlayableScps.Scp049.Zombies
{
	public class ZombieAudioPlayer : global::PlayerRoles.PlayableScps.Subroutines.ScpSubroutineBase
	{
		[global::System.Serializable]
		private class Scp0492Sound
		{
			public global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieAudioPlayer.Scp0492SoundId Id;

			public float MaxDistance;
		}

		public enum Scp0492SoundId : byte
		{
			Growl = 0,
			AngryGrowl = 1,
			Attack = 2
		}

		private const float GrowlMaxCooldown = 7.5f;

		private const float GrowlMinCooldown = 11.25f;

		private static bool _soundsSerialized = false;

		private static readonly global::System.Collections.Generic.Dictionary<byte, global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieAudioPlayer.Scp0492Sound> Sounds = new global::System.Collections.Generic.Dictionary<byte, global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieAudioPlayer.Scp0492Sound>();

		public readonly global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown GrowlTimer = new global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown();

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieAudioPlayer.Scp0492Sound[] _sounds;

		private global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieBloodlustAbility _visionTracker;

		private byte _soundToSend;

		private global::UnityEngine.Vector3 _lastPos;

		public void ServerGrowl()
		{
			GrowlTimer.Trigger(global::UnityEngine.Random.Range(11.25f, 7.5f));
			ServerSendSound(_visionTracker.LookingAtTarget ? global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieAudioPlayer.Scp0492SoundId.AngryGrowl : global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieAudioPlayer.Scp0492SoundId.Growl);
		}

		private void Update()
		{
			if (global::Mirror.NetworkServer.active && GrowlTimer.IsReady)
			{
				ServerGrowl();
			}
		}

		protected override void Awake()
		{
			base.Awake();
			(base.Role as global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieRole).SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieBloodlustAbility>(out _visionTracker);
			if (!_soundsSerialized)
			{
				global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieAudioPlayer.Scp0492Sound[] sounds = _sounds;
				foreach (global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieAudioPlayer.Scp0492Sound scp0492Sound in sounds)
				{
					Sounds.Add((byte)scp0492Sound.Id, scp0492Sound);
				}
				_soundsSerialized = true;
			}
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			writer.WriteByte(_soundToSend);
			global::RelativePositioning.RelativePositionSerialization.WriteRelativePosition(writer, new global::RelativePositioning.RelativePosition(_lastPos));
		}

		public void ServerSendSound(global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieAudioPlayer.Scp0492SoundId soundId)
		{
			_soundToSend = (byte)soundId;
			_lastPos = (base.Role as global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieRole).FpcModule.Position;
			if (Sounds.TryGetValue(_soundToSend, out var value))
			{
				float disSqr = value.MaxDistance * value.MaxDistance;
				ServerSendRpc((ReferenceHub x) => !(x.roleManager.CurrentRole is global::PlayerRoles.FirstPersonControl.IFpcRole fpcRole) || (fpcRole.FpcModule.Position - _lastPos).sqrMagnitude <= disSqr);
			}
		}
	}
}
