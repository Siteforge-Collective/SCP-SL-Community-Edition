using System;
using InventorySystem.Items.Firearms.Attachments;
using UnityEngine;

namespace InventorySystem.Items.Firearms
{
	[Serializable]
	public struct FirearmBaseStats
	{
		public float BaseDamage;

		[Range(0f, 100f)]
		public int BasePenetrationPercent;

		public float FullDamageDistance;

		public float DamageFalloff;

		public float BulletInaccuracy;

		public float HipInaccuracy;

		public float AdsInaccuracy;

		public float BaseDrawTime;

		public float MaxDistance()
		{
			return FullDamageDistance + 100f / DamageFalloff;
		}

		public float DamageAtDistance(Firearm firearm, float dis)
		{
			if (dis >= MaxDistance())
			{
				return 0f;
			}
			float num = BaseDamage * firearm.AttachmentsValue(AttachmentParam.DamageMultiplier);
			if (dis > FullDamageDistance)
			{
				float num2 = 100f - DamageFalloff * (dis - FullDamageDistance);
				num *= num2 / 100f;
			}
			return num;
		}

		public float GetInaccuracy(Firearm firearm, bool isAds, float movementSpeed, bool isGrounded)
		{
			if (!isGrounded)
			{
				movementSpeed = firearm.GlobalSettingsPreset.MaxWeaponMovementSpeed;
			}
			float num = (isAds ? firearm.AttachmentsValue(AttachmentParam.AdsInaccuracyMultiplier) : firearm.AttachmentsValue(AttachmentParam.HipInaccuracyMultiplier));
			float num2 = (isAds ? (AdsInaccuracy * num) : (HipInaccuracy * num));
			num2 += num * firearm.GlobalSettingsPreset.MovementSpeedToRunningInaccuracy.Evaluate(movementSpeed) * firearm.GlobalSettingsPreset.RunningInaccuracyCurve.Evaluate(firearm.Length * firearm.Weight) * firearm.GlobalSettingsPreset.OverallRunningInaccuracyMultiplier;
			if (!isGrounded)
			{
				num2 += firearm.GlobalSettingsPreset.AbsoluteJumpInaccuracy;
			}
			return num2 + BulletInaccuracy * firearm.AttachmentsValue(AttachmentParam.BulletInaccuracyMultiplier);
		}
	}
}
