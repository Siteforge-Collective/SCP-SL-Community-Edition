namespace InventorySystem.Items.Pickups
{
	public class CollisionDetectionPickup : global::InventorySystem.Items.Pickups.ItemPickupBase
	{
		[global::System.Serializable]
		private struct SoundOverVelocity
		{
			[global::UnityEngine.SerializeField]
			private float _minimalVelocity;

			[global::UnityEngine.SerializeField]
			private global::UnityEngine.AudioClip[] _randomClips;

			[global::UnityEngine.Range(0f, 0.5f)]
			[global::UnityEngine.SerializeField]
			private float _randomizePitch;

			[global::UnityEngine.SerializeField]
			private float _maxRange;

			public bool TryGetSound(float vel, global::UnityEngine.AudioSource src)
			{
				if (vel < _minimalVelocity)
				{
					return false;
				}
				if (global::UnityEngine.Time.timeSinceLevelLoad > 15f && src != null)
				{
					src.PlayOneShot(_randomClips[global::UnityEngine.Random.Range(0, _randomClips.Length)]);
					src.maxDistance = _maxRange;
					src.pitch = global::UnityEngine.Random.Range(1f - _randomizePitch, 1f / (1f - _randomizePitch));
				}
				return true;
			}
		}

		private const float MinimalTimeFromLoad = 15f;

		private const float MinimalJoulesToInduceDamage = 15f;

		private const float DamagePerJoule = 0.4f;

		private const float MinimalSoundCooldown = 0.1f;

		[global::UnityEngine.SerializeField]
		private global::InventorySystem.Items.Pickups.CollisionDetectionPickup.SoundOverVelocity[] _soundsOverVelocity;

		public event global::System.Action<global::UnityEngine.Collision> OnCollided;

		private void OnCollisionEnter(global::UnityEngine.Collision collision)
		{
			ProcessCollision(collision);
		}

		public float GetRangeOfCollisionVelocity(float sqrVel)
		{
			global::UnityEngine.AudioSource free = global::AudioPooling.AudioSourcePoolManager.GetFree(reserved: false);
			for (int num = _soundsOverVelocity.Length - 1; num >= 0; num--)
			{
				if (_soundsOverVelocity[num].TryGetSound(sqrVel, free))
				{
					free.Stop();
					return free.maxDistance;
				}
			}
			return 0f;
		}

		protected virtual void ProcessCollision(global::UnityEngine.Collision collision)
		{
			this.OnCollided?.Invoke(collision);
			float sqrMagnitude = collision.relativeVelocity.sqrMagnitude;
			if (global::Mirror.NetworkServer.active)
			{
				float num = Info.Weight * sqrMagnitude / 2f;
				if (num > 15f)
				{
					float damage = num * 0.4f;
					if (collision.collider.TryGetComponent<BreakableWindow>(out var component))
					{
						component.Damage(damage, null, global::UnityEngine.Vector3.zero);
					}
				}
			}
			MakeCollisionSound(sqrMagnitude);
		}

		protected void MakeCollisionSound(float sqrtVelocity)
		{
			if (_stopwatch.Elapsed.TotalSeconds < (double)MinSoundCooldown)
    return;

if (_audioSrc == null || _soundsOverVelocity == null)
    return;

for (int i = _soundsOverVelocity.Length - 1; i >= 0; i--)
{
    if (sqrVelocity >= _soundsOverVelocity[i].MinimalVelocity)
    {
        if (_soundsOverVelocity[i].TryPlaySound(sqrVelocity, _audioSrc))
        {
            _stopwatch.Restart();
            break;
        }
    }
}
		}

		private void MirrorProcessed()
		{
		}
	}
}
