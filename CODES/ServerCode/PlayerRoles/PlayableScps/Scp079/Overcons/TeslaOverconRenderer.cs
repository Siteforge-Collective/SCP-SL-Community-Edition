namespace PlayerRoles.PlayableScps.Scp079.Overcons
{
	public class TeslaOverconRenderer : global::PlayerRoles.PlayableScps.Scp079.Overcons.PooledOverconRenderer
	{
		private static readonly global::UnityEngine.Vector3 Offset = new global::UnityEngine.Vector3(0f, 2.2f, 0f);

		internal override void SpawnOvercons(global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera newCamera)
		{
			ReturnAll();
			foreach (TeslaGate teslaGate in TeslaGateController.Singleton.TeslaGates)
			{
				global::UnityEngine.Vector3 position = teslaGate.transform.position;
				if (global::MapGeneration.RoomIdUtils.IsTheSameRoom(newCamera.Position, position))
				{
					global::PlayerRoles.PlayableScps.Scp079.Overcons.TeslaOvercon fromPool = GetFromPool<global::PlayerRoles.PlayableScps.Scp079.Overcons.TeslaOvercon>();
					fromPool.transform.position = position + Offset;
					fromPool.Rescale(newCamera);
				}
			}
		}
	}
}
