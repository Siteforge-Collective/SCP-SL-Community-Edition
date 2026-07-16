using Interactables.Interobjects.DoorUtils;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp939.Ripples
{
	public class DoorRippleTrigger : RippleTriggerBase
	{
		private static readonly Vector3 PosOffset;

		private SurfaceRippleTrigger _surfaceRippleTrigger;

		private bool _rippleAssigned;

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}

		private void OnDoorAction(DoorVariant dv, DoorAction da, ReferenceHub hub)
		{
		}
	}
}
