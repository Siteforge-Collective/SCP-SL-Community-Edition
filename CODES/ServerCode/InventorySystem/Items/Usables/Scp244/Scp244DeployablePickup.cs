namespace InventorySystem.Items.Usables.Scp244
{
	public class Scp244DeployablePickup : global::InventorySystem.Items.Pickups.CollisionDetectionPickup, IDestructible
	{
		private const float SquaredDisUpdateDiff = 1f;

		private const float ForceBoundsUpdateSqrtDiff = 100f;

		private const float UpdateCooldownTime = 2.2f;

		private const int VertsPerFrame = 30;

		private const float ParticleSize = 3f;

		public static readonly global::System.Collections.Generic.HashSet<global::InventorySystem.Items.Usables.Scp244.Scp244DeployablePickup> Instances = new global::System.Collections.Generic.HashSet<global::InventorySystem.Items.Usables.Scp244.Scp244DeployablePickup>();

		public float MaxDiameter;

		[global::Mirror.SyncVar]
		private byte _syncSizePercent;

		[global::Mirror.SyncVar]
		private byte _syncState;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _growSpeedOverLifetime;

		[global::UnityEngine.SerializeField]
		private float _timeToDecay;

		[global::UnityEngine.SerializeField]
		private float _transitionDistance;

		[global::UnityEngine.SerializeField]
		private float _fullSubmergeDistance;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _visibleModel;

		[global::UnityEngine.SerializeField]
		private float _minimalInfluenceDistance;

		[global::UnityEngine.SerializeField]
		private float _activationDot;

		[global::UnityEngine.SerializeField]
		private float _health;

		[global::UnityEngine.SerializeField]
		private float _deployedPickupTime;

		private global::UnityEngine.Vector3 _previousPos;

		private float _lastActiveSize;

		private float _lastUpdateTime;

		private bool _conditionsSet;

		private readonly global::System.Diagnostics.Stopwatch _lifeTime = global::System.Diagnostics.Stopwatch.StartNew();

		private float GrowSpeed => global::UnityEngine.Time.deltaTime * (MaxDiameter / TimeToGrow);

		private float TimeToGrow => 1f / _growSpeedOverLifetime.Evaluate((float)_lifeTime.Elapsed.TotalSeconds);

		private float CurTime => global::UnityEngine.Time.timeSinceLevelLoad;

		public bool ModelDestroyed
		{
			get
			{
				if (State != global::InventorySystem.Items.Usables.Scp244.Scp244State.Destroyed)
				{
					return State == global::InventorySystem.Items.Usables.Scp244.Scp244State.PickedUp;
				}
				return true;
			}
		}

		public float CurrentDiameter
		{
			get
			{
				if (State == global::InventorySystem.Items.Usables.Scp244.Scp244State.Active)
				{
					_lastActiveSize = CurrentSizePercent * MaxDiameter;
				}
				return _lastActiveSize;
			}
		}

		public global::UnityEngine.Bounds CurrentBounds { get; private set; }

		public float CurrentSizePercent { get; private set; }

		public global::InventorySystem.Items.Usables.Scp244.Scp244TransferCondition[] Conditions { get; private set; }

		public global::InventorySystem.Items.Usables.Scp244.Scp244State State
		{
			get
			{
				return (global::InventorySystem.Items.Usables.Scp244.Scp244State)_syncState;
			}
			set
			{
				Network_syncState = (byte)value;
			}
		}

		public uint NetworkId => base.netId;

		public global::UnityEngine.Vector3 CenterOfMass => Rb.worldCenterOfMass;

		public byte Network_syncSizePercent
		{
			get
			{
				return _syncSizePercent;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref _syncSizePercent))
				{
					byte syncSizePercent = _syncSizePercent;
					SetSyncVar(value, ref _syncSizePercent, 1uL);
				}
			}
		}

		public byte Network_syncState
		{
			get
			{
				return _syncState;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref _syncState))
				{
					byte syncState = _syncState;
					SetSyncVar(value, ref _syncState, 2uL);
				}
			}
		}

		private void Update()
		{
			UpdateCurrentRoom();
			UpdateConditions();
			UpdateRange();
			UpdateEffects();
		}

		protected override void Start()
		{
			base.Start();
			Instances.Add(this);
			SetupEffects();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Instances.Remove(this);
		}

		private void UpdateCurrentRoom()
		{
			global::UnityEngine.Vector3 position = base.transform.position;
			if (!((position - _previousPos).sqrMagnitude < 1f) && !(_lastUpdateTime + 2.2f > CurTime) && global::MapGeneration.SeedSynchronizer.MapGenerated)
			{
				Conditions = global::InventorySystem.Items.Usables.Scp244.Scp244TransferCondition.GenerateTransferConditions(this);
				_previousPos = position;
				_lastUpdateTime = CurTime;
				_conditionsSet = true;
			}
		}

		private void UpdateConditions()
		{
			if (!_conditionsSet)
			{
				return;
			}
			bool flag = true;
			global::UnityEngine.Bounds currentBounds = default(global::UnityEngine.Bounds);
			global::InventorySystem.Items.Usables.Scp244.Scp244TransferCondition[] conditions = Conditions;
			foreach (global::InventorySystem.Items.Usables.Scp244.Scp244TransferCondition scp244TransferCondition in conditions)
			{
				bool flag2 = true;
				global::Interactables.Interobjects.DoorUtils.DoorVariant[] doors = scp244TransferCondition.Doors;
				for (int j = 0; j < doors.Length; j++)
				{
					if (!doors[j].IsConsideredOpen())
					{
						flag2 = false;
						break;
					}
				}
				if (flag2)
				{
					if (flag)
					{
						currentBounds = scp244TransferCondition.BoundsToEncapsulate;
					}
					else
					{
						currentBounds.Encapsulate(scp244TransferCondition.BoundsToEncapsulate);
					}
					flag = false;
				}
			}
			global::UnityEngine.Bounds bounds = new global::UnityEngine.Bounds(base.transform.position, global::UnityEngine.Vector3.one * (CurrentDiameter + 3f));
			currentBounds.SetMinMax(global::UnityEngine.Vector3.Max(bounds.min, currentBounds.min), global::UnityEngine.Vector3.Min(bounds.max, currentBounds.max));
			if ((CurrentBounds.center - currentBounds.center).sqrMagnitude < 100f)
			{
				global::UnityEngine.Vector3 vector = CurrentBounds.size - currentBounds.size;
				float growSpeed = GrowSpeed;
				growSpeed = ((vector.x == 0f || vector.z == 0f) ? (growSpeed / 2f) : (growSpeed * 2f));
				global::UnityEngine.Vector3 center = global::UnityEngine.Vector3.MoveTowards(CurrentBounds.center, currentBounds.center, growSpeed / 2f);
				global::UnityEngine.Vector3 size = global::UnityEngine.Vector3.MoveTowards(CurrentBounds.size, currentBounds.size, growSpeed);
				CurrentBounds = new global::UnityEngine.Bounds(center, size);
			}
			else
			{
				CurrentBounds = currentBounds;
			}
		}

		private void UpdateRange()
		{
			if (ModelDestroyed && _visibleModel.activeSelf)
			{
				Rb.constraints = global::UnityEngine.RigidbodyConstraints.FreezeAll;
				_visibleModel.SetActive(value: false);
			}
			if (!global::Mirror.NetworkServer.active)
			{
				CurrentSizePercent = (int)_syncSizePercent;
				CurrentSizePercent /= 255f;
				return;
			}
			if (State == global::InventorySystem.Items.Usables.Scp244.Scp244State.Idle && global::UnityEngine.Vector3.Dot(base.transform.up, global::UnityEngine.Vector3.up) < _activationDot)
			{
				State = global::InventorySystem.Items.Usables.Scp244.Scp244State.Active;
				_lifeTime.Restart();
			}
			float num = ((State == global::InventorySystem.Items.Usables.Scp244.Scp244State.Active) ? TimeToGrow : (0f - _timeToDecay));
			CurrentSizePercent = global::UnityEngine.Mathf.Clamp01(CurrentSizePercent + global::UnityEngine.Time.deltaTime / num);
			Network_syncSizePercent = (byte)global::UnityEngine.Mathf.RoundToInt(CurrentSizePercent * 255f);
			if (ModelDestroyed && !(CurrentSizePercent > 0f))
			{
				_timeToDecay -= global::UnityEngine.Time.deltaTime;
				if (_timeToDecay <= 0f)
				{
					global::Mirror.NetworkServer.Destroy(base.gameObject);
				}
			}
		}

		private void SetupEffects()
		{
		}

		private void UpdateEffects()
		{
		}

		public float FogPercentForPoint(global::UnityEngine.Vector3 worldPoint)
		{
			if (State == global::InventorySystem.Items.Usables.Scp244.Scp244State.Idle)
			{
				return 0f;
			}
			float num = CurrentDiameter / 2f;
			float sqrMagnitude = (base.transform.position - worldPoint).sqrMagnitude;
			float num2 = num + _transitionDistance + 3f;
			if (sqrMagnitude >= num2 * num2)
			{
				return 0f;
			}
			global::UnityEngine.Bounds bounds = new global::UnityEngine.Bounds(CurrentBounds.center, CurrentBounds.size);
			bounds.Expand(0f - _fullSubmergeDistance);
			float a = global::UnityEngine.Vector3.Distance(bounds.ClosestPoint(worldPoint), worldPoint);
			float b = global::UnityEngine.Mathf.Sqrt(sqrMagnitude) - num2 + 3f + _fullSubmergeDistance;
			float num3 = 1f - global::UnityEngine.Mathf.Clamp01(global::UnityEngine.Mathf.Max(a, b) / _transitionDistance);
			if (ModelDestroyed)
			{
				num3 *= CurrentSizePercent;
			}
			if (num < _minimalInfluenceDistance)
			{
				num3 *= num / _minimalInfluenceDistance;
			}
			return num3;
		}

		public bool Damage(float damage, global::PlayerStatsSystem.DamageHandlerBase handler, global::UnityEngine.Vector3 exactHitPos)
		{
			if (!(handler is global::PlayerStatsSystem.ExplosionDamageHandler))
			{
				return false;
			}
			if (_health <= 0f || ModelDestroyed)
			{
				return false;
			}
			_health -= damage;
			if (_health <= 0f)
			{
				State = global::InventorySystem.Items.Usables.Scp244.Scp244State.Destroyed;
			}
			return true;
		}

		public override float SearchTimeForPlayer(ReferenceHub hub)
		{
			float num = base.SearchTimeForPlayer(hub);
			if (State == global::InventorySystem.Items.Usables.Scp244.Scp244State.Active)
			{
				num += _deployedPickupTime;
			}
			return num;
		}

		private void MirrorProcessed()
		{
		}

		public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
		{
			bool result = base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				global::Mirror.NetworkWriterExtensions.WriteByte(writer, _syncSizePercent);
				global::Mirror.NetworkWriterExtensions.WriteByte(writer, _syncState);
				return true;
			}
			global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 1L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteByte(writer, _syncSizePercent);
				result = true;
			}
			if ((base.syncVarDirtyBits & 2L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteByte(writer, _syncState);
				result = true;
			}
			return result;
		}

		public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				byte syncSizePercent = _syncSizePercent;
				Network_syncSizePercent = global::Mirror.NetworkReaderExtensions.ReadByte(reader);
				byte syncState = _syncState;
				Network_syncState = global::Mirror.NetworkReaderExtensions.ReadByte(reader);
				return;
			}
			long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
			if ((num & 1L) != 0L)
			{
				byte syncSizePercent2 = _syncSizePercent;
				Network_syncSizePercent = global::Mirror.NetworkReaderExtensions.ReadByte(reader);
			}
			if ((num & 2L) != 0L)
			{
				byte syncState2 = _syncState;
				Network_syncState = global::Mirror.NetworkReaderExtensions.ReadByte(reader);
			}
		}
	}
}
