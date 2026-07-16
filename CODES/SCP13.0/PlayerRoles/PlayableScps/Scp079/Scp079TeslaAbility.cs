using Mirror;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079TeslaAbility : Scp079KeyAbilityBase
	{
		[SerializeField]
		private int _cost;

		[SerializeField]
		private float _cooldown;

		private string _abilityName;

		private string _cooldownMessage;

		private double _nextUseTime;

		public override bool IsVisible => false;

		public override bool IsReady => false;

		public override string FailMessage => null;

		public override ActionName ActivationKey => default(ActionName);

		public override string AbilityName => null;

		protected override void Start()
		{
		}

		protected override void Trigger()
		{
		}

		public override void ResetObject()
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
	}
}
