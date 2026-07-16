using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClassPresetChooser : MonoBehaviour
{
	[Serializable]
	public class PickerPreset
	{
		public string classID;

		public Texture icon;

		public int health;

		public float wSpeed;

		public float rSpeed;

		public float stamina;

		public Texture[] startingItems;

		public string en_additionalInfo;

		public string pl_additionalInfo;
	}

	public GameObject bottomMenuItem;

	public Transform bottomMenuHolder;

	public PickerPreset[] presets;

	private string curKey;

	private List<PickerPreset> curPresets;

	public Slider health;

	public Slider wSpeed;

	public Slider rSpeed;

	public RawImage[] startItems;

	public RawImage avatar;

	public TextMeshProUGUI addInfo;

	public void RefreshBottomItems(string key)
	{
	}

	private void Update()
	{
	}
}
