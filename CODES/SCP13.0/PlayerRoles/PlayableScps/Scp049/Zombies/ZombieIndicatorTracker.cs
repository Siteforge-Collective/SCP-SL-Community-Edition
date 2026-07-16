using System.Collections.Generic;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp049.Zombies
{
	public class ZombieIndicatorTracker : StandardSubroutine<ZombieRole>
	{
		[SerializeField]
		private GameObject _effectPrefab;

		private bool _hasInstances;

		private readonly Dictionary<ReferenceHub, GameObject> _instances;

		private static int _cachedCallId;

		public override void ResetObject()
		{
		}

		protected override void Awake()
		{
		}

		private void Update()
		{
		}

		private Scp049CallAbility GetCallFast(Scp049Role scp049)
		{
			return null;
		}

		private void ValidateAll()
		{
		}

		private bool ValidatePlayer(ReferenceHub hub)
		{
			return false;
		}

		private void ShowIndicator(ReferenceHub target)
		{
		}

		private void DestroyIndicator(ReferenceHub target)
		{
		}

		private void DestroyAll()
		{
		}
	}
}
