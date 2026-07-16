using System;
using CursorManagement;
using InventorySystem.Items.Pickups;
using Mirror;
using Mirror.LiteNetLib4Mirror;
using UnityEngine;
using UnityEngine.UI;
using UserSettings;
using UserSettings.ControlsSettings;
using Utils;

namespace InventorySystem.Searching
{
    [RequireComponent(typeof(ReferenceHub))]
    public class SearchCoordinator : NetworkBehaviour, ICursorOverride
    {
        private struct ProgressBezier : IEquatable<ProgressBezier>
        {
            private readonly SearchRequest _request;
            private readonly SearchSession _session;
            private readonly double _requestProgress;
            private readonly double _sessionReceivedTime;

            public double Progress
            {
                get
                {
                    double t = MoreMath.InverseLerp(_request.Body.InitialTime, _request.Body.FinishTime, NetworkTime.time);
                    return MoreMath.BezierQuadratic(t, _requestProgress, 1.0, 1.0);
                }
            }

            public ProgressBezier(SearchRequest request, SearchSession session)
            {
                _request = request;
                _session = session;
                _requestProgress = MoreMath.InverseLerp(request.Body.InitialTime, request.Body.FinishTime, NetworkTime.time);
                _sessionReceivedTime = NetworkTime.time;
            }

            public bool Equals(ProgressBezier other)
            {
                return _request.Equals(other._request) &&
                       _session.Equals(other._session) &&
                       _requestProgress.Equals(other._requestProgress) &&
                       _sessionReceivedTime.Equals(other._sessionReceivedTime);
            }

            public override bool Equals(object obj)
            {
                return obj is ProgressBezier other && Equals(other);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = _request.GetHashCode();
                    hash = (hash * 397) ^ _session.GetHashCode();
                    hash = (hash * 397) ^ _requestProgress.GetHashCode();
                    hash = (hash * 397) ^ _sessionReceivedTime.GetHashCode();
                    return hash;
                }
            }
        }

        public const string DebugKey = "SEARCH";

        private bool _isSearching;

        private byte _tickedId;

        private ProgressBezier _progressBezier;

        [Header("Network Shared")]
        [SerializeField]
        [SyncVar(hook = nameof(SetRayDistance))]
        private float rayDistance = 3f;

        [Header("Server only")]
        [SerializeField]
        private float serverRayDistanceThreshold = 1.2f;

        [SerializeField]
        private double serverDelayThreshold = 1.4;

        private Image _radialImage;

        private GameObject _radialObject;

        private static readonly CachedUserSetting<bool> ToggleSearch = new CachedUserSetting<bool>(MiscControlsSetting.SearchToggle);

        public ReferenceHub Hub { get; private set; }

        public LayerMask InteractMask { get; private set; }

        public CursorOverrideMode CursorOverride => CursorOverrideMode.NoOverride;

        public bool LockMovement => _isSearching;

        public float MaxDistanceSqr { get; private set; }

        public float ServerMaxRayDistanceSqr { get; private set; }

        public float RayDistance
        {
            get => rayDistance;
            set
            {
                if (!NetworkServer.active)
                {
                    throw new InvalidOperationException("The ray distance can only be set by the server.");
                }
                rayDistance = value;
                UpdateMaxDistanceSqr();
            }
        }

        private byte TicketId => ++_tickedId;

        public bool IsSearching
        {
            get => _isSearching;
            private set
            {
                if (value != _isSearching)
                {
                    _isSearching = value;
                    _radialObject.SetActive(value);
                    _radialImage.fillAmount = 0f;
                }
            }
        }

        public SearchSessionPipe SessionPipe { get; private set; }

        public SearchCompletor Completor { get; private set; }

        private void SetRayDistance(float oldValue, float newValue)
        {
            UpdateMaxDistanceSqr();
        }

        private void UpdateMaxDistanceSqr()
        {
            MaxDistanceSqr = rayDistance * rayDistance;
            ServerMaxRayDistanceSqr = rayDistance * rayDistance * serverRayDistanceThreshold;
        }

        private void Start()
        {
            UpdateMaxDistanceSqr();
            Hub = ReferenceHub.GetHub(gameObject);
            _radialImage = UserMainInterface.Singleton.searchRadial;
            _radialObject = UserMainInterface.Singleton.searchObject;
            InteractMask = Hub.playerInteract.mask;
            SessionPipe = new SearchSessionPipe(this, NetworkServer.active ? Hub.playerRateLimitHandler.RateLimits[0] : null);
            SessionPipe.RequestUpdated += HandleRequest;
            SessionPipe.SessionUpdated += HandleSession;
            SessionPipe.RegisterHandlers();
            if (isLocalPlayer)
            {
                CursorManager.Register(this);
            }
        }

        private void OnDestroy()
        {
            CursorManager.Unregister(this);
        }

        private void Update()
        {
            if (isLocalPlayer)
            {
                if (SessionPipe.Status != SearchSessionPipe.Activity.Idle)
                {
                    bool isSearching = ContinuePickupClient();
                    if (isSearching != IsSearching)
                    {
                        IsSearching = isSearching;
                    }
                }
                else
                {
                    if (IsSearching)
                    {
                        IsSearching = false;
                    }
                    Raycast();
                }
            }
            else if (NetworkServer.active && SessionPipe.Status == SearchSessionPipe.Activity.Promised)
            {
                ContinuePickupServer();
            }

            SessionPipe.Update();
        }

        private void HandleRequest()
        {
            bool flag;
            SearchSession? session;
            SearchCompletor completor;
            try
            {
                flag = ReceiveRequestUnsafe(out session, out completor);
            }
            catch (Exception exception)
            {
                SessionPipe.Invalidate();
                GameCore.Console.AddDebugLog(DebugKey, $"Exception in HandleRequest: {exception.Message}", MessageImportance.LessImportant, false);
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

        private void HandleSession()
        {
            _progressBezier = new ProgressBezier(SessionPipe.Request, SessionPipe.Session);
        }

        private bool ReceiveRequestUnsafe(out SearchSession? session, out SearchCompletor completor)
        {
            SearchRequest request = SessionPipe.Request;
            completor = SearchCompletor.FromPickup(this, request.Target, ServerMaxRayDistanceSqr);
            if (!completor.ValidateStart())
            {
                session = null;
                return true;
            }
            SearchSession body = request.Body;
            if (!isLocalPlayer)
            {
                double latency = NetworkTime.time - request.InitialTime;
                double pingThreshold = LiteNetLib4MirrorServer.Peers[connectionToClient.connectionId].Ping * 0.001 * serverDelayThreshold;
                float searchTime = request.Target.SearchTimeForPlayer(Hub);
                if (latency < 0.0 || latency > pingThreshold)
                {
                    body.InitialTime = NetworkTime.time - pingThreshold;
                    body.FinishTime = body.InitialTime + searchTime;
                }
                else if (Math.Abs(body.FinishTime - body.InitialTime - searchTime) > 0.001)
                {
                    body.FinishTime = body.InitialTime + searchTime;
                }
            }
            session = body;
            return true;
        }

        private void ContinuePickupServer()
        {
            if (Completor.ValidateUpdate())
            {
                if (NetworkTime.time >= SessionPipe.Session.FinishTime)
                {
                    Completor.Complete();
                }
            }
            else
            {
                SessionPipe.Invalidate();
            }
        }

        private bool ContinuePickupClient()
        {
            bool isToggle = ToggleSearch.Value;
            KeyCode keyCode = NewInput.GetKey(isToggle ? ActionName.Interact : ActionName.Interact);
            bool isKeyDown = Input.GetKeyDown(keyCode);
            if (SessionPipe.Status == SearchSessionPipe.Activity.Requested)
            {
                if (isKeyDown)
                {
                    SessionPipe.Clear();
                    return false;
                }

                double progress = MoreMath.InverseLerp(SessionPipe.Request.Body.InitialTime, SessionPipe.Request.Body.FinishTime, NetworkTime.time);
                _radialImage.fillAmount = (float)progress;

                if (Completor.ValidateUpdate())
                {
                    return true;
                }
            }
            else if (SessionPipe.Status == SearchSessionPipe.Activity.Promised)
            {
                if (isKeyDown)
                {
                    NetworkClient.Send(new SearchInvalidation(SessionPipe.Request.Id));
                    GameCore.Console.AddDebugLog(DebugKey, "Sent abortion", MessageImportance.LessImportant, false);
                    SessionPipe.Clear();
                    return false;
                }

                double progress = _progressBezier.Progress;
                _radialImage.fillAmount = (float)progress;

                if (Completor.ValidateUpdate())
                {
                    if (NetworkTime.time >= SessionPipe.Session.FinishTime)
                    {
                        Completor.Complete();
                    }
                    return true;
                }
            }

            SessionPipe.Clear();
            return false;
        }

        private void Raycast()
        {
            bool isToggle = ToggleSearch.Value;
            KeyCode keyCode = NewInput.GetKey(isToggle ? ActionName.Interact : ActionName.Interact);
            bool isKeyDown = Input.GetKeyDown(keyCode);
            if (isKeyDown)
            {
                if (Physics.Raycast(Hub.PlayerCameraReference.position, Hub.PlayerCameraReference.forward, out RaycastHit hit, rayDistance, InteractMask))
                {
                    ItemPickupBase pickup = hit.transform.GetComponentInParent<ItemPickupBase>();
                    if (pickup != null && !pickup.Info.Locked && !pickup.Info.InUse)
                    {
                        SearchCompletor completor = SearchCompletor.FromPickup(this, pickup, MaxDistanceSqr);
                        if (completor.ValidateStart())
                        {
                            Completor = completor;
                            float searchTime = pickup.SearchTimeForPlayer(Hub);
                            SearchSession session = new SearchSession(NetworkTime.time, NetworkTime.time + searchTime, pickup);
                            SearchRequest request = new SearchRequest(TicketId, session);
                            SessionPipe.Request = request;
                            NetworkClient.Send(request);
                            GameCore.Console.AddDebugLog(DebugKey, "Sent request", MessageImportance.LessImportant, false);
                        }
                    }
                }
            }
        }
    }
}