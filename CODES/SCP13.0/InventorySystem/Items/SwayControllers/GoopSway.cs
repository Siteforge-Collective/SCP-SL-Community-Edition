using System;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.FirstPersonControl.Thirdperson;
using UnityEngine;
using Mirror;

namespace InventorySystem.Items.SwayControllers
{
    public class GoopSway : IItemSwayController
    {
        [Serializable]
        public struct GoopSwaySettings
        {
            public Transform TargetTransform;
            public float SwayIntensity;
            public float TranslationIntensity;
            public float ZAxisIntensity;
            public float SwaySmoothness;
            public float TranslationSmoothness;
            public float BobIntensity;
            public float CentrifugalIntensity;
            public int Invert;

            public GoopSwaySettings(
                        Transform targetTransform = null,
                        float swayIntensity = 1f,
                        float translationIntensity = 1f,
                        float zAxisIntensity = 1f,
                        float swaySmoothness = 1f,
                        float translationSmoothness = 1f,
                        float bobIntensity = 1f,
                        float centrifugalIntensity = 1f,
                        bool invertSway = false)
            {
                TargetTransform = targetTransform;
                SwayIntensity = swayIntensity;
                TranslationIntensity = translationIntensity;
                ZAxisIntensity = zAxisIntensity;
                SwaySmoothness = swaySmoothness;
                TranslationSmoothness = translationSmoothness;
                BobIntensity = bobIntensity;
                CentrifugalIntensity = centrifugalIntensity;
                Invert = invertSway ? -1 : 1;
            }
        }

        private const float MaximumReasonableMouseSpeed = 15f;
        private const float OverallBobMultiplier = 12f;
        private const float OverallSwayMultiplier = 0.013f;

        private readonly GoopSwaySettings _settings;
        private Vector3 _positionOffset;
        private readonly ReferenceHub _owner;
        private readonly Transform _ownerTransform;
        private readonly Transform _camTransform;

        private float _prevRotX;
        private float _prevRotY;

        private float CurRotX => _ownerTransform != null ? _ownerTransform.localEulerAngles.y : 0f;
        private float CurRotY => _camTransform != null ? _camTransform.localEulerAngles.x : 0f;

        private AnimatedCharacterModel CharModel
        {
            get
            {
                if (_owner.roleManager.CurrentRole is IFpcRole fpcRole)
                {
                    return fpcRole.FpcModule.CharacterModelInstance as AnimatedCharacterModel;
                }
                return null;
            }
        }

        public GoopSway(GoopSwaySettings settings, ReferenceHub owner)
        {
            _settings = settings;
            _owner = owner;
            _ownerTransform = owner?.transform;
            _camTransform = owner?.PlayerCameraReference;

            if (_settings.TargetTransform != null)
                _positionOffset = _settings.TargetTransform.localPosition;

            _prevRotX = CurRotX;
            _prevRotY = CurRotY;
        }

        public void UpdateSway()
        {
            if (!NetworkClient.active) return;

            CameraSway(_settings.TargetTransform);
            Transition(_settings.TargetTransform);
        }

        private void CameraSway(Transform tr)
        {
            float deltaX = Mathf.DeltaAngle(_prevRotX, CurRotX);
            float deltaY = Mathf.DeltaAngle(_prevRotY, CurRotY);

            deltaX = Mathf.Clamp(deltaX, -MaximumReasonableMouseSpeed, MaximumReasonableMouseSpeed);
            deltaY = Mathf.Clamp(deltaY, -MaximumReasonableMouseSpeed, MaximumReasonableMouseSpeed);

            float inv = (float)_settings.Invert;

            Quaternion targetRot = Quaternion.AngleAxis(deltaX * _settings.SwayIntensity * inv, Vector3.up) *
                                  Quaternion.AngleAxis(deltaY * _settings.SwayIntensity, Vector3.right) *
                                  Quaternion.AngleAxis(deltaX * _settings.SwayIntensity * inv, Vector3.forward);

            tr.localRotation = Quaternion.Slerp(tr.localRotation, targetRot, Time.deltaTime * _settings.SwaySmoothness);

            _prevRotX = CurRotX;
            _prevRotY = CurRotY;
        }

        private void Transition(Transform tr)
        {
            Vector3 velocity = _owner.GetVelocity();
            Vector3 localVelocity = _ownerTransform.InverseTransformDirection(velocity);

            float moveX = localVelocity.x * _settings.TranslationIntensity;
            float moveZ = localVelocity.z * _settings.TranslationIntensity;

            float deltaX = Mathf.DeltaAngle(_prevRotX, CurRotX);
            float centrifugal = deltaX * _settings.CentrifugalIntensity;

            Vector3 targetPos = _positionOffset;
            targetPos.x += moveX + centrifugal;
            targetPos.z -= moveZ * _settings.ZAxisIntensity;

            AnimatedCharacterModel charModel = CharModel;
            if (charModel != null)
            {
                float bob = charModel.Animator.GetFloat("P_Speed") * _settings.BobIntensity * OverallBobMultiplier;
                targetPos.y += Mathf.Sin(Time.time * 10f) * bob * 0.01f;
            }

            tr.localPosition = Vector3.Lerp(tr.localPosition, targetPos, Time.deltaTime * _settings.TranslationSmoothness);
        }
    }
}