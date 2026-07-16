namespace PlayerRoles.PlayableScps.Scp079.Overcons
{
	public class CameraOverconRenderer : global::PlayerRoles.PlayableScps.Scp079.Overcons.PooledOverconRenderer
	{
		private const float MaxSqrDistanceOtherRoom = 2165f;

		private const float ElevatorIconHeight = 3f;

		private const float ElevatorHeightMaxDiff = 50f;

		public static global::System.Collections.Generic.HashSet<global::PlayerRoles.PlayableScps.Scp079.Overcons.CameraOvercon> VisibleOvercons = new global::System.Collections.Generic.HashSet<global::PlayerRoles.PlayableScps.Scp079.Overcons.CameraOvercon>();

		internal override void SpawnOvercons(global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera newCamera)
		{
			ReturnAll();
			VisibleOvercons.Clear();
			foreach (global::PlayerRoles.PlayableScps.Scp079.Scp079InteractableBase allInstance in global::PlayerRoles.PlayableScps.Scp079.Scp079InteractableBase.AllInstances)
			{
				if (CheckVisibility(newCamera, allInstance))
				{
					global::PlayerRoles.PlayableScps.Scp079.Overcons.CameraOvercon fromPool = GetFromPool<global::PlayerRoles.PlayableScps.Scp079.Overcons.CameraOvercon>();
					fromPool.Setup(newCamera, allInstance as global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera, isElevator: false);
					VisibleOvercons.Add(fromPool);
				}
			}
			if (!global::Interactables.Interobjects.DoorUtils.DoorVariant.DoorsByRoom.TryGetValue(newCamera.Room, out var value))
			{
				return;
			}
			foreach (global::Interactables.Interobjects.DoorUtils.DoorVariant item in value)
			{
				if (!(item is global::Interactables.Interobjects.ElevatorDoor elevatorDoor) || global::UnityEngine.Mathf.Abs(elevatorDoor.TargetPosition.y - newCamera.Position.y) > 50f || GetClosestCamera(elevatorDoor.transform.position, newCamera.Room) != newCamera || !global::Interactables.Interobjects.ElevatorDoor.AllElevatorDoors.TryGetValue(elevatorDoor.Group, out var value2))
				{
					continue;
				}
				global::Interactables.Interobjects.ElevatorDoor elevatorDoor2 = value2[(value2.IndexOf(elevatorDoor) + 1) % value2.Count];
				if (elevatorDoor2.Rooms.Length == 1)
				{
					global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera closestCamera = GetClosestCamera(elevatorDoor2.transform.position, elevatorDoor2.Rooms[0]);
					if (!(closestCamera == null))
					{
						global::PlayerRoles.PlayableScps.Scp079.Overcons.CameraOvercon fromPool2 = GetFromPool<global::PlayerRoles.PlayableScps.Scp079.Overcons.CameraOvercon>();
						fromPool2.Setup(newCamera, closestCamera, isElevator: true);
						fromPool2.Position = elevatorDoor.transform.position + global::UnityEngine.Vector3.up * 3f;
						fromPool2.Rescale(newCamera);
						VisibleOvercons.Add(fromPool2);
					}
				}
			}
		}

		private global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera GetClosestCamera(global::UnityEngine.Vector3 pos, global::MapGeneration.RoomIdentifier targetRoom)
		{
			global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera result = null;
			float num = float.MaxValue;
			foreach (global::PlayerRoles.PlayableScps.Scp079.Scp079InteractableBase allInstance in global::PlayerRoles.PlayableScps.Scp079.Scp079InteractableBase.AllInstances)
			{
				if (allInstance is global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera scp079Camera && !(scp079Camera.Room != targetRoom))
				{
					float sqrMagnitude = (scp079Camera.Position - pos).sqrMagnitude;
					if (!(num < sqrMagnitude))
					{
						result = scp079Camera;
						num = sqrMagnitude;
					}
				}
			}
			return result;
		}

		private bool CheckVisibility(global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera cur, global::PlayerRoles.PlayableScps.Scp079.Scp079InteractableBase target)
		{
			if (!(target is global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera scp079Camera) || cur == scp079Camera)
			{
				return false;
			}
			if (cur.Room == target.Room)
			{
				return true;
			}
			if ((cur.Position - target.Position).sqrMagnitude > 2165f)
			{
				return false;
			}
			if (!scp079Camera.IsMain)
			{
				return false;
			}
			foreach (global::Interactables.Interobjects.DoorUtils.DoorVariant item in global::Interactables.Interobjects.DoorUtils.DoorVariant.DoorsByRoom[cur.Room])
			{
				if (item.Rooms.Contains(scp079Camera.Room))
				{
					return true;
				}
			}
			return false;
		}
	}
}
