using System.Collections.Generic;
using CustomPlayerEffects;
using GameObjectPools;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp939.Ripples
{
	public class RipplePlayer : SubroutineBase, IPoolSpawnable
	{
		[SerializeField]
		private RippleInstance _rippleTemplate;

		private Scp939FocusAbility _focus;

		private Deafened _deafened;

		private int _poolCount;

		private readonly Queue<RippleInstance> _pool;

		private bool CanHear => false;

		public void Play(Vector3 position, Color color)
		{
		}

		public void Play(HumanRole human)
		{
		}

		public void SpawnObject()
		{
		}

		protected override void Awake()
		{
		}
	}
}
