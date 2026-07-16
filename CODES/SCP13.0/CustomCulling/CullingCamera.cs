using MapGeneration;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using PlayerStatsSystem;
using System.Collections.Generic;
using UnityEngine;

namespace CustomCulling
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    public class CullingCamera : MonoBehaviour
    {
        public int RoomDepth = 2;

        private const float CloseToDoorDistance_Far = 9f;
        private const float VisionAngleMultiplier = 1.5f;
        private const float FarVisionAngleMultiplier = 1.2f;
        private const float DoorDistanceAdjustment = 1f;
        public const float NoClipInitialCullDistance = 75f;
        private const float DoorDistance = 13.7f;

        internal static List<CullableRoom> EnabledCullableRooms = new List<CullableRoom>(8);
        internal static CullableRoom[] AllRooms;
        internal static CullableRoom OutsideRoom;
        internal static NoClipCamExtraPoint[] NoClipCamPoints;
        internal static AspectRatioSync AspectSyncer;

        private static float _fullVisionAngle;
        private static float _farVisionAngle;
        private static Vector3 _cachedPosition;
        private static Vector2 _cached2DPos;
        private static Vector2 _cached2DLookDir;
        private static float _closeToDoorDistance;

        private static CullableRoom _myCachedRoom;
        private static CullableRoom _cachedNextRoom;
        private static DoorLinkingRooms _cachedNextDoor;
        private static CullingCamera _cachedCloseToDoor_FarCheck;

        private void OnEnable()
        {
            MainCameraController.OnUpdated += UpdateCulling;
        }

        private void OnDisable()
        {
            MainCameraController.OnUpdated -= UpdateCulling;
        }

        private void UpdateCulling()
        {
            Transform camTransform = MainCameraController.CurrentCamera;
            if (camTransform == null) return;

            _cachedPosition = camTransform.position;
            _cached2DPos = new Vector2(_cachedPosition.x, _cachedPosition.z);
            _cached2DLookDir = new Vector2(camTransform.forward.x, camTransform.forward.z).normalized;

            if (AspectSyncer != null)
            {
                _fullVisionAngle = AspectSyncer.XScreenEdge * VisionAngleMultiplier;
                _farVisionAngle = AspectSyncer.XScreenEdge * FarVisionAngleMultiplier;
                _closeToDoorDistance = AspectSyncer.AspectRatio * 1.1f;
            }

            ResetCullableBases();

            if (!ReferenceHub.TryGetLocalHub(out var hub)) return;

            if (Outside.Singleton.LocalHostIsOutside)
            {
                EnableCullableRoom(OutsideRoom, RoomState.Enabled);
            }
            else
            {
                var adminFlags = hub.playerStats.GetModule<AdminFlagsStat>();

                if (adminFlags.Flags != AdminFlags.None)
                {
                    DistanceBasedCulling(NoClipInitialCullDistance);
                }
                else
                {
                    PerformPortalCulling(hub);
                }
            }

            UpdateCullableBases();
        }

        private void PerformPortalCulling(ReferenceHub hub)
        {
            _cachedCloseToDoor_FarCheck = null;

            var roomId = RoomIdUtils.RoomAtPositionRaycasts(hub.transform.position);
            _myCachedRoom = roomId != null ? roomId.GetComponent<CullableRoom>() : null;

            if (_myCachedRoom != null)
            {
                EnableCullableRoom(_myCachedRoom, RoomState.Enabled);

                foreach (var door in _myCachedRoom.Doors)
                    TestFirstDoor(door);
            }
            else
            {
                EnableCullableRoom(OutsideRoom, RoomState.Enabled);
            }
        }

        private void DistanceBasedCulling(float distanceToCheck)
        {
            for (int i = 0; i < AllRooms.Length; i++)
            {
                var room = AllRooms[i];
                if (room == null) continue;

                float dist = Vector3.Distance(_cachedPosition, room.transform.position);
                if (dist <= distanceToCheck)
                    EnableCullableRoom(room, RoomState.Enabled, null);
            }

            for (int i = 0; i < NoClipCamPoints.Length; i++)
            {
                var point = NoClipCamPoints[i];
                if (point == null) continue;

                float dist = Vector3.Distance(_cachedPosition, point.transform.position);
                if (dist <= distanceToCheck)
                    EnableCullableRoom(point.CullableRoom, RoomState.Enabled, null);
            }
        }

        private void TestFirstDoor(DoorLinkingRooms door)
        {
            if (door == null) return;

            CullableRoom myRoom = _myCachedRoom;
            if (myRoom == null) return;

            bool isForwardSide = door.ForwardRoom != null &&
                                       door.ForwardRoom.gameObject == myRoom.gameObject;
            CullableRoom otherRoom = isForwardSide ? door.BackwardRoom : door.ForwardRoom;

            if (!CullableRoom.CheckRoom(otherRoom)) return;

            Vector3 doorWorldPos = door.transform.position;
            Vector2 doorPos2D = new Vector2(doorWorldPos.x, doorWorldPos.z);
            Vector2 toDoor2D = doorPos2D - _cached2DPos;
            float distToDoor = toDoor2D.magnitude;

            bool inVision = distToDoor < 0.5f; 
            if (!inVision && distToDoor > 0f)
                inVision = Vector2.Dot(toDoor2D / distToDoor, _cached2DLookDir) >= _fullVisionAngle;

            bool withinDoorRange = distToDoor <= DoorDistance;

            if (!inVision && !withinDoorRange) return;

            float lightRange = isForwardSide ? door.ForwardRoomLightRange
                                             : door.BackwardRoomLightRange;

            if (door.CanNotSeeThrough)
            {
                if (lightRange > distToDoor)
                    EnableCullableRoom(otherRoom, RoomState.LightCrossover, door);
                return;
            }

            if (inVision || withinDoorRange)
                EnableCullableRoom(otherRoom, RoomState.AllCrossover, door);

            if (_cachedCloseToDoor_FarCheck == null && (inVision || withinDoorRange))
            {
                _cachedCloseToDoor_FarCheck = this;
                EnableCullableRoom(otherRoom, RoomState.Enabled, door);
                TestFarDoor(otherRoom, door, RoomDepth);
            }

            float myLightRange = isForwardSide ? door.BackwardRoomLightRange
                                               : door.ForwardRoomLightRange;
            if (myLightRange > distToDoor)
                EnableCullableRoom(otherRoom, RoomState.LightCrossover, door);
        }

        private void TestFarDoor(CullableRoom targetRoom, DoorLinkingRooms myDoor, int renderDepth)
        {
            if (renderDepth <= 0 || targetRoom == null) return;

            _cachedNextRoom = null;
            _cachedNextDoor = null;

            foreach (var door in targetRoom.Doors)
            {
                if (door == null) continue;
                if (myDoor != null && door.gameObject == myDoor.gameObject) continue;

                TestFarDoor(myDoor != null ? myDoor.transform.position : _cachedPosition,
                            door, targetRoom, renderDepth);
            }

            if (_cachedNextRoom != null)
            {
                var nextRoom = _cachedNextRoom;
                var nextDoor = _cachedNextDoor;
                _cachedNextRoom = null;
                _cachedNextDoor = null;
                TestFarDoor(nextRoom, nextDoor, renderDepth - 1);
            }
        }

        private void TestFarDoor(Vector3 currentDoorPos,
                                  DoorLinkingRooms door,
                                  CullableRoom currentRoom,
                                  int renderDepth)
        {
            if (door == null || currentRoom == null) return;

            bool isForwardRoom = door.ForwardRoom != null &&
                                       door.ForwardRoom.gameObject == currentRoom.gameObject;
            CullableRoom otherRoom = isForwardRoom ? door.BackwardRoom : door.ForwardRoom;

            if (!CullableRoom.CheckRoom(otherRoom)) return;

            Vector3 doorWorldPos = door.transform.position;
            Vector2 doorPos2D = new Vector2(doorWorldPos.x, doorWorldPos.z);
            Vector2 toDoor2D = (doorPos2D - _cached2DPos);
            float distToCamera = toDoor2D.magnitude;

            if (distToCamera < 0.001f) return;

            float dotProduct = Vector2.Dot(toDoor2D / distToCamera, _cached2DLookDir);

            bool inFarVision = dotProduct >= _farVisionAngle;

            if (door.CanNotSeeThrough)
            {
                if (inFarVision)
                {
                    float lr = isForwardRoom ? door.ForwardRoomLightRange
                                             : door.BackwardRoomLightRange;
                    if (lr > distToCamera)
                        EnableCullableRoom(otherRoom, RoomState.LightCrossover, door);
                }
                return;
            }

            if (!inFarVision) return;

            bool closeFar = distToCamera <= CloseToDoorDistance_Far + DoorDistanceAdjustment;

            if (closeFar)
            {
                EnableCullableRoom(otherRoom, RoomState.AllCrossover, door);
                _cachedNextRoom = otherRoom;
                _cachedNextDoor = door;
            }
            else
            {
                EnableCullableRoom(otherRoom, RoomState.LightCrossover, door);
            }
        }

        private void ResetCullableBases()
        {
            foreach (var room in EnabledCullableRooms)
            {
                if (room != null)
                    room.CurrentRoomState = RoomState.Disabled;
            }
            EnabledCullableRooms.Clear();
        }

        private void UpdateCullableBases()
        {
            for (int i = EnabledCullableRooms.Count - 1; i >= 0; i--)
            {
                var room = EnabledCullableRooms[i];
                if (!room.CullEnabled)
                {
                    EnabledCullableRooms.RemoveAt(i);
                }
                else
                {
                    room.UpdateCulling();
                }
            }
        }

        internal void EnableCullableRoom(CullableRoom cullableRoom,
                                          RoomState roomState,
                                          DoorLinkingRooms crossoverDoor = null)
        {
            if (cullableRoom == null) return;

            cullableRoom.CrossoverDoor = crossoverDoor;
            cullableRoom.CurrentRoomState = roomState;

            if (!EnabledCullableRooms.Contains(cullableRoom))
                EnabledCullableRooms.Add(cullableRoom);
        }
    }
}