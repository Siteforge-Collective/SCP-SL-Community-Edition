using InventorySystem.Items.Thirdperson;
using PlayerRoles.FirstPersonControl.Thirdperson;
using UnityEngine;

namespace InventorySystem.Items.Jailbird
{
	public class JailbirdThirdperson : ThirdpersonItemBase
	{
		private const ThirdpersonItemAnimationName OverrideOffset = ThirdpersonItemAnimationName.Override0;

		private const int AttackLayer = 4;

		private int _targetBlend;

		private static readonly int AdditiveHash;

		[SerializeField]
		private AnimationClip _idleOverride;

		[SerializeField]
		private AnimationClip _loadChargeOverride;

		[SerializeField]
		private AnimationClip _chargingOverride;

		[SerializeField]
		private AnimationClip _attackClip;

		[SerializeField]
		private AudioClip _attackSound;

		[SerializeField]
		private AudioClip _chargeSound;

		[SerializeField]
		private AudioSource _audioSource;

		[SerializeField]
		private float _blendAdjustSpeed;

		[SerializeField]
		private JailbirdMaterialController _materialController;

		[SerializeField]
		private GameObject _chargeLoadParticles;

		[SerializeField]
		private GameObject _chargingParticles;

		private float OverrideBlend
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public override void ResetObject()
		{
		}

		internal override void Initialize(HumanCharacterModel model, ItemIdentifier id)
		{
		}

		private void Update()
		{
		}

		private void OnRpcReceived(ushort serial, JailbirdMessageType rpc)
		{
		}

		private void SetAnim(ThirdpersonItemAnimationName anim, AnimationClip clip)
		{
		}
	}
}
