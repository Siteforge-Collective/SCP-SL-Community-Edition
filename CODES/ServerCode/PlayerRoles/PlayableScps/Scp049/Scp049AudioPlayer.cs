namespace PlayerRoles.PlayableScps.Scp049
{
	public class Scp049AudioPlayer : global::PlayerRoles.PlayableScps.Subroutines.ScpSubroutineBase
	{
		[global::System.Serializable]
		private class Scp049Sound
		{
			public global::PlayerRoles.PlayableScps.Scp049.Scp049AudioPlayer.SoundType Id;

			public float MaxDistance;
		}

		public enum SoundType : byte
		{
			ChaseMusic = 0,
			Teleport = 1,
			Snap = 2
		}

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _chaseMusic;

		[global::UnityEngine.SerializeField]
		private float _volume;

		private global::PlayerRoles.PlayableScps.Scp049.Scp049SenseAbility _senseAbility;

		private byte _soundToSend;

		public void ServerSendSound(global::PlayerRoles.PlayableScps.Scp049.Scp049AudioPlayer.SoundType soundId)
		{
			_soundToSend = (byte)soundId;
			ServerSendRpc(toAll: false);
			if (soundId == global::PlayerRoles.PlayableScps.Scp049.Scp049AudioPlayer.SoundType.ChaseMusic && _senseAbility.HasTarget)
			{
				ServerSendRpc(_senseAbility.Target);
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
		}

		protected override void Awake()
		{
			base.Awake();
			if (base.Role is global::PlayerRoles.PlayableScps.Scp049.Scp049Role scp049Role)
			{
				scp049Role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp049.Scp049SenseAbility>(out _senseAbility);
			}
		}
	}
}
