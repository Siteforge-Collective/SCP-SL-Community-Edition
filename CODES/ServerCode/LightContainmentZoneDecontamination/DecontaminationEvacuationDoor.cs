namespace LightContainmentZoneDecontamination
{
	[global::System.Obsolete("Replaced by the new door system", true)]
	public class DecontaminationEvacuationDoor : global::UnityEngine.MonoBehaviour
	{
		public static global::System.Collections.Generic.List<global::LightContainmentZoneDecontamination.DecontaminationEvacuationDoor> Instances = new global::System.Collections.Generic.List<global::LightContainmentZoneDecontamination.DecontaminationEvacuationDoor>();

		public bool ShouldBeOpened = true;

		public bool ShouldBeClosed = true;

		[global::UnityEngine.SerializeField]
		private global::Interactables.Interobjects.DoorUtils.DoorVariant _door;

		private void Awake()
		{
			Instances.Add(this);
		}

		private void OnDestroy()
		{
			Instances.Remove(this);
		}

		public void Open()
		{
			if (ShouldBeOpened)
			{
				_door.NetworkTargetState = true;
				_door.ServerChangeLock(global::Interactables.Interobjects.DoorUtils.DoorLockReason.DecontEvacuate, newState: true);
			}
		}

		public void Close()
		{
			if (ShouldBeClosed)
			{
				_door.NetworkTargetState = false;
				_door.ServerChangeLock(global::Interactables.Interobjects.DoorUtils.DoorLockReason.DecontEvacuate, newState: false);
				_door.ServerChangeLock(global::Interactables.Interobjects.DoorUtils.DoorLockReason.DecontLockdown, newState: true);
			}
		}
	}
}
