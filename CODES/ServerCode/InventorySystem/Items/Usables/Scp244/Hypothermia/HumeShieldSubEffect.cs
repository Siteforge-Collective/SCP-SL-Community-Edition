namespace InventorySystem.Items.Usables.Scp244.Hypothermia
{
	public class HumeShieldSubEffect : global::InventorySystem.Items.Usables.Scp244.Hypothermia.HypothermiaSubEffectBase, global::PlayerRoles.PlayableScps.HumeShield.IHumeShieldBlocker
	{
		[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Size = 1)]
		public struct HumeBlockMsg : global::Mirror.NetworkMessage
		{
		}

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip[] _freezeSounds;

		[global::UnityEngine.SerializeField]
		private float _hsSustainTime;

		[global::UnityEngine.SerializeField]
		private float _hsDecreaseStartTime;

		[global::UnityEngine.SerializeField]
		private float _hsDecreaseAbsolute;

		[global::UnityEngine.SerializeField]
		private float _hsDecreasePerExposure;

		private float _decreaseTimer;

		private static global::InventorySystem.Items.Usables.Scp244.Hypothermia.HumeShieldSubEffect _localEffect;

		private readonly global::System.Diagnostics.Stopwatch _cooldownSw = global::System.Diagnostics.Stopwatch.StartNew();

		private readonly global::System.Diagnostics.Stopwatch _sustainSw = new global::System.Diagnostics.Stopwatch();

		public override bool IsActive
		{
			get
			{
				if (!HumeShieldBlocked)
				{
					if (_sustainSw.IsRunning)
					{
						return _sustainSw.Elapsed.TotalSeconds < (double)_hsSustainTime;
					}
					return false;
				}
				return true;
			}
		}

		public bool HumeShieldBlocked { get; private set; }

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Register()
		{
			CustomNetworkManager.OnClientReady += delegate
			{
				global::Mirror.NetworkClient.ReplaceHandler<global::InventorySystem.Items.Usables.Scp244.Hypothermia.HumeShieldSubEffect.HumeBlockMsg>(OnMessageReceived);
			};
		}

		private static void OnMessageReceived(global::InventorySystem.Items.Usables.Scp244.Hypothermia.HumeShieldSubEffect.HumeBlockMsg msg)
		{
			if (!(_localEffect == null))
			{
				_localEffect.ReceiveHumeBlockMessage();
			}
		}

		internal override void Init(global::CustomPlayerEffects.StatusEffectBase mainEffect)
		{
			base.Init(mainEffect);
			if (mainEffect.IsLocalPlayer)
			{
				_localEffect = this;
			}
		}

		internal override void UpdateEffect(float curExposure)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				if (HumeShieldBlocked && curExposure <= 0f)
				{
					HumeShieldBlocked = false;
				}
				return;
			}
			bool humeShieldBlocked = HumeShieldBlocked;
			HumeShieldBlocked = UpdateHumeShield(curExposure);
			if (HumeShieldBlocked)
			{
				if (!humeShieldBlocked)
				{
					base.Hub.networkIdentity.connectionToClient.Send(default(global::InventorySystem.Items.Usables.Scp244.Hypothermia.HumeShieldSubEffect.HumeBlockMsg));
				}
			}
			else
			{
				_decreaseTimer = 0f;
			}
		}

		private bool UpdateHumeShield(float expo)
		{
			if (expo == 0f || !TryGetController(out var ctrl) || base.Hub.characterClassManager.GodMode)
			{
				return false;
			}
			ctrl.AddBlocker(this);
			_sustainSw.Restart();
			_decreaseTimer += expo * global::UnityEngine.Time.deltaTime;
			if (_decreaseTimer < _hsDecreaseStartTime)
			{
				return true;
			}
			ctrl.HsCurrent -= (expo * _hsDecreasePerExposure + _hsDecreaseAbsolute) * global::UnityEngine.Time.deltaTime;
			return true;
		}

		private void ReceiveHumeBlockMessage()
		{
		}

		private bool TryGetController(out global::PlayerRoles.PlayableScps.HumeShield.DynamicHumeShieldController ctrl)
		{
			if (!(base.Hub.roleManager.CurrentRole is global::PlayerRoles.PlayableScps.HumeShield.IHumeShieldedRole humeShieldedRole) || !(humeShieldedRole.HumeShieldModule is global::PlayerRoles.PlayableScps.HumeShield.DynamicHumeShieldController dynamicHumeShieldController))
			{
				ctrl = null;
				return false;
			}
			ctrl = dynamicHumeShieldController;
			return true;
		}
	}
}
