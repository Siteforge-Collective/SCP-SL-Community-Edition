using System.Collections.Generic;
using Decals;
using UnityEngine;

namespace CustomCulling
{
    internal enum RoomState : byte
    {
        Disabled = 0,
        Enabled = 1,
        LightCrossover = 2,
        AllCrossover = 3,
        DynamicBases = 4
    }

    internal readonly struct UpdateType
    {
        public bool UpdateRenderers { get; }
        public bool UpdateBehaviours { get; }
        public bool UpdateLights { get; }

        public UpdateType(bool updateRenderers, bool updateBehaviours, bool updateLights)
        {
            UpdateRenderers = updateRenderers;
            UpdateBehaviours = updateBehaviours;
            UpdateLights = updateLights;
        }
    }

    public class CullableRoom : CullableBase
    {
        private List<CullableBase> cullableBases = new List<CullableBase>();
        private RoomState currentRoomState = RoomState.Disabled;
        private RoomState previousRoomState = RoomState.Disabled;
        private UpdateType currentUpdateType = _disableAllUpdateType;

        internal List<DoorLinkingRooms> Doors = new List<DoorLinkingRooms>(1);
        internal DoorLinkingRooms CrossoverDoor;

        public List<CullableBase> CullableBases => cullableBases;

        internal RoomState CurrentRoomState
        {
            get => currentRoomState;
            set
            {
                previousRoomState = currentRoomState;
                currentRoomState = value;
            }
        }

        internal UpdateType CurrentUpdateType
        {
            get => currentUpdateType;
            set => currentUpdateType = value;
        }

        public override bool CullEnabled => currentRoomState != RoomState.Disabled;

        private static readonly UpdateType _disableAllUpdateType = new UpdateType(false, false, false);
        private static readonly UpdateType _enableAllUpdateType = new UpdateType(true, true, true);
        private static readonly UpdateType _skipLightUpdateType = new UpdateType(true, true, false);

        public void AddDecal(Decal d)
        {
            if (d == null) return;
            if (d.DecalMaterial == null) return;

            Decals.Add(d);
            d.enabled = CullEnabled;
        }

        internal void AddCullableBase(CullableBase cullableBase)
        {
            if (cullableBase == null || cullableBases.Contains(cullableBase)) return;

            cullableBases.Add(cullableBase);

            UpdateBehaviours(cullableBase, currentUpdateType.UpdateBehaviours);
            UpdateRenderers(cullableBase, currentUpdateType.UpdateRenderers);
            UpdateLights(cullableBase, currentUpdateType.UpdateLights);

            foreach (var light in cullableBase.lights)
            {
                if (light != null)
                    AddLightToNearbyDoors(light);
            }
        }

        internal void RemoveCullableBase(CullableBase cullableBase)
        {
            if (cullableBase == null || !cullableBases.Remove(cullableBase)) return;

            foreach (var light in cullableBase.lights)
            {
                if (light != null)
                    RemoveLightFromNearbyDoors(light);
            }

            ForceUpdateBehaviours(cullableBase, false);

            cullableBase.renderers.RemoveAll(r => r == null);
            cullableBase.otherBehaviours.RemoveAll(b => b == null);
            cullableBase.lights.RemoveAll(l => l == null);
            cullableBase.Decals.RemoveAll(d => d == null);
        }

        internal void AddLightToNearbyDoorsPublic(Light light) => AddLightToNearbyDoors(light);

        private void AddLightToNearbyDoors(Light light)
        {
            foreach (var door in Doors)
            {
                if (door == null) continue;

                bool isForwardRoom = door.ForwardRoom != null &&
                                     door.ForwardRoom.gameObject == gameObject;

                if (CrossoverDoor != null && door.gameObject != CrossoverDoor.gameObject)
                {
                    // Still register with non-crossover doors if the light is in range
                }

                float distanceToDoor = Vector3.Distance(light.transform.position, door.transform.position);

                if (light.range > distanceToDoor)
                {
                    door.AddLight(isForwardRoom, light, distanceToDoor);
                    light.enabled = CullEnabled && !CullingManager.AllLightsDisabled;
                }
            }
        }

        private void RemoveLightFromNearbyDoors(Light light)
        {
            foreach (var door in Doors)
            {
                if (door == null) continue;

                bool isForwardRoom = door.ForwardRoom != null &&
                                     door.ForwardRoom.gameObject == gameObject;

                var nearbyLights = isForwardRoom ? door.ForwardRoomNearbyLights
                                                 : door.BackwardRoomNearbyLights;
                if (nearbyLights == null) continue;

                int idx = nearbyLights.IndexOf(light);
                if (idx >= 0)
                {
                    nearbyLights.RemoveAt(idx);
                    door.RefreshDistances(isForwardRoom);
                }
            }
        }

        internal void UpdateNearbyLight(Light light)
        {
            if (light == null) return;

            foreach (var door in Doors)
            {
                if (door == null) continue;

                bool isCrossoverMismatch = CrossoverDoor != null &&
                                           door.gameObject != CrossoverDoor.gameObject;

                bool isForwardRoom = door.ForwardRoom != null &&
                                     door.ForwardRoom.gameObject == gameObject;

                var nearbyLights = isForwardRoom ? door.ForwardRoomNearbyLights
                                                 : door.BackwardRoomNearbyLights;
                if (nearbyLights == null) continue;

                float distToDoor = Vector3.Distance(light.transform.position, door.transform.position);
                bool inRange = light.range > distToDoor;
                bool alreadyIn = nearbyLights.Contains(light);

                if (inRange && !alreadyIn)
                {
                    door.AddLight(isForwardRoom, light, distToDoor);
                }
                else if (!inRange && alreadyIn)
                {
                    nearbyLights.Remove(light);
                    door.RefreshDistances(isForwardRoom);
                }
            }

            light.enabled = CullEnabled && !CullingManager.AllLightsDisabled;
        }

        internal void UpdateAllLighting()
        {
            bool lightsAllowed = !CullingManager.AllLightsDisabled;

            for (int i = cullableBases.Count - 1; i >= 0; i--)
            {
                var baseObj = cullableBases[i];
                if (baseObj == null) { cullableBases.RemoveAt(i); continue; }
                UpdateLights(baseObj, currentUpdateType.UpdateLights);
            }

            UpdateLights(this, currentUpdateType.UpdateLights);
        }

        internal void UpdateCulling()
        {
            bool stateChanged = currentRoomState != previousRoomState;

            if (stateChanged)
            {
                currentUpdateType = currentRoomState switch
                {
                    RoomState.Enabled => _enableAllUpdateType,
                    RoomState.Disabled => _disableAllUpdateType,
                    // AllCrossover, LightCrossover, DynamicBases
                    _ => _skipLightUpdateType,
                };
                UpdateDoors();
            }

            UpdateRoom();
            UpdateCullableBases();

            if (stateChanged &&
                (currentRoomState == RoomState.Enabled || currentRoomState == RoomState.AllCrossover))
            {
                UpdateCrossoverDoorLights();
            }
        }

        private void UpdateRoom()
        {
            UpdateBehaviours(this, currentUpdateType.UpdateBehaviours);
            UpdateRenderers(this, currentUpdateType.UpdateRenderers);
            UpdateLights(this, currentUpdateType.UpdateLights);
        }

        private void UpdateCullableBases()
        {
            for (int i = cullableBases.Count - 1; i >= 0; i--)
            {
                var item = cullableBases[i];
                if (item == null) { cullableBases.RemoveAt(i); continue; }

                UpdateBehaviours(item, currentUpdateType.UpdateBehaviours);
                UpdateRenderers(item, currentUpdateType.UpdateRenderers);
                UpdateLights(item, currentUpdateType.UpdateLights);
            }
        }

        private void UpdateDoors()
        {
            foreach (var door in Doors)
            {
                if (door == null) continue;

                if (currentRoomState == RoomState.Disabled)
                {
                    door.CullEnabled = false;
                    continue;
                }

                if (currentRoomState == RoomState.AllCrossover)
                {
                    bool isCrossover = CrossoverDoor != null &&
                                       door.gameObject == CrossoverDoor.gameObject;
                    door.CullEnabled = isCrossover;
                }
                else
                {
                    var otherRoom = door.GetOtherRoom(this);
                    door.CullEnabled = CheckRoom(otherRoom) || currentRoomState == RoomState.Enabled;
                }

                door.UpdateBehaviours();
            }
        }

        private void UpdateCrossoverDoorLights()
        {
            foreach (var door in Doors)
            {
                if (door == null) continue;

                if (CrossoverDoor != null && door.gameObject == CrossoverDoor.gameObject)
                    continue;

                bool isForwardRoom = door.ForwardRoom != null &&
                                     door.ForwardRoom.gameObject == gameObject;

                var nearbyLights = isForwardRoom ? door.ForwardRoomNearbyLights
                                                 : door.BackwardRoomNearbyLights;
                if (nearbyLights == null) continue;

                for (int i = nearbyLights.Count - 1; i >= 0; i--)
                {
                    var l = nearbyLights[i];
                    if (l == null) { nearbyLights.RemoveAt(i); continue; }
                    l.enabled = CullEnabled && !CullingManager.AllLightsDisabled;
                }
            }
        }

        private void UpdateBehaviours(CullableBase cullBase, bool isEnabled)
        {
            bool enabled = cullBase.StaticObject || isEnabled;

            for (int i = cullBase.otherBehaviours.Count - 1; i >= 0; i--)
            {
                var b = cullBase.otherBehaviours[i];
                if (b == null) { cullBase.otherBehaviours.RemoveAt(i); continue; }
                b.enabled = enabled;
            }

            for (int i = cullBase.Decals.Count - 1; i >= 0; i--)
            {
                var d = cullBase.Decals[i];
                if (d == null) { cullBase.Decals.RemoveAt(i); continue; }
                d.enabled = enabled;
            }
        }

        private void UpdateRenderers(CullableBase cullBase, bool isEnabled)
        {
            bool enabled = cullBase.StaticObject || isEnabled;

            for (int i = cullBase.renderers.Count - 1; i >= 0; i--)
            {
                var r = cullBase.renderers[i];
                if (r == null) { cullBase.renderers.RemoveAt(i); continue; }
                r.enabled = enabled;
            }
        }

        private void UpdateLights(CullableBase cullBase, bool isEnabled)
        {
            bool lightEnabled = (cullBase.StaticObject || isEnabled) &&
                                !CullingManager.AllLightsDisabled;

            for (int i = cullBase.lights.Count - 1; i >= 0; i--)
            {
                var l = cullBase.lights[i];
                if (l == null) { cullBase.lights.RemoveAt(i); continue; }
                l.enabled = lightEnabled;
            }
        }

        private void ForceUpdateBehaviours(CullableBase cullBase, bool enabled)
        {
            foreach (var r in cullBase.renderers) if (r) r.enabled = enabled;
            foreach (var b in cullBase.otherBehaviours) if (b) b.enabled = enabled;
            foreach (var d in cullBase.Decals) if (d) d.enabled = enabled;

            bool lightEnabled = enabled && !CullingManager.AllLightsDisabled &&
                                (cullBase.StaticObject ||
                                 currentRoomState == RoomState.Enabled ||
                                 currentRoomState == RoomState.AllCrossover ||
                                 currentRoomState == RoomState.LightCrossover);

            foreach (var l in cullBase.lights) if (l) l.enabled = lightEnabled;
        }

        internal static bool CheckRoom(CullableRoom room) => room != null;
    }
}