using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem.Items.Usables.Scp244.Hypothermia
{
	public class AttackCooldownSubEffect : HypothermiaSubEffectBase
	{
		private float _prevExpo;

		private static readonly Dictionary<uint, float> MultipliersOfPlayers;

		[SerializeField]
		private float _cooldownMultiplierPerExposure;

		public override bool IsActive => false;

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		public static float CurrentAttackCooldownMultiplier(ReferenceHub hub)
		{
			return 0f;
		}

		internal override void UpdateEffect(float curExposure)
		{
		}
	}
}
