using Interactables.Interobjects;
using UnityEngine;

namespace RemoteAdmin.Elements
{
	public class RaElevatorButton : MapControlButton
	{
		public ElevatorDoor Target;

		private bool _hasTarget;

		public override void SetState(bool isSelected)
		{
		}

		public override void Setup()
		{
		}

		protected override Color BackgroundColor()
		{
			return default(Color);
		}

		protected override Color OutlineColor()
		{
			return default(Color);
		}
	}
}
