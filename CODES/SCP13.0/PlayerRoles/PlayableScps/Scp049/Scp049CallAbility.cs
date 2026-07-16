using Mirror;
using PlayerRoles.PlayableScps.HUDs;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp049
{
	public class Scp049CallAbility : KeySubroutine<Scp049Role>
	{
		private const float BaseCooldown = 60f;

		private const float EffectDuration = 20f;

		public readonly AbilityCooldown Cooldown;

		public readonly AbilityCooldown Duration;

		private bool _serverTriggered;

		public AbilityHud CallAbilityHUD;

		[SerializeField]
		private AudioClip _startCall;

		[SerializeField]
		private AudioClip _endCall;

		[SerializeField]
		private CooldownAudio _cooldownAudio;

		private AudioSource _callSource;

		public bool IsMarkerShown => false;

		protected override ActionName TargetKey => default(ActionName);

		private void ServerRefreshDuration()
		{
		}

		public override void ServerProcessCmd(NetworkReader reader)
		{
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		private void AbilityAudio(bool start)
		{
		}

		private void Start()
		{
		}

		protected override void Update()
		{
		}

		protected override void OnKeyDown()
		{
		}

		public override void ResetObject()
		{
		}
	}
}
