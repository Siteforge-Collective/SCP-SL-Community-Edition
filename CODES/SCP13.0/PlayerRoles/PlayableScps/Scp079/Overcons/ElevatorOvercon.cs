using Interactables.Interobjects;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.Overcons
{
	public class ElevatorOvercon : StandardOvercon
	{
		private static Color _busyColor;

		public ElevatorDoor Target { get; internal set; }

		private Color TargetColor => default(Color);

		private void LateUpdate()
		{
		}
	}
}
