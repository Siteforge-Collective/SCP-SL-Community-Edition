namespace Elevators
{
	public abstract class ElevatorFollowerBase : global::UnityEngine.MonoBehaviour
	{
		public global::UnityEngine.Vector3 LastPosition;

		public global::Interactables.Interobjects.ElevatorChamber TrackedChamber { get; private set; }

		public bool InElevator { get; private set; }

		protected virtual void Awake()
		{
			global::Interactables.Interobjects.ElevatorChamber.OnElevatorMoved += OnElevatorMoved;
		}

		protected virtual void OnDestroy()
		{
			global::Interactables.Interobjects.ElevatorChamber.OnElevatorMoved -= OnElevatorMoved;
		}

		protected virtual void LateUpdate()
		{
		}

		protected virtual void OnElevatorMoved(global::UnityEngine.Bounds elevatorBounds, global::Interactables.Interobjects.ElevatorChamber chamber, global::UnityEngine.Vector3 deltaPos, global::UnityEngine.Quaternion deltaRot)
		{
			bool flag = InElevator && chamber == TrackedChamber;
			if (!elevatorBounds.Contains(LastPosition))
			{
				if (flag)
				{
					InElevator = false;
					base.transform.position -= deltaPos;
					base.transform.SetParent(null);
				}
			}
			else if (!flag)
			{
				base.transform.SetParent(chamber.transform);
				base.transform.position += deltaPos;
				TrackedChamber = chamber;
				InElevator = true;
			}
		}
	}
}
