namespace CustomPlayerEffects
{
	public class Concussed : global::CustomPlayerEffects.StatusEffectBase, global::CustomPlayerEffects.IHealablePlayerEffect
	{
		public override bool AllowEnabling => !global::CustomPlayerEffects.Vitality.CheckPlayer(base.Hub);

		public bool IsHealable(ItemType it)
		{
			if (it != ItemType.SCP500 && it != ItemType.Adrenaline)
			{
				return it == ItemType.Painkillers;
			}
			return true;
		}
	}
}
