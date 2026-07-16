using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem.Items.Firearms.Attachments
{
	public abstract class AttachmentSelectorBase : MonoBehaviour
	{
		public static Action OnPresetLoaded;

		public static Action OnPresetSaved;

		public static Action OnAttachmentsReset;

		public Action OnSummaryToggled;

		protected AttachmentSlot SelectedSlot;

		[SerializeField]
		protected MonoBehaviour[] SlotsPool;

		[SerializeField]
		protected MonoBehaviour[] SelectableAttachmentsPool;

		[SerializeField]
		private TextMeshProUGUI _attachmentName;

		[SerializeField]
		private TextMeshProUGUI _attachmentDescription;

		[SerializeField]
		private TextMeshProUGUI _pros;

		[SerializeField]
		private TextMeshProUGUI _cons;

		[SerializeField]
		private CanvasGroup _attachmentDimmer;

		[SerializeField]
		private float _dimmerSpeed;

		[SerializeField]
		private RawImage _bodyImage;

		[SerializeField]
		private RectTransform _fullscreenRect;

		[SerializeField]
		private RectTransform _sideRect;

		[SerializeField]
		private RectTransform _selectableRect;

		[SerializeField]
		private GameObject _selectorScreen;

		[SerializeField]
		private GameObject _summaryScreen;

		[SerializeField]
		private float _selectableMaxHeight;

		[SerializeField]
		private float _selectableMaxWidth;

		[SerializeField]
		private float _selectableMaxScale;

		[SerializeField]
		private float _maxDisplayedScale;

		[SerializeField]
		private RectTransform _configIcon;

		[SerializeField]
		private HorizontalOrVerticalLayoutGroup _selectableLayoutGroup;

		private Vector3 _targetScale;

		private Vector3 _targetPosition;

		private bool _isCorrectAttachment;

		private readonly List<GameObject> _spawnedConfigButtons;

		private const byte SlotOffset = 32;

		private const string AttParamsFilename = "AttachmentParameters";

		private static bool _paramsArraySet;

		private static AttachmentParam[] _paramsArray;

		public Firearm SelectedFirearm { get; protected set; }

		protected abstract bool UseLookatMode { get; set; }

		private static Vector3 SpinRotation => default(Vector3);

		public void ProcessCollider(byte colId)
		{
		}

		public void ShowStats(int attachmentId)
		{
		}

		protected abstract void LoadPreset(uint loadedCode);

		protected abstract void SelectAttachmentId(byte attachmentId);

		public abstract void RegisterAction(RectTransform t, Action action);

		public bool CanSaveAsPreference(int presetId)
		{
			return false;
		}

		public void SaveAsPreset(int presetId)
		{
		}

		public void LoadPreset(int presetId)
		{
		}

		public void ResetAttachments()
		{
		}

		public void ToggleSummaryScreen(bool summary)
		{
		}

		public void ToggleSummaryScreen()
		{
		}

		protected void LerpRects(float lerpState)
		{
		}

		private void Lookat(Vector3 pos)
		{
		}

		private void DisableAllSelectableAttachments()
		{
		}

		protected bool RefreshState(Firearm firearm, byte? refreshReason)
		{
			return false;
		}

		private void FitToRect(RectTransform rt)
		{
		}

		private void Encapsulate(ref Bounds b, RectTransform rct)
		{
		}
	}
}
