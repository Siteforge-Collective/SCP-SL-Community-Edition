using UnityEngine;

namespace InventorySystem.Items.ThrowableProjectiles
{
	public class PhantomProjectile : MonoBehaviour
	{
		public Rigidbody Rigidbody;

		[SerializeField]
		private float _minimalExistenceTime;

		[SerializeField]
		private float _transitionTime;

		[SerializeField]
		private Vector3 _startScale;

		[SerializeField]
		private Vector3 _targetScale;

		private const float AutoDestroyTime = 0.5f;

		private ushort _projectileSerial;

		private float _scaleFactor;

		private float _transitionFactor;

		private float _replaceTime;

		private bool _hasPickup;

		private ThrownProjectile _pickupToReplace;

		private float CurTime => 0f;

		public void Init(ushort serial)
		{
		}

		public void Activate(Transform cam, Vector3 relativePosition)
		{
		}

		private void OnDestroy()
		{
		}

		private void Update()
		{
		}

		private void Replace()
		{
		}

		private void OnSpawned(ThrownProjectile projectile)
		{
		}
	}
}
