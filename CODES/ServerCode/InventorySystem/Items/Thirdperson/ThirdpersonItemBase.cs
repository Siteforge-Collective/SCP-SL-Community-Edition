namespace InventorySystem.Items.Thirdperson
{
	public abstract class ThirdpersonItemBase : global::GameObjectPools.PoolObject, global::GameObjectPools.IPoolResettable
	{
		private global::UnityEngine.Transform _tr;

		protected static readonly int HashOverrideBlend = global::UnityEngine.Animator.StringToHash("OverrideBlend");

		protected static readonly int HashPrimaryAdditiveBlend = global::UnityEngine.Animator.StringToHash("PrimaryAdditiveBlend");

		protected static readonly int HashSecondaryAdditiveBlend = global::UnityEngine.Animator.StringToHash("SecondaryAdditiveBlend");

		public global::InventorySystem.Items.ItemIdentifier Identifier { get; private set; }

		public global::PlayerRoles.FirstPersonControl.Thirdperson.HumanCharacterModel TargetModel { get; private set; }

		public virtual float RotationOffset => 0f;

		public virtual void ResetObject()
		{
			TargetModel = null;
		}

		public virtual float GetTransitionTime(global::InventorySystem.Items.ItemIdentifier iid)
		{
			return 0.5f;
		}

		internal virtual void OnFadeChanged(float newFade)
		{
			_tr.localScale = global::UnityEngine.Vector3.one * newFade;
		}

		internal virtual void Initialize(global::PlayerRoles.FirstPersonControl.Thirdperson.HumanCharacterModel model, global::InventorySystem.Items.ItemIdentifier id)
		{
			_tr = base.transform;
			_tr.parent = model.ItemSpawnpoint;
			_tr.localScale = global::UnityEngine.Vector3.one;
			_tr.localPosition = global::UnityEngine.Vector3.zero;
			_tr.localRotation = global::UnityEngine.Quaternion.identity;
			TargetModel = model;
			Identifier = id;
			OnFadeChanged(model.Fade);
		}
	}
}
