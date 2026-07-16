public class PlayerListElement : global::UnityEngine.MonoBehaviour
{
	public ReferenceHub instance;

	public global::TMPro.TextMeshProUGUI TextNick;

	public global::TMPro.TextMeshProUGUI TextBadge;

	public global::UnityEngine.UI.RawImage ImgVerified;

	public global::UnityEngine.UI.Image ImgBackground;

	public global::UnityEngine.UI.Toggle ToggleMute;

	public global::UnityEngine.GameObject OpenProfile;

	private void Start()
	{
		global::VoiceChat.VoiceChatMutes.OnFlagsSet += RefreshMute;
		RefreshMute(instance, global::VoiceChat.VoiceChatMutes.GetFlags(instance));
	}

	private void OnDestroy()
	{
		global::VoiceChat.VoiceChatMutes.OnFlagsSet -= RefreshMute;
	}

	private void RefreshMute(ReferenceHub hub, global::VoiceChat.VcMuteFlags flags)
	{
		if (!(hub != instance))
		{
			ToggleMute.isOn = flags != global::VoiceChat.VcMuteFlags.None;
		}
	}

	public void Mute(bool b)
	{
	}

	public void OpenSteamAccount()
	{
	}

	public void Report()
	{
	}
}
