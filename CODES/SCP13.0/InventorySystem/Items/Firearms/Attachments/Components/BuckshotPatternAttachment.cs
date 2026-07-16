using InventorySystem.Items.Firearms.Modules;
using UnityEngine;

namespace InventorySystem.Items.Firearms.Attachments.Components
{
	public class BuckshotPatternAttachment : SerializableAttachment
	{
		[field: SerializeField]
		public BuckshotHitreg.BuckshotSettings HitregSettings { get; private set; }
	}
}
