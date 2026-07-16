public class ClutterSpawner : global::UnityEngine.MonoBehaviour
{
	[global::UnityEngine.SerializeField]
	private global::System.Collections.Generic.List<ClutterStruct> clutters = new global::System.Collections.Generic.List<ClutterStruct>();

	private static bool noHolidays;

	public static bool IsHolidayActive(Holidays holiday)
	{
		global::System.DateTime utcNow = global::System.DateTime.UtcNow;
		if (noHolidays && holiday == Holidays.NoHoliday)
		{
			return true;
		}
		switch (holiday)
		{
		case Holidays.Always:
			return true;
		case Holidays.AprilFools:
			if (utcNow.Day == 1 && utcNow.Month == 4)
			{
				return !noHolidays;
			}
			return false;
		case Holidays.FirstHalfOfApril:
			if (utcNow.Day <= 15 && utcNow.Month == 4)
			{
				return !noHolidays;
			}
			return false;
		case Holidays.October:
			if (utcNow.Month == 10)
			{
				return !noHolidays;
			}
			return false;
		case Holidays.December:
			if (utcNow.Month == 12)
			{
				return !noHolidays;
			}
			return false;
		case Holidays.OctoberOrDecember:
			if (utcNow.Month == 10 || utcNow.Month == 12)
			{
				return !noHolidays;
			}
			return false;
		case Holidays.Christmas:
			if (utcNow.Day >= 24 && utcNow.Month == 12)
			{
				return !noHolidays;
			}
			return false;
		case Holidays.Halloween:
			if ((utcNow.Day >= 28 && utcNow.Month == 10) || (utcNow.Day <= 3 && utcNow.Month == 11))
			{
				return !noHolidays;
			}
			return false;
		case Holidays.NoHoliday:
			return true;
		default:
			return false;
		}
	}

	private void Awake()
	{
		global::MapGeneration.SeedSynchronizer.OnMapGenerated += GenerateClutter;
	}

	private void OnDestroy()
	{
		global::MapGeneration.SeedSynchronizer.OnMapGenerated -= GenerateClutter;
	}

	private void Start()
	{
		noHolidays = global::GameCore.ConfigFile.ServerConfig.GetBool("no_holidays");
	}

	public void GenerateClutter()
	{
		for (int num = clutters.Count - 1; num >= 0; num--)
		{
			ClutterStruct clutterStruct = clutters[num];
			global::GameCore.Console.AddDebugLog("MGCLTR", "Checking spawn conditions for clutter struct \"" + clutterStruct.descriptor + "\" on object \"" + base.gameObject.name + "\"", MessageImportance.LeastImportant, nospace: true);
			bool flag = true;
			if ((bool)clutterStruct.clutterComponent && !clutterStruct.clutterComponent.spawned)
			{
				if (clutterStruct.chanceToSpawn <= 0f)
				{
					flag = false;
				}
				else if ((float)global::UnityEngine.Random.Range(1, 101) > clutterStruct.chanceToSpawn)
				{
					flag = false;
				}
				else if (!clutterStruct.invertTimespan)
				{
					bool flag2 = false;
					Holidays[] validTimespan = clutterStruct.validTimespan;
					for (int i = 0; i < validTimespan.Length; i++)
					{
						if (IsHolidayActive(validTimespan[i]))
						{
							flag2 = true;
						}
					}
					flag = flag2;
				}
				else
				{
					Holidays[] validTimespan = clutterStruct.validTimespan;
					for (int i = 0; i < validTimespan.Length; i++)
					{
						if (IsHolidayActive(validTimespan[i]))
						{
							flag = false;
						}
					}
				}
				if (flag)
				{
					clutterStruct.clutterComponent.SpawnClutter();
				}
				else
				{
					clutterStruct.clutterComponent.gameObject.SetActive(value: false);
					global::UnityEngine.Object.Destroy(clutterStruct.clutterComponent.holderObject);
				}
			}
		}
	}
}
