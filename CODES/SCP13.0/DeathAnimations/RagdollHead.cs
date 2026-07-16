using PlayerRoles.Spectating;
using UnityEngine;

namespace DeathAnimations
{
    public class RagdollHead : FirstpersonDeathAnimation
    {
        public float CameraFixSpeed;

        public GameObject SpectatorCameraAnchor;

        protected override void OnAnimationStarted()
        {
            if (IsFirstperson)
            {
                EventAssigned = true;
            }

            if (!IsFirstperson)
            {
                Destroy(this);
                return;
            }

            enabled = true;
            SpectatorCameraAnchor.transform.SetParent(null);
            SpectatorTargetTracker.SetTrackedTransform(SpectatorCameraAnchor.transform);
            SpectatorCameraAnchor.SetActive(true);
        }

        protected override void OnAnimationEnded()
        {
            SpectatorTargetTracker.SetTrackedTransform(null);
            Destroy(this);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.relativeVelocity.magnitude > 40f)
            {
                BloodEffectsSystem.LocalPlayerSingleton.AddScrapes(collision.relativeVelocity.magnitude / 40f);
            }
        }
    }
}
