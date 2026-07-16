using Mirror;
using RelativePositioning;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.Pinging
{
	public class Scp079PingAbility : Scp079KeyAbilityBase
	{
		[SerializeField]
		private int _cost;

		[SerializeField]
		private float _instantCooldown;

		[SerializeField]
		private float _groupCooldown;

		[SerializeField]
		private int _groupSize;

		[SerializeField]
		private Scp079PingInstance _prefab;

		[SerializeField]
		private Sprite[] _icons;

		private string _abilityName;

		private string _cooldownMsg;

		private RateLimiter _rateLimiter;

		private byte _syncProcessorIndex;

		private RelativePosition _syncPos;

		private Vector3 _syncNormal;

		private const float RaycastMaxDis = 130f;

		private static readonly IPingProcessor[] PingProcessors;

		public override ActionName ActivationKey => default(ActionName);

		public override bool IsReady => false;

		public override bool IsVisible => false;

		public override string AbilityName => null;

		public override string FailMessage => null;

		private void SpawnIndicator(int processorIndex, RelativePosition pos, Vector3 normal)
		{
		}

		private void WriteSyncVars(NetworkWriter writer)
		{
		}

		private bool ServerCheckReceiver(ReferenceHub hub, Vector3 point, int processorIndex)
		{
			return false;
		}

		protected override void Start()
		{
		}

		protected override void Trigger()
		{
		}

		public override void ClientWriteCmd(NetworkWriter writer)
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
