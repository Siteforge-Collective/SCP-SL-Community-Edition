using System.Collections.Generic;
using MapGeneration;
using PlayerRoles.PlayableScps.Scp079.Cameras;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.Map
{
	public class HczMap : ProceduralZoneMap
	{
		private static readonly Color GeneratorColor;

		private const RoomName RotateRoom = RoomName.HczCheckpointToEntranceZone;

		private const float AngleOffset = 180f;

		[SerializeField]
		private ProceduralZoneMap _entranceMap;

		protected override void PlaceRooms()
		{
		}

		public override void UpdateOpened(Scp079Camera curCam)
		{
		}

		private void Rotate(List<RoomNode> nodesToRotate, float angleDeg)
		{
		}
	}
}
