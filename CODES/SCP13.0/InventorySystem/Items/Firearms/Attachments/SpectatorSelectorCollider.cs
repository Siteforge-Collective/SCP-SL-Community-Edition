using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem.Items.Firearms.Attachments
{
	public class SpectatorSelectorCollider : MonoBehaviour, IAttachmentSelectorButton
	{
		private const float DefaultColor = 0.9f;

		private const float CurrentColor = 1f;

		private const float LerpSpeed = 12f;

		[SerializeField]
		private RawImage _image;

		[SerializeField]
		private SpectatorAttachmentSelector _selector;

		private Firearm _firearm;

		private AttachmentSlot _mySlot;

		public RectTransform RectTransform => null;

		public byte ButtonId { get; set; }

		public void Setup(Texture icon, AttachmentSlot slot, Vector2? pos, Firearm fa)
		{
		}

		public void Click()
		{
		}

		public void Hover(bool isHovering)
		{
		}

		public void UpdateColors(AttachmentSlot slot)
		{
		}
	}
}
