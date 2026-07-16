public class PlayButton : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		if (global::UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Facility")
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
