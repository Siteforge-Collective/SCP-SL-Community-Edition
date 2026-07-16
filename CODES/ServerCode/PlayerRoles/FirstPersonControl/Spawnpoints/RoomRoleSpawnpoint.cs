namespace PlayerRoles.FirstPersonControl.Spawnpoints
{
	[global::System.Serializable]
	public class RoomRoleSpawnpoint : global::PlayerRoles.FirstPersonControl.Spawnpoints.ISpawnpointHandler
	{
		private readonly global::System.Collections.Generic.List<global::PlayerRoles.FirstPersonControl.Spawnpoints.BoundsRoleSpawnpoint> _spawnpoints;

		[global::UnityEngine.SerializeField]
		private global::MapGeneration.RoomName _fName;

		[global::UnityEngine.SerializeField]
		private global::MapGeneration.FacilityZone _fZone;

		[global::UnityEngine.SerializeField]
		private global::MapGeneration.RoomShape _fShape;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Vector3 _localPoint;

		[global::UnityEngine.SerializeField]
		private float _lookAngle;

		[global::UnityEngine.SerializeField]
		private float _angleVar;

		[global::UnityEngine.SerializeField]
		private float _width;

		[global::UnityEngine.SerializeField]
		private float _length;

		[global::UnityEngine.SerializeField]
		private int _wNum;

		[global::UnityEngine.SerializeField]
		private int _lNum;

		private static readonly global::MapGeneration.RoomName[] ExcludedRooms = new global::MapGeneration.RoomName[1] { global::MapGeneration.RoomName.HczCheckpointToEntranceZone };

		public RoomRoleSpawnpoint(global::UnityEngine.Vector3 localPoint, float lookRotation, float lookAngleVariation, float boundsWidth, float boundsLength, int spawnpointsInWidth, int spawnpoitnsInLength, global::MapGeneration.RoomName nameFilter = global::MapGeneration.RoomName.Unnamed, global::MapGeneration.FacilityZone zoneFilter = global::MapGeneration.FacilityZone.None, global::MapGeneration.RoomShape shapeFilter = global::MapGeneration.RoomShape.Undefined)
		{
			_spawnpoints = new global::System.Collections.Generic.List<global::PlayerRoles.FirstPersonControl.Spawnpoints.BoundsRoleSpawnpoint>();
			_fName = nameFilter;
			_fZone = zoneFilter;
			_fShape = shapeFilter;
			_localPoint = localPoint;
			_lookAngle = lookRotation;
			_angleVar = lookAngleVariation;
			_width = boundsWidth;
			_length = boundsLength;
			_wNum = spawnpointsInWidth;
			_lNum = spawnpoitnsInLength;
			RefreshSpawnpoints();
			global::MapGeneration.SeedSynchronizer.OnMapGenerated += RefreshSpawnpoints;
		}

		public RoomRoleSpawnpoint(global::PlayerRoles.FirstPersonControl.Spawnpoints.RoomRoleSpawnpoint t)
			: this(t._localPoint, t._lookAngle, t._angleVar, t._width, t._length, t._wNum, t._lNum, t._fName, t._fZone, t._fShape)
		{
		}

		public bool TryGetSpawnpoint(out global::UnityEngine.Vector3 position, out float horizontalRot)
		{
			return _spawnpoints.RandomItem().TryGetSpawnpoint(out position, out horizontalRot);
		}

		private void RefreshSpawnpoints()
		{
			_spawnpoints.Clear();
			foreach (global::MapGeneration.RoomIdentifier item in global::MapGeneration.RoomIdUtils.FindRooms(_fName, _fZone, _fShape))
			{
				if (!ExcludedRooms.Contains(item.Name))
				{
					global::UnityEngine.Transform transform = item.transform;
					global::UnityEngine.Bounds bounds = new global::UnityEngine.Bounds(transform.TransformPoint(_localPoint), transform.rotation * new global::UnityEngine.Vector3(_width, 0f, _length));
					global::UnityEngine.Vector3 vector = transform.rotation * new global::UnityEngine.Vector3(_wNum, 0f, _lNum);
					global::UnityEngine.Vector3Int size = new global::UnityEngine.Vector3Int(global::UnityEngine.Mathf.RoundToInt(global::UnityEngine.Mathf.Abs(vector.x)), 1, global::UnityEngine.Mathf.RoundToInt(global::UnityEngine.Mathf.Abs(vector.z)));
					float num = transform.rotation.eulerAngles.y + _lookAngle;
					_spawnpoints.Add(new global::PlayerRoles.FirstPersonControl.Spawnpoints.BoundsRoleSpawnpoint(bounds, num - _angleVar, num + _angleVar, size));
				}
			}
		}
	}
}
