using UnityEngine;

namespace InventorySystem.Items.Usables
{
	public class Scp500 : Consumable
	{
		[SerializeField]
		private AnimationCurve _healProgress;

		private const int InstantHealth = 100;

		private const float TotalRegenerationTime = 10f;

		private const int TotalHpToRegenerate = 100;

		private const float AchievementMaxHp = 20f;

		protected override void OnEffectsActivated()
		{
		}
	}
}
