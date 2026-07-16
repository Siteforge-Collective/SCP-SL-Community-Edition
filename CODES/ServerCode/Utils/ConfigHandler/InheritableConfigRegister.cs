namespace Utils.ConfigHandler
{
	public abstract class InheritableConfigRegister : global::Utils.ConfigHandler.ConfigRegister
	{
		public global::Utils.ConfigHandler.ConfigRegister ParentConfigRegister { get; protected set; }

		protected InheritableConfigRegister(global::Utils.ConfigHandler.ConfigRegister parentConfigRegister = null)
		{
			ParentConfigRegister = parentConfigRegister;
		}

		public abstract bool ShouldInheritConfigEntry(global::Utils.ConfigHandler.ConfigEntry configEntry);

		public abstract void UpdateConfigValueInheritable(global::Utils.ConfigHandler.ConfigEntry configEntry);

		public override void UpdateConfigValue(global::Utils.ConfigHandler.ConfigEntry configEntry)
		{
			if (configEntry != null && configEntry.Inherit && ParentConfigRegister != null && ShouldInheritConfigEntry(configEntry))
			{
				ParentConfigRegister.UpdateConfigValue(configEntry);
			}
			else
			{
				UpdateConfigValueInheritable(configEntry);
			}
		}

		public global::Utils.ConfigHandler.ConfigRegister[] GetConfigRegisterHierarchy(bool highestToLowest = true)
		{
			global::System.Collections.Generic.List<global::Utils.ConfigHandler.ConfigRegister> list = global::NorthwoodLib.Pools.ListPool<global::Utils.ConfigHandler.ConfigRegister>.Shared.Rent();
			global::Utils.ConfigHandler.ConfigRegister configRegister = this;
			while (configRegister != null && !list.Contains(configRegister))
			{
				list.Add(configRegister);
				if (!(configRegister is global::Utils.ConfigHandler.InheritableConfigRegister inheritableConfigRegister))
				{
					break;
				}
				configRegister = inheritableConfigRegister.ParentConfigRegister;
			}
			if (highestToLowest)
			{
				list.Reverse();
			}
			global::Utils.ConfigHandler.ConfigRegister[] result = list.ToArray();
			global::NorthwoodLib.Pools.ListPool<global::Utils.ConfigHandler.ConfigRegister>.Shared.Return(list);
			return result;
		}
	}
}
