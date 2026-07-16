namespace AdminToys
{
	public class ShootingTarget : global::AdminToys.AdminToyBase, IDestructible, global::Interactables.IClientInteractable, global::Interactables.IInteractable, global::Interactables.IServerInteractable
	{
		private enum TargetButton
		{
			IncreaseHP = 0,
			DecreaseHP = 1,
			IncreaseResetTime = 2,
			DecreaseResetTime = 3,
			ManualReset = 4,
			Remove = 5,
			GlobalResults = 6
		}

		private float _hp = 10f;

		private int _maxHp = 10;

		private int _autoDestroyTime;

		private float _avg;

		private global::UnityEngine.GameObject _prevHit;

		[global::Mirror.SyncVar]
		private bool _syncMode;

		[global::UnityEngine.SerializeField]
		private float _stepSize = 0.12f;

		[global::UnityEngine.SerializeField]
		private string _targetName;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioSource _source;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _hitSound;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _killSound;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip[] _score;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _hitIndicator;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Transform _bullsEye;

		[global::UnityEngine.SerializeField]
		private float _bullsEyeRadius;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Vector3[] _bullsEyeBounds;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Material _prevHitMat;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.UI.Text _lastHitInfo;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.UI.Text _syncText;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.UI.Text _settingsWindow;

		private readonly global::System.Collections.Generic.List<global::UnityEngine.GameObject> _hits = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>();

		public uint NetworkId => base.netId;

		public global::Interactables.Verification.IVerificationRule VerificationRule => global::Interactables.Verification.StandardDistanceVerification.Default;

		public global::UnityEngine.Vector3 CenterOfMass => _bullsEye.position;

		public override string CommandName => "Target" + _targetName;

		public bool Network_syncMode
		{
			get
			{
				return _syncMode;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref _syncMode))
				{
					bool flag = _syncMode;
					SetSyncVar(value, ref _syncMode, 16uL);
				}
			}
		}

		public override void OnSpawned(ReferenceHub admin, global::System.ArraySegment<string> arguments)
		{
			if (global::UnityEngine.Physics.Raycast(admin.transform.position - admin.transform.forward, global::UnityEngine.Vector3.down, out var hitInfo, 2f))
			{
				base.transform.position = hitInfo.point;
				base.transform.rotation = global::UnityEngine.Quaternion.Euler(global::UnityEngine.Vector3.up * (global::UnityEngine.Mathf.Round((admin.transform.rotation.eulerAngles.y + 90f) / 10f) * 10f));
			}
			base.OnSpawned(admin, arguments);
		}

		public bool Damage(float damage, global::PlayerStatsSystem.DamageHandlerBase handler, global::UnityEngine.Vector3 exactHit)
		{
			if (!(handler is global::PlayerStatsSystem.AttackerDamageHandler attackerDamageHandler))
			{
				return false;
			}
			ReferenceHub hub = attackerDamageHandler.Attacker.Hub;
			if (hub == null)
			{
				return false;
			}
			if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerDamagedShootingTarget, hub, this, handler, damage))
			{
				return false;
			}
			float distance = global::UnityEngine.Vector3.Distance(hub.transform.position, _bullsEye.position);
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				if (_syncMode || allHub == hub)
				{
					TargetRpcReceiveData(allHub.characterClassManager.connectionToClient, damage, distance, exactHit, handler);
				}
			}
			return true;
		}

		public void ClientInteract(global::Interactables.InteractableCollider collider)
		{
			UseButton((global::AdminToys.ShootingTarget.TargetButton)collider.ColliderId);
		}

		private void UseButton(global::AdminToys.ShootingTarget.TargetButton tb)
		{
			switch (tb)
			{
			case global::AdminToys.ShootingTarget.TargetButton.ManualReset:
				ClearTarget();
				break;
			case global::AdminToys.ShootingTarget.TargetButton.IncreaseHP:
				_maxHp = global::UnityEngine.Mathf.Clamp(_maxHp * 2, 1, 256);
				break;
			case global::AdminToys.ShootingTarget.TargetButton.DecreaseHP:
				_maxHp /= 2;
				break;
			case global::AdminToys.ShootingTarget.TargetButton.IncreaseResetTime:
				_autoDestroyTime = global::UnityEngine.Mathf.Min(_autoDestroyTime + 1, 10);
				break;
			case global::AdminToys.ShootingTarget.TargetButton.DecreaseResetTime:
				_autoDestroyTime = global::UnityEngine.Mathf.Max(_autoDestroyTime - 1, 0);
				break;
			}
		}

		private void ClearTarget()
		{
			foreach (global::UnityEngine.GameObject hit in _hits)
			{
				global::UnityEngine.Object.Destroy(hit);
			}
			_hits.Clear();
			_avg = 0f;
			_hp = _maxHp;
		}

		public void ServerInteract(ReferenceHub ply, byte colliderId)
		{
			if (!PermissionsHandler.IsPermitted(ply.serverRoles.Permissions, PlayerPermissions.FacilityManagement) || !global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerInteractShootingTarget, ply, this))
			{
				return;
			}
			switch ((global::AdminToys.ShootingTarget.TargetButton)colliderId)
			{
			case global::AdminToys.ShootingTarget.TargetButton.Remove:
				global::Mirror.NetworkServer.Destroy(base.gameObject);
				return;
			case global::AdminToys.ShootingTarget.TargetButton.GlobalResults:
				Network_syncMode = !_syncMode;
				return;
			}
			if (_syncMode && !ply.isLocalPlayer)
			{
				UseButton((global::AdminToys.ShootingTarget.TargetButton)colliderId);
				RpcSendInfo(_maxHp, _autoDestroyTime);
			}
		}

		[global::Mirror.TargetRpc]
		private void TargetRpcReceiveData(global::Mirror.NetworkConnection conn, float damage, float distance, global::UnityEngine.Vector3 pos, global::PlayerStatsSystem.DamageHandlerBase handler)
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			global::Mirror.NetworkWriterExtensions.WriteSingle(writer, damage);
			global::Mirror.NetworkWriterExtensions.WriteSingle(writer, distance);
			global::Mirror.NetworkWriterExtensions.WriteVector3(writer, pos);
			global::PlayerStatsSystem.DamageHandlerReaderWriter.WriteDamageHandler(writer, handler);
			SendTargetRPCInternal(conn, typeof(global::AdminToys.ShootingTarget), "TargetRpcReceiveData", writer, 0);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		[global::Mirror.ClientRpc]
		private void RpcSendInfo(int maxHp, int autoReset)
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			global::Mirror.NetworkWriterExtensions.WriteInt32(writer, maxHp);
			global::Mirror.NetworkWriterExtensions.WriteInt32(writer, autoReset);
			SendRPCInternal(typeof(global::AdminToys.ShootingTarget), "RpcSendInfo", writer, 0, includeOwner: true);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		private void OnDrawGizmosSelected()
		{
			global::UnityEngine.Gizmos.color = global::UnityEngine.Color.red;
			global::UnityEngine.Gizmos.DrawWireSphere(_bullsEye.position, _bullsEyeRadius);
			global::UnityEngine.Gizmos.DrawWireSphere(_bullsEye.position, _stepSize);
			global::UnityEngine.Vector3[] bullsEyeBounds = _bullsEyeBounds;
			for (int i = 0; i < bullsEyeBounds.Length; i++)
			{
				global::UnityEngine.Vector3 vector = bullsEyeBounds[i];
				global::UnityEngine.Gizmos.DrawWireCube(_bullsEye.TransformPoint(new global::UnityEngine.Vector3(0f, vector.y, vector.x)), new global::UnityEngine.Vector3(0.04f, 1f, 1f) * vector.z);
			}
		}

		private void MirrorProcessed()
		{
		}

		private void UserCode_TargetRpcReceiveData(global::Mirror.NetworkConnection conn, float damage, float distance, global::UnityEngine.Vector3 pos, global::PlayerStatsSystem.DamageHandlerBase handler)
		{
			float num;
			if (_bullsEyeBounds.Length == 0)
			{
				num = global::UnityEngine.Vector3.Distance(_bullsEye.position, pos);
			}
			else
			{
				num = float.PositiveInfinity;
				global::UnityEngine.Vector3[] bullsEyeBounds = _bullsEyeBounds;
				for (int i = 0; i < bullsEyeBounds.Length; i++)
				{
					global::UnityEngine.Vector3 vector = bullsEyeBounds[i];
					float num2 = global::UnityEngine.Vector3.Distance(new global::UnityEngine.Bounds(_bullsEye.TransformPoint(new global::UnityEngine.Vector3(0f, vector.y, vector.x)), global::UnityEngine.Vector3.one * vector.z).ClosestPoint(pos), pos);
					if (num2 < num)
					{
						num = num2;
					}
				}
			}
			num = global::UnityEngine.Mathf.Max(0f, num - _bullsEyeRadius);
			int num3 = global::UnityEngine.Mathf.Min(global::UnityEngine.Mathf.CeilToInt(num / _stepSize), _score.Length - 1);
			float num4 = 1f - (float)num3 / ((float)_score.Length - 1f);
			_avg += num4;
			global::UnityEngine.GameObject gameObject = global::UnityEngine.GameObject.CreatePrimitive(global::UnityEngine.PrimitiveType.Sphere);
			gameObject.GetComponent<global::UnityEngine.Collider>().enabled = false;
			gameObject.GetComponent<global::UnityEngine.MeshRenderer>().sharedMaterial = _hitIndicator.GetComponent<global::UnityEngine.MeshRenderer>().sharedMaterial;
			gameObject.transform.localScale = _hitIndicator.transform.localScale;
			gameObject.transform.parent = _hitIndicator.transform.parent;
			gameObject.transform.position = pos;
			_hp -= damage;
			_source.Stop();
			_source.PlayOneShot(_score[num3]);
			_source.PlayOneShot((_hp < 0f) ? _killSound : _hitSound);
			if (_prevHit != null && _prevHit.TryGetComponent<global::UnityEngine.MeshRenderer>(out var component))
			{
				component.sharedMaterial = _prevHitMat;
			}
			_prevHit = gameObject;
			if (_autoDestroyTime > 0)
			{
				global::UnityEngine.Object.Destroy(gameObject, _autoDestroyTime);
			}
			else
			{
				_hits.Add(gameObject);
			}
		}

		protected static void InvokeUserCode_TargetRpcReceiveData(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkClient.active)
			{
				global::UnityEngine.Debug.LogError("TargetRPC TargetRpcReceiveData called on server.");
			}
			else
			{
				((global::AdminToys.ShootingTarget)obj).UserCode_TargetRpcReceiveData(global::Mirror.NetworkClient.readyConnection, global::Mirror.NetworkReaderExtensions.ReadSingle(reader), global::Mirror.NetworkReaderExtensions.ReadSingle(reader), global::Mirror.NetworkReaderExtensions.ReadVector3(reader), global::PlayerStatsSystem.DamageHandlerReaderWriter.ReadDamageHandler(reader));
			}
		}

		private void UserCode_RpcSendInfo(int maxHp, int autoReset)
		{
			_maxHp = maxHp;
			_autoDestroyTime = autoReset;
			ClearTarget();
		}

		protected static void InvokeUserCode_RpcSendInfo(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkClient.active)
			{
				global::UnityEngine.Debug.LogError("RPC RpcSendInfo called on server.");
			}
			else
			{
				((global::AdminToys.ShootingTarget)obj).UserCode_RpcSendInfo(global::Mirror.NetworkReaderExtensions.ReadInt32(reader), global::Mirror.NetworkReaderExtensions.ReadInt32(reader));
			}
		}

		static ShootingTarget()
		{
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(global::AdminToys.ShootingTarget), "RpcSendInfo", InvokeUserCode_RpcSendInfo);
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(global::AdminToys.ShootingTarget), "TargetRpcReceiveData", InvokeUserCode_TargetRpcReceiveData);
		}

		public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
		{
			bool result = base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, _syncMode);
				return true;
			}
			global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 0x10L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, _syncMode);
				result = true;
			}
			return result;
		}

		public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				bool flag = _syncMode;
				Network_syncMode = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
				return;
			}
			long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
			if ((num & 0x10L) != 0L)
			{
				bool flag2 = _syncMode;
				Network_syncMode = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
			}
		}
	}
}
