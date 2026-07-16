using System.Runtime.CompilerServices;
using Mirror;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp106
{
	public class Scp106Attack : Scp106VigorAbilityBase
	{
		public delegate void PlayerTeleported(ReferenceHub scp106, ReferenceHub hub);

		private const float TargetTraceTime = 0.35f;

		private const float VigorCaptureReward = 0.3f;

		private const float CooldownReductionReward = 5f;

		private const float CorrodingTime = 20f;

		private ReferenceHub _targetHub;

		private Quaternion _claimedOwnerCamRotation;

		private Vector3 _claimedOwnerPosition;

		private Vector3 _claimedTargetPosition;

		private double _nextAttack;

		[SerializeField]
		private AnimationCurve _dotOverDistance;

		[SerializeField]
		private float _maxRangeSqr;

		[SerializeField]
		private float _hitCooldown;

		[SerializeField]
		private float _missCooldown;

		[SerializeField]
		private int _damage;

		private Transform OwnerCam => null;

		protected override ActionName TargetKey => default(ActionName);

		public static event PlayerTeleported OnPlayerTeleported
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		protected override void OnKeyDown()
		{
		}

		private void SendCooldown(float cooldown)
		{
		}

		private void ReduceSinkholeCooldown()
		{
		}

		private bool VerifyShot()
		{
			return false;
		}

		private void ServerShoot()
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
