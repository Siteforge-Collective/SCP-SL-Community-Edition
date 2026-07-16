namespace CustomPlayerEffects
{
	public class Invisible : global::CustomPlayerEffects.StatusEffectBase, global::CustomPlayerEffects.ISpectatorDataPlayerEffect, global::RemoteAdmin.Interfaces.ICustomRADisplay
	{
		public override global::CustomPlayerEffects.StatusEffectBase.EffectClassification Classification => global::CustomPlayerEffects.StatusEffectBase.EffectClassification.Positive;

		public string DisplayName => "Invisibility";

		public bool CanBeDisplayed => true;

		public bool GetSpectatorText(out string s)
		{
			s = "SCP-268";
			return true;
		}
	}
}
