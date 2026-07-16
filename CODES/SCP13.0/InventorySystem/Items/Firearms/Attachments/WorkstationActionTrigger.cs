using System;
using Interactables;
using Interactables.Verification;
using UnityEngine;

namespace InventorySystem.Items.Firearms.Attachments
{
	public class WorkstationActionTrigger : InteractableCollider, IClientInteractable, IInteractable
	{
		private const int BaselineWidth = 10;

		private const int ParentWidth = 5;

		private RectTransform _rt;

		private BoxCollider _col;

		private float _depth;

		public Action TargetAction { get; internal set; }

		public IVerificationRule VerificationRule => null;

		protected override void Awake()
		{
		}

		private void Update()
		{
		}

		private void UpdateSize()
		{
		}

		public void ClientInteract(InteractableCollider _)
		{
		}
	}
}
