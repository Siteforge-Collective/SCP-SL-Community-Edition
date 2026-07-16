namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public interface IScp079Notification
	{
		string DisplayedText { get; }

		float Opacity { get; }

		bool Delete { get; }

		global::PlayerRoles.PlayableScps.Scp079.GUI.NotificationSound Sound { get; }

		float Height { get; }
	}
}
