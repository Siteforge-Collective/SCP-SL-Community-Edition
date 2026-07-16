using System.Collections.Generic;
using UnityEngine;

public class ClutterSpawner : MonoBehaviour
{
	[SerializeField]
	private List<ClutterStruct> clutters;

	private static bool noHolidays;

	public static bool IsHolidayActive(Holidays holiday)
	{
		return false;
	}

	private void Awake()
	{
	}

	private void OnDestroy()
	{
	}

	private void Start()
	{
	}

	public void GenerateClutter()
	{
	}
}
