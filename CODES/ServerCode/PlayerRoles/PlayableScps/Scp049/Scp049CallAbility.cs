namespace PlayerRoles.PlayableScps.Scp049
{
	public class Scp049CallAbility : global::PlayerRoles.PlayableScps.Subroutines.ScpKeySubroutine<global::PlayerRoles.PlayableScps.Scp049.Scp049Role>
	{
		private const float BaseCooldown = 60f;

		private const float EffectDuration = 20f;

		public readonly global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown Cooldown = new global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown();

		public readonly global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown Duration = new global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown();

		private bool _serverTriggered;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp049.Scp049AudioPlayer _audio;

		public bool IsMarkerShown
		{
			get
			{
				if (!Duration.IsReady)
				{
					if (global::Mirror.NetworkServer.active)
					{
						return _serverTriggered;
					}
					return true;
				}
				return false;
			}
		}

		protected override ActionName TargetKey => ActionName.Reload;

		private void ServerRefreshDuration()
		{
			if (_serverTriggered && Duration.IsReady)
			{
				Cooldown.Trigger(60f);
				_serverTriggered = false;
				ServerSendRpc(toAll: true);
			}
		}

		public override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			if (!_serverTriggered && Cooldown.IsReady)
			{
				Duration.Trigger(20f);
				_serverTriggered = true;
				ServerSendRpc(toAll: true);
			}
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			Cooldown.WriteCooldown(writer);
			Duration.WriteCooldown(writer);
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			Cooldown.ReadCooldown(reader);
			Duration.ReadCooldown(reader);
		}

		protected override void Update()
		{
			base.Update();
			if (global::Mirror.NetworkServer.active)
			{
				ServerRefreshDuration();
			}
		}

		protected override void OnKeyDown()
		{
			base.OnKeyDown();
			ClientSendCmd();
		}

		public override void ResetObject()
		{
			base.ResetObject();
			Cooldown.Clear();
			Duration.Clear();
			_serverTriggered = false;
		}
	}
}
