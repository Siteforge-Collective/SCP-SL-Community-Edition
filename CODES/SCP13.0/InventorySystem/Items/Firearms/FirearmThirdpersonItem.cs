using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using InventorySystem.Items.Firearms.BasicMessages;
using InventorySystem.Items.Firearms.FunctionalParts;
using InventorySystem.Items.Thirdperson;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.FirstPersonControl.Thirdperson;
using UnityEngine;

namespace InventorySystem.Items.Firearms
{
	public class FirearmThirdpersonItem : ThirdpersonItemBase
	{
		[Serializable]
		private struct AttachmentAnimOverride
		{
			public int AttachmentId;

			public ThirdpersonItemAnimationName AnimName;

			public AnimationClip Override;
		}

		[SerializeField]
		private FirearmWorldmodel _instance;

		[SerializeField]
		private AnimationClip _shootAnim;

		[SerializeField]
		private AnimationClip _reloadAnim;

		[SerializeField]
		private AnimationClip _hipAnim;

		[SerializeField]
		private AnimationClip _adsAnim;

		[SerializeField]
		private AnimationClip _lookForward;

		[SerializeField]
		private AnimationClip _lookDown;

		[SerializeField]
		private AnimationClip _lookUp;

		[SerializeField]
		private AnimationClip _corrLeft;

		[SerializeField]
		private AnimationClip _corrRight;

		[SerializeField]
		private AnimationClip _corrStraight;

		[SerializeField]
		private AnimationClip _corrBack;

		[SerializeField]
		private AnimationClip _corrCenter;

		[SerializeField]
		private AnimationCurve _rotatorDotCurve;

		[SerializeField]
		private AnimationCurve _rotatorDisCurve;

		[SerializeField]
		private AttachmentAnimOverride[] _attachmentOverrides;

		[SerializeField]
		private bool _isAdsing;

		[SerializeField]
		private bool _isReloading;

		private FirearmFlashlight _flashlight;

		private int _prevFlashlight;

		private float _rotOffset;

		private float _prevBlend;

		private bool _shotReceived;

		private Transform _hubTransform;

		private FirstPersonMovementModule _fpmm;

		private FirearmStatus _lastStatus;

		private const float DefaultEquipTime = 0.5f;

		private const float AdsTransitionSpeed = 4f;

		private const float ShootNormalizedTime = 0.1f;

		private const float OffsetAdjustSpeed = 40f;

		private const float FlashlightRange = 22f;

		private const int ShootLayer = 3;

		private readonly Dictionary<Light, Color> _defaultColors;

		private static readonly int HashHeadTilt;

		private static readonly int HashShoot;

		public override float RotationOffset => 0f;

		public event Action OnShot
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

		public override float GetTransitionTime(ItemIdentifier iid)
		{
			return 0f;
		}

		public override void ResetObject()
		{
		}

		internal override void Initialize(HumanCharacterModel model, ItemIdentifier id)
		{
		}

		internal override void OnFadeChanged(float newFade)
		{
		}

		private void InitializeAnims()
		{
		}

		private void SetAnim(ThirdpersonItemAnimationName n, AnimationClip clip)
		{
		}

		private void ApplyAttachmentOverrides()
		{
		}

		private void AudioReceived(ReferenceHub hub, ItemType weapon, FirearmAudioClip clip)
		{
		}

		private void ConfirmationReceived(RequestMessage msg)
		{
		}

		private void Awake()
		{
		}

		private void Update()
		{
		}

		private void UpdateStatus()
		{
		}

		private void UpdateAnims()
		{
		}
	}
}
