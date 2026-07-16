using System;
using UnityEngine;

namespace InventorySystem.Items.Firearms.Attachments.Components
{
	public class AttachmentConfigWindow : MonoBehaviour
	{
		private bool _wasActive;

		[SerializeField]
		private RectTransform _exitTransform;

		public Action OnDestroyed;

		protected AttachmentSelectorBase Selector { get; private set; }

		protected Attachment Attachment { get; private set; }

		private void DestroySelf()
		{
		}

		private bool Validate()
		{
			return false;
		}

		protected virtual void OnDestroy()
		{
		}

		protected virtual void OnDisable()
		{
		}

		protected virtual void Update()
		{
		}

		protected virtual void SafeUpdate()
		{
		}

		public virtual void Setup(AttachmentSelectorBase selector, Attachment attachment, RectTransform transformToFit)
		{
		}

		protected virtual void SetLayout(RectTransform transformToFit)
		{
		}
	}
}
