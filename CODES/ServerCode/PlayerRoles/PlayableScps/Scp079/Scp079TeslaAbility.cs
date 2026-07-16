namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079TeslaAbility : global::PlayerRoles.PlayableScps.Scp079.Scp079KeyAbilityBase
	{
		[global::UnityEngine.SerializeField]
		private int _cost;

		[global::UnityEngine.SerializeField]
		private float _cooldown;

		private string _abilityName;

		private string _cooldownMessage;

		private double _nextUseTime;

		public override bool IsVisible
		{
			get
			{
				if (!global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079CursorManager.LockCameras && global::PlayerRoles.PlayableScps.Scp079.Overcons.OverconManager.Singleton.HighlightedOvercon is global::PlayerRoles.PlayableScps.Scp079.Overcons.TeslaOvercon teslaOvercon)
				{
					return teslaOvercon != null;
				}
				return false;
			}
		}

		public override bool IsReady
		{
			get
			{
				if (base.AuxManager.CurrentAux >= (float)_cost)
				{
					return _nextUseTime < global::Mirror.NetworkTime.time;
				}
				return false;
			}
		}

		public override string FailMessage
		{
			get
			{
				if (base.AuxManager.CurrentAux < (float)_cost)
				{
					return GetNoAuxMessage(_cost);
				}
				int num = global::UnityEngine.Mathf.CeilToInt((float)(_nextUseTime - global::Mirror.NetworkTime.time));
				if (num > 0)
				{
					return _cooldownMessage + "\n" + base.AuxManager.GenerateCustomETA(num);
				}
				return null;
			}
		}

		public override ActionName ActivationKey => ActionName.Shoot;

		public override string AbilityName => string.Format(_abilityName, _cost);

		protected override void Start()
		{
			base.Start();
			_abilityName = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.FireTeslaGate);
			_cooldownMessage = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.TeslaGateCooldown);
		}

		protected override void Trigger()
		{
			ClientSendCmd();
		}

		public override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			base.ServerProcessCmd(reader);
			if (IsReady)
			{
				global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera cam = base.CurrentCamSync.CurrentCamera;
				if (global::Utils.NonAllocLINQ.ListExtensions.TryGetFirst(TeslaGateController.Singleton.TeslaGates, (TeslaGate x) => global::MapGeneration.RoomIdUtils.IsTheSameRoom(cam.Position, x.transform.position), out var first) && global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp079UseTesla, base.Owner, first))
				{
					base.RewardManager.MarkRoom(cam.Room);
					base.AuxManager.CurrentAux -= _cost;
					first.RpcInstantBurst();
					_nextUseTime = global::Mirror.NetworkTime.time + (double)_cooldown;
					ServerSendRpc(toAll: false);
				}
			}
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			global::Mirror.NetworkWriterExtensions.WriteDouble(writer, _nextUseTime);
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			_nextUseTime = global::Mirror.NetworkReaderExtensions.ReadDouble(reader);
		}
	}
}
