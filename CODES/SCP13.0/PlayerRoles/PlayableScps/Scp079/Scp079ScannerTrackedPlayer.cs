using MapGeneration;
using RelativePositioning;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079ScannerTrackedPlayer
	{
		public readonly int PlayerHash;

		public readonly ReferenceHub Hub;

		private readonly HumanRole _role;

		private double _resetTime;

		private RelativePosition _centerPos;

		private const float EnemyProxSqr = 22500f;

		public bool IsCamping { get; private set; }

		public FacilityZone LastZone { get; private set; }

		public Vector3 PlyPos => default(Vector3);

		public Scp079ScannerTrackedPlayer(ReferenceHub hub)
		{
		}

		public void Update(float baselineRadius, float additiveRadius, float maxCampingTime)
		{
		}

		private bool CheckEnemy(ReferenceHub hub)
		{
			return false;
		}

		private void ResetPosition()
		{
		}
	}
}
