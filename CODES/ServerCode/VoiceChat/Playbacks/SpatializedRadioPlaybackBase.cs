namespace VoiceChat.Playbacks
{
	public class SpatializedRadioPlaybackBase : global::VoiceChat.Playbacks.VoiceChatPlaybackBase
	{
		private global::UnityEngine.Transform _t;

		private const int MaxAudibleRadios = 4;

		private static readonly global::VoiceChat.Playbacks.SpatializedRadioPlaybackBase[] AudibleRadioInst = new global::VoiceChat.Playbacks.SpatializedRadioPlaybackBase[4];

		private static readonly float[] AudibleRadiosDis = new float[4];

		public const int MaxSignals = 8;

		public global::VoiceChat.Networking.PlaybackBuffer[] Buffers;

		public int RangeId;

		public uint IgnoredNetId;

		public static readonly global::System.Collections.Generic.HashSet<global::VoiceChat.Playbacks.SpatializedRadioPlaybackBase> AllInstances = new global::System.Collections.Generic.HashSet<global::VoiceChat.Playbacks.SpatializedRadioPlaybackBase>();

		[field: global::UnityEngine.SerializeField]
		public global::UnityEngine.AudioSource NoiseSource { get; private set; }

		public global::UnityEngine.Vector3 LastPosition { get; private set; }

		public bool Culled { get; private set; }

		public override int MaxSamples
		{
			get
			{
				int num = 0;
				for (int i = 0; i < 8; i++)
				{
					num = global::UnityEngine.Mathf.Max(num, Buffers[i].Length);
				}
				return num;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			_t = base.transform;
			Buffers = new global::VoiceChat.Networking.PlaybackBuffer[8];
			for (int i = 0; i < 8; i++)
			{
				Buffers[i] = new global::VoiceChat.Networking.PlaybackBuffer();
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			AllInstances.Add(this);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			AllInstances.Remove(this);
		}

		protected override void Update()
		{
			base.Update();
			bool flag = false;
			for (int i = 0; i < 8; i++)
			{
				if (Buffers[i].Length != 0)
				{
					flag = true;
				}
			}
			NoiseSource.mute = !flag;
			LastPosition = _t.position;
		}

		protected override float ReadSample()
		{
			float num = 0f;
			for (int i = 0; i < 8; i++)
			{
				num += Buffers[i].Read();
			}
			return global::UnityEngine.Mathf.Clamp(num, -1f, 1f);
		}

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			StaticUnityMethods.OnLateUpdate += delegate
			{
				if (MainCameraController.InstanceActive)
				{
					int num = 0;
					global::UnityEngine.Vector3 position = MainCameraController.CurrentCamera.position;
					foreach (global::VoiceChat.Playbacks.SpatializedRadioPlaybackBase allInstance in AllInstances)
					{
						allInstance.Culled = true;
						float sqrMagnitude = (allInstance.LastPosition - position).sqrMagnitude;
						float maxDistance = allInstance.Source.maxDistance;
						if (!(sqrMagnitude > maxDistance * maxDistance))
						{
							if (num < 4)
							{
								AudibleRadiosDis[num] = sqrMagnitude;
								AudibleRadioInst[num] = allInstance;
								num++;
							}
							else
							{
								int num2 = -1;
								for (int i = 0; i < 4; i++)
								{
									float num3 = AudibleRadiosDis[i];
									if (!(num3 < sqrMagnitude))
									{
										num2 = i;
									}
								}
								if (num2 != -1)
								{
									AudibleRadiosDis[num2] = sqrMagnitude;
									AudibleRadioInst[num2] = allInstance;
								}
							}
						}
					}
					for (int j = 0; j < num; j++)
					{
						AudibleRadioInst[j].Culled = false;
					}
				}
			};
		}
	}
}
