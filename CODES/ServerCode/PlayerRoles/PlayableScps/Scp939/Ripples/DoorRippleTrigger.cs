namespace PlayerRoles.PlayableScps.Scp939.Ripples
{
	public class DoorRippleTrigger : global::PlayerRoles.PlayableScps.Scp939.Ripples.RippleTriggerBase
	{
		private static readonly global::UnityEngine.Vector3 PosOffset = global::UnityEngine.Vector3.up * 1.25f;

		public override void SpawnObject()
		{
			base.SpawnObject();
			global::Interactables.Interobjects.DoorUtils.DoorEvents.OnDoorAction += OnDoorAction;
		}

		public override void ResetObject()
		{
			base.ResetObject();
			global::Interactables.Interobjects.DoorUtils.DoorEvents.OnDoorAction -= OnDoorAction;
		}

		private void OnDoorAction(global::Interactables.Interobjects.DoorUtils.DoorVariant dv, global::Interactables.Interobjects.DoorUtils.DoorAction da, ReferenceHub hub)
		{
			if ((da == global::Interactables.Interobjects.DoorUtils.DoorAction.Closed || da == global::Interactables.Interobjects.DoorUtils.DoorAction.Opened) && base.IsLocalOrSpectated && dv is global::Interactables.Interobjects.BasicDoor basicDoor)
			{
				PlayInRange(dv.transform.position + PosOffset, basicDoor.MainSource.maxDistance, global::UnityEngine.Color.red);
			}
		}
	}
}
