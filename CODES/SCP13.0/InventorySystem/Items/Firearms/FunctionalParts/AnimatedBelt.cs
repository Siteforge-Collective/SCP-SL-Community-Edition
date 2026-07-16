using System.Diagnostics;
using UnityEngine;

namespace InventorySystem.Items.Firearms.FunctionalParts
{
	public class AnimatedBelt : FunctionalFirearmPart
	{
		[SerializeField]
		private int _idleLayer;

		[SerializeField]
		private float _minimalCooldown;

		[SerializeField]
		private GameObject[] _bullets;

		private int _prevBullets;

		private bool _wasActive;

		private readonly Stopwatch _stopwatch;

		private FirearmStatus CurStatus => default(FirearmStatus);

		private void OnEnable()
		{
		}

		private void Start()
		{
		}

		private void LateUpdate()
		{
		}

		private int GetTargetAmmo(int curTag, int prev, FirearmStatus status)
		{
			return 0;
		}

		private void OnDestroy()
		{
		}

		private void OnShot()
		{
		}

		private void Refresh(int curAmmo)
		{
		}
	}
}
