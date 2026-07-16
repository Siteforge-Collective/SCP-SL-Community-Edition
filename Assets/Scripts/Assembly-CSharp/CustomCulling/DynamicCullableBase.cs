using MapGeneration;
using UnityEngine;

namespace CustomCulling
{
    public class DynamicCullableBase : CullableBase
    {
        private const float PositionCheckInterval = 0.05f;
        private const float MovedSqrThreshold = 0.0001f;

        public bool IgnoreRoomCulling = false;

        private Transform _transform;
        private Vector3 _savedPosition;
        private float _nextCheckTime;
        private bool _wasInitialized;

        public CullableRoom LinkedRoom { get; private set; }

        internal override bool CullEnabled
        {
            get
            {
                if (IgnoreRoomCulling || LinkedRoom == null)
                    return true;

                return LinkedRoom.CurrentRoomState != CullableRoom.RoomState.Disabled;
            }
        }

        public void UpdateRoom(bool updateChildDynamicCullableBases = false)
        {
            if (!CullingManager.Initialized || _transform == null || IgnoreRoomCulling)
                return;

            Vector3 pos = _transform.position;

            RoomIdentifier roomId = RoomIdUtils.RoomAtPositionRaycasts(pos, false)
                                    ?? RoomIdUtils.RoomAtPosition(pos);
            if (roomId == null)
                return;

            if (!roomId.TryGetComponent<CullableRoom>(out var room))
                return;

            if (LinkedRoom != null && LinkedRoom.gameObject == room.gameObject)
            {
                if (Lights != null)
                {
                    for (int i = Lights.Count - 1; i >= 0; i--)
                    {
                        Light light = Lights[i];
                        if (light == null) { Lights.RemoveAt(i); continue; }
                        LinkedRoom.UpdateNearbyLight(light);
                    }
                }
                return;
            }

            LinkToRoom(room);

            if (updateChildDynamicCullableBases)
            {
                foreach (DynamicCullableBase child in GetComponentsInChildren<DynamicCullableBase>())
                    if (child != null && child != this)
                        child.LinkToRoom(room);
            }
        }

        protected override void OnAwake()
        {
            _transform = GetComponent<Transform>();
            if (_transform != null)
                _savedPosition = _transform.position;

            _currentlyEnabled = false;
            UpdateBehaviours();
        }

        private void Update()
        {
            if (_transform == null)
                return;

            if (!CullingManager.Initialized)
            {
                HandleDeinitialization();
                return;
            }

            HandleInitialization();

            if (IgnoreRoomCulling)
                return;

            if (Time.unscaledTime < _nextCheckTime)
                return;

            Vector3 currentPos = _transform.position;
            if (LinkedRoom == null || (currentPos - _savedPosition).sqrMagnitude > MovedSqrThreshold)
            {
                _savedPosition = currentPos;
                _nextCheckTime = Time.unscaledTime + PositionCheckInterval;
                UpdateRoom(false);
            }
        }

        private void OnDestroy()
        {
            if (LinkedRoom != null)
            {
                LinkedRoom.RemoveCullableBase(this);
                LinkedRoom = null;
            }
        }

        private void LinkToRoom(CullableRoom room)
        {
            LinkedRoom?.RemoveCullableBase(this);
            LinkedRoom = room;
            LinkedRoom?.AddCullableBase(this);
        }

        private void HandleInitialization()
        {
            if (_wasInitialized) return;

            _wasInitialized = true;
            _savedPosition = _transform.position;
            _nextCheckTime = 0f;
        }

        private void HandleDeinitialization()
        {
            if (!_wasInitialized) return;

            _wasInitialized = false;

            if (LinkedRoom != null)
            {
                LinkedRoom.RemoveCullableBase(this);
                LinkedRoom = null;
            }

            _currentlyEnabled = false;
            UpdateBehaviours();
        }
    }
}