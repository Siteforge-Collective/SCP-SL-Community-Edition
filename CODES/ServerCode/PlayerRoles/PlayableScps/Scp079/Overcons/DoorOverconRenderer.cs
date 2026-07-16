namespace PlayerRoles.PlayableScps.Scp079.Overcons
{
	public class DoorOverconRenderer : global::PlayerRoles.PlayableScps.Scp079.Overcons.PooledOverconRenderer
	{
		private const float DoorHeight = 1.25f;

		internal override void SpawnOvercons(global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera newCamera)
		{
			ReturnAll();
			if (!global::Interactables.Interobjects.DoorUtils.DoorVariant.DoorsByRoom.TryGetValue(newCamera.Room, out var value))
			{
				return;
			}
			foreach (global::Interactables.Interobjects.DoorUtils.DoorVariant item in value)
			{
				if (!global::PlayerRoles.PlayableScps.Scp079.Scp079DoorAbility.CheckVisibility(item, newCamera))
				{
					continue;
				}
				global::UnityEngine.Vector3 vector;
				if (item is global::Interactables.Interobjects.CheckpointDoor checkpointDoor)
				{
					global::UnityEngine.Vector3 zero = global::UnityEngine.Vector3.zero;
					global::Interactables.Interobjects.DoorUtils.DoorVariant[] subDoors = checkpointDoor.SubDoors;
					foreach (global::Interactables.Interobjects.DoorUtils.DoorVariant doorVariant in subDoors)
					{
						zero += doorVariant.transform.position;
					}
					vector = zero / checkpointDoor.SubDoors.Length;
				}
				else
				{
					vector = item.transform.position;
				}
				global::PlayerRoles.PlayableScps.Scp079.Overcons.DoorOvercon fromPool = GetFromPool<global::PlayerRoles.PlayableScps.Scp079.Overcons.DoorOvercon>();
				fromPool.Target = item;
				fromPool.transform.position = vector + global::UnityEngine.Vector3.up * 1.25f;
				fromPool.Rescale(newCamera);
			}
		}
	}
}
