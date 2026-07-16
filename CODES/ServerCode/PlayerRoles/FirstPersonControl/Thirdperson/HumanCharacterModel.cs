namespace PlayerRoles.FirstPersonControl.Thirdperson
{
	public class HumanCharacterModel : global::PlayerRoles.FirstPersonControl.Thirdperson.AnimatedCharacterModel
	{
		private static readonly int HashCuffed = global::UnityEngine.Animator.StringToHash("Cuffed");

		private static readonly int HashGrounded = global::UnityEngine.Animator.StringToHash("Grounded");

		private static readonly int HashHeadTilt = global::UnityEngine.Animator.StringToHash("HeadTilt");

		private readonly global::System.Diagnostics.Stopwatch _itemTransitionSw = global::System.Diagnostics.Stopwatch.StartNew();

		private const float DefaultTransitionTime = 0.5f;

		private const float SpawnGroundedLock = 0.3f;

		private global::InventorySystem.Items.ItemIdentifier _prevItem;

		private global::UnityEngine.AnimationCurve _transitionAnimation;

		private float _prevTransitionWeight;

		private float _prevRotationOffset;

		private global::UnityEngine.Quaternion _initialRotation;

		private bool _modelUpdated;

		private bool _hasThirdpersonItem;

		private bool _hasItemSpawnpoint;

		[global::UnityEngine.SerializeField]
		private int[] _itemLayers;

		public bool PoolHeldItem;

		public global::UnityEngine.Transform ItemSpawnpoint;

		public global::InventorySystem.Items.Thirdperson.ThirdpersonItemBase LastItemInstance { get; private set; }

		public override float Fade
		{
			get
			{
				return base.Fade;
			}
			set
			{
				if (Fade != global::UnityEngine.Mathf.Clamp01(value))
				{
					base.Fade = value;
				}
			}
		}

		private void ResetThirdpersonItem()
		{
			global::InventorySystem.Items.Thirdperson.ThirdpersonItemAnimationManager.ResetOverrides(this);
			if (_hasThirdpersonItem)
			{
				LastItemInstance.ReturnToPool();
				_hasThirdpersonItem = false;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			_initialRotation = base.CachedTransform.localRotation;
			_hasItemSpawnpoint = ItemSpawnpoint != null;
		}

		protected override void Update()
		{
			base.Update();
			if (!base.Pooled)
			{
				UpdateHeldItem(base.OwnerHub.inventory.CurItem);
				UpdateHeadTilt(base.OwnerHub.PlayerCameraReference);
				base.Animator.SetBool(HashCuffed, global::InventorySystem.Disarming.DisarmedPlayers.IsDisarmed(base.OwnerHub.inventory));
				base.Animator.SetBool(HashGrounded, base.FpcModule.Noclip.IsActive || base.FpcModule.IsGrounded || base.Role.ActiveTime < 0.3f);
			}
		}

		public override void SetVisibility(bool newState)
		{
			base.SetVisibility(newState);
		}

		public override void ResetObject()
		{
			base.ResetObject();
			global::InventorySystem.Items.Thirdperson.ThirdpersonItemAnimationManager.ResetOverrides(this);
		}

		public void UpdateHeldItem(global::InventorySystem.Items.ItemIdentifier item)
		{
			if (base.Pooled || !base.OwnerHub.isLocalPlayer)
			{
				if (_prevItem != item)
				{
					_prevItem = item;
					_modelUpdated = false;
					_itemTransitionSw.Restart();
					global::InventorySystem.Items.ItemBase result;
					float num = ((global::InventorySystem.InventoryItemLoader.TryGetItem<global::InventorySystem.Items.ItemBase>(item.TypeId, out result) && result.ThirdpersonModel != null) ? result.ThirdpersonModel.GetTransitionTime(item) : 0.5f);
					_transitionAnimation = new global::UnityEngine.AnimationCurve(new global::UnityEngine.Keyframe(0f, _prevTransitionWeight), new global::UnityEngine.Keyframe(num / 2f, 0f), new global::UnityEngine.Keyframe(num, 1f));
					_prevTransitionWeight = 1f;
				}
				double totalSeconds = _itemTransitionSw.Elapsed.TotalSeconds;
				float num2 = _transitionAnimation.Evaluate((float)totalSeconds);
				int[] itemLayers = _itemLayers;
				foreach (int layerIndex in itemLayers)
				{
					base.Animator.SetLayerWeight(layerIndex, num2);
				}
				if (!_modelUpdated && (num2 > _prevTransitionWeight || num2 == 0f))
				{
					ResetThirdpersonItem();
					_hasThirdpersonItem = global::InventorySystem.Items.Thirdperson.ThirdpersonItemPoolManager.TryGet(this, item, out var result2, PoolHeldItem);
					LastItemInstance = result2;
					_modelUpdated = true;
				}
				float num3 = (_hasThirdpersonItem ? LastItemInstance.RotationOffset : 0f);
				if (num3 != _prevRotationOffset)
				{
					base.CachedTransform.localRotation = _initialRotation * global::UnityEngine.Quaternion.Euler(global::UnityEngine.Vector3.up * num3);
					_prevRotationOffset = num3;
				}
				_prevTransitionWeight = num2;
				_prevItem = item;
			}
		}

		public void UpdateHeadTilt(global::UnityEngine.Transform cam)
		{
			float x = cam.localRotation.eulerAngles.x;
			x = ((x > 180f) ? (x - 360f) : x);
			x = global::UnityEngine.Mathf.InverseLerp(-88f, 88f, 0f - x) * 2f - 1f;
			base.Animator.SetFloat(HashHeadTilt, x);
		}
	}
}
