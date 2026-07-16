using System;
using Mirror;
using PlayerRoles.Subroutines;
using UnityEngine;
using UnityEngine.Rendering;

namespace PlayerRoles.PlayableScps.Scp173
{
	public class Scp173TeleportAbility : KeySubroutine<Scp173Role>
	{
		[Flags]
		private enum CmdTeleportData
		{
			Aiming = 1,
			WantsToTeleport = 2
		}

		private const float BlinkDistance = 8f;

		private const float BreakneckDistanceMultiplier = 1.8f;

		private const float KillRadiusSqr = 1.66f;

		private const float KillHeight = 2.2f;

		private const float KillBacktracking = 0.4f;

		private const float ClientDistanceAddition = 0.1f;

		private const int GlassLayerMask = 16384;

		private const float GlassDestroyRadius = 0.8f;

		private static readonly Collider[] DetectedColliders;

		private Scp173MovementModule _fpcModule;

		private Scp173ObserversTracker _observersTracker;

		private Scp173BreakneckSpeedsAbility _breakneckSpeedsAbility;

		private Scp173BlinkTimer _blinkTimer;

		private Scp173AudioPlayer _audioSubroutine;

		private bool _isAiming;

		private float _targetDis;

		private Vector3 _tpPosition;

		private float _lastBlink;

		private CmdTeleportData _cmdData;

		[SerializeField]
		private Scp173TeleportIndicator _tpIndicator;

		[SerializeField]
		private AnimationCurve _blinkIntensity;

		[SerializeField]
		private Volume _blinkEffect;

		private float EffectiveBlinkDistance => 0f;

		protected override ActionName TargetKey => default(ActionName);

		public ReferenceHub BestTarget => null;

		protected override void Awake()
		{
		}

		protected override void Update()
		{
		}

		private void UpdateAiming(bool wantsToTeleport)
		{
		}

		private bool TryBlink(float maxDis)
		{
			return false;
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

		public override void ResetObject()
		{
		}

		private bool HasDataFlag(CmdTeleportData ctd)
		{
			return false;
		}
	}
}
