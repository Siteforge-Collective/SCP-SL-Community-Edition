using System.Collections.Generic;
using Mirror;
using PlayerRoles.Ragdolls;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp049
{
	public abstract class RagdollIndicatorsBase<T> : StandardSubroutine<T> where T : PlayerRoleBase
	{
		private readonly struct Indicator
		{
			public readonly GameObject Instance;

			private readonly CanvasGroup _group;

			public void SetAlpha(float f)
			{
			}

			public Indicator(GameObject inst)
			{
				Instance = null;
				_group = null;
			}
		}

		private enum ListSyncRpcType
		{
			FullResync = 0,
			Add = 1,
			Remove = 2
		}

		[SerializeField]
		private float _showDelay;

		[SerializeField]
		private float _fullOpacityDistance;

		[SerializeField]
		private float _visibleDistance;

		[SerializeField]
		private GameObject _indicatorTemplate;

		[SerializeField]
		private Vector3 _posOffset;

		private readonly Dictionary<uint, Indicator> _indicatorInstances;

		private readonly HashSet<BasicRagdoll> _availableRagdolls;

		private ListSyncRpcType _rpcType;

		private uint _syncRagdoll;

		protected virtual void Update()
		{
		}

		protected abstract bool ValidateRagdoll(BasicRagdoll ragdoll);

		private bool ServerCheckNew()
		{
			return false;
		}

		private void ServerRevalidateOld()
		{
		}

		private void UpdateIndicators()
		{
		}

		private void OnHudToggled(bool newState)
		{
		}

		private void OnSpectatorTargetChanged()
		{
		}

		private void RefreshIndicator(Vector3 camPos, BasicRagdoll ragdoll)
		{
		}

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		private void ClientProcessRpcSingularNetId(uint netId, ListSyncRpcType rpcType)
		{
		}

		private void OnRoleChanged(ReferenceHub hub, PlayerRoleBase prevRole, PlayerRoleBase newRole)
		{
		}

		private void ServerSendRpc(ListSyncRpcType rpcType, BasicRagdoll ragdoll)
		{
		}

		private void ClientRemoveRagdoll(BasicRagdoll ragdoll)
		{
		}
	}
}
