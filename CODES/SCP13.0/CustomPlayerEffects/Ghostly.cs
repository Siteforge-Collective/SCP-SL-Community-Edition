using System;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.PlayableScps.Scp106;
using UnityEngine;

namespace CustomPlayerEffects
{
    public class Ghostly : StatusEffectBase, IFpcCollisionModifier
    {
        public LayerMask DetectionMask => Scp106MovementModule.PassableDetectionMask;

        public override EffectClassification Classification => EffectClassification.Positive;

        public void ProcessColliders(ArraySegment<Collider> detections)
        {
            int count = detections.Count;
            for (int i = 0; i < count; i++)
            {
                Collider collider = detections.Array[detections.Offset + i];
                if (collider != null)
                {
                    float slowdown = Scp106MovementModule.GetSlowdownFromCollider(collider);
                    collider.enabled = slowdown == 0f;
                }
            }
        }

        protected override void Enabled()
        {
            base.Enabled();

            if (Hub.roleManager.CurrentRole is IFpcRole fpcRole)
            {
                FpcCollisionProcessor.AddModifier(this, fpcRole);
            }
        }

        protected override void Disabled()
        {
            base.Disabled();

            FpcCollisionProcessor.RemoveModifier(this);
        }
    }
}