using UnityEngine;

namespace InventorySystem.Items.Usables.Scp244
{
	public class Scp244Lid : MonoBehaviour
	{
		[SerializeField]
		private Scp244DeployablePickup _pickup;

		[SerializeField]
		private Vector3 _offset;

		[SerializeField]
		private float _upDot;

		[SerializeField]
		private AudioSource _pressureSound;

		private void Update()
		{
		}
	}
}
