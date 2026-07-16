using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp939.Ripples
{
	public class SpawnableRipple : MonoBehaviour
	{
		[field: SerializeField]
		public float Range { get; private set; }

		public static event Action<SpawnableRipple> OnSpawned
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		private void OnEnabled()
		{
		}
	}
}
