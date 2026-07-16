using Interactables.Interobjects.DoorUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Mirror;
using UnityEngine;

namespace Interactables.Interobjects
{
	public class ElevatorChamber : MonoBehaviour
	{
		public delegate void ElevatorMoved(Bounds elevatorBounds, ElevatorChamber chamber, Vector3 deltaPos, Quaternion deltaRot);

		private enum ElevatorSequence
		{
			DoorClosing = 0,
			MovingAway = 1,
			Arriving = 2,
			DoorOpening = 3,
			Ready = 4
		}

        public List<ElevatorPanel> AllPanels = new List<ElevatorPanel>();

        private static readonly int DoorAnimHash = Animator.StringToHash("isOpen");

        private const float RotationGrowth = 1.8f;

		private ElevatorDoor _lastDestination;

		private bool _goingUp;

		private float _percentOfRotation;

        private ElevatorSequence _curSequence = ElevatorSequence.Ready;

        private ElevatorManager.ElevatorGroup _assignedGroup;

        private Bounds _cachedBounds;

		private bool _cachedBoundsUpToDate;

		private bool _eventAssigned;

        private readonly Stopwatch _seqTimer = Stopwatch.StartNew();

        [SerializeField]
		private Animator _doorAnimator;

		[SerializeField]
		private Vector3 _boundsCenter;

		[SerializeField]
		private float _boundsSize;

		[SerializeField]
		private float _boundsHeight;

		[SerializeField]
		private float _doorOpenTime;

		[SerializeField]
		private float _doorCloseTime;

		[SerializeField]
		private float _animationTime;

		[SerializeField]
		private float _rotationTime;

		[SerializeField]
		private AnimationCurve _translationCurve;

		[SerializeField]
		private AnimationCurve _rotationCurve;

		[SerializeField]
		private List<AudioSource> _travelSounds;

		public ElevatorDoor CurrentDestination { get; private set; }

		public int CurrentLevel { get; private set; }

        public ElevatorManager.ElevatorGroup AssignedGroup
        {
            get
            {
                return _assignedGroup;
            }
            set
            {
                _assignedGroup = value;
                foreach (ElevatorPanel allPanel in AllPanels)
                {
                    allPanel.SetChamber(this);
                }
                ElevatorDoor.OnPairsChanged = (Action<ElevatorManager.ElevatorGroup, ElevatorDoor>)Delegate.Combine(ElevatorDoor.OnPairsChanged, new Action<ElevatorManager.ElevatorGroup, ElevatorDoor>(AddNewPanel));
                _eventAssigned = true;
                if (!ElevatorDoor.AllElevatorDoors.TryGetValue(value, out var value2))
                {
                    return;
                }
                foreach (ElevatorDoor item in value2)
                {
                    AddNewPanel(value, item);
                }
            }
        }

        public DoorLockReason ActiveLocks { get; private set; }

        public bool IsReady => _curSequence == ElevatorSequence.Ready;

        public Bounds WorldspaceBounds
        {
            get
            {
                if (!_cachedBoundsUpToDate)
                {
                    float num = _boundsSize * (_percentOfRotation * 1.8f + 1f);
                    _cachedBounds = new Bounds(base.transform.TransformPoint(_boundsCenter), new Vector3(num, _boundsHeight, num));
                    _cachedBoundsUpToDate = true;
                }
                return _cachedBounds;
            }
        }

        public static event ElevatorMoved OnElevatorMoved;

        public bool TrySetDestination(int targetLevel, bool force = false)
        {
            if (!ElevatorDoor.AllElevatorDoors.TryGetValue(AssignedGroup, out var value))
            {
                return false;
            }
            if (targetLevel < 0 || targetLevel >= value.Count)
            {
                return false;
            }
            ElevatorDoor elevatorDoor = value[targetLevel];
            if (!force)
            {
                if (!IsReady)
                {
                    return false;
                }
                if (elevatorDoor == _lastDestination)
                {
                    return false;
                }
            }
            if (_lastDestination != null)
            {
                if (NetworkServer.active)
                {
                    _lastDestination.TargetState = false;
                }
                _goingUp = _lastDestination.TargetPosition.y < elevatorDoor.TargetPosition.y;
                _travelSounds.ForEach(delegate (AudioSource x)
                {
                    x.Play();
                });
                _curSequence = ElevatorSequence.DoorClosing;
                _seqTimer.Restart();
                AllPanels.ForEach(delegate (ElevatorPanel x)
                {
                    x.SetMoving(_goingUp);
                });
                SetInnerDoor(state: false);
            }
            else
            {
                if (NetworkServer.active)
                {
                    elevatorDoor.TargetState = true;
                }
                base.transform.position = elevatorDoor.TargetPosition;
                base.transform.rotation = elevatorDoor.transform.rotation;
                base.transform.SetParent(elevatorDoor.transform.parent);
                _curSequence = ElevatorSequence.Ready;
                _lastDestination = elevatorDoor;
                if (ActiveLocks == DoorLockReason.None)
                {
                    AllPanels.ForEach(delegate (ElevatorPanel x)
                    {
                        x.SetLevel(targetLevel);
                    });
                }
                else
                {
                    AllPanels.ForEach(delegate (ElevatorPanel x)
                    {
                        x.SetLocked();
                    });
                }
                SetInnerDoor(state: true);
            }
            CurrentLevel = targetLevel;
            CurrentDestination = elevatorDoor;
            return true;
        }

        private void SetInnerDoor(bool state)
        {
            _doorAnimator.SetBool(DoorAnimHash, state);
            _cachedBoundsUpToDate = false;
        }

        private void AddNewPanel(ElevatorManager.ElevatorGroup group, ElevatorDoor door)
        {
            ElevatorPanel targetPanel = door.TargetPanel;
            if (!(targetPanel == null) && !AllPanels.Contains(targetPanel))
            {
                targetPanel.SetChamber(this);
                AllPanels.Add(targetPanel);
            }
        }

        private void Update()
        {
            switch (_curSequence)
            {
                case ElevatorSequence.MovingAway:
                case ElevatorSequence.Arriving:
                    {
                        Transform transform = base.transform;
                        Bounds worldspaceBounds = WorldspaceBounds;
                        Vector3 position = transform.position;
                        Quaternion rotation = transform.rotation;
                        UpdateMovement(transform, _curSequence == ElevatorSequence.Arriving);
                        _cachedBoundsUpToDate = false;
                        Vector3 deltaPos = transform.position - position;
                        Quaternion deltaRot = transform.rotation * Quaternion.Inverse(rotation);
                        ElevatorChamber.OnElevatorMoved?.Invoke(worldspaceBounds, this, deltaPos, deltaRot);
                        break;
                    }
                case ElevatorSequence.DoorClosing:
                    if (!(_seqTimer.Elapsed.TotalSeconds < (double)_doorCloseTime))
                    {
                        _curSequence = ElevatorSequence.MovingAway;
                        _seqTimer.Restart();
                    }
                    break;
                case ElevatorSequence.DoorOpening:
                    if (!(_seqTimer.Elapsed.TotalSeconds < (double)_doorOpenTime))
                    {
                        _curSequence = ElevatorSequence.Ready;
                    }
                    break;
            }
        }

        private void Awake()
        {
            ElevatorDoor.OnLocksChanged = (Action<ElevatorManager.ElevatorGroup, ElevatorDoor>)Delegate.Combine(ElevatorDoor.OnLocksChanged, new Action<ElevatorManager.ElevatorGroup, ElevatorDoor>(RefreshLocks));
        }

        private void OnDestroy()
        {
            if (_eventAssigned)
            {
                ElevatorDoor.OnPairsChanged = (Action<ElevatorManager.ElevatorGroup, ElevatorDoor>)Delegate.Remove(ElevatorDoor.OnPairsChanged, new Action<ElevatorManager.ElevatorGroup, ElevatorDoor>(AddNewPanel));
                ElevatorDoor.OnLocksChanged = (Action<ElevatorManager.ElevatorGroup, ElevatorDoor>)Delegate.Remove(ElevatorDoor.OnLocksChanged, new Action<ElevatorManager.ElevatorGroup, ElevatorDoor>(RefreshLocks));
            }
        }

        private void RefreshLocks(ElevatorManager.ElevatorGroup group, ElevatorDoor elevDoor)
        {
            if (group != AssignedGroup)
            {
                return;
            }
            ActiveLocks = DoorLockReason.None;
            if (ElevatorDoor.AllElevatorDoors.TryGetValue(group, out var value))
            {
                value.ForEach(delegate (ElevatorDoor x)
                {
                    ActiveLocks |= (DoorLockReason)x.ActiveLocks;
                });
            }
            bool flag = IsReady || _curSequence == ElevatorSequence.DoorOpening;
            if (ActiveLocks == DoorLockReason.None)
            {
                if (flag)
                {
                    AllPanels.ForEach(delegate (ElevatorPanel x)
                    {
                        x.SetLevel(CurrentLevel);
                    });
                }
                else
                {
                    AllPanels.ForEach(delegate (ElevatorPanel x)
                    {
                        x.SetMoving(_goingUp);
                    });
                }
            }
            else if (flag)
            {
                AllPanels.ForEach(delegate (ElevatorPanel x)
                {
                    x.SetLocked();
                });
            }
        }

        private void UpdateMovement(Transform t, bool arriving)
        {
            ElevatorDoor currentDestination = CurrentDestination;
            float num = Mathf.Clamp01((float)_seqTimer.Elapsed.TotalSeconds / _animationTime);
            UpdateRotation(t, currentDestination, num, arriving);
            if (arriving)
            {
                Vector3 a = (_goingUp ? currentDestination.BottomPosition : currentDestination.TopPosition);
                t.position = Vector3.Lerp(a, currentDestination.TargetPosition, _translationCurve.Evaluate(num));
                if (num < 1f)
                {
                    return;
                }
                if (NetworkServer.active)
                {
                    currentDestination.TargetState = true;
                }
                SetInnerDoor(state: true);
                _lastDestination = currentDestination;
                if (ActiveLocks == DoorLockReason.None)
                {
                    AllPanels.ForEach(delegate (ElevatorPanel x)
                    {
                        x.SetLevel(CurrentLevel);
                    });
                }
                else
                {
                    AllPanels.ForEach(delegate (ElevatorPanel x)
                    {
                        x.SetLocked();
                    });
                }
                _curSequence = ElevatorSequence.DoorOpening;
                _seqTimer.Restart();
            }
            else
            {
                Vector3 b = (_goingUp ? _lastDestination.TopPosition : _lastDestination.BottomPosition);
                t.position = Vector3.Lerp(_lastDestination.TargetPosition, b, 1f - _translationCurve.Evaluate(1f - num));
                if (!(num < 1f))
                {
                    t.SetParent(currentDestination.transform.parent);
                    _curSequence = ElevatorSequence.Arriving;
                    _seqTimer.Restart();
                }
            }
        }


        private void UpdateRotation(Transform t, ElevatorDoor dest, float f, bool arriving)
        {
            if (arriving)
            {
                f += 1f;
            }
            f = Mathf.InverseLerp(_rotationTime, 2f - _rotationTime, f);
            _percentOfRotation = (arriving ? (1f - f) : f);
            Quaternion rotation = _lastDestination.transform.rotation;
            Quaternion rotation2 = dest.transform.rotation;
            t.rotation = Quaternion.Lerp(rotation, rotation2, _rotationCurve.Evaluate(f));
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(WorldspaceBounds.center, WorldspaceBounds.size);
            Gizmos.color = new Color(0f, 1f, 0f, 0.1f);
            Gizmos.DrawCube(WorldspaceBounds.center, WorldspaceBounds.size);
        }
    }
}
