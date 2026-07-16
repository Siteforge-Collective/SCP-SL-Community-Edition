namespace CustomPlayerEffects
{
	public abstract class StatusEffectBase : global::UnityEngine.MonoBehaviour, global::System.IEquatable<global::CustomPlayerEffects.StatusEffectBase>
	{
		public enum EffectClassification
		{
			Negative = 0,
			Mixed = 1,
			Positive = 2
		}

		private byte _intensity;

		private float _duration;

		private float _timeLeft;

		public byte Intensity
		{
			get
			{
				return _intensity;
			}
			set
			{
				if (_intensity == value || (!AllowEnabling && value > _intensity))
				{
					return;
				}
				byte intensity = _intensity;
				bool active = global::Mirror.NetworkServer.active;
				bool flag = intensity == 0 && value > 0;
				if (!(flag && active) || global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerReceiveEffect, Hub, this, value, Duration))
				{
					_intensity = (byte)global::UnityEngine.Mathf.Min(value, MaxIntensity);
					if (active)
					{
						Hub.playerEffectsController.ServerSyncEffect(this);
					}
					if (flag)
					{
						Enabled();
						global::CustomPlayerEffects.StatusEffectBase.OnEnabled?.Invoke(this);
					}
					else if (intensity > 0 && value == 0)
					{
						Disabled();
						global::CustomPlayerEffects.StatusEffectBase.OnDisabled?.Invoke(this);
					}
					IntensityChanged(intensity, value);
					global::CustomPlayerEffects.StatusEffectBase.OnIntensityChanged?.Invoke(this, intensity, value);
				}
			}
		}

		public virtual byte MaxIntensity { get; } = byte.MaxValue;

		public bool IsEnabled
		{
			get
			{
				return Intensity > 0;
			}
			set
			{
				if (value != IsEnabled)
				{
					Intensity = (byte)(value ? 1 : 0);
				}
			}
		}

		public virtual bool AllowEnabling => true;

		public virtual global::CustomPlayerEffects.StatusEffectBase.EffectClassification Classification => global::CustomPlayerEffects.StatusEffectBase.EffectClassification.Negative;

		public bool IsLocalPlayer => Hub.isLocalPlayer;

		public bool IsSpectated => global::PlayerRoles.Spectating.SpectatorNetworking.IsLocallySpectated(Hub);

		public float Duration
		{
			get
			{
				return _duration;
			}
			private set
			{
				_duration = global::UnityEngine.Mathf.Max(0f, value);
			}
		}

		public float TimeLeft
		{
			get
			{
				return _timeLeft;
			}
			set
			{
				_timeLeft = global::UnityEngine.Mathf.Max(0f, value);
				if (_timeLeft == 0f && Duration != 0f)
				{
					DisableEffect();
				}
			}
		}

		public ReferenceHub Hub { get; private set; }

		public static event global::System.Action<global::CustomPlayerEffects.StatusEffectBase> OnEnabled;

		public static event global::System.Action<global::CustomPlayerEffects.StatusEffectBase> OnDisabled;

		public static event global::System.Action<global::CustomPlayerEffects.StatusEffectBase, byte, byte> OnIntensityChanged;

		[global::Mirror.Server]
		public void ServerSetState(byte intensity, float duration = 0f, bool addDuration = false)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void CustomPlayerEffects.StatusEffectBase::ServerSetState(System.Byte,System.Single,System.Boolean)' called when server was not active");
				return;
			}
			Intensity = intensity;
			ServerChangeDuration(duration, addDuration);
		}

		[global::Mirror.Server]
		public void ServerDisable()
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void CustomPlayerEffects.StatusEffectBase::ServerDisable()' called when server was not active");
			}
			else
			{
				DisableEffect();
			}
		}

		[global::Mirror.Server]
		public void ServerChangeDuration(float duration, bool addDuration = false)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void CustomPlayerEffects.StatusEffectBase::ServerChangeDuration(System.Single,System.Boolean)' called when server was not active");
			}
			else if (addDuration && duration > 0f)
			{
				Duration += duration;
				TimeLeft += duration;
			}
			else
			{
				Duration = duration;
				TimeLeft = Duration;
			}
		}

		private void Awake()
		{
			Hub = ReferenceHub.GetHub(base.transform.root.gameObject);
			OnAwake();
		}

		protected virtual void Start()
		{
			_intensity = 1;
			DisableEffect();
		}

		protected virtual void Update()
		{
			if (IsEnabled)
			{
				RefreshTime();
				OnEffectUpdate();
			}
		}

		private void RefreshTime()
		{
			if (Duration != 0f)
			{
				TimeLeft -= global::UnityEngine.Time.deltaTime;
			}
		}

		protected virtual void Enabled()
		{
		}

		protected virtual void Disabled()
		{
		}

		protected virtual void OnAwake()
		{
		}

		protected virtual void OnEffectUpdate()
		{
		}

		protected virtual void IntensityChanged(byte prevState, byte newState)
		{
		}

		public virtual void OnBeginSpectating()
		{
		}

		public virtual void OnStopSpectating()
		{
		}

		internal virtual void OnRoleChanged(global::PlayerRoles.PlayerRoleBase previousRole, global::PlayerRoles.PlayerRoleBase newRole)
		{
			DisableEffect();
		}

		internal virtual void OnDeath(global::PlayerRoles.PlayerRoleBase previousRole)
		{
			DisableEffect();
		}

		protected virtual void DisableEffect()
		{
			if (global::Mirror.NetworkServer.active)
			{
				Intensity = 0;
			}
		}

		public bool Equals(global::CustomPlayerEffects.StatusEffectBase other)
		{
			if (other != null)
			{
				return other.gameObject == base.gameObject;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (this == obj)
			{
				return true;
			}
			if (obj.GetType() != GetType())
			{
				return false;
			}
			return Equals((global::CustomPlayerEffects.StatusEffectBase)obj);
		}

		public override int GetHashCode()
		{
			return base.gameObject.GetHashCode();
		}
	}
}
