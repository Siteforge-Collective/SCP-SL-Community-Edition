using CustomPlayerEffects;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.FirstPersonControl.Thirdperson;
using PlayerStatsSystem;
using UnityEngine;

namespace InventorySystem.Items.MarshmallowMan
{
	public class MarshmallowEffect : StatusEffectBase, IDamageModifierEffect, IMovementSpeedModifier, IStaminaModifier, IFriendlyFireModifier
	{
		private static readonly int DeployAnimHash;

		private static readonly int FadeHash;

		private static readonly int AttackTriggerHash;

		private static readonly int AttackMirrorHash;

		[SerializeField]
		private AnimatedCharacterModel _marshmallowModelTemplate;

		[SerializeField]
		private float _deployTime;

		[SerializeField]
		private float _turnBackTime;

		[SerializeField]
		private AnimationCurve _originalFadeOverProgress;

		private bool _turningBack;

		private bool _mirrorAttack;

		private float _progress;

		private bool _instanceSet;

		private Transform _parent;

		private AnimatedCharacterModel _marshmallowModelInstance;

		private AnimatedCharacterModel _originalCharacterModel;

		private MarshmallowAudio _marshmallowAudio;

		private const float DamageReduction = 0.25f;

		private const float TurnBackTime = 1.1f;

		private const byte TurnBackIntensityCode = byte.MaxValue;

		private float DeployProgress
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public bool DamageModifierActive => false;

		public bool MovementModifierActive => false;

		public float MovementSpeedMultiplier => 0f;

		public float MovementSpeedLimit => 0f;

		public bool StaminaModifierActive => false;

		public bool SprintingDisabled => false;

		public float GetDamageModifier(float baseDamage, DamageHandlerBase handler, HitboxType hitboxType)
		{
			return 0f;
		}

		public bool AllowFriendlyFire(float baseDamage, AttackerDamageHandler handler, HitboxType hitboxType)
		{
			return false;
		}

		protected override void Enabled()
		{
		}

		protected override void Disabled()
		{
		}

		protected override void OnEffectUpdate()
		{
		}

		protected override void IntensityChanged(byte prevState, byte newState)
		{
		}

		private void OnDestroy()
		{
		}

		private void SetupLink()
		{
		}

		private void Unlink()
		{
		}

		private void UpdateMaterials()
		{
		}

		private void SetShaderFloat(AnimatedCharacterModel model, int hash, float fade)
		{
		}

		private void OnHolsterRequested(ushort serial)
		{
		}

		private void OnSwing(ushort serial)
		{
		}

		private void OnVisibilityChanged()
		{
		}

		private void OnFadeChanged()
		{
		}
	}
}
