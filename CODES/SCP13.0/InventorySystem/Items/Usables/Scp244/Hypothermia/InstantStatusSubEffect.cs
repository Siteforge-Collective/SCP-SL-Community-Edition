using System.Collections.Generic;
using CustomPlayerEffects;
using InventorySystem.Items.Firearms.Attachments;
using InventorySystem.Searching;
using PlayerRoles.FirstPersonControl;
using UnityEngine;

namespace InventorySystem.Items.Usables.Scp244.Hypothermia
{
	public class InstantStatusSubEffect : HypothermiaSubEffectBase, IWeaponModifierPlayerEffect, ISearchTimeModifier, IMovementSpeedModifier
	{
		private readonly Dictionary<AttachmentParam, float> _dictionarized;

		private float _currentIntensity;

		private float _statsPrevIntensity;

		[SerializeField]
		private float _decaySpeed;

		[SerializeField]
		private float _maxExposure;

		[SerializeField]
		private float _movementSpeedMultiplier;

		[SerializeField]
		private float _searchTimeAdditionIncrease;

		[SerializeField]
		private float _searchTimeMultiplierIncrease;

		[SerializeField]
		private AttachmentParameterValuePair[] _weaponStats;

		private float CurIntensity => 0f;

		private float VitalityMultiplier => 0f;

		public override bool IsActive => false;

		public bool ParamsActive => false;

		public bool MovementModifierActive => false;

		public float MovementSpeedMultiplier => 0f;

		public float MovementSpeedLimit => 0f;

		public float ProcessSearchTime(float val)
		{
			return 0f;
		}

		internal override void UpdateEffect(float curExposure)
		{
		}

		public override void DisableEffect()
		{
		}

		public bool TryGetWeaponParam(AttachmentParam param, out float val)
		{
			val = default(float);
			return false;
		}
	}
}
