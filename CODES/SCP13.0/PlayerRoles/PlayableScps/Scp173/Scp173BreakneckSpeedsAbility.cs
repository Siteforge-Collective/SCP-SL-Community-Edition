using System;
using System.Diagnostics;
using Mirror;
using PlayerRoles.Subroutines;
using UnityEngine;
using UnityEngine.Rendering;

namespace PlayerRoles.PlayableScps.Scp173
{
	public class Scp173BreakneckSpeedsAbility : KeySubroutine<Scp173Role>
	{
		private const float RechargeTime = 40f;

		private const float StareLimit = 10f;

		private const float MinimalTime = 1f;

		private readonly Stopwatch _duration;

		private float _disableTime;

		private Scp173ObserversTracker _observersTracker;

		[SerializeField]
		private Volume _ppVolume;

		[SerializeField]
		private float _ppLerpSpeed;

		public readonly AbilityCooldown Cooldown;

		public Action OnToggled;

		private float Elapsed => 0f;

		public bool IsActive
		{
			get
			{
				return false;
			}
			private set
			{
			}
		}

		protected override ActionName TargetKey => default(ActionName);

		private void UpdateServerside()
		{
		}

		protected override void OnKeyDown()
		{
		}

		protected override void Update()
		{
		}

		protected override void Awake()
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

		public override void SpawnObject()
		{
		}
	}
}
