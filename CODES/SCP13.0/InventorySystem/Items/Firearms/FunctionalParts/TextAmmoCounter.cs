using TMPro;
using UnityEngine;

namespace InventorySystem.Items.Firearms.FunctionalParts
{
	public class TextAmmoCounter : FunctionalFirearmPart
	{
		[SerializeField]
		private TMP_Text _targetText;

		[SerializeField]
		private int _digits;

		private string _toStringFormat;

		private string _unloaded;

		private Vector3 _targetScale;

		private byte _activeCooldown;

		private void OnDisable()
		{
		}

		private void OnEnable()
		{
		}

		private void Awake()
		{
		}

		private void Start()
		{
		}

		private void LateUpdate()
		{
		}
	}
}
