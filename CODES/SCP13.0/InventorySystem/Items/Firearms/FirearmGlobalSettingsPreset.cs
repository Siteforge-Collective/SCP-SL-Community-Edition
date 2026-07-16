using UnityEngine;

namespace InventorySystem.Items.Firearms
{
	[CreateAssetMenu(fileName = "New Global Settings Preset", menuName = "ScriptableObject/Firearms/Global Settings Preset")]
	public class FirearmGlobalSettingsPreset : ScriptableObject
	{
		public float OverallRunningInaccuracyMultiplier;

		public float AbsoluteJumpInaccuracy;

		public AnimationCurve AdsMovementSpeedCurve;

		public float MaxWeaponMovementSpeed;

		public AnimationCurve AdsAnimationCurve;

		public AnimationCurve MovementSpeedToRunningInaccuracy;

		public AnimationCurve RunningInaccuracyCurve;

		public AnimationCurve WeightToStaminaUsage;

		public AnimationCurve WeightToMovementSpeed;
	}
}
