namespace Waits
{
	public class AndWaitManager : global::Waits.UntilWaitManager
	{
		protected override bool KeepRunning()
		{
			return global::System.Linq.Enumerable.All(waitHandles, (global::MEC.CoroutineHandle x) => x.IsRunning);
		}
	}
}
