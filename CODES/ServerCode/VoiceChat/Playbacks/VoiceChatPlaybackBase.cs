namespace VoiceChat.Playbacks
{
	[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.AudioSource))]
	public abstract class VoiceChatPlaybackBase : global::UnityEngine.MonoBehaviour
	{
		public float VolumeScale = 1f;

		private float _collectedLoudness;

		private int _collectedSamples;

		private float _targetLoudness;

		private const int LoudnessCollectorThreshold = 1200;

		private const float LoudnessCollectorMultiplier = 5f;

		private const float LoudnessLerpSpeed = 40f;

		private static int _flatlinePcmLen = 0;

		private static float[] _flatlinePcm = new float[0];

		public global::UnityEngine.AudioSource Source { get; private set; }

		public float Loudness { get; private set; }

		public abstract int MaxSamples { get; }

		private static global::UnityEngine.AudioClip Flatline
		{
			get
			{
				int num = 48000;
				int channels = 1;
				return global::UnityEngine.AudioClip.Create(string.Empty, num, channels, num, stream: true, delegate(float[] buf)
				{
					int num2 = buf.Length;
					if (_flatlinePcmLen < num2)
					{
						_flatlinePcm = new float[num2];
						for (int i = 0; i < num2; i++)
						{
							_flatlinePcm[i] = 1f;
						}
						_flatlinePcmLen = num2;
					}
					global::System.Array.Copy(_flatlinePcm, buf, num2);
				});
			}
		}

		private void OnAudioFilterRead(float[] data, int channels)
		{
			int num = MaxSamples * channels;
			int num2 = data.Length;
			if (num == 0)
			{
				global::System.Array.Clear(data, 0, num2);
				return;
			}
			int num3 = 0;
			bool flag = num < num2;
			int num4 = (flag ? num : num2);
			while (num3 < num4)
			{
				float num5 = ReadSample();
				for (int i = 0; i < channels; i++)
				{
					data[num3] *= num5 * VolumeScale;
					num3++;
				}
				_collectedLoudness += global::UnityEngine.Mathf.Abs(num5);
			}
			if (flag)
			{
				global::System.Array.Clear(data, num, num2 - num);
			}
			_collectedSamples += num2;
		}

		protected virtual void OnDisable()
		{
			Source.Stop();
		}

		protected virtual void OnEnable()
		{
			Source.Play();
		}

		protected virtual void Awake()
		{
			Source = GetComponent<global::UnityEngine.AudioSource>();
			Source.clip = Flatline;
			Source.loop = true;
			Source.bypassReverbZones = true;
		}

		protected virtual void Update()
		{
			if (_collectedSamples >= 1200)
			{
				float num = _collectedLoudness * 5f;
				_targetLoudness = global::UnityEngine.Mathf.Sqrt(num / (float)_collectedSamples);
				_collectedLoudness = 0f;
				_collectedSamples = 0;
			}
			Loudness = global::UnityEngine.Mathf.Lerp(Loudness, _targetLoudness, global::UnityEngine.Time.deltaTime * 40f);
		}

		protected abstract float ReadSample();
	}
}
