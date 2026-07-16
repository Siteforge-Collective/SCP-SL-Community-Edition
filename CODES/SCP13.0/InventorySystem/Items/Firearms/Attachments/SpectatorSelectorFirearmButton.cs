using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem.Items.Firearms.Attachments
{
	public class SpectatorSelectorFirearmButton : Button
	{
		private SpectatorAttachmentSelector _selector;

		private Firearm _fa;

		private RawImage _img;

		private Color _normalColor;

		private const float NormalColor = 0.75f;

		public void Setup(SpectatorAttachmentSelector selector, Firearm fa)
		{
		}

		private void Update()
		{
		}

		public void Click()
		{
		}
	}
}
