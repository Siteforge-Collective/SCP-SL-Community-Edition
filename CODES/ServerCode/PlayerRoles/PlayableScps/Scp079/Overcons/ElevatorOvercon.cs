namespace PlayerRoles.PlayableScps.Scp079.Overcons
{
	public class ElevatorOvercon : global::PlayerRoles.PlayableScps.Scp079.Overcons.StandardOvercon
	{
		private static global::UnityEngine.Color _busyColor = new global::UnityEngine.Color(1f, 1f, 1f, 0.1f);

		public global::Interactables.Interobjects.ElevatorDoor Target { get; internal set; }

		private global::UnityEngine.Color TargetColor
		{
			get
			{
				if (!Target.TargetPanel.AssignedChamber.IsReady)
				{
					return _busyColor;
				}
				if (!IsHighlighted)
				{
					return global::PlayerRoles.PlayableScps.Scp079.Overcons.StandardOvercon.NormalColor;
				}
				return global::PlayerRoles.PlayableScps.Scp079.Overcons.StandardOvercon.HighlightedColor;
			}
		}

		private void LateUpdate()
		{
			TargetSprite.color = TargetColor;
		}
	}
}
