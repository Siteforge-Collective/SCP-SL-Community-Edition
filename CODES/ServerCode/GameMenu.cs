public class GameMenu : global::ToggleableMenus.SimpleToggleableMenu
{
	public void Disconnect()
	{
		if (global::Mirror.NetworkServer.active)
		{
			global::Mirror.NetworkManager.singleton.StopHost();
		}
		else
		{
			global::Mirror.NetworkManager.singleton.StopClient();
		}
	}

	public void Exit()
	{
		IsEnabled = false;
	}
}
