namespace Interactables.Interobjects
{
	public class DummyDoor : global::Interactables.Interobjects.DoorUtils.DoorVariant, global::Interactables.Interobjects.DoorUtils.IDamageableDoor, global::Interactables.Interobjects.DoorUtils.INonInteractableDoor
	{
		public bool IsDestroyed
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		[field: global::UnityEngine.SerializeField]
		public bool IgnoreLockdowns { get; private set; }

		[field: global::UnityEngine.SerializeField]
		public bool IgnoreRemoteAdmin { get; private set; }

		public override void LockBypassDenied(ReferenceHub ply, byte colliderId)
		{
		}

		public override void PermissionsDenied(ReferenceHub ply, byte colliderId)
		{
		}

		public override bool AllowInteracting(ReferenceHub ply, byte colliderId)
		{
			return false;
		}

		public override bool AnticheatPassageApproved()
		{
			return false;
		}

		public override float GetExactState()
		{
			return TargetState ? 1 : 0;
		}

		public float GetHealthPercent()
		{
			return 0f;
		}

		public override bool IsConsideredOpen()
		{
			return TargetState;
		}

		public bool ServerDamage(float hp, global::Interactables.Interobjects.DoorUtils.DoorDamageType type)
		{
			return false;
		}

		public void ClientDestroyEffects()
		{
		}

		private void MirrorProcessed()
		{
		}
	}
}
