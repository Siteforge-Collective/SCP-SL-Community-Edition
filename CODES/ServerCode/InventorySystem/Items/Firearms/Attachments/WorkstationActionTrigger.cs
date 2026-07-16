namespace InventorySystem.Items.Firearms.Attachments
{
	public class WorkstationActionTrigger : global::Interactables.InteractableCollider, global::Interactables.IClientInteractable, global::Interactables.IInteractable
	{
		private const int BaselineWidth = 10;

		private const int ParentWidth = 5;

		private global::UnityEngine.RectTransform _rt;

		private global::UnityEngine.BoxCollider _col;

		private float _depth;

		public global::System.Action TargetAction { get; internal set; }

		public global::Interactables.Verification.IVerificationRule VerificationRule => global::Interactables.Verification.StandardDistanceVerification.Default;

		protected override void Awake()
		{
			Target = this;
			_rt = GetComponent<global::UnityEngine.RectTransform>();
			_col = base.gameObject.AddComponent<global::UnityEngine.BoxCollider>();
			_depth = 10f;
			global::UnityEngine.Transform parent = _rt.parent;
			while (parent != null)
			{
				if (parent.TryGetComponent<global::InventorySystem.Items.Firearms.Attachments.WorkstationActionTrigger>(out var _))
				{
					_depth += 5f;
				}
				parent = parent.parent;
			}
			UpdateSize();
		}

		private void Update()
		{
			UpdateSize();
		}

		private void UpdateSize()
		{
			_col.size = new global::UnityEngine.Vector3(_rt.rect.size.x, _rt.rect.size.y, _depth);
		}

		public void ClientInteract(global::Interactables.InteractableCollider _)
		{
			TargetAction?.Invoke();
		}
	}
}
