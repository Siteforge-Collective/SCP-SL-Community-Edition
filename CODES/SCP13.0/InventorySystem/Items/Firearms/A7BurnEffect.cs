using UnityEngine;

namespace InventorySystem.Items.Firearms
{
	public class A7BurnEffect : MonoBehaviour
	{
		[SerializeField]
		private int _maxDuration;

		[SerializeField]
		private int _perShotDuration;

		[SerializeField]
		private float _forwardOffset;

		[SerializeField]
		private float _radius;

		private Firearm _firearm;

		private void Awake()
		{
		}

		private void OnFired()
		{
		}
	}
}
