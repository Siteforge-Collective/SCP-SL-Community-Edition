namespace InventorySystem.Searching
{
	[global::UnityEngine.RequireComponent(typeof(ReferenceHub))]
	public class SearchCoordinator : global::Mirror.NetworkBehaviour, global::CursorManagement.ICursorOverride
	{
		public const string DebugKey = "SEARCH";

		[global::UnityEngine.Header("Network Shared")]
		[global::UnityEngine.SerializeField]
		[global::Mirror.SyncVar(hook = "SetRayDistance")]
		private float rayDistance = 3f;

		[global::UnityEngine.Header("Server only")]
		[global::UnityEngine.SerializeField]
		private float serverRayDistanceThreshold = 1.2f;

		[global::UnityEngine.SerializeField]
		private double serverDelayThreshold = 1.399999976158142;

		public ReferenceHub Hub { get; private set; }

		public global::CursorManagement.CursorOverrideMode CursorOverride => global::CursorManagement.CursorOverrideMode.NoOverride;

		public bool LockMovement => false;

		public float ServerMaxRayDistanceSqr { get; private set; }

		public float RayDistance
		{
			get
			{
				return rayDistance;
			}
			set
			{
				if (!global::Mirror.NetworkServer.active)
				{
					throw new global::System.InvalidOperationException("The ray distance can only be set by the server.");
				}
				NetworkrayDistance = value;
				UpdateMaxDistanceSqr();
			}
		}

		public global::InventorySystem.Searching.SearchSessionPipe SessionPipe { get; private set; }

		public global::InventorySystem.Searching.SearchCompletor Completor { get; private set; }

		public float NetworkrayDistance
		{
			get
			{
				return rayDistance;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref rayDistance))
				{
					float oldValue = rayDistance;
					SetSyncVar(value, ref rayDistance, 1uL);
					if (global::Mirror.NetworkServer.localClientActive && !getSyncVarHookGuard(1uL))
					{
						setSyncVarHookGuard(1uL, value: true);
						SetRayDistance(oldValue, value);
						setSyncVarHookGuard(1uL, value: false);
					}
				}
			}
		}

		private void SetRayDistance(float oldValue, float newValue)
		{
			UpdateMaxDistanceSqr();
		}

		private void UpdateMaxDistanceSqr()
		{
			ServerMaxRayDistanceSqr = rayDistance * rayDistance * serverRayDistanceThreshold;
		}

		private void Awake()
		{
		}

		private void Start()
		{
			UpdateMaxDistanceSqr();
			Hub = ReferenceHub.GetHub(base.gameObject);
			SessionPipe = new global::InventorySystem.Searching.SearchSessionPipe(this, global::Mirror.NetworkServer.active ? Hub.playerRateLimitHandler.RateLimits[0] : null);
			SessionPipe.RequestUpdated += HandleRequest;
			SessionPipe.RegisterHandlers();
			if (base.isLocalPlayer)
			{
				global::CursorManagement.CursorManager.Register(this);
			}
		}

		private void OnDestroy()
		{
			global::CursorManagement.CursorManager.Unregister(this);
		}

		private void Update()
		{
			if (global::Mirror.NetworkServer.active && SessionPipe.Status == global::InventorySystem.Searching.SearchSessionPipe.Activity.Promised)
			{
				ContinuePickupServer();
			}
			SessionPipe.Update();
		}

		private void HandleRequest()
		{
			bool flag;
			global::InventorySystem.Searching.SearchSession? session;
			global::InventorySystem.Searching.SearchCompletor completor;
			try
			{
				flag = ReceiveRequestUnsafe(out session, out completor);
			}
			catch (global::System.Exception exception)
			{
				SessionPipe.Invalidate();
				DebugLog.LogException(exception);
				return;
			}
			if (flag)
			{
				if (session.HasValue)
				{
					SessionPipe.Session = session.Value;
				}
				else
				{
					SessionPipe.Invalidate();
				}
			}
			Completor = completor;
		}

		private bool ReceiveRequestUnsafe(out global::InventorySystem.Searching.SearchSession? session, out global::InventorySystem.Searching.SearchCompletor completor)
		{
			global::InventorySystem.Searching.SearchRequest request = SessionPipe.Request;
			completor = global::InventorySystem.Searching.SearchCompletor.FromPickup(this, request.Target, ServerMaxRayDistanceSqr);
			if (!completor.ValidateStart())
			{
				session = null;
				completor = null;
				return true;
			}
			global::InventorySystem.Searching.SearchSession body = request.Body;
			if (!base.isLocalPlayer)
			{
				double num = global::Mirror.NetworkTime.time - request.InitialTime;
				double num2 = (double)global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorServer.Peers[base.connectionToClient.connectionId].Ping * 0.001 * serverDelayThreshold;
				float num3 = request.Target.SearchTimeForPlayer(Hub);
				if (num < 0.0 || num2 < num)
				{
					body.InitialTime = global::Mirror.NetworkTime.time - num2;
					body.FinishTime = body.InitialTime + (double)num3;
				}
				else if (global::System.Math.Abs(body.FinishTime - body.InitialTime - (double)num3) > 0.001)
				{
					body.FinishTime = body.InitialTime + (double)num3;
				}
			}
			session = body;
			return true;
		}

		private void ContinuePickupServer()
		{
			if (Completor.ValidateUpdate())
			{
				if (global::Mirror.NetworkTime.time >= SessionPipe.Session.FinishTime)
				{
					Completor.Complete();
				}
			}
			else
			{
				SessionPipe.Invalidate();
			}
		}

		private void MirrorProcessed()
		{
		}

		public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
		{
			bool result = base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				global::Mirror.NetworkWriterExtensions.WriteSingle(writer, rayDistance);
				return true;
			}
			global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 1L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteSingle(writer, rayDistance);
				result = true;
			}
			return result;
		}

		public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				float num = rayDistance;
				NetworkrayDistance = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
				if (!SyncVarEqual(num, ref rayDistance))
				{
					SetRayDistance(num, rayDistance);
				}
				return;
			}
			long num2 = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
			if ((num2 & 1L) != 0L)
			{
				float num3 = rayDistance;
				NetworkrayDistance = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
				if (!SyncVarEqual(num3, ref rayDistance))
				{
					SetRayDistance(num3, rayDistance);
				}
			}
		}
	}
}
