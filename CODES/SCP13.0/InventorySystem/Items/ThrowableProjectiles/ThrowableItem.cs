using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using CustomPlayerEffects;
using InventorySystem.Drawers;
using InventorySystem.Items.Pickups;
using UnityEngine;

namespace InventorySystem.Items.ThrowableProjectiles
{
	public class ThrowableItem : ItemBase, IEquipDequipModifier, IItemDescription, IItemNametag, IItemAlertDrawer, IItemDrawer
	{
		[Serializable]
		public struct ProjectileSettings
		{
			public float StartVelocity;

			public float UpwardsFactor;

			public float TriggerTime;

			public Vector3 StartTorque;

			public Vector3 RelativePosition;
		}

		public ThrownProjectile Projectile;

		public PhantomProjectile Phantom;

		public ProjectileSettings WeakThrowSettings;

		public ProjectileSettings FullThrowSettings;

		public float ThrowingAnimTime;

		public float CancelAnimTime;

		public AudioClip ThrowClip;

		public AudioClip BeginClip;

		public AudioClip CancelClip;

		public readonly Stopwatch ThrowStopwatch;

		public readonly Stopwatch CancelStopwatch;

		[SerializeField]
		private float _weight;

		[SerializeField]
		private float _pinPullTime;

		[SerializeField]
		private float _postThrownAnimationTime;

		[SerializeField]
		private bool _repickupable;

		private float _destroyTime;

		private bool _tryFire;

		private bool _alreadyFired;

		private bool _fireWeak;

		private bool _phantomPropelled;

		private bool _messageSent;

		private KeyCode _primaryKey;

		private KeyCode _secondaryKey;

		private PhantomProjectile _phantom;

		private Vector3 _releaseSpeed;

		private Scp1853 _scp1853;

		private const float ServerTimeTolerance = 0.8f;

		private const float MaxTraceTime = 0.1f;

		private const float MaxAheadTime = 0.2f;

		private const float HintBlinkRate = 9f;

		private const float HintBlinkStartTime = 1f;

		private const float HintBlinkTotalTime = 0.7f;

		private const ActionName CancelAction = ActionName.Reload;

		private static readonly Stopwatch TriggerDelay;

		public override float Weight => 0f;

		public bool AllowHolster => false;

		public bool AllowEquip => false;

		public string AlertText => null;

		public string Description => null;

		public string Name => null;

		public float ScaledThrowElapsed => 0f;

		public float ScaledCancelElapsed => 0f;

		private float CurrentTimeTolerance => 0f;

		private bool ReadyToThrow => false;

		private bool ReadyToCancel => false;

		private KeyCode CancelKey => default(KeyCode);

		private float SpeedMultiplier => 0f;

		public event Action<ThrowableNetworkHandler.RequestType> OnRequestSent
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		public override void OnAdded(ItemPickupBase pickup)
		{
		}

		public override void EquipUpdate()
		{
		}

		public override void OnRemoved(ItemPickupBase pickup)
		{
		}

		public override void OnHolstered()
		{
		}

		private void ClientUpdatePostFire()
		{
		}

		private void ClientUpdateTryFire()
		{
		}

		private void ClientUpdateAiming()
		{
		}

		private void ClientUpdateIdle()
		{
		}

		private bool ClientTryCancel()
		{
			return false;
		}

		private void PlaySound(AudioClip clip)
		{
		}

		private void UpdateServer()
		{
		}

		private void PropelBody(Rigidbody rb, Vector3 torque, Vector3 relativeVelocity, float forceAmount, float upwardFactor)
		{
		}

		private void ServerThrow(float forceAmount, float upwardFactor, Vector3 torque, Vector3 startVel)
		{
		}

		public void ServerProcessThrowConfirmation(bool fullForce, Vector3 startPos, Quaternion startRot, Vector3 startVel)
		{
		}

		public void ServerProcessInitiation()
		{
		}

		public void ServerProcessCancellation()
		{
		}
	}
}
