namespace MapGeneration.Distributors
{
	public abstract class DistributorSpawnpointBase : global::UnityEngine.MonoBehaviour
	{
		private global::MapGeneration.RoomName _roomName;

		private bool _roomSet;

		public global::MapGeneration.RoomName RoomName
		{
			get
			{
				if (!_roomSet)
				{
					_roomName = global::MapGeneration.RoomIdUtils.RoomAtPosition(base.transform.position).Name;
					_roomSet = true;
				}
				return _roomName;
			}
		}

		private void Awake()
		{
			base.transform.localScale = global::UnityEngine.Vector3.one;
		}
	}
}
