using UnityEngine;

namespace PlayerRoles.PlayableScps
{
	public class CooldownAudio : MonoBehaviour
	{
		public double Cooldown;

		[SerializeField]
		private AudioClip _cooldownAudio;

		[SerializeField]
		private PlayerRoleBase _player;

		private double _lastTime;

		public void PlayAudio()
		{
		}
	}
}
