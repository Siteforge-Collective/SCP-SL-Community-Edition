using RelativePositioning;
using System;
using UnityEngine;

namespace RelativePositioning
{
    public class WaypointBacktracker : IDisposable
    {
        public Vector3 _prevPosition;

        public Quaternion _prevRotation;

        public WaypointBase _waypoint;

        public WaypointBacktracker(byte waypointId, double lagCompensation)
        {
            if (WaypointBase.TryGetWaypoint(waypointId, out var wp))
            {
                Setup(wp, lagCompensation);
            }
        }

        public WaypointBacktracker(WaypointBase wp, double lagCompensation)
        {
            Setup(wp, lagCompensation);
        }

        public void Dispose()
        {
            if (_waypoint != null)
            {
                _waypoint.transform.SetPositionAndRotation(_prevPosition, _prevRotation);
            }
        }

        public void Setup(WaypointBase wp, double lagCompensation)
        {
            if (wp is IBacktrackableWaypoint backtrackableWaypoint)
            {
                _waypoint = wp;
                wp.transform.GetPositionAndRotation(out _prevPosition, out _prevRotation);
                backtrackableWaypoint.Backtrack(lagCompensation);
            }
        }
    }
}