using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using InventorySystem.Items.Pickups;
using Mirror;
using PlayerStatsSystem;
using UnityEngine;

namespace InventorySystem.Items.Usables.Scp244
{
	public class Scp244DeployablePickup : CollisionDetectionPickup, IDestructible
	{
		private const float SquaredDisUpdateDiff = 1f;

		private const float ForceBoundsUpdateSqrtDiff = 100f;

		private const float UpdateCooldownTime = 2.2f;

		private const int VertsPerFrame = 30;

		private const float ParticleSize = 3.5f;

		public static readonly HashSet<Scp244DeployablePickup> Instances;

		public float MaxDiameter;

		public AnimationCurve FogDistanceCurve;

		public AnimationCurve FogLerpCurve;

		[SyncVar]
		private byte _syncSizePercent;

		[SyncVar]
		private byte _syncState;

		[SerializeField]
		private AnimationCurve _growSpeedOverLifetime;

		[SerializeField]
		private float _timeToDecay;

		[SerializeField]
		private float _transitionDistance;

		[SerializeField]
		private float _fullSubmergeDistance;

		[SerializeField]
		private GameObject _visibleModel;

		[SerializeField]
		private float _minimalInfluenceDistance;

		[SerializeField]
		private float _activationDot;

		[SerializeField]
		private float _health;

		[SerializeField]
		private float _deployedPickupTime;

		[SerializeField]
		private float _heightRadiusRatio;

		[SerializeField]
		private ParticleSystem _mainEffect;

		[SerializeField]
		private GameObject _destroyedModel;

		[SerializeField]
		private Mesh _referenceMesh;

		[SerializeField]
		private AnimationCurve _emissionOverPercent;

		[SerializeField]
		private AnimationCurve _sizeOverDiameter;

		[SerializeField]
		private AudioClip[] _destroyClips;

		[SerializeField]
		private AudioSource _emissionSoundSource;

		private Vector3[] _templateVerticles;

		private Vector3[] _updatedVerticles;

		private int _meshVertsCount;

		private int _particleTimer;

		private Mesh _generatedMesh;

		private Vector2 _initialSize;

		private Vector3 _previousPos;

		private float _lastActiveSize;

		private float _lastUpdateTime;

		private bool _conditionsSet;

		private readonly Stopwatch _lifeTime;

		private float GrowSpeed => 0f;

		private float TimeToGrow => 0f;

		private float CurTime => 0f;

		private Rigidbody Rb => null;

		public bool ModelDestroyed => false;

		public float CurrentDiameter => 0f;

		public Bounds CurrentBounds { get; private set; }

		public float CurrentSizePercent { get; private set; }

		public Scp244TransferCondition[] Conditions { get; private set; }

		public Scp244State State
		{
			get
			{
				return default(Scp244State);
			}
			set
			{
			}
		}

		public uint NetworkId => 0u;

		public Vector3 CenterOfMass => default(Vector3);

		private void Update()
		{
		}

		protected override void Awake()
		{
		}

		protected override void Start()
		{
		}

		protected override void OnDestroy()
		{
		}

		private void UpdateCurrentRoom()
		{
		}

		private void UpdateConditions()
		{
		}

		private void UpdateRange()
		{
		}

		private void SetupEffects()
		{
		}

		private void UpdateEffects()
		{
		}

		public float FogPercentForPoint(Vector3 worldPoint)
		{
			return 0f;
		}

		public bool Damage(float damage, DamageHandlerBase handler, Vector3 exactHitPos)
		{
			return false;
		}

		public override float SearchTimeForPlayer(ReferenceHub hub)
		{
			return 0f;
		}
	}
}
