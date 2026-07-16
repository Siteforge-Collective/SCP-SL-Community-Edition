namespace Elevators
{
	public class TransformElevatorFollower : global::Elevators.ElevatorFollowerBase
	{
		private global::UnityEngine.Transform _cachedTransform;

		protected override void Awake()
		{
			base.Awake();
			_cachedTransform = base.gameObject.transform;
			LastPosition = _cachedTransform.position;
		}

		protected override void LateUpdate()
		{
			base.LateUpdate();
			LastPosition = _cachedTransform.position;
		}
	}
}
