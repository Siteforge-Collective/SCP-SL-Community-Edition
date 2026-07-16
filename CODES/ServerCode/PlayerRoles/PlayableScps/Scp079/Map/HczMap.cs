namespace PlayerRoles.PlayableScps.Scp079.Map
{
	public class HczMap : global::PlayerRoles.PlayableScps.Scp079.Map.ProceduralZoneMap
	{
		private static readonly global::UnityEngine.Color GeneratorColor = new global::UnityEngine.Color(1f, 0.1f, 0f, 0.15f);

		private const global::MapGeneration.RoomName RotateRoom = global::MapGeneration.RoomName.HczCheckpointToEntranceZone;

		private const float AngleOffset = 180f;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp079.Map.ProceduralZoneMap _entranceMap;

		protected override void PlaceRooms()
		{
			base.PlaceRooms();
			if (global::Utils.NonAllocLINQ.HashsetExtensions.TryGetFirst(global::MapGeneration.RoomIdentifier.AllRoomIdentifiers, (global::MapGeneration.RoomIdentifier x) => x.Name == global::MapGeneration.RoomName.HczCheckpointToEntranceZone && x.Zone == global::MapGeneration.FacilityZone.HeavyContainment, out var first) && NodesByRoom.TryGetValue(first, out var value))
			{
				float z = value.rectTransform.localEulerAngles.z;
				Rotate(NodesByRoom, z);
				Rotate(_entranceMap.NodesByRoom, z);
			}
		}

		public override void UpdateOpened(global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera curCam)
		{
			base.UpdateOpened(curCam);
			float f = global::UnityEngine.Mathf.Sin(global::UnityEngine.Time.timeSinceLevelLoad * (float)global::System.Math.PI);
			foreach (global::MapGeneration.Distributors.Scp079Generator allGenerator in global::PlayerRoles.PlayableScps.Scp079.Scp079Recontainer.AllGenerators)
			{
				if (allGenerator.Activating)
				{
					global::MapGeneration.RoomIdentifier key = global::MapGeneration.RoomIdUtils.RoomAtPosition(allGenerator.transform.position);
					if (NodesByRoom.TryGetValue(key, out var value))
					{
						value.color = global::UnityEngine.Color.Lerp(value.color, GeneratorColor, global::UnityEngine.Mathf.Abs(f));
					}
				}
			}
		}

		private void Rotate(global::System.Collections.Generic.Dictionary<global::MapGeneration.RoomIdentifier, global::UnityEngine.UI.Image> dic, float angleDeg)
		{
			global::UnityEngine.Vector3 vector = global::UnityEngine.Vector3.forward * (180f - angleDeg);
			foreach (global::System.Collections.Generic.KeyValuePair<global::MapGeneration.RoomIdentifier, global::UnityEngine.UI.Image> item in dic)
			{
				global::UnityEngine.RectTransform rectTransform = item.Value.rectTransform;
				rectTransform.localPosition = global::UnityEngine.Quaternion.Euler(vector) * rectTransform.localPosition;
				rectTransform.Rotate(vector, global::UnityEngine.Space.Self);
			}
		}
	}
}
