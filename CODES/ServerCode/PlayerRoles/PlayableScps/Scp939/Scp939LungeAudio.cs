namespace PlayerRoles.PlayableScps.Scp939
{
	[global::System.Serializable]
	public class Scp939LungeAudio
	{
		private global::UnityEngine.Transform _t;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _harsh;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _land;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip[] _hits;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _launch;

		public void Init(global::PlayerRoles.PlayableScps.Scp939.Scp939LungeAbility lunge)
		{
			lunge.OnStateChanged += OnStateChanged;
			_t = lunge.transform;
		}

		private void OnStateChanged(global::PlayerRoles.PlayableScps.Scp939.Scp939LungeState state)
		{
			switch (state)
			{
			case global::PlayerRoles.PlayableScps.Scp939.Scp939LungeState.LandHit:
				Play(_hits.RandomItem(), 25f);
				break;
			case global::PlayerRoles.PlayableScps.Scp939.Scp939LungeState.LandHarsh:
				Play(_harsh, 12.5f);
				break;
			case global::PlayerRoles.PlayableScps.Scp939.Scp939LungeState.Triggered:
				Play(_launch, 25f);
				return;
			case global::PlayerRoles.PlayableScps.Scp939.Scp939LungeState.None:
				return;
			}
			Play(_land, 25f);
		}

		private void Play(global::UnityEngine.AudioClip clip, float dis)
		{
			global::AudioPooling.AudioSourcePoolManager.PlaySound(clip, _t, dis);
		}
	}
}
