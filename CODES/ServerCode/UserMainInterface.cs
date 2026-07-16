public class UserMainInterface : global::UnityEngine.MonoBehaviour
{
	public static UserMainInterface singleton;

	public float LerpSpeed = 4f;

	private void Awake()
	{
		singleton = this;
	}

	private void Start()
	{
		ResolutionManager.RefreshScreen();
	}
}
