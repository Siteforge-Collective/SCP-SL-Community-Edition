using System;
using UnityEngine;

namespace Interactables
{
    public class CenterScreenRaycast : MonoBehaviour
    {
        [Header("Raycast Settings")]
        [SerializeField]
        private float _rayDistance;

        [SerializeField]
        private LayerMask _centerScreenRayHits; 

        public static event Action<RaycastHit> OnCenterRaycastHit;
        public static event Action OnCenterRaycastMissed;

        private void Awake()
        {
            MainCameraController.OnUpdated += PerformRaycast;
        }

        private void OnDestroy()
        {
            MainCameraController.OnUpdated -= PerformRaycast;
        }

        private void PerformRaycast()
        {
            Transform cameraTransform = MainCameraController.CurrentCamera;
            if (cameraTransform == null) return;

            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, _rayDistance, _centerScreenRayHits))
            {
                OnCenterRaycastHit?.Invoke(hit);
            }
            else
            {
                OnCenterRaycastMissed?.Invoke();
            }
        }
    }
}