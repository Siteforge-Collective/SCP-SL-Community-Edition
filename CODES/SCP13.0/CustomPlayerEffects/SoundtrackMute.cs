using RemoteAdmin.Interfaces;

namespace CustomPlayerEffects
{
    public class SoundtrackMute : StatusEffectBase, ISoundtrackMutingEffect, ICustomRADisplay
    {
        public string DisplayName { get; }

        public bool CanBeDisplayed { get; }

        public bool MuteSoundtrack => base.IsEnabled;
    }
}
