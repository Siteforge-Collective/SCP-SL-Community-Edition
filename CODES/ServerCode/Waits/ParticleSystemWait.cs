namespace Waits
{
	public class ParticleSystemWait : global::Waits.Wait
	{
		private global::System.Func<bool> isAliveDelegate;

		public global::UnityEngine.ParticleSystem particleSystem;

		protected virtual void Awake()
		{
			isAliveDelegate = particleSystem.IsAlive;
		}

		public override global::System.Collections.Generic.IEnumerator<float> _Run()
		{
			yield return global::MEC.Timing.WaitUntilFalse(isAliveDelegate);
		}
	}
}
