namespace Waits
{
	public abstract class WaitManager : global::UnityEngine.MonoBehaviour
	{
		protected global::Waits.Wait[] waits;

		protected global::MEC.CoroutineHandle[] waitHandles;

		protected virtual void Awake()
		{
			waits = GetComponents<global::Waits.Wait>();
			waitHandles = new global::MEC.CoroutineHandle[waits.Length];
		}

		protected void StartAll()
		{
			for (int i = 0; i < waits.Length; i++)
			{
				waitHandles[i] = global::MEC.Timing.RunCoroutine(waits[i]._Run());
			}
		}

		public abstract global::System.Collections.Generic.IEnumerator<float> _Run();
	}
}
