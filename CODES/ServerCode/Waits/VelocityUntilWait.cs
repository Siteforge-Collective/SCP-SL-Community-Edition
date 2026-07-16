namespace Waits
{
	public class VelocityUntilWait : global::Waits.UntilWait
	{
		[global::System.NonSerialized]
		private float sqrThreshold;

		public global::UnityEngine.Rigidbody rigidbody;

		public float threshold = 0.05f;

		protected override void Awake()
		{
			base.Awake();
			sqrThreshold = threshold * threshold;
		}

		protected override bool Predicate()
		{
			return rigidbody.velocity.sqrMagnitude < sqrThreshold;
		}
	}
}
