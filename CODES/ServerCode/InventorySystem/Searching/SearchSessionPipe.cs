namespace InventorySystem.Searching
{
	public class SearchSessionPipe
	{
		public enum Activity
		{
			Idle = 0,
			Requested = 1,
			Promised = 2
		}

		private readonly global::InventorySystem.Searching.SearchCoordinator _owner;

		private readonly global::Security.RateLimit _rateLimiter;

		private global::InventorySystem.Searching.SearchRequest _request;

		private global::InventorySystem.Searching.SearchSession _session;

		private global::InventorySystem.Searching.SearchInvalidation Invalidation => new global::InventorySystem.Searching.SearchInvalidation(_request.Id);

		public global::InventorySystem.Searching.SearchRequest Request => _request;

		public global::InventorySystem.Searching.SearchSession Session
		{
			get
			{
				return _session;
			}
			set
			{
				if (!global::Mirror.NetworkServer.active)
				{
					throw new global::System.InvalidOperationException("The promise can only be set from the server.");
				}
				if (!value.Equals(_session))
				{
					_owner.connectionToClient.Send(value);
					Status = global::InventorySystem.Searching.SearchSessionPipe.Activity.Promised;
					_session = value;
				}
			}
		}

		public global::InventorySystem.Searching.SearchSessionPipe.Activity Status { get; private set; }

		public event global::System.Action RequestUpdated;

		public event global::System.Action SessionAborted;

		public SearchSessionPipe(global::InventorySystem.Searching.SearchCoordinator owner, global::Security.RateLimit rateLimit)
		{
			_owner = owner;
			_rateLimiter = rateLimit;
		}

		private static void ReceiveRequest(global::Mirror.NetworkConnection source, global::InventorySystem.Searching.SearchRequest request)
		{
			global::InventorySystem.Searching.SearchCoordinator searchCoordinator = ((source == null) ? ReferenceHub.LocalHub.searchCoordinator : source.identity.GetComponent<global::InventorySystem.Searching.SearchCoordinator>());
			if (searchCoordinator == null)
			{
				return;
			}
			global::InventorySystem.Searching.SearchSessionPipe sessionPipe = searchCoordinator.SessionPipe;
			if (request.Target == null)
			{
				sessionPipe.Invalidate();
				source?.identity.GetComponent<GameConsoleTransmission>().SendToClient(source.identity.connectionToClient, "Pickup request rejected - target is null.", "red");
				return;
			}
			global::InventorySystem.Items.Pickups.PickupSyncInfo info = request.Target.Info;
			if (info.Locked)
			{
				sessionPipe.Invalidate();
				source?.identity.GetComponent<GameConsoleTransmission>().SendToClient(source.identity.connectionToClient, "Pickup request rejected - target is locked.", "red");
			}
			else if (info.InUse)
			{
				sessionPipe.Invalidate();
				source?.identity.GetComponent<GameConsoleTransmission>().SendToClient(source.identity.connectionToClient, "Pickup request rejected - target is in use.", "red");
			}
			else
			{
				info.InUse = true;
				request.Target.NetworkInfo = info;
				sessionPipe.HandleRequest(request);
			}
		}

		private void HandleRequest(global::InventorySystem.Searching.SearchRequest request)
		{
			if (_rateLimiter.CanExecute())
			{
				_request = request;
				Status = global::InventorySystem.Searching.SearchSessionPipe.Activity.Requested;
				this.RequestUpdated?.Invoke();
			}
		}

		private void ReceivePromise(global::Mirror.NetworkConnection source, global::InventorySystem.Searching.SearchSession session)
		{
			HandlePromise(session);
		}

		private void HandlePromise(global::InventorySystem.Searching.SearchSession session)
		{
		}

		private static void ReceiveAbortion(global::Mirror.NetworkConnection source, global::InventorySystem.Searching.SearchInvalidation invalidation)
		{
			global::InventorySystem.Searching.SearchCoordinator searchCoordinator = ((source == null) ? ReferenceHub.LocalHub.searchCoordinator : source.identity.GetComponent<global::InventorySystem.Searching.SearchCoordinator>());
			if (!(searchCoordinator == null))
			{
				searchCoordinator.SessionPipe.HandleAbort(invalidation);
			}
		}

		private void HandleAbort(global::InventorySystem.Searching.SearchInvalidation invalidation)
		{
			if (_request.Id != invalidation.Id)
			{
				return;
			}
			if (_request.Target != null)
			{
				global::InventorySystem.Items.Pickups.PickupSyncInfo info = _request.Target.Info;
				info.InUse = false;
				_request.Target.NetworkInfo = info;
			}
			try
			{
				Status = global::InventorySystem.Searching.SearchSessionPipe.Activity.Idle;
				this.SessionAborted?.Invoke();
			}
			finally
			{
				_request = default(global::InventorySystem.Searching.SearchRequest);
				_session = default(global::InventorySystem.Searching.SearchSession);
			}
		}

		private void ReceiveInvalidation(global::Mirror.NetworkConnection source, global::InventorySystem.Searching.SearchInvalidation invalidation)
		{
			HandleInvalidate(invalidation);
		}

		private void HandleInvalidate(global::InventorySystem.Searching.SearchInvalidation invalidation)
		{
		}

		public void RegisterHandlers()
		{
			global::Mirror.NetworkServer.ReplaceHandler<global::InventorySystem.Searching.SearchRequest>(ReceiveRequest);
			global::Mirror.NetworkServer.ReplaceHandler<global::InventorySystem.Searching.SearchInvalidation>(ReceiveAbortion);
			global::Mirror.NetworkClient.ReplaceHandler<global::InventorySystem.Searching.SearchInvalidation>(ReceiveInvalidation);
			global::Mirror.NetworkClient.ReplaceHandler<global::InventorySystem.Searching.SearchSession>(ReceivePromise);
		}

		public void Invalidate()
		{
			if (!global::Mirror.NetworkServer.active)
			{
				throw new global::System.InvalidOperationException("An invalidation can only be performed by the server.");
			}
			if (_request.Target != null)
			{
				global::InventorySystem.Items.Pickups.PickupSyncInfo info = _request.Target.Info;
				info.InUse = false;
				_request.Target.NetworkInfo = info;
			}
			_owner.connectionToClient.Send(Invalidation);
			Status = global::InventorySystem.Searching.SearchSessionPipe.Activity.Idle;
		}

		public void Update()
		{
			global::InventorySystem.Searching.SearchSessionPipe.Activity activity = Status;
			global::InventorySystem.Searching.SearchSessionPipe.Activity activity2;
			do
			{
				activity2 = activity;
				switch ((int)Status)
				{
				case 2:
					if (!(global::Mirror.NetworkTime.time >= Session.FinishTime))
					{
						break;
					}
					activity = global::InventorySystem.Searching.SearchSessionPipe.Activity.Requested;
					continue;
				case 1:
					if (!(global::Mirror.NetworkTime.time >= Request.FinishTime))
					{
						break;
					}
					activity = global::InventorySystem.Searching.SearchSessionPipe.Activity.Idle;
					continue;
				}
				activity = Status;
			}
			while (activity != activity2);
			Status = activity;
		}
	}
}
