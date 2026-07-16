namespace PlayerStatsSystem
{
	public class HumeShieldStat : global::PlayerStatsSystem.SyncedStatBase
	{
		public override byte SyncId => 5;

		public override global::PlayerStatsSystem.SyncedStatBase.SyncMode Mode => global::PlayerStatsSystem.SyncedStatBase.SyncMode.PrivateAndSpectators;

		public override float MinValue => 0f;

		public override float MaxValue
		{
			get
			{
				if (!TryGetHsModule(out var controller))
				{
					return 0f;
				}
				return controller.HsMax;
			}
		}

		public override float CurValue
		{
			get
			{
				return base.CurValue;
			}
			set
			{
				base.CurValue = global::UnityEngine.Mathf.Max(0f, value);
			}
		}

		public override bool CheckDirty(float prevValue, float newValue)
		{
			return global::UnityEngine.Mathf.CeilToInt(prevValue) != global::UnityEngine.Mathf.CeilToInt(newValue);
		}

		public override float ReadValue(global::Mirror.NetworkReader reader)
		{
			return (int)global::Mirror.NetworkReaderExtensions.ReadUInt16(reader);
		}

		public override void WriteValue(global::Mirror.NetworkWriter writer)
		{
			int num = global::UnityEngine.Mathf.Clamp(global::UnityEngine.Mathf.CeilToInt(CurValue), 0, 65535);
			global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, (ushort)num);
		}

		internal override void Update()
		{
			base.Update();
			if (!global::Mirror.NetworkServer.active || !TryGetHsModule(out var controller) || controller.HsRegeneration == 0f)
			{
				return;
			}
			float hsCurrent = controller.HsCurrent;
			float num = controller.HsRegeneration * global::UnityEngine.Time.deltaTime;
			if (num > 0f)
			{
				if (!(hsCurrent >= controller.HsMax))
				{
					CurValue = global::UnityEngine.Mathf.MoveTowards(hsCurrent, controller.HsMax, num);
				}
			}
			else if (!(hsCurrent <= 0f))
			{
				CurValue = hsCurrent + num;
			}
		}

		internal override void ClassChanged()
		{
			base.ClassChanged();
			if (!(base.Hub.roleManager.CurrentRole is global::PlayerRoles.PlayableScps.HumeShield.IHumeShieldedRole))
			{
				CurValue = 0f;
			}
		}

		protected override void OnValueChanged(float prevValue, float newValue)
		{
			if (TryGetHsModule(out var controller))
			{
				controller.OnHsValueChanged(prevValue, newValue);
			}
		}

		private bool TryGetHsModule(out global::PlayerRoles.PlayableScps.HumeShield.HumeShieldModuleBase controller)
		{
			if (base.Hub.roleManager.CurrentRole is global::PlayerRoles.PlayableScps.HumeShield.IHumeShieldedRole humeShieldedRole)
			{
				controller = humeShieldedRole.HumeShieldModule;
				return true;
			}
			controller = null;
			return false;
		}
	}
}
