namespace Waits
{
	public class ParticleSystemUntilWait : global::Waits.UntilWait
	{
		public global::UnityEngine.ParticleSystem particleSystem;

		protected override bool Predicate()
		{
			return !particleSystem.IsAlive();
		}
	}
}
