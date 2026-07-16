namespace PlayerStatsSystem
{
	public static class DamageHandlers
	{
		public static readonly global::System.Func<global::PlayerStatsSystem.DamageHandlerBase>[] DefinedConstructors = new global::System.Func<global::PlayerStatsSystem.DamageHandlerBase>[14]
		{
			() => new global::PlayerStatsSystem.RecontainmentDamageHandler(default(global::Footprinting.Footprint)),
			() => new global::PlayerStatsSystem.FirearmDamageHandler(),
			() => new global::PlayerStatsSystem.WarheadDamageHandler(),
			() => new global::PlayerStatsSystem.UniversalDamageHandler(),
			() => new global::PlayerStatsSystem.ScpDamageHandler(),
			() => new global::PlayerStatsSystem.Scp096DamageHandler(),
			() => new global::PlayerStatsSystem.Scp049DamageHandler(),
			() => new global::PlayerStatsSystem.MicroHidDamageHandler(null, 0f),
			() => new global::PlayerStatsSystem.CustomReasonDamageHandler(string.Empty),
			() => new global::PlayerStatsSystem.ExplosionDamageHandler(default(global::Footprinting.Footprint), global::UnityEngine.Vector3.zero, 0f, 0),
			() => new global::PlayerStatsSystem.Scp018DamageHandler(null, 0f, ignoreFF: false),
			() => new global::PlayerStatsSystem.DisruptorDamageHandler(default(global::Footprinting.Footprint), 0f),
			() => new global::PlayerStatsSystem.JailbirdDamageHandler(),
			() => new global::PlayerRoles.PlayableScps.Scp939.Scp939DamageHandler(null)
		};

		public static readonly global::System.Collections.Generic.Dictionary<byte, global::System.Func<global::PlayerStatsSystem.DamageHandlerBase>> ConstructorsById = new global::System.Collections.Generic.Dictionary<byte, global::System.Func<global::PlayerStatsSystem.DamageHandlerBase>>();

		public static readonly global::System.Collections.Generic.Dictionary<int, byte> IdsByTypeHash = new global::System.Collections.Generic.Dictionary<int, byte>();

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void PrepDictionaries()
		{
			byte b = 0;
			global::System.Func<global::PlayerStatsSystem.DamageHandlerBase>[] definedConstructors = DefinedConstructors;
			foreach (global::System.Func<global::PlayerStatsSystem.DamageHandlerBase> func in definedConstructors)
			{
				IdsByTypeHash.Add(global::Mirror.Extensions.GetStableHashCode(func().GetType().FullName), b);
				ConstructorsById.Add(b, func);
				b++;
			}
		}
	}
}
