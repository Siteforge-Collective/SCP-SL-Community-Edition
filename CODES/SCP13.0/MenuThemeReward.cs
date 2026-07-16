using System;
using System.Collections.Generic;
using UnityEngine;

public class MenuThemeReward : MonoBehaviour
{
	[Serializable]
	private struct MenuUnlockableTheme
	{
		public string Name;

		public AudioClip Theme;

		public string AchievementID;

		public Holidays Holiday;
	}

	private static MenuThemeReward _instance;

	[SerializeField]
	private List<MenuUnlockableTheme> _unlockableThemes;

	public static bool IsHolidayThemeActive { get; private set; }

	public static void LoadAchievementThemes()
	{
	}

	private void Awake()
	{
	}
}
