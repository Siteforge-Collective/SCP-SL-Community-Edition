using Mirror;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079SpeakerAbility : Scp079KeyAbilityBase, IScp079AuxRegenModifier
	{
		[SerializeField]
		private float _regenMultiplier;

		private bool _syncUsing;

		private string _abilityName;

		private Scp079VoiceModule _voiceModule;

		public float AuxRegenMultiplier => 0f;

		public string AuxReductionMessage { get; private set; }

		public bool CanTransmit => false;

		public override ActionName ActivationKey => default(ActionName);

		public override bool IsReady => false;

		public override bool IsVisible => false;

		public override string AbilityName => null;

		public override string FailMessage => null;

		private bool IsUsingSpeaker => false;

		protected override void Trigger()
		{
		}

		private void RefreshNearestSpeaker()
		{
		}

		protected override void Awake()
		{
		}

		protected override void Update()
		{
		}

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
	}
}
