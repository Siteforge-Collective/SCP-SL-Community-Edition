using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class NewServerBrowser : MonoBehaviour
{
	public RectTransform ServerInfo;

	public Text LoadingText;

	public GameObject ServerTabs;

	private static bool _errorDirty;

	private static bool _refresh;

	private static bool _redownload;

	private static bool _threadStarted;

	private static string _errorMessage;

	private static readonly Thread _refreshThread;

	public static ServerListItem[] Servers;

	private ServerFilter _serverFilter;

	private static string LoadingErrorMessage
	{
		set
		{
		}
	}

	private void Start()
	{
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void OnDestroy()
	{
	}

	private void Update()
	{
	}

	public void Refresh()
	{
	}

	public void AuthCompleted()
	{
	}

	internal static void StopThread()
	{
	}

	private static void DownloadList()
	{
	}
}
