using System;
using UnityEngine;

namespace InventorySystem.Items.Firearms.Attachments
{
	public class WorkstationAttachmentSelector : AttachmentSelectorBase
	{
		[SerializeField]
		private WorkstationController _controllerRef;

		[SerializeField]
		private float _rescaleSpeed;

		[SerializeField]
		private GameObject _panelMain;

		[SerializeField]
		private GameObject _panelNoWeapon;

		[SerializeField]
		private GameObject _panelUnknownWeapon;

		protected override bool UseLookatMode { get; set; }

		protected override void SelectAttachmentId(byte attachmentId)
		{
		}

		protected override void LoadPreset(uint loadedCode)
		{
		}

		public override void RegisterAction(RectTransform rt, Action action)
		{
		}

		private void SentChangeRequest(uint code)
		{
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		private void ShowInactivePanel(GameObject go)
		{
		}

		private void ShowPanel(GameObject go)
		{
		}
	}
}
