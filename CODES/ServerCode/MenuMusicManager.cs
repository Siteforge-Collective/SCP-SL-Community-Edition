public class MenuMusicManager : global::UnityEngine.MonoBehaviour
{
	private float curState;

	public float lerpSpeed = 1f;

	private bool creditsChanged;

	[global::UnityEngine.Space(15f)]
	public global::UnityEngine.AudioSource mainSource;

	public global::UnityEngine.AudioSource creditsSource;

	[global::UnityEngine.Space(8f)]
	public global::UnityEngine.GameObject creditsHolder;

	private void Update()
	{
		curState = global::UnityEngine.Mathf.Lerp(curState, creditsHolder.activeSelf ? 1f : 0f, lerpSpeed * global::UnityEngine.Time.deltaTime);
		mainSource.mute = (double)curState > 0.5;
		creditsSource.volume = curState;
		if (creditsChanged != creditsHolder.activeSelf)
		{
			creditsChanged = creditsHolder.activeSelf;
			if (creditsChanged)
			{
				creditsSource.Play();
			}
		}
	}
}
