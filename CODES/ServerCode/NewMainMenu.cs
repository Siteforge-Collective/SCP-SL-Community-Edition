public class NewMainMenu : global::UnityEngine.MonoBehaviour
{
	public void QuitGame()
	{
		Shutdown.Quit();
	}

	public void Refresh()
	{
		SimpleMenu.LoadCorrectScene();
	}

	private void Start()
	{
	}
}
