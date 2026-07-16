namespace InventorySystem.Items.Jailbird
{
	public class JailbirdThirdperson : global::InventorySystem.Items.Thirdperson.ThirdpersonItemBase
	{
		private const global::InventorySystem.Items.Thirdperson.ThirdpersonItemAnimationName OverrideOffset = global::InventorySystem.Items.Thirdperson.ThirdpersonItemAnimationName.Override0;

		private const int AttackLayer = 4;

		private int _targetBlend;

		private static readonly int AdditiveHash = global::UnityEngine.Animator.StringToHash(global::InventorySystem.Items.Thirdperson.ThirdpersonItemAnimationName.PrimaryAdditive.ToString());

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationClip _idleOverride;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationClip _loadChargeOverride;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationClip _chargingOverride;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationClip _attackClip;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _attackSound;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _chargeSound;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioSource _audioSource;

		[global::UnityEngine.SerializeField]
		private float _blendAdjustSpeed;

		[global::UnityEngine.SerializeField]
		private global::InventorySystem.Items.Jailbird.JailbirdMaterialController _materialController;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _chargeLoadParticles;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _chargingParticles;

		private float OverrideBlend
		{
			get
			{
				return base.TargetModel.Animator.GetFloat(global::InventorySystem.Items.Thirdperson.ThirdpersonItemBase.HashOverrideBlend);
			}
			set
			{
				base.TargetModel.Animator.SetFloat(global::InventorySystem.Items.Thirdperson.ThirdpersonItemBase.HashOverrideBlend, value);
			}
		}

		public override void ResetObject()
		{
			base.ResetObject();
			global::InventorySystem.Items.Jailbird.JailbirdItem.OnRpcReceived -= OnRpcReceived;
			_chargeLoadParticles.SetActive(value: false);
			_chargingParticles.SetActive(value: false);
		}

		internal override void Initialize(global::PlayerRoles.FirstPersonControl.Thirdperson.HumanCharacterModel model, global::InventorySystem.Items.ItemIdentifier id)
		{
			base.Initialize(model, id);
			_materialController.SetSerial(id.SerialNumber);
			SetAnim(global::InventorySystem.Items.Thirdperson.ThirdpersonItemAnimationName.Override0, _idleOverride);
			SetAnim(global::InventorySystem.Items.Thirdperson.ThirdpersonItemAnimationName.Override1, _loadChargeOverride);
			SetAnim(global::InventorySystem.Items.Thirdperson.ThirdpersonItemAnimationName.Override2, _chargingOverride);
			_targetBlend = 0;
			OverrideBlend = 0f;
			global::InventorySystem.Items.Jailbird.JailbirdItem.OnRpcReceived += OnRpcReceived;
		}

		private void Update()
		{
			OverrideBlend = global::UnityEngine.Mathf.MoveTowards(OverrideBlend, _targetBlend, global::UnityEngine.Time.deltaTime * _blendAdjustSpeed);
		}

		private void OnRpcReceived(ushort serial, global::InventorySystem.Items.Jailbird.JailbirdMessageType rpc)
		{
			if (serial != base.Identifier.SerialNumber)
			{
				return;
			}
			global::InventorySystem.Items.Thirdperson.ThirdpersonItemAnimationName thirdpersonItemAnimationName = (global::InventorySystem.Items.Thirdperson.ThirdpersonItemAnimationName)(_targetBlend + 0);
			bool flag = false;
			bool flag2 = false;
			switch ((int)rpc)
			{
			default:
				return;
			case 4:
				if (thirdpersonItemAnimationName != global::InventorySystem.Items.Thirdperson.ThirdpersonItemAnimationName.Override2)
				{
					return;
				}
				goto case 3;
			case 3:
				thirdpersonItemAnimationName = global::InventorySystem.Items.Thirdperson.ThirdpersonItemAnimationName.Override0;
				SetAnim(global::InventorySystem.Items.Thirdperson.ThirdpersonItemAnimationName.PrimaryAdditive, _attackClip);
				base.TargetModel.Animator.Play(AdditiveHash, 4, 0f);
				_audioSource.Stop();
				_audioSource.PlayOneShot(_attackSound);
				break;
			case 6:
				thirdpersonItemAnimationName = global::InventorySystem.Items.Thirdperson.ThirdpersonItemAnimationName.Override0;
				break;
			case 5:
				thirdpersonItemAnimationName = global::InventorySystem.Items.Thirdperson.ThirdpersonItemAnimationName.Override1;
				flag = true;
				break;
			case 7:
				thirdpersonItemAnimationName = global::InventorySystem.Items.Thirdperson.ThirdpersonItemAnimationName.Override2;
				flag2 = true;
				_audioSource.PlayOneShot(_chargeSound);
				break;
			}
			_chargingParticles.SetActive(flag2);
			_chargeLoadParticles.SetActive(flag || flag2);
			_targetBlend = (int)(thirdpersonItemAnimationName - 0);
		}

		private void SetAnim(global::InventorySystem.Items.Thirdperson.ThirdpersonItemAnimationName anim, global::UnityEngine.AnimationClip clip)
		{
			global::InventorySystem.Items.Thirdperson.ThirdpersonItemAnimationManager.SetAnimation(base.TargetModel, anim, clip);
		}
	}
}
