namespace PlayerRoles.PlayableScps.Scp079.Overcons
{
	public class ElevatorOverconRenderer : global::PlayerRoles.PlayableScps.Scp079.Overcons.PooledOverconRenderer
	{
		private const float Height = 1.25f;

		internal override void SpawnOvercons(global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera newCamera)
		{
			ReturnAll();
			if (!global::Interactables.Interobjects.DoorUtils.DoorVariant.DoorsByRoom.TryGetValue(newCamera.Room, out var value))
			{
				return;
			}
			foreach (global::Interactables.Interobjects.DoorUtils.DoorVariant item in value)
			{
				if (item is global::Interactables.Interobjects.ElevatorDoor elevatorDoor)
				{
					global::PlayerRoles.PlayableScps.Scp079.Overcons.ElevatorOvercon fromPool = GetFromPool<global::PlayerRoles.PlayableScps.Scp079.Overcons.ElevatorOvercon>();
					fromPool.Target = elevatorDoor;
					fromPool.transform.position = elevatorDoor.transform.position + global::UnityEngine.Vector3.up * 1.25f;
					fromPool.Rescale(newCamera);
				}
			}
		}
	}
}
