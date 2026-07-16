using UnityEngine;

namespace CustomCulling
{
    public class DynamicCullableBase : CullableBase
    {
        private CullableRoom _linkedRoom;
        private Transform _transform;
        private Vector3 _savedPosition;

        public CullableRoom LinkedRoom
        {
            get => _linkedRoom;
            private set => _linkedRoom = value;
        }

        public override bool CullEnabled
        {
            get
            {
                if (_linkedRoom == null) return true;
                return _linkedRoom.CullEnabled;
            }
        }

        private void LinkToRoom(CullableRoom room)
        {
            if (_linkedRoom != null)
                _linkedRoom.RemoveCullableBase(this);

            _linkedRoom = room;

            if (_linkedRoom != null)
            {
                _linkedRoom.AddCullableBase(this);

                if (!_linkedRoom.CullEnabled)
                    enabled = false;
            }
        }

        public void UpdateRoom(bool updateChildDynamicCullableBases = false)
        {
            Vector3 currentPosition = transform.position;

            var roomId = MapGeneration.RoomIdUtils.RoomAtPositionRaycasts(currentPosition);

            CullableRoom newRoom = null;
            if (roomId != null)
                newRoom = roomId.GetComponent<CullableRoom>();

            if (newRoom == null)
                newRoom = CullingCamera.OutsideRoom;

            if (_linkedRoom != null && newRoom != null &&
                _linkedRoom.gameObject == newRoom.gameObject)
            {
                for (int i = lights.Count - 1; i >= 0; i--)
                {
                    var l = lights[i];
                    if (l == null) { lights.RemoveAt(i); continue; }
                    _linkedRoom.UpdateNearbyLight(l);
                }
                return;
            }

            if (updateChildDynamicCullableBases)
            {
                var children = GetComponentsInChildren<DynamicCullableBase>();
                foreach (var child in children)
                {
                    if (child != null)
                        child.LinkToRoom(newRoom);
                }
            }
            else
            {
                LinkToRoom(newRoom);
            }
        }

        private void Update()
        {
            Vector3 currentPos = _transform.position;
            float distanceMoved = Vector3.Distance(currentPos, _savedPosition);

            if (distanceMoved > 0.01f)
            {
                _savedPosition = currentPos;
                UpdateRoom(false);
            }
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            _transform = transform;
            _savedPosition = _transform.position;
            UpdateRoom(false);
        }
    }
}