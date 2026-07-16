using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using MapGeneration;
using Mirror;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp939
{
	public class Scp939AmnesticCloudAbility : KeySubroutine<Scp939Role>
	{
		private static readonly Dictionary<RoomName, float> WhitelistedFloors;

		private const float FloorTolerance = 0.2f;

		private bool _targetState;

		private float _sendDuration;

		private Scp939FocusAbility _focusAbility;

		private readonly Stopwatch _beginHeldSw;

		[SerializeField]
		private Scp939AmnesticCloudInstance _instancePrefab;

		[SerializeField]
		private float _failedCooldown;

		[SerializeField]
		private float _placedCooldown;

		public readonly AbilityCooldown Duration;

		public readonly AbilityCooldown Cooldown;

		public readonly AbilityCooldown HudIndicatorMin;

		public readonly AbilityCooldown HudIndicatorMax;

		protected override ActionName TargetKey => default(ActionName);

		protected override bool KeyPressable => false;

		public float HoldDuration => 0f;

		public bool TargetState
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public event Action<Scp939HudTranslation> OnDeployFailed
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

		private void OnStateEnabled()
		{
		}

		private void OnStateDisabled()
		{
		}

		protected override void Awake()
		{
		}

		protected override void Update()
		{
		}

		protected override void OnKeyDown()
		{
		}

		protected override void OnKeyUp()
		{
		}

		public bool ValidateFloor()
		{
			return false;
		}

		public void ClientCancel(Scp939HudTranslation reason)
		{
		}

		public void ServerConfirmPlacement(float duration)
		{
		}

		public void ServerFailPlacement()
		{
		}

		public override void ClientWriteCmd(NetworkWriter writer)
		{
		}

		public override void ResetObject()
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

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}
	}
}
