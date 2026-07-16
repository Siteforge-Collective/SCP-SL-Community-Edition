using UnityEngine;

namespace InventorySystem.Items.Firearms.Attachments.Components
{
	public class SerializableAttachment : Attachment, IDisplayableAttachment
	{
		[SerializeField]
		private AttachmentName _name;

		[SerializeField]
		private AttachmentSlot _slot;

		[Space]
		[SerializeField]
		private float _weight;

		[SerializeField]
		private float _length;

		[SerializeField]
		[Space]
		private AttachmentDescriptiveAdvantages _extraPros;

		[SerializeField]
		private AttachmentDescriptiveDownsides _extraCons;

		[SerializeField]
		[Space]
		private Texture _icon;

		[SerializeField]
		private Vector2 _iconOffset;

		[SerializeField]
		private int _parentId;

		[SerializeField]
		private Vector2 _parentOffset;

		[Space]
		[SerializeField]
		private AttachmentParameterValuePair[] _params;

		public override AttachmentName Name => default(AttachmentName);

		public override AttachmentSlot Slot => default(AttachmentSlot);

		public override float Weight => 0f;

		public override float Length => 0f;

		public override AttachmentDescriptiveAdvantages DescriptivePros => default(AttachmentDescriptiveAdvantages);

		public override AttachmentDescriptiveDownsides DescriptiveCons => default(AttachmentDescriptiveDownsides);

		public Texture Icon => null;

		public Vector2 IconOffset => default(Vector2);

		public int ParentId => 0;

		public Vector2 ParentOffset => default(Vector2);

		private void Awake()
		{
		}

		private void Reset()
		{
		}
	}
}
