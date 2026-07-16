using GameObjectPools;
using MapGeneration;
using Mirror;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp106
{
	public class Scp106HuntersAtlasAbility : Scp106VigorAbilityBase, IPoolResettable
	{
		public const float CostPerMeter = 0.019f;

		private const ActionName SelectKey = ActionName.Shoot;

		private const int SyncAccuracy = 50;

		private const float HeightOffset = 0.2f;

		private const float NormalMultiplier = 1.1f;

		private const float GroundDetectorHeight = 2f;

		private const float DissolvePercent = 0.5f;

		private const float MaxRetakeRange = 15f;

		private const float HeightTolerance = 400f;

		private const float DoorHeightTolerance = 50f;

		private const int OverlapSphereMaxDetections = 8;

		private const float MinVigor = 0.25f;

		private Vector3 _syncPos;

		private RoomIdentifier _syncRoom;

		private bool _submerged;

		private bool _dissolveAnim;

		private float _lastDissolveAmount;

		private float _estimatedCost;

		private static readonly Collider[] DetectionsNonAlloc;

		private static readonly float DebugDuration;

		private static bool _maskSet;

		private static int _mask;

		private static int DetectionMask => 0;

		protected override ActionName TargetKey => default(ActionName);

		public override bool IsSubmerged => false;

		protected override bool KeyPressable => false;

		private void SetSubmerged(bool val)
		{
		}

		private void UpdateAny()
		{
		}

		private void UpdateClientside()
		{
		}

		private void UpdateServerside()
		{
		}

		private Vector3 GetSafePosition()
		{
			return default(Vector3);
		}

		private Vector3 ClosestDoorPosition(Vector3 doorPos)
		{
			return default(Vector3);
		}

		private bool TrySphereCast(Color debugColor, Vector3 origin, Vector3 dir, float radius, float height, float maxDis, out Vector3 pos)
		{
			pos = default(Vector3);
			return false;
		}

		private void DebugHitPoint(Vector3 point, Color debugColor)
		{
		}

		protected override void Update()
		{
		}

		public override void ClientWriteCmd(NetworkWriter writer)
		{
		}

		public override void ServerProcessCmd(NetworkReader reader)
		{
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		public override void ResetObject()
		{
		}
	}
}
