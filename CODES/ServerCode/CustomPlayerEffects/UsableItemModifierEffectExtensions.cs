namespace CustomPlayerEffects
{
	public static class UsableItemModifierEffectExtensions
	{
		public static float GetSpeedMultiplier(this ItemType type, ReferenceHub player)
		{
			type.TryGetSpeedMultiplier(player, out var multiplier);
			return multiplier;
		}

		public static bool TryGetSpeedMultiplier(this ItemType type, ReferenceHub player, out float multiplier)
		{
			PlayerEffectsController playerEffectsController = player.playerEffectsController;
			bool result = false;
			multiplier = 1f;
			for (int i = 0; i < playerEffectsController.EffectsLength; i++)
			{
				global::CustomPlayerEffects.StatusEffectBase statusEffectBase = playerEffectsController.AllEffects[i];
				if (statusEffectBase.IsEnabled && statusEffectBase is global::CustomPlayerEffects.IUsableItemModifierEffect usableItemModifierEffect && usableItemModifierEffect.TryGetSpeed(type, out var speed))
				{
					multiplier *= speed;
					result = true;
				}
			}
			return result;
		}
	}
}
