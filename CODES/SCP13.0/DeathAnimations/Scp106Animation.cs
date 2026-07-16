using PlayerRoles.PlayableScps.Scp106;
using UnityEngine;

namespace DeathAnimations
{
    public class Scp106Animation : FirstpersonDeathAnimation
    {
        public float delay;

        public Transform portalAnchor;

        private bool _disable;

        private float _fadeTime;

        private void Update()
        {
            _fadeTime += Time.deltaTime;

            if (!_disable)
            {
                Scp106Hud.SetDissolveAnimation(_fadeTime);
            }

            if (delay > _fadeTime) return;

            if (IsFirstperson)
            {
                var renderers = GetComponentsInChildren<Renderer>();
                foreach (var renderer in renderers)
                {
                    renderer.gameObject.SetActive(false);
                }
                /*
                if (portalAnchor != null)
                {
                    var portal = GameObject.FindObjectOfType<Scp106Portal>();
                    if (portal != null)
                    {
                        portalAnchor.parent = portal.transform;
                    }
                }
                */
            }

            if (!IsFirstperson)
            {
                _disable = true;
            }
        }

        protected override void OnAnimationStarted()
        {
            base.OnAnimationStarted();
            _fadeTime = 0f;
            _disable = false;
        }

        protected override void OnAnimationEnded()
        {
            base.OnAnimationEnded();
            if (!_disable)
            {
                Scp106Hud.SetDissolveAnimation(0f);
                _disable = true;
            }
        }
    }
}
