namespace Waits
{
	public abstract class UntilWait : global::Waits.Wait
	{
		private global::System.Func<bool> allocatedPredicate;

		protected virtual void Awake()
		{
			allocatedPredicate = Predicate;
		}

		protected abstract bool Predicate();

		public override global::System.Collections.Generic.IEnumerator<float> _Run()
		{
			yield return global::MEC.Timing.WaitUntilTrue(allocatedPredicate);
		}
	}
}
