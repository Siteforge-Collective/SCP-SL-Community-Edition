using UnityEngine;
using UnityEngine.Rendering;

namespace CustomPlayerEffects
{
    [RequireComponent(typeof(Volume))]
    public class LerpPostProcessVisuals : LerpVisualsBase
    {
        protected Volume ProcessVolume { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            ProcessVolume = GetComponent<Volume>();
        }

        protected override void OnWeightChanged(float weight)
        {
            if (ProcessVolume != null)
            {
                ProcessVolume.weight = weight;
            }
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            if (ProcessVolume != null)
            {
                ProcessVolume.enabled = true;
            }
        }

        protected override void OnShutdown()
        {
            base.OnShutdown();

            if (ProcessVolume != null)
            {
                ProcessVolume.enabled = false;
            }
        }

        public LerpPostProcessVisuals()
        {
            UpdateOnRoleChange = true;
            EnableSpeed = 1f;
            DisableSpeed = 1f;
        }
    }
}