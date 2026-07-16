namespace PlayerRoles.PlayableScps.Scp173
{
	public class Scp173ObserversTracker : global::PlayerRoles.PlayableScps.Subroutines.ScpStandardSubroutine<global::PlayerRoles.PlayableScps.Scp173.Scp173Role>
	{
		public delegate void ObserversChanged(int prev, int current);

		public readonly global::System.Collections.Generic.HashSet<ReferenceHub> Observers = new global::System.Collections.Generic.HashSet<ReferenceHub>();

		private const float WidthMultiplier = 0.2f;

		[global::UnityEngine.SerializeField]
		private float _modelWidth;

		[global::UnityEngine.SerializeField]
		private float _maxViewDistance;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Vector2[] _visibilityReferencePoints;

		private int _curObservers;

		private int _simulatedTargets;

		private float _simulatedStareTime;

		private readonly global::System.Diagnostics.Stopwatch _simulatedStareSw = global::System.Diagnostics.Stopwatch.StartNew();

		public int CurrentObservers
		{
			get
			{
				return _curObservers;
			}
			private set
			{
				if (value != _curObservers)
				{
					int curObservers = _curObservers;
					_curObservers = value;
					this.OnObserversChanged?.Invoke(curObservers, value);
				}
			}
		}

		public bool IsObserved => CurrentObservers > 0;

		public float SimulatedStare
		{
			get
			{
				return global::UnityEngine.Mathf.Max(0f, _simulatedStareTime - (float)_simulatedStareSw.Elapsed.TotalSeconds);
			}
			set
			{
				_simulatedStareTime = value;
				_simulatedStareSw.Restart();
			}
		}

		public event global::PlayerRoles.PlayableScps.Scp173.Scp173ObserversTracker.ObserversChanged OnObserversChanged;

		private void Update()
		{
			UpdateObservers();
		}

		private void CheckRemovedPlayer(ReferenceHub ply)
		{
			if (global::Mirror.NetworkServer.active && Observers.Remove(ply))
			{
				CurrentObservers--;
			}
		}

		private int UpdateObserver(ReferenceHub targetHub)
		{
			if (!targetHub.IsHuman())
			{
				if (!Observers.Remove(targetHub))
				{
					return 0;
				}
				return -1;
			}
			if (IsObservedBy(targetHub, 0.2f))
			{
				if (Observers.Add(targetHub))
				{
					return 1;
				}
			}
			else if (Observers.Remove(targetHub))
			{
				return -1;
			}
			return 0;
		}

		protected override void Awake()
		{
			base.Awake();
			ReferenceHub.OnPlayerRemoved = (global::System.Action<ReferenceHub>)global::System.Delegate.Combine(ReferenceHub.OnPlayerRemoved, new global::System.Action<ReferenceHub>(CheckRemovedPlayer));
		}

		public bool IsObservedBy(ReferenceHub target, float widthMultiplier = 1f)
		{
			global::UnityEngine.Vector3 position = base.ScpRole.FpcModule.Position;
			global::MapGeneration.RoomIdentifier roomIdentifier = global::MapGeneration.RoomIdUtils.RoomAtPosition(position);
			if (!global::PlayerRoles.PlayableScps.VisionInformation.GetVisionInformation(target, target.PlayerCameraReference, position, _modelWidth, (roomIdentifier != null && roomIdentifier.Zone == global::MapGeneration.FacilityZone.Surface) ? (_maxViewDistance * 2f) : _maxViewDistance, checkFog: false, checkLineOfSight: false).IsLooking)
			{
				return false;
			}
			global::UnityEngine.Vector3 position2 = target.PlayerCameraReference.position;
			global::UnityEngine.Vector3 vector = target.PlayerCameraReference.TransformDirection(global::UnityEngine.Vector3.right);
			global::UnityEngine.Vector2[] visibilityReferencePoints = _visibilityReferencePoints;
			for (int i = 0; i < visibilityReferencePoints.Length; i++)
			{
				global::UnityEngine.Vector2 vector2 = visibilityReferencePoints[i];
				if (!global::UnityEngine.Physics.Linecast(position + vector2.x * widthMultiplier * vector + global::UnityEngine.Vector3.up * vector2.y, position2, global::PlayerRoles.PlayableScps.VisionInformation.VisionLayerMask))
				{
					if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp173NewObserver, base.Owner, target))
					{
						return false;
					}
					return true;
				}
			}
			return false;
		}

		public void UpdateObservers()
		{
			if (!global::Mirror.NetworkServer.active)
			{
				return;
			}
			int num = CurrentObservers;
			int num2 = ((SimulatedStare > 0f) ? 1 : 0);
			if (_simulatedTargets != num2)
			{
				num += num2 - _simulatedTargets;
				_simulatedTargets = num2;
			}
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				num += UpdateObserver(allHub);
			}
			CurrentObservers = num;
			if (!base.Owner.isLocalPlayer)
			{
				ServerSendRpc(toAll: true);
			}
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			writer.WriteByte((byte)global::UnityEngine.Mathf.Clamp(CurrentObservers, 0, 255));
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			CurrentObservers = reader.ReadByte();
		}

		public override void ResetObject()
		{
			base.ResetObject();
			_curObservers = 0;
			_simulatedTargets = 0;
			_simulatedStareTime = 0f;
			Observers.Clear();
		}
	}
}
