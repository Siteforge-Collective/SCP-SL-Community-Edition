using System.Collections.Generic;
using System.Runtime.InteropServices;
using Hazards;
using Mirror;
using PlayerRoles.PlayableScps.Subroutines;
using PlayerRoles.Subroutines;
using PlayerStatsSystem;
using RelativePositioning;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace PlayerRoles.PlayableScps.Scp939
{
	public class Scp939AmnesticCloudInstance : TemporaryHazard
	{
		private enum CloudState
		{
			Spawning = 0,
			Created = 1,
			Destroyed = 2
		}

		public static readonly List<Scp939AmnesticCloudInstance> ActiveInstances;

		private readonly AbilityCooldown _overallCooldown;

		private readonly Dictionary<uint, AbilityCooldown> _individualCooldown;

		private static readonly int HashRadiusPercent;

		private static readonly int HashStatusPercent;

		private Scp939AmnesticCloudAbility _cloud;

		private Scp939LungeAbility _lunge;

		private Scp939ClawAbility _claw;

		private Scp939Role _scpRole;

		private Transform _t;

		private Material _mat;

		private bool _abilitiesSet;

		private float _targetDuration;

		private float _lastHoldTime;

		private float _prevRange;

		private bool _localOwner;

		private bool _alreadyCreated;

		[SyncVar]
		private byte _syncHoldTime;

		[SyncVar]
		private byte _syncState;

		[SyncVar]
		private uint _syncOwner;

		[SyncVar]
		private RelativePosition _syncPos;

		[SerializeField]
		[Header("Balance")]
		private float _minHoldTime;

		[SerializeField]
		private float _maxHoldTime;

		[SerializeField]
		private AnimationCurve _rangeOverHeldTime;

		[SerializeField]
		private AnimationCurve _durationOverHeldTime;

		[SerializeField]
		private float _amnesiaDuration;

		[SerializeField]
		private float _pauseDuration;

		[SerializeField]
		[Header("Audiovisual")]
		private float _destroyTime;

		[SerializeField]
		private float _soundDropRate;

		[SerializeField]
		private float _sizeLerpTime;

		[SerializeField]
		private float _colorLerpTime;

		[SerializeField]
		private AudioSource _deploySound;

		[SerializeField]
		private AudioSource _chargeupSound;

		[SerializeField]
		private AnimationCurve _chargeupVolumeOverSize;

		[SerializeField]
		private DecalProjector _decalProjector;

		private CloudState State
		{
			get
			{
				return default(CloudState);
			}
			set
			{
			}
		}

		private float NormalizedHoldTime => 0f;

		protected override float HazardDuration => 0f;

		protected override float DecaySpeed => 0f;

		public Vector2 MinMaxTime => default(Vector2);

		public byte Network_syncHoldTime
		{
			get
			{
				return 0;
			}
			[param: In]
			set
			{
			}
		}

		public byte Network_syncState
		{
			get
			{
				return 0;
			}
			[param: In]
			set
			{
			}
		}

		public uint Network_syncOwner
		{
			get
			{
				return 0u;
			}
			[param: In]
			set
			{
			}
		}

		public RelativePosition Network_syncPos
		{
			get
			{
				return default(RelativePosition);
			}
			[param: In]
			set
			{
			}
		}

		[Server]
		public override void ServerDestroy()
		{
		}

		public override void OnStay(ReferenceHub player)
		{
		}

		protected override void Start()
		{
		}

		protected override void OnDestroy()
		{
		}

		protected override void Update()
		{
		}

		private void TryGetPlayer(out bool is939, out bool isOwner)
		{
			is939 = default(bool);
			isOwner = default(bool);
		}

		private void OnAttacked(AttackResult attackResult)
		{
		}

		private void OnAnyPlayerDamaged(ReferenceHub hub, DamageHandlerBase dhb)
		{
		}

		private void OnLungeStateChanged(Scp939LungeState state)
		{
		}

		private void PauseAll()
		{
		}

		private void SetAbilityCache()
		{
		}

		private void RefreshPosition(ReferenceHub owner)
		{
		}

		private void UpdateLocal()
		{
		}

		private void UpdateVisuals(float normalizedSize, float lerpTime)
		{
		}

		private void UpdateChargeup(float normalizedSize, bool isOwner)
		{
		}

		private void UpdateFade(bool isVisible)
		{
		}

		private void UpdateRadius(float normSize, float lerpTime)
		{
		}

		[Server]
		private void ServerUpdateSpawning()
		{
		}

		[Server]
		private void ServerUpdateDestroyed()
		{
		}

		[ClientRpc]
		private void RpcPlayCreateSound()
		{
		}

		[Server]
		public void ServerSetup(ReferenceHub owner)
		{
		}

		static Scp939AmnesticCloudInstance()
		{
		}

		protected void UserCode_RpcPlayCreateSound()
		{
		}

		protected static void InvokeUserCode_RpcPlayCreateSound(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
		}
	}
}
