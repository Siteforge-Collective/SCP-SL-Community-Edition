using System;
using System.Runtime.CompilerServices;
using CursorManagement;
using Mirror;
using PlayerRoles.Ragdolls;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp049
{
	public abstract class RagdollAbilityBase<T> : KeySubroutine<T>, ICursorOverride where T : FpcStandardScp
	{
		private readonly AbilityCooldown _process;

		private Transform _ragdollTransform;

		private DynamicRagdoll _syncRagdoll;

		private byte _errorCode;

		private double _completionTime;

		public virtual CursorOverrideMode CursorOverride => default(CursorOverrideMode);

		public virtual bool LockMovement => false;

		protected override ActionName TargetKey => default(ActionName);

		public bool IsInProgress
		{
			get
			{
				return false;
			}
			private set
			{
			}
		}

		public float ProgressStatus => 0f;

		protected abstract float RangeSqr { get; }

		protected abstract float Duration { get; }

		protected BasicRagdoll CurRagdoll { get; private set; }

		public event Action<byte> OnErrorReceived
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

		public event Action OnStop
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

		public event Action OnStart
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

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}

		protected abstract void ServerComplete();

		protected abstract byte ServerValidateBegin(BasicRagdoll ragdoll);

		protected virtual void OnProgressSet()
		{
		}

		protected virtual byte ServerValidateCancel()
		{
			return 0;
		}

		protected virtual bool ServerValidateAny()
		{
			return false;
		}

		protected virtual void ClientProcessErrorCode(byte code)
		{
		}

		protected override void Update()
		{
		}

		protected void ClientTryStart()
		{
		}

		protected void ClientTryCancel()
		{
		}

		protected virtual bool ClientValidateBegin(BasicRagdoll raycastedRagdoll)
		{
			return false;
		}

		private bool IsCorpseNearby(Vector3 position, DynamicRagdoll ragdoll, out Transform ragdollPosition)
		{
			ragdollPosition = null;
			return false;
		}

		private bool CanFindCorpse(Transform tr, out BasicRagdoll ragdoll)
		{
			ragdoll = null;
			return false;
		}

		private bool IsCloseEnough(Vector3 position, Vector3 ragdollPosition)
		{
			return false;
		}
	}
}
