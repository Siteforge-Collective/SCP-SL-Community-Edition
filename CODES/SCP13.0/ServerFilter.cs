using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerFilter : MonoBehaviour
{
	public InputField NameFilterField;

	public Sprite[] SelectBoxes;

	public Image[] FilterImages;

	public ServerTab CurrentTab;

	public List<ServerListItem> FilteredListItems;

	private static FilterSettings _curFilters;

	private NewServerBrowser _browser;

	public int ScrollStartPoint;

	public ServerElementButton[] PremadeElements;

	private void Awake()
	{
	}

	public void ChangeTab(int tab)
	{
	}

	public void ReapplyFilters(bool forceCleanup = false)
	{
	}

	public void DisplayServers()
	{
	}

	private void ChangeTab(ServerTab tab)
	{
	}

	public void ApplyNameFilter()
	{
	}

	private void ApplyNameFilter(string nameFilter)
	{
	}

	public void Filters(int id)
	{
	}

	private static bool CheckCheckboxes(ServerListItem server)
	{
		return false;
	}

	private static void SetNameFilterPoints(int id)
	{
	}
}
