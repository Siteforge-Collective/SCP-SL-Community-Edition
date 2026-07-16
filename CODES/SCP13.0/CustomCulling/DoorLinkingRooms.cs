using Interactables.Interobjects.DoorUtils;
using System.Collections.Generic;
using UnityEngine;

namespace CustomCulling
{
    public class DoorLinkingRooms : CullableBase
    {
        internal float ForwardRoomLightRange;
        internal CullableRoom ForwardRoom;
        internal List<Light> ForwardRoomNearbyLights = new List<Light>();

        internal float BackwardRoomLightRange;
        internal CullableRoom BackwardRoom;
        internal List<Light> BackwardRoomNearbyLights = new List<Light>();

        private DoorVariant _doorVariant;
        private bool _initialized;

        internal bool CanNotSeeThrough
        {
            get
            {
                if (_doorVariant == null) return false;
                if (!_doorVariant.CanSeeThrough)
                {
                    return _doorVariant.GetExactState() == 0f;
                }
                return false;
            }
        }

        internal CullableRoom GetOtherRoom(CullableRoom currentRoom)
        {
            if (ForwardRoom != null && ForwardRoom.gameObject == currentRoom.gameObject)
                return BackwardRoom;
            return ForwardRoom;
        }

        internal void Initialize()
        {
            if (_initialized) return;

            Transform t = transform;

            var forwardRoomId = MapGeneration.RoomIdUtils.RoomAtPosition(
                t.position + t.forward);
            if (forwardRoomId != null)
                ForwardRoom = forwardRoomId.GetComponentInParent<CullableRoom>();

            var backwardRoomId = MapGeneration.RoomIdUtils.RoomAtPosition(
                t.position - t.forward);
            if (backwardRoomId != null)
                BackwardRoom = backwardRoomId.GetComponentInParent<CullableRoom>();

            if (ForwardRoom == null || BackwardRoom == null) return;

            if (ForwardRoom.gameObject == BackwardRoom.gameObject)
            {
                ForwardRoom.AddCullableBase(this);
                return;
            }

            _initialized = true;
            _doorVariant = GetComponent<DoorVariant>();

            ForwardRoom.Doors.Add(this);
            BackwardRoom.Doors.Add(this);

            GetNearbyLights(true);  
            GetNearbyLights(false); 
        }

        internal void AddLight(bool isForwardRoom, Light light, float distanceToDoor)
        {
            float range = light.range;

            if (isForwardRoom)
            {
                if (range > ForwardRoomLightRange)
                    ForwardRoomLightRange = range;
                ForwardRoomNearbyLights.Add(light);
            }
            else
            {
                if (range > BackwardRoomLightRange)
                    BackwardRoomLightRange = range;
                BackwardRoomNearbyLights.Add(light);
            }
        }

        internal void RefreshDistances(bool forwardRoom)
        {
            var lights = forwardRoom ? ForwardRoomNearbyLights : BackwardRoomNearbyLights;
            Vector3 doorPos = transform.position;
            float maxRange = 0f;

            for (int i = lights.Count - 1; i >= 0; i--)
            {
                var light = lights[i];
                if (light == null) { lights.RemoveAt(i); continue; }

                float dist = Vector3.Distance(light.transform.position, doorPos);
                if (light.range > dist)
                    maxRange = Mathf.Max(maxRange, light.range);
                else
                    lights.RemoveAt(i); 
            }

            if (forwardRoom) ForwardRoomLightRange = maxRange;
            else BackwardRoomLightRange = maxRange;
        }

        private void GetNearbyLights(bool isForward)
        {
            var room = isForward ? ForwardRoom : BackwardRoom;
            if (room == null) return;

            Vector3 doorPos = transform.position;

            for (int i = room.lights.Count - 1; i >= 0; i--)
            {
                var light = room.lights[i];
                if (light == null) { room.lights.RemoveAt(i); continue; }

                float dist = Vector3.Distance(light.transform.position, doorPos);
                if (light.range > dist)
                    AddLight(isForward, light, dist);
            }
        }

        private void Start()
        {
            Initialize();
        }
    }
}