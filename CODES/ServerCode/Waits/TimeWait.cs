namespace Waits
{
	public class TimeWait : global::Waits.Wait
	{
		public float duration = 10f;

		public override global::System.Collections.Generic.IEnumerator<float> _Run()
		{
			yield return global::MEC.Timing.WaitForSeconds(duration);
		}
	}
}
