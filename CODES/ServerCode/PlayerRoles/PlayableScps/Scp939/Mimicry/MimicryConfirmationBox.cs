namespace PlayerRoles.PlayableScps.Scp939.Mimicry
{
	public class MimicryConfirmationBox : global::UnityEngine.MonoBehaviour, global::CursorManagement.ICursorOverride
	{
		private const string PrefsKey = "MimicryRememberChoice";

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _moreInfoRoot;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.UI.Image _progress;

		[global::UnityEngine.SerializeField]
		private float _duration;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Canvas _hideHudCanvas;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.UI.Toggle _rememberToggle;

		public static bool Remember
		{
			get
			{
				return PlayerPrefsSl.Get("MimicryRememberChoice", defaultValue: false);
			}
			set
			{
				PlayerPrefsSl.Set("MimicryRememberChoice", value);
			}
		}

		public global::CursorManagement.CursorOverrideMode CursorOverride => global::CursorManagement.CursorOverrideMode.Free;

		public bool LockMovement => false;

		public void ButtonOk()
		{
			if (_rememberToggle.isOn)
			{
				Remember = true;
			}
			global::UnityEngine.Object.Destroy(base.gameObject);
		}

		public void ButtonDelete()
		{
			global::VoiceChat.VcPrivacyFlags vcPrivacyFlags = global::VoiceChat.VoiceChatPrivacySettings.PrivacyFlags & ~global::VoiceChat.VcPrivacyFlags.AllowRecording;
			if (_rememberToggle.isOn)
			{
				global::VoiceChat.VoiceChatPrivacySettings.PrivacyFlags = vcPrivacyFlags;
				global::VoiceChat.VoiceChatPrivacySettings.Singleton.UpdateToggles();
			}
			global::Mirror.NetworkClient.Send(new global::VoiceChat.VoiceChatPrivacySettings.VcPrivacyMessage
			{
				Flags = (byte)vcPrivacyFlags
			});
			global::UnityEngine.Object.Destroy(base.gameObject);
		}

		private void Update()
		{
			if (_moreInfoRoot.activeSelf)
			{
				_progress.fillAmount = 1f;
				return;
			}
			_progress.fillAmount -= global::UnityEngine.Time.deltaTime / _duration;
			if (!(_progress.fillAmount > 0f))
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}
}
