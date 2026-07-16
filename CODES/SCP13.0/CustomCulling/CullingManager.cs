using System.Collections.Generic;
using Decals;
using MEC;
using UnityEngine;

namespace CustomCulling
{
    public class CullingManager : MonoBehaviour
    {
        private static bool _allLightsDisabled;
        private static bool _initialized;

        public static bool Initialized
        {
            get => _initialized;
            set => _initialized = value;
        }

        public static bool AllLightsDisabled
        {
            get => _allLightsDisabled;
            set
            {
                _allLightsDisabled = value;
                for (int i = CullingCamera.EnabledCullableRooms.Count - 1; i >= 0; i--)
                {
                    var room = CullingCamera.EnabledCullableRooms[i];
                    if (room != null)
                        room.UpdateAllLighting();
                    else
                        CullingCamera.EnabledCullableRooms.RemoveAt(i);
                }
            }
        }

        public static void TryAddBehaviours<T>(T[] behaviours, CullableRoom room = null)
            where T : Behaviour
        {
            if (behaviours == null || behaviours.Length == 0) return;

            if (room == null && behaviours[0] != null)
            {
                if (!CheckForRoom(behaviours[0].transform, out room))
                    return;
            }

            foreach (var behaviour in behaviours)
            {
                if (behaviour == null || room == null) continue;
                room.otherBehaviours.Add(behaviour);
                behaviour.enabled = room.CullEnabled;
            }
        }

        public static void TryAddBehaviours(Renderer[] renderers, CullableRoom room = null)
        {
            if (renderers == null || renderers.Length == 0) return;

            if (room == null && renderers[0] != null)
            {
                if (!CheckForRoom(renderers[0].transform, out room))
                    return;
            }

            foreach (var renderer in renderers)
            {
                if (renderer == null || !IsRendererValid(renderer) || room == null) continue;
                room.renderers.Add(renderer);
                renderer.enabled = room.CullEnabled;
            }
        }

        public static void TryAddBehaviour<T>(T behaviour, CullableRoom room = null)
            where T : Behaviour
        {
            if (behaviour == null) return;

            if (room == null && !CheckForRoom(behaviour.transform, out room))
                return;

            if (room == null) return;

            if (behaviour is Light light)
            {
                room.lights.Add(light);
                room.AddLightToNearbyDoorsPublic(light);
                light.enabled = room.CullEnabled && !AllLightsDisabled;
                return;
            }

            if (behaviour is Decal decal)
            {
                room.AddDecal(decal);
                return;
            }

            room.otherBehaviours.Add(behaviour);
            behaviour.enabled = room.CullEnabled;
        }

        public static void TryAddBehaviour(Renderer renderer, CullableRoom room = null)
        {
            if (renderer == null || !IsRendererValid(renderer)) return;

            if (room == null && !CheckForRoom(renderer.transform, out room))
                return;

            if (room != null)
            {
                room.renderers.Add(renderer);
                renderer.enabled = room.CullEnabled;
            }
        }
        internal static bool IsRendererValid(Renderer targetRenderer)
        {
            return !targetRenderer.gameObject.name.StartsWith("RID");
        }

        private static bool CheckForRoom(Transform cullTransform, out CullableRoom room)
        {
            room = null;
            if (cullTransform == null) return false;

            var roomIdentifier = MapGeneration.RoomIdUtils.RoomAtPosition(cullTransform.position);
            if (roomIdentifier != null)
                room = roomIdentifier.GetComponent<CullableRoom>();

            return room != null;
        }

        private void Awake()
        {
            Timing.RunCoroutine(DelayInitCull());
        }

        private void OnDestroy() { }

        private bool CanInitializeCulling()
        {
            if (ReferenceHub.TryGetLocalHub(out var hub))
                return hub.roleManager.CurrentRole.RoleTypeId != PlayerRoles.RoleTypeId.Scp173;
            return false;
        }

        private IEnumerator<float> DelayInitCull()
        {
            yield return Timing.WaitUntilTrue(CanInitializeCulling);

            CullingCamera.NoClipCamPoints = FindObjectsOfType<NoClipCamExtraPoint>();
            CullingCamera.AllRooms = FindObjectsOfType<CullableRoom>();

            foreach (var room in CullingCamera.AllRooms)
            {
                if (room.transform.position.y > 900f)
                {
                    CullingCamera.OutsideRoom = room;
                    break;
                }
            }

            var doors = FindObjectsOfType<DoorLinkingRooms>();
            foreach (var door in doors)
                door.Initialize();

            var terrains = FindObjectsOfType<Terrain>();
            TryAddBehaviours(terrains);

            if (ReferenceHub.LocalHub != null)
                CullingCamera.AspectSyncer = ReferenceHub.LocalHub.aspectRatioSync;

            CullingCamera.EnabledCullableRooms = new List<CullableRoom>();

            Initialized = true;
        }
    }
}