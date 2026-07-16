using PlayerRoles.PlayableScps.HUDs;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp939
{
	public class Scp939FpsAnimator : ScpViewmodelBase
	{
		private const int JumpLayer = 2;

		private const float MinFocusStateToDisplay = 0.4f;

		private const float JumpLayerAdjustmentSpeed = 4.5f;

		private const float JumpOverLifetime = 0.4f;

		private const int CloudLayer = 4;

		private const float CloudTransitionSpeed = 1.5f;

		private const float CloudMaxWeight = 2.5f;

		private static readonly int WalkCycleHash;

		private static readonly int WalkBlendHash;

		private static readonly int ClawAttackHash;

		private static readonly int FocusActiveHash;

		private static readonly int JumpingHash;

		private static readonly int LungeStateHash;

		private static readonly int LungeTriggerHash;

		private static readonly int CloudHash;

		[SerializeField]
		private float _dampTimeBlend;

		[SerializeField]
		private AudioClip _attackSound;

		[SerializeField]
		private Vector2 _pitchRandomization;

		private Scp939Model _model;

		private Scp939MovementModule _fpc;

		private Scp939FocusAbility _focusAbility;

		private Scp939LungeAbility _lungeAbility;

		private Scp939ClawAbility _clawAbility;

		private Scp939AmnesticCloudAbility _cloudAbility;

		public override float CamFOV => 0f;

		protected override void Start()
		{
		}

		private void OnLungeStateChanged(Scp939LungeState state)
		{
		}

		private void OnAttackTriggered()
		{
		}

		protected override void OnDestroy()
		{
		}

		private void SetCloudLayer(float weight, bool charging)
		{
		}

		protected override void UpdateAnimations()
		{
		}
	}
}
