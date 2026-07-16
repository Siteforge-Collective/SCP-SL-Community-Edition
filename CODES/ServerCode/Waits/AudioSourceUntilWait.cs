namespace Waits
{
	public class AudioSourceUntilWait : global::Waits.UntilWait
	{
		public global::UnityEngine.AudioSource audioSource;

		protected override bool Predicate()
		{
			return !audioSource.isPlaying;
		}
	}
}
