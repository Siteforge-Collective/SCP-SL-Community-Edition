using System;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp939
{
	[Serializable]
	public class Scp939LungeAudio
	{
		private Transform _t;

		[SerializeField]
		private AudioClip _harsh;

		[SerializeField]
		private AudioClip _land;

		[SerializeField]
		private AudioClip[] _hits;

		[SerializeField]
		private AudioClip _launch;

		public void Init(Scp939LungeAbility lunge)
		{
		}

		private void OnStateChanged(Scp939LungeState state)
		{
		}

		private void Play(AudioClip clip, float dis)
		{
		}
	}
}
