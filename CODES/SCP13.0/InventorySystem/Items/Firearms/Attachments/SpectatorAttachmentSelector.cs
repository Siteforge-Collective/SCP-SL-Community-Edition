using System;
using PlayerRoles;
using UnityEngine;

namespace InventorySystem.Items.Firearms.Attachments
{
	public class SpectatorAttachmentSelector : AttachmentSelectorBase
	{
		[SerializeField]
		private SpectatorSelectorFirearmButton _firearmButton;

		[SerializeField]
		private GameObject _summaryButton;

		[SerializeField]
		private float _rescaleSpeed;

		private Firearm _instantiatedFirearm;

		protected override bool UseLookatMode { get; set; }

		protected override void SelectAttachmentId(byte attachmentId)
		{
		}

		protected override void LoadPreset(uint loadedCode)
		{
		}

		public override void RegisterAction(RectTransform t, Action action)
		{
		}

		public void SelectFirearm(Firearm fa)
		{
		}

		private void ResendPreference()
		{
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private void Start()
		{
		}

		private void OnEnable()
		{
		}

		private void Update()
		{
		}

		private static void OnRoleChanged(ReferenceHub hub, PlayerRoleBase oldRole, PlayerRoleBase newRole)
		{
		}

		private static void SendPreferences()
		{
		}
	}
}
