using CursorManagement;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerRoles.PlayableScps.Scp939.Mimicry
{
	public class MimicryConfirmationBox : MonoBehaviour, ICursorOverride
	{
		private const string PrefsKey = "MimicryRememberChoice";

		[SerializeField]
		private GameObject _moreInfoRoot;

		[SerializeField]
		private Image _progress;

		[SerializeField]
		private float _duration;

		[SerializeField]
		private Canvas _hideHudCanvas;

		[SerializeField]
		private Toggle _rememberToggle;

		public static bool Remember
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public CursorOverrideMode CursorOverride => default(CursorOverrideMode);

		public bool LockMovement => false;

		public void ButtonOk()
		{
		}

		public void ButtonDelete()
		{
		}

		private void Update()
		{
		}

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private void ToggleHud(bool b)
		{
		}
	}
}
