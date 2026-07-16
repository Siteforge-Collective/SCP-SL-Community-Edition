namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079NotificationEntry : global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079GuiElementBase
	{
		[global::UnityEngine.SerializeField]
		private global::TMPro.TextMeshProUGUI _text;

		public global::PlayerRoles.PlayableScps.Scp079.GUI.IScp079Notification Content { get; internal set; }

		public global::TMPro.TextMeshProUGUI Text => _text;
	}
}
