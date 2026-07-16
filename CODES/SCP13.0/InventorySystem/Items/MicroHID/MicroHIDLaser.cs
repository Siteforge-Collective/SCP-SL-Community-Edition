using System.Diagnostics;
using GameObjectPools;
using UnityEngine;

namespace InventorySystem.Items.MicroHID
{
	public class MicroHIDLaser : MonoBehaviour, IPoolSpawnable
	{
		[SerializeField]
		private MicroHIDViewmodel _hidViewmodel;

		[SerializeField]
		private Transform _targetTransform;

		[SerializeField]
		private GameObject _rootGameObject;

		[SerializeField]
		private Light _targetLight;

		[SerializeField]
		private Transform _forwardTransform;

		[SerializeField]
		private Transform _gauge;

		[SerializeField]
		private LayerMask _laserMask;

		[SerializeField]
		private float _laserMaxDistance;

		[SerializeField]
		private AnimationCurve _laserScaleOverDistance;

		[SerializeField]
		private float _scalingSpeed;

		private float _currentScale;

		private float _lastFiringScale;

		private bool _laserTriggered;

		private bool _thirdpersonMode;

		private ushort _serial;

		private readonly Stopwatch _firingWarmup;

		private bool Firing => false;

		private void Start()
		{
		}

		private void Update()
		{
		}

		public void SpawnObject()
		{
		}

		public void TriggerLaser()
		{
		}
	}
}
