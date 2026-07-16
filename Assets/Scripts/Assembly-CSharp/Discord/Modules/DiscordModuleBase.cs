using UnityEngine;

namespace Discord.Modules
{
    public abstract class DiscordModuleBase : MonoBehaviour
    {
        [SerializeField]
        protected bool _isEnabled;

        public virtual bool IsEnabled
        {
            get => _isEnabled;
            set => _isEnabled = value;
        }

        public void UpdateModule()
        {
            if (this.ToString() != null)
            {
            }
        }

        protected virtual void OnUpdateModule()
        {
        }

        protected virtual void OnDestroy()
        {
            // base.stop() - FMOD.IChannelControl::stop()
        }
    }
}
