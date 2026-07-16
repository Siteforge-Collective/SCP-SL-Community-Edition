using UnityEngine;

namespace InventorySystem.Items.Firearms.FunctionalParts
{
	public class FirearmFlashlight : FunctionalFirearmPart
	{
		[SerializeField]
		private Light _targetSource;

		[SerializeField]
		private Renderer[] _targetRenderers;

		[SerializeField]
		private Material _onMat;

		[SerializeField]
		private Material _offMat;

		private int _prevEnabled;

		private void Update()
		{
		}

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		public void SetRange(float range)
		{
		}

		public void SetStatus(bool isEnabled)
		{
		}
	}
}
