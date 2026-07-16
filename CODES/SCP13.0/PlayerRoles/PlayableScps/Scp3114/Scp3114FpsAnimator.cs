using PlayerRoles.PlayableScps.HUDs;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp3114
{
	public class Scp3114FpsAnimator : ScpViewmodelBase
	{
		[SerializeField]
		private float _defaultFov;

		[SerializeField]
		private Vector2 _attackVariantMinMax;

		[SerializeField]
		private int _hideHandsLayer;

		[SerializeField]
		private float _animDampTime;

		private Scp3114Slap _slap;

		private Scp3114Role _scpRole;

		private Scp3114Strangle _strangle;

		private Scp3114Dance _dance;

		private int _prevRand;

		private static readonly int AttackHash;

		private static readonly int VariantHash;

		private static readonly int StatusHash;

		private static readonly int WalkCycleHash;

		private static readonly int WalkBlendHash;

		private static readonly int GroundedHash;

		private static readonly int StrangleHash;

		private float HideHandsWeight => 0f;

		public override float CamFOV => 0f;

		protected override void Start()
		{
		}

		protected override void OnDestroy()
		{
		}

		protected override void UpdateAnimations()
		{
		}

		private void PlayAttackAnim()
		{
		}
	}
}
