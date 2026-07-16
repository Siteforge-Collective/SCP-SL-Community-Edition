using System;
using System.Runtime.CompilerServices;
using GameObjectPools;
using Mirror;
using PlayerRoles.PlayableScps.Scp079.Cameras;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079LostSignalHandler : SubroutineBase, IPoolSpawnable
	{
		[SerializeField]
		private float _ghostlightLockoutDuration;

		private Scp079CurrentCameraSync _curCamSync;

		private Scp079AuxManager _auxManager;

		private double _recoveryTime;

		private bool _prevLost;

		public bool Lost => false;

		public float RemainingTime => 0f;

		public event Action OnStatusChanged
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

		private void Update()
		{
		}

		protected override void Awake()
		{
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		public void ServerLoseSignal(float duration)
		{
		}

		public void SpawnObject()
		{
		}
	}
}
