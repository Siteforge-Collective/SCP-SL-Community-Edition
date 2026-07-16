public class LCZ_LabelManager : global::UnityEngine.MonoBehaviour
{
	[global::System.Serializable]
	public class LCZ_Label_Preset
	{
		public string nameToContain;

		public global::UnityEngine.Material mat;
	}

	public LCZ_LabelManager.LCZ_Label_Preset[] chars;

	public global::UnityEngine.Material[] numbers;

	private global::System.Collections.Generic.List<LCZ_Label> _labels = new global::System.Collections.Generic.List<LCZ_Label>();

	private readonly global::System.Collections.Generic.List<global::UnityEngine.GameObject> _rooms = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>();

	private void Start()
	{
		global::MapGeneration.SeedSynchronizer.OnMapGenerated += RefreshLabels;
		_labels = global::System.Linq.Enumerable.ToList(global::UnityEngine.Object.FindObjectsOfType<LCZ_Label>());
		for (int i = 0; i < base.transform.childCount; i++)
		{
			if (base.transform.GetChild(i).name.StartsWith("Root_", global::System.StringComparison.Ordinal) || base.transform.GetChild(i).name.StartsWith("LCZ_", global::System.StringComparison.Ordinal))
			{
				_rooms.Add(base.transform.GetChild(i).gameObject);
			}
		}
	}

	private void OnDestroy()
	{
		global::MapGeneration.SeedSynchronizer.OnMapGenerated -= RefreshLabels;
	}

	public void RefreshLabels()
	{
		foreach (LCZ_Label label in _labels)
		{
			bool flag = true;
			global::UnityEngine.Transform transform = label.transform;
			global::UnityEngine.Vector3 position = transform.position;
			position += transform.forward * 10f;
			foreach (global::UnityEngine.GameObject room in _rooms)
			{
				if (!(global::UnityEngine.Vector3.Distance(room.transform.position, position) < 10f))
				{
					continue;
				}
				LCZ_LabelManager.LCZ_Label_Preset[] array = chars;
				foreach (LCZ_LabelManager.LCZ_Label_Preset lCZ_Label_Preset in array)
				{
					if (!global::NorthwoodLib.StringUtils.Contains(room.name, lCZ_Label_Preset.nameToContain, global::System.StringComparison.Ordinal))
					{
						continue;
					}
					flag = false;
					int num = 0;
					if (global::NorthwoodLib.StringUtils.Contains(room.name, "(", global::System.StringComparison.Ordinal))
					{
						try
						{
							string text = room.name.Remove(0, room.name.IndexOf('(') + 1);
							num = int.Parse(text.Remove(text.IndexOf(')')));
						}
						catch
						{
						}
					}
					label.Refresh(lCZ_Label_Preset.mat, numbers[num]);
				}
			}
			if (flag)
			{
				label.Refresh(chars[0].mat, numbers[0]);
			}
		}
	}
}
