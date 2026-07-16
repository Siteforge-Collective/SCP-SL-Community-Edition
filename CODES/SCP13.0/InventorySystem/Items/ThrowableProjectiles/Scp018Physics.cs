using InventorySystem.Items.Pickups;
using Mirror;
using RelativePositioning;
using UnityEngine;
using Utils.Networking;

namespace InventorySystem.Items.ThrowableProjectiles
{
	public class Scp018Physics : PickupPhysicsModule
	{
		private struct BounceData
		{
			public float VerticalSpeed;

			public RelativePosition RelPos;

			public double Time;
		}

		public const int RpcSize = 19;

		private readonly OrderedBufferQueue<BounceData> _buffer;

		private readonly Scp018Projectile _scp018;

		private readonly ParticleSystem _trail;

		private readonly LayerMask _detectionMask;

		private readonly float _radius;

		private readonly float _maxVel;

		private readonly float _velPerBounce;

		private readonly bool _wasServer;

		private const int UpdateFrequency = 10;

		private const float PrecalcTime = 0.2f;

		private Vector3 _lastVelocity;

		private RelativePosition _lastPosition;

		private Vector3? _lastSafeOrigin;

		private double _lastTime;

		private bool _outOfBounds;

		private BounceData _prevBounce;

		private BounceData _nextBounce;

		protected override ItemPickupBase Pickup => null;

		public Vector3 Position => default(Vector3);

		public Scp018Physics(Scp018Projectile thrownScp018, ParticleSystem trail, float radius, float maxVel, float velPerBounce)
		{
		}

		public override void DestroyModule()
		{
		}

		internal override void ClientProcessRpc(NetworkReader rpcData)
		{
		}

		[Server]
		private void ServerUpdatePrediction()
		{
		}

		private float GetFreefallHeight(float elapsed)
		{
			return 0f;
		}

		private BounceData PrecomputeNextBounce()
		{
			return default(BounceData);
		}

		private void BounceTrajectory(Vector3 normal)
		{
		}

		private bool TrySphereCast(Vector3 origin, Vector3 dir, float maxDis, out RaycastHit hit)
		{
			hit = default(RaycastHit);
			return false;
		}
	}
}
