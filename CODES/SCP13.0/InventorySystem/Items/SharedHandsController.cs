using System;
using PlayerRoles;
using UnityEngine;

namespace InventorySystem.Items
{
    public class SharedHandsController : MonoBehaviour
    {
        [Serializable]
        private struct GlovesForRole
        {
            public RoleTypeId Role;
            public GameObject Gloves;
        }

        [Serializable]
        private struct SleevesForRole
        {
            public RoleTypeId Role;
            public Material Sleeves;
        }

        public Animator Hands;

        private Transform _trackedPosition;

        private static bool _eventAssigned;
        private static bool _singletonSet;

        [SerializeField] [Header("Sleeves")]
        private Renderer[] _sleevesRenderers;

        [SerializeField]
        private Material _defaultSleeves;

        [SerializeField]
        private SleevesForRole[] _sleevesForRole;

        [SerializeField] [Header("Gloves")]
        private GlovesForRole[] _glovesForRole;

        [SerializeField]
        private GameObject _defaultGloves;

        public static SharedHandsController Singleton { get; private set; }

        public static void UpdateInstance(ItemViewmodelBase ivb)
        {
            if (ivb == null) return;

            if (ivb is AnimatedViewmodelBase animated)
            {
                if (animated.Animator != null)
                {
                    animated.Animator.gameObject.SetActive(true);
                    if (Singleton != null && Singleton.Hands != null)
                    {
                        animated.Animator.avatar = Singleton.Hands.avatar;
                        animated.Animator.runtimeAnimatorController = Singleton.Hands.runtimeAnimatorController;
                    }
                    animated.Animator.Rebind();
                }
            }
            else
            {
                if (Singleton != null && Singleton.Hands != null)
                {
                    Singleton.Hands.gameObject.SetActive(false);
                }
            }
        }

        private void LateUpdate()
        {
            UpdateTrackedPosition();
        }

        private void UpdateTrackedPosition()
        {
            if (_trackedPosition == null) return;

            Hands.transform.localScale = new Vector3(
                Hands.transform.localScale.x,
                Hands.transform.localScale.y,
                _trackedPosition.localScale.z);

            Hands.transform.SetPositionAndRotation(
                _trackedPosition.position,
                _trackedPosition.rotation);
        }

        private void Awake()
        {
            Singleton = this;
            _singletonSet = true;

            Hands.fireEvents = false;

            Action updatePosition = UpdateTrackedPosition;
            AnimatedViewmodelBase.OnSwayUpdated += updatePosition;

            if (!_eventAssigned)
            {
                PlayerRoleManager.OnRoleChanged += RoleChanged;
                _eventAssigned = true;
            }
        }

        private void OnDestroy()
        {
            Action updatePosition = UpdateTrackedPosition;
            AnimatedViewmodelBase.OnSwayUpdated -= updatePosition;

            if (_singletonSet)
            {
                Singleton = null;
                _singletonSet = false;
            }
        }

        private static void RoleChanged(ReferenceHub hub, PlayerRoleBase oldRole, PlayerRoleBase newRole)
        {
            if (hub.isLocalPlayer)
            {
                if (Singleton != null)
                {
                    SetRoleGloves(newRole.RoleTypeId);
                }
            }
        }

        public static void SetRoleGloves(RoleTypeId id)
        {
            if (Singleton == null) return;

            foreach (var renderer in Singleton._sleevesRenderers)
            {
                if (renderer != null)
                {
                    renderer.material = Singleton._defaultSleeves;
                }
            }

            foreach (var sleeve in Singleton._sleevesForRole)
            {
                if (sleeve.Role == id && sleeve.Sleeves != null)
                {
                    foreach (var renderer in Singleton._sleevesRenderers)
                    {
                        if (renderer != null)
                        {
                            renderer.material = sleeve.Sleeves;
                        }
                    }
                    break;
                }
            }

            if (Singleton._defaultGloves != null)
            {
                Singleton._defaultGloves.SetActive(true);
            }

            foreach (var glove in Singleton._glovesForRole)
            {
                if (glove.Gloves != null)
                {
                    glove.Gloves.SetActive(false);
                }
            }

            foreach (var glove in Singleton._glovesForRole)
            {
                if (glove.Role == id && glove.Gloves != null)
                {
                    glove.Gloves.SetActive(true);
                    if (Singleton._defaultGloves != null)
                    {
                        Singleton._defaultGloves.SetActive(false);
                    }
                    break;
                }
            }
        }
    }
}