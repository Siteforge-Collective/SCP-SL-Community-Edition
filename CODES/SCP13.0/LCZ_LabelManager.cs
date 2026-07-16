using System;
using System.Collections.Generic;
using UnityEngine;

public class LCZ_LabelManager : MonoBehaviour
{
	[Serializable]
	public class LCZ_Label_Preset
	{
		public string nameToContain;

		public Material mat;
	}

	public LCZ_Label_Preset[] chars;

	public Material[] numbers;

	private List<LCZ_Label> _labels;

	private readonly List<GameObject> _rooms;

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}

	public void RefreshLabels()
	{
	}
}
