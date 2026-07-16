namespace PlayerRoles.FirstPersonControl.Spawnpoints
{
	public class BoundsRoleSpawnpoint : global::PlayerRoles.FirstPersonControl.Spawnpoints.ISpawnpointHandler
	{
		private int _lastIndex;

		private readonly global::UnityEngine.Vector3[] _positions;

		private readonly float _rotMin;

		private readonly float _rotMax;

		private const int AmountThreshold = 64;

		private int NextIndex
		{
			get
			{
				if (++_lastIndex >= _positions.Length)
				{
					_lastIndex = 0;
				}
				return _lastIndex;
			}
		}

		public BoundsRoleSpawnpoint(global::UnityEngine.Vector3 pos, float rot)
		{
			_positions = new global::UnityEngine.Vector3[1] { pos };
			_rotMin = rot;
			_rotMax = rot;
		}

		public BoundsRoleSpawnpoint(global::UnityEngine.Vector3 posMin, global::UnityEngine.Vector3 posMax, float rotMin, float rotMax, global::UnityEngine.Vector3Int size)
		{
			_positions = GeneratePositions(posMin, posMax, size);
			_rotMin = rotMin;
			_rotMax = rotMax;
		}

		public BoundsRoleSpawnpoint(global::UnityEngine.Bounds bounds, float rotMin, float rotMax, global::UnityEngine.Vector3Int size)
		{
			_positions = GeneratePositions(bounds.min, bounds.max, size);
			_rotMin = rotMin;
			_rotMax = rotMax;
		}

		public bool TryGetSpawnpoint(out global::UnityEngine.Vector3 position, out float horizontalRot)
		{
			horizontalRot = global::UnityEngine.Random.Range(_rotMin, _rotMax);
			if (_positions.Length != 0)
			{
				position = _positions[NextIndex];
				return true;
			}
			position = global::UnityEngine.Vector3.zero;
			return false;
		}

		private global::UnityEngine.Vector3[] GeneratePositions(global::UnityEngine.Vector3 min, global::UnityEngine.Vector3 max, global::UnityEngine.Vector3Int size)
		{
			int num = size.x * size.y * size.z;
			if (num > 64)
			{
				global::UnityEngine.Debug.LogError($"One of the spawnpoints has more than {64} potential positions. Consider reducing its size.");
			}
			global::UnityEngine.Vector3 stepSize = new global::UnityEngine.Vector3(1f / (float)size.x, 1f / (float)size.y, 1f / (float)size.z);
			global::System.Collections.Generic.List<global::UnityEngine.Vector3> list = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>();
			for (int i = 0; i < size.x; i++)
			{
				for (int j = 0; j < size.y; j++)
				{
					for (int k = 0; k < size.z; k++)
					{
						list.Add(GeneratePosition(stepSize, i, j, k, min, max));
					}
				}
			}
			global::UnityEngine.Vector3[] array = new global::UnityEngine.Vector3[num];
			for (int l = 0; l < num; l++)
			{
				array[l] = list.PullRandomItem();
			}
			return array;
		}

		private global::UnityEngine.Vector3 GeneratePosition(global::UnityEngine.Vector3 stepSize, int x, int y, int z, global::UnityEngine.Vector3 min, global::UnityEngine.Vector3 max)
		{
			global::UnityEngine.Vector3 vector = new global::UnityEngine.Vector3(stepSize.x * (float)x, stepSize.y * (float)y, stepSize.z * (float)z) + stepSize / 2f;
			return new global::UnityEngine.Vector3(global::UnityEngine.Mathf.Lerp(min.x, max.x, vector.x), global::UnityEngine.Mathf.Lerp(min.y, max.y, vector.y), global::UnityEngine.Mathf.Lerp(min.z, max.z, vector.z));
		}
	}
}
