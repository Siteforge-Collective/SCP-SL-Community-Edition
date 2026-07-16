using Interactables;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem.Items.Firearms.Attachments
{
	public class WorkstationSelectorCollider : InteractableCollider, IAttachmentSelectorButton
	{
		private const float HighlightColor = 0.71f;

		private const float DefaultColor = 0.38f;

		private const float CurrentColor = 0.54f;

		private const float LerpSpeed = 12f;

		private const float SizeRatio = 1f;

		private const float MinColliderWidth = 40f;

		private const float MinColliderDepth = 1f;

		[SerializeField]
		private BoxCollider _collider;

		[SerializeField]
		private RawImage _image;

		private Firearm _firearm;

		private AttachmentSlot _mySlot;

		private bool _prevHighlighted;

		public RectTransform RectTransform => null;

		public byte ButtonId
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public void Setup(Texture icon, AttachmentSlot slot, Vector2? pos, Firearm fa)
		{
		}

		public void UpdateColors(AttachmentSlot slot)
		{
		}
	}
}
