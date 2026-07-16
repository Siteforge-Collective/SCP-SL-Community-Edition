using InventorySystem.Items.SwayControllers;
using UnityEngine;

namespace InventorySystem.Items
{
    public class StandardAnimatedViemodel : AnimatedViewmodelBase
    {
        [SerializeField] private Transform _handsPivot;
        [SerializeField] private Transform _trackerCamera;
        [SerializeField] private float _trackerForceScale = 1f;
        [SerializeField] private Vector3 _trackerOffset;
        [SerializeField] private float _fov = 50f;

        private GoopSway _goopSway;

        public override IItemSwayController SwayController => _goopSway;

        public override float ViewmodelCameraFOV => _fov;

        public override void InitAny()
        {
            base.InitAny();

            _goopSway = new GoopSway(new GoopSway.GoopSwaySettings(
                targetTransform: _trackerCamera,
                swayIntensity: 0.5f,
                zAxisIntensity: 0f
            ), Hub);

        }

        internal override void OnEquipped()
        {
            base.OnEquipped();

            if (_trackerCamera != null && Mathf.Abs(_trackerOffset.z) > 0.001f)
            {
                Quaternion rotationOffset = Quaternion.Euler(0f, 0f, _trackerOffset.z);
                var trackerShake = new CameraShaking.TrackerShake(_trackerCamera, rotationOffset, _trackerForceScale);
                CameraShaking.CameraShakeController.AddEffect(trackerShake);
            }
        }
    }
}