using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.Cameras
{
	public abstract class CameraAxisBase
	{
		private float _val;

		private bool _wasEverSet;

		private bool _wasMoving;

		private const float LocalPlayerLerpMultiplier = 1f;

		private const float LocalPlayerPitchMultiplier = 1f;

		private const float LocalPlayerVolumeMultiplier = 0.4f;

		[SerializeField]
		private float _soundLerpSpeed;

		[SerializeField]
		private float _soundStopSpeed;

		[SerializeField]
		private float _localPlayerDiffLimiter;

		[SerializeField]
		private Vector2 _constraints;

		[SerializeField]
		protected AudioSource SoundEffectSource;

		[SerializeField]
		protected AnimationCurve SpeedCurve;

		[SerializeField]
		protected AnimationCurve VolumeCurve;

		[SerializeField]
		protected AnimationCurve PitchCurve;

		private bool IsFirstperson => false;

		private bool IsSpectating => false;

		protected virtual float SpectatorLerpMultiplier => 0f;

		public float CurValue { get; internal set; }

		public float MinValue => 0f;

		public float MaxValue => 0f;

		public float TargetValue
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public ushort Value16BitCompression
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public byte Value8BitCompression
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		internal virtual void Update(Scp079Camera cam)
		{
		}

		internal virtual void Awake(Scp079Camera cam)
		{
		}

		protected abstract void OnValueChanged(float newValue, Scp079Camera cam);

		private int Compress(Vector2 constraints, float val, int maxVal)
		{
			return 0;
		}

		private float Uncompress(Vector2 constraints, float val, int maxVal)
		{
			return 0f;
		}
	}
}
