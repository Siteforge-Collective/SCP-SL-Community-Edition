namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079GuiElementBase : global::UnityEngine.MonoBehaviour
	{
		protected global::PlayerRoles.PlayableScps.Scp079.Scp079Role Role { get; private set; }

		protected ReferenceHub Owner { get; private set; }

		internal virtual void Init(global::PlayerRoles.PlayableScps.Scp079.Scp079Role role, ReferenceHub owner)
		{
			Role = role;
			Owner = owner;
		}

		protected global::UnityEngine.AudioSource PlaySound(global::UnityEngine.AudioClip clip)
		{
			return global::AudioPooling.AudioSourcePoolManager.PlaySound(clip, null, 1f, 1f, FalloffType.Exponential, global::AudioPooling.AudioMixerChannelType.DefaultSfx, 0f);
		}
	}
}
