using System;
using UnityEngine;

namespace RelativePositioning
{
    public abstract class WaypointBase : MonoBehaviour
    {
        private static readonly bool[] SetWaypoints = new bool[255];

        private static readonly byte[] WaypointIndexes = new byte[255];

        private static readonly WaypointBase[] AllWaypoints = new WaypointBase[255];

        private byte _id;

        private byte _index;

        protected abstract float SqrDistanceTo(Vector3 pos);

        public abstract Vector3 GetWorldspacePosition(Vector3 relPosition);

        public abstract Vector3 GetRelativePosition(Vector3 worldPos);

        public virtual Quaternion GetWorldspaceRotation(Quaternion relRotation)
        {
            return relRotation;
        }

        public virtual Quaternion GetRelativeRotation(Quaternion worldRot)
        {
            return worldRot;
        }

        protected virtual void Start()
        {
            for (byte b = 1; b < byte.MaxValue; b++)
            {
                if (!SetWaypoints[b])
                {
                    _index = b;
                    AllWaypoints[b] = this;
                    SetWaypoints[b] = true;
                    return;
                }
            }
            Debug.LogError("Could not add waypoint '" + base.name + "' - the list is full.");
        }

        protected virtual void OnDestroy()
        {
            AllWaypoints[_index] = null;
            SetWaypoints[_index] = false;
        }

        protected void SetId(byte newId)
        {
            if (newId == 0)
            {
                throw new InvalidOperationException("Cannot assign ID of 0 to a waypoint. This ID is reserved for the value of null.");
            }
            _id = newId;
            WaypointIndexes[_id] = _index;
        }

        public static void GetRelativePosition(Vector3 point, out byte closestId, out Vector3 rel)
        {
            float num = float.MaxValue;
            rel = Vector3.zero;
            closestId = 0;
            WaypointBase waypointBase = null;
            for (byte b = 1; b < byte.MaxValue; b++)
            {
                if (SetWaypoints[b])
                {
                    WaypointBase waypointBase2 = AllWaypoints[b];
                    float num2 = waypointBase2.SqrDistanceTo(point);
                    if (!(num2 > num))
                    {
                        num = num2;
                        waypointBase = waypointBase2;
                        closestId = waypointBase2._id;
                    }
                }
            }
            if (closestId != 0)
            {
                rel = waypointBase.GetRelativePosition(point);
            }
        }

        public static Vector3 GetWorldPosition(byte id, Vector3 point)
        {
            if (!TryGetWaypoint(id, out var wp))
            {
                return point;
            }
            return wp.GetWorldspacePosition(point);
        }

        public static Quaternion GetRelativeRotation(byte id, Quaternion rot)
        {
            if (!TryGetWaypoint(id, out var wp))
            {
                return rot;
            }
            return wp.GetRelativeRotation(rot);
        }

        public static Quaternion GetWorldRotation(byte id, Quaternion rot)
        {
            if (!TryGetWaypoint(id, out var wp))
            {
                return rot;
            }
            return wp.GetWorldspaceRotation(rot);
        }

        public static bool TryGetWaypoint(byte id, out WaypointBase wp)
        {
            int num = WaypointIndexes[id];
            bool result = SetWaypoints[num];
            wp = AllWaypoints[num];
            return result;
        }
    }
}
