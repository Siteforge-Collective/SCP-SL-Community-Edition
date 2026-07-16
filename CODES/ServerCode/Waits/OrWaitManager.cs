namespace Waits
{
	public class OrWaitManager : global::Waits.UntilWaitManager
	{
		protected override bool KeepRunning()
		{
			return global::System.Linq.Enumerable.Any(waitHandles, (global::MEC.CoroutineHandle x) => x.IsRunning);
		}
	}
}
