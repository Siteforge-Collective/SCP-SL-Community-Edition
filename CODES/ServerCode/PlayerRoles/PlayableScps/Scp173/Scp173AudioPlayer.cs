namespace PlayerRoles.PlayableScps.Scp173
{
	public class Scp173AudioPlayer : global::PlayerRoles.PlayableScps.Subroutines.ScpSubroutineBase
	{
		[global::System.Serializable]
		private class Scp173Sound
		{
			public global::PlayerRoles.PlayableScps.Scp173.Scp173AudioPlayer.Scp173SoundId Id;

			public float MaxDistance;
		}

		public enum Scp173SoundId : byte
		{
			Hit = 0,
			Teleport = 1,
			Snap = 2
		}

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp173.Scp173AudioPlayer.Scp173Sound[] _sounds;

		private byte _soundToSend;

		private global::UnityEngine.Vector3 _lastPos;

		private static bool _soundsDictionarized = false;

		private static readonly global::System.Collections.Generic.Dictionary<byte, global::PlayerRoles.PlayableScps.Scp173.Scp173AudioPlayer.Scp173Sound> Sounds = new global::System.Collections.Generic.Dictionary<byte, global::PlayerRoles.PlayableScps.Scp173.Scp173AudioPlayer.Scp173Sound>();

		protected override void Awake()
		{
			base.Awake();
			if (!_soundsDictionarized)
			{
				global::PlayerRoles.PlayableScps.Scp173.Scp173AudioPlayer.Scp173Sound[] sounds = _sounds;
				foreach (global::PlayerRoles.PlayableScps.Scp173.Scp173AudioPlayer.Scp173Sound scp173Sound in sounds)
				{
					Sounds.Add((byte)scp173Sound.Id, scp173Sound);
				}
				_soundsDictionarized = true;
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

		public void ServerSendSound(global::PlayerRoles.PlayableScps.Scp173.Scp173AudioPlayer.Scp173SoundId soundId)
		{
			if (base.Role.TryGetOwner(out var hub) && !global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp173PlaySound, hub, soundId))
			{
				return;
			}
			_soundToSend = (byte)soundId;
			_lastPos = (base.Role as global::PlayerRoles.PlayableScps.Scp173.Scp173Role).FpcModule.Position;
			if (Sounds.TryGetValue(_soundToSend, out var value))
			{
				float disSqr = value.MaxDistance * value.MaxDistance;
				ServerSendRpc((ReferenceHub x) => !(x.roleManager.CurrentRole is global::PlayerRoles.FirstPersonControl.IFpcRole fpcRole) || (fpcRole.FpcModule.Position - _lastPos).sqrMagnitude <= disSqr);
			}
		}
	}
}
