using Mirror;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp3114
{
	public class Scp3114StrangleAudio : StandardSubroutine<Scp3114Role>
	{
		private enum RpcType
		{
			ChokeSync = 0,
			Kill = 1
		}

		private Scp3114Strangle _strangle;

		private bool _isChoking;

		private RpcType _rpcType;

		private double _syncKillTime;

		private readonly AbilityCooldown _syncCooldown;

		[SerializeField]
		private AudioSource _chokeSource;

		[SerializeField]
		private AudioClip _killSoundClip;

		[SerializeField]
		private float _killSoundRange;

		[SerializeField]
		private float _volumeAdjustSpeed;

		[SerializeField]
		private float _killEventTimeSeconds;

		[SerializeField]
		private float _minPitchAdjust;

		[SerializeField]
		private float _maxPitchAdjust;

		private const float MinSyncCooldownSeconds = 0.2f;

		private double _samplesToSeconds;

		private double _secondsToSamples;

		private bool LocallyStrangled => false;

		private void ServerSendRpc(RpcType rpcType)
		{
		}

		private void Update()
		{
		}

		private void UpdateServer()
		{
		}

		private void ResyncAudio()
		{
		}

		protected override void Awake()
		{
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		public override void ResetObject()
		{
		}
	}
}
