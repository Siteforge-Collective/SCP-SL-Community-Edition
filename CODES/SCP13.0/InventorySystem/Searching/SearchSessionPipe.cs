using System;
using System.Runtime.InteropServices;
using CursorManagement;
using InventorySystem.Items.Pickups;
using Mirror;
using Mirror.LiteNetLib4Mirror;
using Security;
using UnityEngine;
using UnityEngine.UI;
using UserSettings;
using UserSettings.ControlsSettings;
using Utils;

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

        private readonly SearchCoordinator _owner;

        private readonly RateLimit _rateLimiter;

        private SearchRequest _request;

        private SearchSession _session;

        private SearchInvalidation Invalidation => new SearchInvalidation(_request.Id);

        public SearchRequest Request
        {
            get => _request;
            set
            {
                _request = value;
                Status = Activity.Requested;
                RequestUpdated?.Invoke();
            }
        }

        public SearchSession Session
        {
            get => _session;
            set
            {
                if (!NetworkServer.active)
                {
                    throw new InvalidOperationException("The promise can only be set from the server.");
                }
                if (!value.Equals(_session))
                {
                    _owner.connectionToClient.Send(value);
                    Status = Activity.Promised;
                    _session = value;
                    SessionUpdated?.Invoke();
                }
            }
        }

        public Activity Status { get; private set; }

        public event Action RequestUpdated;

        public event Action SessionUpdated;

        public event Action SessionAborted;

        public event Action SessionInvalidated;

        public SearchSessionPipe(SearchCoordinator owner, RateLimit rateLimit)
        {
            _owner = owner;
            _rateLimiter = rateLimit;
        }

        private static void ReceiveRequest(NetworkConnection source, SearchRequest request)
        {
            SearchCoordinator searchCoordinator = source?.identity.GetComponent<SearchCoordinator>() ?? ReferenceHub.LocalHub.searchCoordinator;
            if (searchCoordinator == null)
            {
                return;
            }
            SearchSessionPipe sessionPipe = searchCoordinator.SessionPipe;
            if (request.Target == null)
            {
                sessionPipe.Invalidate();
                source?.identity.GetComponent<GameConsoleTransmission>().SendToClient("Pickup request rejected - target is null.", "red");
                return;
            }
            PickupSyncInfo info = request.Target.Info;
            if (info.Locked)
            {
                sessionPipe.Invalidate();
                source?.identity.GetComponent<GameConsoleTransmission>().SendToClient("Pickup request rejected - target is locked.", "red");
                return;
            }
            if (info.InUse)
            {
                sessionPipe.Invalidate();
                source?.identity.GetComponent<GameConsoleTransmission>().SendToClient("Pickup request rejected - target is in use.", "red");
                return;
            }
            info.InUse = true;
            request.Target.Info = info;
            sessionPipe.HandleRequest(request);
        }

        private void HandleRequest(SearchRequest request)
        {
            if (_rateLimiter.CanExecute())
            {
                _request = request;
                Status = Activity.Requested;
                RequestUpdated?.Invoke();
            }
        }

        private void HandlePromise(SearchSession session)
        {
            _session = session;
            Status = Activity.Promised;
            SessionUpdated?.Invoke();
        }

        private static void ReceiveAbortion(NetworkConnection source, SearchInvalidation invalidation)
        {
            SearchCoordinator searchCoordinator = source?.identity.GetComponent<SearchCoordinator>() ?? ReferenceHub.LocalHub.searchCoordinator;
            if (searchCoordinator != null)
            {
                searchCoordinator.SessionPipe.HandleAbort(invalidation);
            }
        }

        private void HandleAbort(SearchInvalidation invalidation)
        {
            if (_request.Id != invalidation.Id)
            {
                return;
            }
            if (_request.Target != null)
            {
                PickupSyncInfo info = _request.Target.Info;
                info.InUse = false;
                _request.Target.Info = info;
            }
            try
            {
                Status = Activity.Idle;
                SessionAborted?.Invoke();
            }
            finally
            {
                _request = default;
                _session = default;
            }
        }

        private void ReceiveInvalidation(SearchInvalidation invalidation)
        {
            HandleInvalidate(invalidation);
        }

        private void ReceivePromise(SearchSession session)
        {
            HandlePromise(session);
        }

        private void HandleInvalidate(SearchInvalidation invalidation)
        {
            if (_request.Id == invalidation.Id)
            {
                Status = Activity.Idle;
                SessionInvalidated?.Invoke();
                _request = default;
                _session = default;
            }
        }

        public void RegisterHandlers()
        {
            NetworkServer.ReplaceHandler<SearchRequest>(ReceiveRequest);
            NetworkServer.ReplaceHandler<SearchInvalidation>(ReceiveAbortion);
            NetworkClient.ReplaceHandler<SearchInvalidation>(ReceiveInvalidation);
            NetworkClient.ReplaceHandler<SearchSession>(ReceivePromise);
        }

        public void Abort()
        {
            if (!_owner.isLocalPlayer)
            {
                throw new InvalidOperationException("An abortion can only be performed by the local player.");
            }
            NetworkClient.Send(Invalidation);
            Status = Activity.Idle;
            _request = default;
            _session = default;
        }

        public void Invalidate()
        {
            if (!NetworkServer.active)
            {
                throw new InvalidOperationException("An invalidation can only be performed by the server.");
            }
            if (_request.Target != null)
            {
                PickupSyncInfo info = _request.Target.Info;
                info.InUse = false;
                _request.Target.Info = info;
            }
            _owner.connectionToClient.Send(Invalidation);
            Status = Activity.Idle;
            _request = default;
            _session = default;
        }

        public void Clear()
        {
            Status = Activity.Idle;
            _request = default;
            _session = default;
        }

        public void Update()
        {
            Activity current = Status;
            Activity next;
            do
            {
                next = current;
                switch (Status)
                {
                    case Activity.Promised:
                        if (NetworkTime.time >= _session.FinishTime)
                        {
                            current = Activity.Requested;
                        }
                        break;
                    case Activity.Requested:
                        if (NetworkTime.time >= _request.Body.FinishTime)
                        {
                            current = Activity.Idle;
                        }
                        break;
                }
            } while (current != next);
            Status = current;
        }
    }
}