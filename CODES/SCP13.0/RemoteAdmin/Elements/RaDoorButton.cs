using Interactables.Interobjects.DoorUtils;
using UnityEngine;

namespace RemoteAdmin.Elements
{
	public class RaDoorButton : MapControlButton
	{
		public DoorNametagExtension Door;

		private bool _hasTargetDoor;

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
