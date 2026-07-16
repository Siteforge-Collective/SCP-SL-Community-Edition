using System;
using System.Collections.Generic;
using InventorySystem.Items.Pickups;
using Mirror;
using RelativePositioning;
using UnityEngine;

namespace InventorySystem.Items.ThrowableProjectiles
{
	public class Scp018Projectile : TimeGrenade
	{
		private static readonly CachedLayerMask BounceHitregMask;

		private static readonly CachedLayerMask FlybyHitregMask;

		private static readonly Collider[] HitregDetections;

		private Transform _tr;

		private float _lastVelocity;

		private double _activationTime;

		private bool _bypassBounceSoundCooldown;

		private Vector3 _prevTrPos;

		private RelativePosition? _prevFlybyPos;

		private HashSet<uint> _damagedPlayersSinceLastBounce;

		[SerializeField]
		private float _radius;

		[SerializeField]
		private float _maximumVelocity;

		[SerializeField]
		private float _onBounceVelocityAddition;

		[SerializeField]
		private float _activationVelocitySqr;

		[SerializeField]
		private AnimationCurve _damageOverVelocity;

		[SerializeField]
		private float _doorDamageMultiplier;

		[SerializeField]
		private float _scpDamageMultiplier;

		[SerializeField]
		private float _friendlyFireTime;

		[SerializeField]
		private float _bounceHitregRadius;

		[SerializeField]
		private float _flybyHitregRadius;

		[SerializeField]
		private ParticleSystem _trail;

		private float CurrentDamage => 0f;

		private bool IgnoreFriendlyFire => false;

		public override float MinSoundCooldown => 0f;

		protected override PickupPhysicsModule DefaultPhysicsModule => null;

		public Vector3 RecreatedVelocity { get; private set; }

		private void SetupModule()
		{
		}

		[ClientRpc]
		private void RpcPlayBounce(float velSqr)
		{
		}

		protected override void ProcessCollision(Collision collision)
		{
		}

		protected override void Update()
		{
		}

		protected override void ServerFuseEnd()
		{
		}

		[ClientRpc]
		internal override void SendPhysicsModuleRpc(ArraySegment<byte> arrSeg)
		{
		}

		internal void RegisterBounce(float velocity, Vector3 point)
		{
		}

		static Scp018Projectile()
		{
		}

		/*
		protected void UserCode_RpcPlayBounce__Single(float velSqr)
		{
		}

		protected static void InvokeUserCode_RpcPlayBounce__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
		}

		protected override void UserCode_SendPhysicsModuleRpc__ArraySegment_00601(ArraySegment<byte> arrSeg)
		{
		}

		protected new static void InvokeUserCode_SendPhysicsModuleRpc__ArraySegment_00601(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
		}
		*/
	}
}
