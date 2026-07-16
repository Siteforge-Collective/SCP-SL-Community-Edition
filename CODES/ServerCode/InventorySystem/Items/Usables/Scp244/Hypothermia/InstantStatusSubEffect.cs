namespace InventorySystem.Items.Usables.Scp244.Hypothermia
{
	public class InstantStatusSubEffect : global::InventorySystem.Items.Usables.Scp244.Hypothermia.HypothermiaSubEffectBase, global::CustomPlayerEffects.IWeaponModifierPlayerEffect, global::InventorySystem.Searching.ISearchTimeModifier, global::PlayerRoles.FirstPersonControl.IMovementSpeedModifier
	{
		private readonly global::System.Collections.Generic.Dictionary<global::InventorySystem.Items.Firearms.Attachments.AttachmentParam, float> _dictionarized = new global::System.Collections.Generic.Dictionary<global::InventorySystem.Items.Firearms.Attachments.AttachmentParam, float>();

		private float _currentIntensity;

		private float _statsPrevIntensity;

		[global::UnityEngine.SerializeField]
		private float _decaySpeed;

		[global::UnityEngine.SerializeField]
		private float _maxExposure;

		[global::UnityEngine.SerializeField]
		private float _movementSpeedMultiplier;

		[global::UnityEngine.SerializeField]
		private float _searchTimeAdditionIncrease;

		[global::UnityEngine.SerializeField]
		private float _searchTimeMultiplierIncrease;

		[global::UnityEngine.SerializeField]
		private global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterValuePair[] _weaponStats;

		private float CurIntensity => _currentIntensity * global::CustomPlayerEffects.RainbowTaste.CurrentMultiplier(base.Hub);

		private float VitalityMultiplier => (!global::CustomPlayerEffects.Vitality.CheckPlayer(base.Hub)) ? 1 : 0;

		public override bool IsActive => _currentIntensity > 0f;

		public bool ParamsActive => true;

		public bool MovementModifierActive => true;

		public float MovementSpeedMultiplier => global::UnityEngine.Mathf.LerpUnclamped(1f, _movementSpeedMultiplier, VitalityMultiplier * CurIntensity);

		public float MovementSpeedLimit => float.MaxValue;

		public float ProcessSearchTime(float val)
		{
			float num = global::UnityEngine.Mathf.LerpUnclamped(1f, _searchTimeMultiplierIncrease, CurIntensity);
			return val * num + _searchTimeAdditionIncrease * CurIntensity;
		}

		internal override void UpdateEffect(float curExposure)
		{
			_currentIntensity -= _decaySpeed * global::UnityEngine.Time.deltaTime;
			if (_currentIntensity < curExposure)
			{
				_currentIntensity = curExposure;
			}
			if (_currentIntensity > _maxExposure)
			{
				_currentIntensity = _maxExposure;
			}
			float num = CurIntensity * VitalityMultiplier;
			if (num != _statsPrevIntensity)
			{
				global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterValuePair[] weaponStats = _weaponStats;
				for (int i = 0; i < weaponStats.Length; i++)
				{
					global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterValuePair attachmentParameterValuePair = weaponStats[i];
					_dictionarized[attachmentParameterValuePair.Parameter] = global::UnityEngine.Mathf.LerpUnclamped(1f, attachmentParameterValuePair.Value, num);
				}
				_statsPrevIntensity = num;
			}
		}

		public override void DisableEffect()
		{
			_currentIntensity = 0f;
			_dictionarized.Clear();
		}

		public bool TryGetWeaponParam(global::InventorySystem.Items.Firearms.Attachments.AttachmentParam param, out float val)
		{
			return _dictionarized.TryGetValue(param, out val);
		}
	}
}
