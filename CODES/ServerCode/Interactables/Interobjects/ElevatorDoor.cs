namespace Interactables.Interobjects
{
	public class ElevatorDoor : global::Interactables.Interobjects.BasicDoor, global::Interactables.Interobjects.DoorUtils.INonInteractableDoor
	{
		public static readonly global::System.Collections.Generic.Dictionary<global::Interactables.Interobjects.ElevatorManager.ElevatorGroup, global::System.Collections.Generic.List<global::Interactables.Interobjects.ElevatorDoor>> AllElevatorDoors = new global::System.Collections.Generic.Dictionary<global::Interactables.Interobjects.ElevatorManager.ElevatorGroup, global::System.Collections.Generic.List<global::Interactables.Interobjects.ElevatorDoor>>();

		public static global::System.Action<global::Interactables.Interobjects.ElevatorManager.ElevatorGroup, global::Interactables.Interobjects.ElevatorDoor> OnPairsChanged;

		public static global::System.Action<global::Interactables.Interobjects.ElevatorManager.ElevatorGroup, global::Interactables.Interobjects.ElevatorDoor> OnLocksChanged;

		public global::Interactables.Interobjects.ElevatorPanel TargetPanel;

		[global::UnityEngine.SerializeField]
		private global::Interactables.Interobjects.ElevatorManager.ElevatorGroup _group;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Vector3 _targetPosition;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Vector3 _topPosition;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Vector3 _bottomPosition;

		public global::UnityEngine.Vector3 TargetPosition => base.transform.TransformPoint(_targetPosition);

		public global::UnityEngine.Vector3 TopPosition => base.transform.TransformPoint(_topPosition);

		public global::UnityEngine.Vector3 BottomPosition => base.transform.TransformPoint(_bottomPosition);

		public global::Interactables.Interobjects.ElevatorManager.ElevatorGroup Group => _group;

		public bool IgnoreLockdowns => true;

		public bool IgnoreRemoteAdmin => true;

		public override bool AllowInteracting(ReferenceHub ply, byte colliderId)
		{
			return false;
		}

		protected override void Start()
		{
			base.Start();
			if (!AllElevatorDoors.TryGetValue(_group, out var value) || value == null)
			{
				AllElevatorDoors[_group] = new global::System.Collections.Generic.List<global::Interactables.Interobjects.ElevatorDoor> { this };
				return;
			}
			bool flag = false;
			float y = TargetPosition.y;
			for (int i = 0; i < value.Count; i++)
			{
				if (!(y >= value[i].TargetPosition.y))
				{
					value.Insert(i, this);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				value.Add(this);
			}
			OnPairsChanged?.Invoke(_group, this);
		}

		protected override void LockChanged(ushort prevValue)
		{
			base.LockChanged(prevValue);
			OnLocksChanged?.Invoke(_group, this);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (AllElevatorDoors.TryGetValue(_group, out var value))
			{
				value.Remove(this);
			}
		}

		private void MirrorProcessed()
		{
		}
	}
}
