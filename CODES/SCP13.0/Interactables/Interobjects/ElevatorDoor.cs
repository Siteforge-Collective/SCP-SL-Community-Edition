using System;
using System.Collections.Generic;
using Interactables.Interobjects.DoorUtils;
using UnityEngine;

namespace Interactables.Interobjects
{
	public class ElevatorDoor : BasicDoor, INonInteractableDoor
	{
        public static readonly Dictionary<ElevatorManager.ElevatorGroup, List<ElevatorDoor>> AllElevatorDoors = new Dictionary<ElevatorManager.ElevatorGroup, List<ElevatorDoor>>();

        public static Action<ElevatorManager.ElevatorGroup, ElevatorDoor> OnPairsChanged;

        public static Action<ElevatorManager.ElevatorGroup, ElevatorDoor> OnLocksChanged;

        public ElevatorPanel TargetPanel;

		[SerializeField]
		private ElevatorManager.ElevatorGroup _group;

		[SerializeField]
		private Vector3 _targetPosition;

		[SerializeField]
		private Vector3 _topPosition;

		[SerializeField]
		private Vector3 _bottomPosition;

        public Vector3 TargetPosition => base.transform.TransformPoint(_targetPosition);

        public Vector3 TopPosition => base.transform.TransformPoint(_topPosition);

        public Vector3 BottomPosition => base.transform.TransformPoint(_bottomPosition);

        public ElevatorManager.ElevatorGroup Group => _group;

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
                AllElevatorDoors[_group] = new List<ElevatorDoor> { this };
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
	}
}
