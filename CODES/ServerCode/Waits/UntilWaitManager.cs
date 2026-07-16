namespace Waits
{
	public abstract class UntilWaitManager : global::Waits.WaitManager
	{
		protected global::System.Func<bool> allocatedKeepRunning;

		protected override void Awake()
		{
			base.Awake();
			allocatedKeepRunning = KeepRunning;
		}

		protected abstract bool KeepRunning();

		public override global::System.Collections.Generic.IEnumerator<float> _Run()
		{
			StartAll();
			yield return float.NegativeInfinity;
			yield return global::MEC.Timing.WaitUntilFalse(allocatedKeepRunning);
		}
	}
}
