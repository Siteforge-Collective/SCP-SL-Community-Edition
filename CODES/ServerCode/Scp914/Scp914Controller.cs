namespace Scp914
{
	public class Scp914Controller : global::Mirror.NetworkBehaviour, global::Interactables.IServerInteractable, global::Interactables.IInteractable
	{
		[global::Mirror.SyncVar]
		[global::UnityEngine.SerializeField]
		private global::Scp914.Scp914KnobSetting _knobSetting;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioSource _knobSoundSource;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioSource _upgradingSoundSource;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Transform _knobTransform;

		[global::UnityEngine.SerializeField]
		private float _knobChangeCooldown;

		[global::UnityEngine.SerializeField]
		private float _totalSequenceTime;

		[global::UnityEngine.SerializeField]
		private float _doorCloseTime;

		[global::UnityEngine.SerializeField]
		private float _itemUpgradeTime;

		[global::UnityEngine.SerializeField]
		private float _doorOpenTime;

		[global::UnityEngine.SerializeField]
		private global::Interactables.Interobjects.DoorUtils.DoorVariant[] _doors;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Vector3 _chamberSize;

		private float _remainingCooldown;

		private bool _isUpgrading;

		private bool _itemsAlreadyUpgraded;

		private global::Utils.ConfigHandler.ConfigEntry<global::Scp914.Scp914Mode> _configMode;

		public static global::Scp914.Scp914Controller Singleton { get; private set; }

		[field: global::UnityEngine.SerializeField]
		public global::UnityEngine.Transform IntakeChamber { get; private set; }

		[field: global::UnityEngine.SerializeField]
		public global::UnityEngine.Transform OutputChamber { get; private set; }

		private global::UnityEngine.Vector3 IntakeChamberSize
		{
			get
			{
				global::UnityEngine.Vector3 vector = IntakeChamber.rotation * _chamberSize / 2f;
				return new global::UnityEngine.Vector3(global::UnityEngine.Mathf.Abs(vector.z), global::UnityEngine.Mathf.Abs(vector.y), global::UnityEngine.Mathf.Abs(vector.x));
			}
		}

		public global::Interactables.Verification.IVerificationRule VerificationRule => global::Interactables.Verification.StandardDistanceVerification.Default;

		public global::Scp914.Scp914KnobSetting KnobSetting => _knobSetting;

		public global::Scp914.Scp914KnobSetting Network_knobSetting
		{
			get
			{
				return _knobSetting;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref _knobSetting))
				{
					global::Scp914.Scp914KnobSetting knobSetting = _knobSetting;
					SetSyncVar(value, ref _knobSetting, 1uL);
				}
			}
		}

		public void ServerInteract(ReferenceHub ply, byte colliderId)
		{
			if (_remainingCooldown > 0f)
			{
				return;
			}
			switch ((global::Scp914.Scp914InteractCode)colliderId)
			{
			case global::Scp914.Scp914InteractCode.ChangeMode:
			{
				global::Scp914.Scp914KnobSetting scp914KnobSetting = _knobSetting + 1;
				if (!global::System.Enum.IsDefined(typeof(global::Scp914.Scp914KnobSetting), scp914KnobSetting))
				{
					scp914KnobSetting = global::Scp914.Scp914KnobSetting.Rough;
				}
				if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp914KnobChange, ply, scp914KnobSetting, _knobSetting))
				{
					_remainingCooldown = _knobChangeCooldown;
					Network_knobSetting = scp914KnobSetting;
					RpcPlaySound(0);
				}
				break;
			}
			case global::Scp914.Scp914InteractCode.Activate:
				if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp914Activate, ply, _knobSetting))
				{
					_remainingCooldown = _totalSequenceTime;
					_isUpgrading = true;
					_itemsAlreadyUpgraded = false;
					RpcPlaySound(1);
				}
				break;
			}
		}

		private void Start()
		{
			Singleton = this;
			if (global::Scp914.Scp914Upgrader.SolidObjectMask == 0)
			{
				global::Scp914.Scp914Upgrader.SolidObjectMask = global::UnityEngine.LayerMask.GetMask("Default", "Door");
			}
			if (global::Mirror.NetworkServer.active)
			{
				_configMode = new global::Utils.ConfigHandler.ConfigEntry<global::Scp914.Scp914Mode>("914_mode", global::Scp914.Scp914Mode.DroppedAndPlayerTeleport, "SCP-914 Mode", "The behavior SCP-914 should use when upgrading items.");
				global::GameCore.ConfigFile.ServerConfig.RegisterConfig(_configMode);
			}
		}

		private void OnDestroy()
		{
			if (global::Mirror.NetworkServer.active)
			{
				global::GameCore.ConfigFile.ServerConfig.UnRegisterConfig(_configMode);
			}
		}

		private void Update()
		{
			if (global::Mirror.NetworkServer.active)
			{
				UpdateServerside();
			}
		}

		[global::Mirror.Server]
		private void UpdateServerside()
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void Scp914.Scp914Controller::UpdateServerside()' called when server was not active");
				return;
			}
			if (_isUpgrading)
			{
				float num = _totalSequenceTime - _remainingCooldown;
				if (num >= _doorCloseTime && num < _doorOpenTime && _doors[0].TargetState)
				{
					global::Interactables.Interobjects.DoorUtils.DoorVariant[] doors = _doors;
					for (int i = 0; i < doors.Length; i++)
					{
						doors[i].NetworkTargetState = false;
					}
				}
				else if (num >= _itemUpgradeTime && !_itemsAlreadyUpgraded)
				{
					global::Scp914.Scp914Upgrader.Upgrade(global::UnityEngine.Physics.OverlapBox(IntakeChamber.position, IntakeChamberSize), OutputChamber.position - IntakeChamber.position, _configMode.Value, _knobSetting);
					_itemsAlreadyUpgraded = true;
				}
				else if (num >= _doorOpenTime && !_doors[0].TargetState)
				{
					global::Interactables.Interobjects.DoorUtils.DoorVariant[] doors = _doors;
					for (int i = 0; i < doors.Length; i++)
					{
						doors[i].NetworkTargetState = true;
					}
				}
			}
			if (_remainingCooldown >= 0f)
			{
				_remainingCooldown -= global::UnityEngine.Time.deltaTime;
				if (_remainingCooldown < 0f)
				{
					_isUpgrading = false;
				}
			}
		}

		private void OnDrawGizmosSelected()
		{
			global::UnityEngine.Gizmos.color = global::UnityEngine.Color.green;
			global::UnityEngine.Gizmos.DrawCube(IntakeChamber.position, IntakeChamberSize * 2f);
			global::UnityEngine.Gizmos.DrawCube(OutputChamber.position, IntakeChamberSize * 2f);
		}

		[global::Mirror.ClientRpc]
		private void RpcPlaySound(byte soundId)
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			global::Mirror.NetworkWriterExtensions.WriteByte(writer, soundId);
			SendRPCInternal(typeof(global::Scp914.Scp914Controller), "RpcPlaySound", writer, 0, includeOwner: true);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		private void MirrorProcessed()
		{
		}

		private void UserCode_RpcPlaySound(byte soundId)
		{
			switch ((global::Scp914.Scp914Sound)soundId)
			{
			case global::Scp914.Scp914Sound.KnobChange:
				_knobSoundSource.Stop();
				_knobSoundSource.Play();
				break;
			case global::Scp914.Scp914Sound.Upgrading:
				_upgradingSoundSource.Stop();
				_upgradingSoundSource.Play();
				break;
			}
		}

		protected static void InvokeUserCode_RpcPlaySound(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkClient.active)
			{
				global::UnityEngine.Debug.LogError("RPC RpcPlaySound called on server.");
			}
			else
			{
				((global::Scp914.Scp914Controller)obj).UserCode_RpcPlaySound(global::Mirror.NetworkReaderExtensions.ReadByte(reader));
			}
		}

		static Scp914Controller()
		{
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(global::Scp914.Scp914Controller), "RpcPlaySound", InvokeUserCode_RpcPlaySound);
		}

		public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
		{
			bool result = base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				global::Mirror.GeneratedNetworkCode._Write_Scp914_002EScp914KnobSetting(writer, _knobSetting);
				return true;
			}
			global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 1L) != 0L)
			{
				global::Mirror.GeneratedNetworkCode._Write_Scp914_002EScp914KnobSetting(writer, _knobSetting);
				result = true;
			}
			return result;
		}

		public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				global::Scp914.Scp914KnobSetting knobSetting = _knobSetting;
				Network_knobSetting = global::Mirror.GeneratedNetworkCode._Read_Scp914_002EScp914KnobSetting(reader);
				return;
			}
			long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
			if ((num & 1L) != 0L)
			{
				global::Scp914.Scp914KnobSetting knobSetting2 = _knobSetting;
				Network_knobSetting = global::Mirror.GeneratedNetworkCode._Read_Scp914_002EScp914KnobSetting(reader);
			}
		}
	}
}
