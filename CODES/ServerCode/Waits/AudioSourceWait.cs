namespace Waits
{
	public class AudioSourceWait : global::Waits.Wait
	{
		public global::UnityEngine.AudioSource audioSource;

		public override global::System.Collections.Generic.IEnumerator<float> _Run()
		{
			yield return global::MEC.Timing.WaitUntilFalse(() => audioSource.isPlaying);
		}
	}
}
