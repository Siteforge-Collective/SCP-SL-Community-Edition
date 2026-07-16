using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp173
{
	public class Scp173TeleportIndicator : MonoBehaviour
	{
		[SerializeField]
		private float _volumeAdjustmentSpeed;

		[SerializeField]
		private AudioSource _soundSource;

		[SerializeField]
		private GameObject _normalIndicator;

		[SerializeField]
		private GameObject _killIndicator;

		[SerializeField]
		private GameObject _neutralIndicator;

		[SerializeField]
		private SubroutineManagerModule _subroutineManager;

		private float _targetVolume;

		private Scp173TeleportAbility _teleportAbility;

		private Scp173BreakneckSpeedsAbility _breakneckSpeedsAbility;

		private void Awake()
		{
		}

		private void Update()
		{
		}

		private void SetupVisiblity(bool normal = false, bool kill = false, bool neutral = false)
		{
		}

		public void UpdateVisibility(bool isVisible)
		{
		}
	}
}
