namespace Elevators
{
	public class RigidBodyElevatorFollower : global::Elevators.ElevatorFollowerBase
	{
		public global::UnityEngine.Rigidbody Rigidbody;

		protected override void Awake()
		{
			base.Awake();
			LastPosition = Rigidbody.position;
		}

		protected override void LateUpdate()
		{
			base.LateUpdate();
			if (!Rigidbody.IsSleeping())
			{
				LastPosition = Rigidbody.position;
			}
		}

		private void Reset()
		{
			Rigidbody = GetComponent<global::UnityEngine.Rigidbody>();
		}
	}
}
