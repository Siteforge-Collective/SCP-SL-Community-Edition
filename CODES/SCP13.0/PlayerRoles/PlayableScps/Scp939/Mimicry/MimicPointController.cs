using System;
using System.Runtime.CompilerServices;
using Mirror;
using PlayerRoles.Subroutines;
using RelativePositioning;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp939.Mimicry
{
	public class MimicPointController : StandardSubroutine<Scp939Role>
	{
		private enum RpcStateMsg
		{
			None = 0,
			PlacedByUser = 25,
			RemovedByUser = 26,
			DestroyedByDistance = 27
		}

		[SerializeField]
		private Renderer _mimicPointIcon;

		private bool _active;

		private RelativePosition _syncPos;

		private RpcStateMsg _syncMessage;

		private readonly AbilityCooldown _cooldown;

		private const float CooldownDuration = 0.2f;

		[field: SerializeField]
		public Transform MimicPointTransform { get; private set; }

		[field: SerializeField]
		public float MaxDistance { get; private set; }

		public bool Active
		{
			get
			{
				return false;
			}
			private set
			{
			}
		}

		public float Distance => 0f;

		private bool Visible => false;

		public event Action<Scp939HudTranslation> OnMessageReceived
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

		private void UpdateMimicPoint()
		{
		}

		private void UpdateIcon()
		{
		}

		private void OnHubAdded(ReferenceHub hub)
		{
		}

		private void OnDestroy()
		{
		}

		protected override void Awake()
		{
		}

		public override void ResetObject()
		{
		}

		public void ClientToggle()
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
	}
}
