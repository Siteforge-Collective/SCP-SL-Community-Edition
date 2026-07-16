using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079GuiElementBase : MonoBehaviour
	{
		protected Scp079Role Role { get; private set; }

		protected ReferenceHub Owner { get; private set; }

		internal virtual void Init(Scp079Role role, ReferenceHub owner)
		{
		}

		protected AudioSource PlaySound(AudioClip clip)
		{
			return null;
		}
	}
}
