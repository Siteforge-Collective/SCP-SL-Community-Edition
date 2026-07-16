using UnityEngine;

namespace InventorySystem.Items.ToggleableLights.Lantern
{
	public class LanternJointFix : MonoBehaviour
	{
		private Quaternion _initialLocalRotation;

		private Vector3 _initialLocalPosition;

		private Quaternion _localRotationOnDisable;

		private Vector3 _localPositionOnDisable;

		private bool _hasDisabled;

		private Transform _transform;

		private void Awake()
		{
		}

		private void OnDisable()
		{
		}

		private void Update()
		{
		}
	}
}
