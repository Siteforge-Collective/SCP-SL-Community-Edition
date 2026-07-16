namespace Hazards
{
	public class TantrumEnvironmentalHazard : global::Hazards.TemporaryHazard
	{
		public static readonly global::System.Collections.Generic.List<global::Hazards.TantrumEnvironmentalHazard> AllTantrums;

		private const float ExplosionHeight = 5f;

		private const float DelayedDestroy = 6f;

		[global::System.Runtime.CompilerServices.CompilerGenerated]
		[global::Mirror.SyncVar]
		private global::RelativePositioning.RelativePosition _003CSynchronizedPosition_003Ek__BackingField;

		private readonly float _explodeDistance = 5.25f;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Transform _correctPosition;

		public override global::UnityEngine.Vector3 SourcePosition
		{
			get
			{
				return _correctPosition.position + SourceOffset;
			}
			set
			{
				base.transform.position = value;
			}
		}

		public global::RelativePositioning.RelativePosition SynchronizedPosition
		{
			[global::System.Runtime.CompilerServices.CompilerGenerated]
			get
			{
				return _003CSynchronizedPosition_003Ek__BackingField;
			}
			[global::System.Runtime.CompilerServices.CompilerGenerated]
			set
			{
				Network_003CSynchronizedPosition_003Ek__BackingField = value;
			}
		}

		public bool PlaySizzle { get; set; }

		protected override float HazardDuration => 180f;

		protected override float DecaySpeed
		{
			get
			{
				float num = 1f;
				foreach (global::InventorySystem.Items.Usables.Scp244.Scp244DeployablePickup instance in global::InventorySystem.Items.Usables.Scp244.Scp244DeployablePickup.Instances)
				{
					num += instance.FogPercentForPoint(SourcePosition);
				}
				return num;
			}
		}

		public global::RelativePositioning.RelativePosition Network_003CSynchronizedPosition_003Ek__BackingField
		{
			get
			{
				return SynchronizedPosition;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref SynchronizedPosition))
				{
					global::RelativePositioning.RelativePosition relativePosition = SynchronizedPosition;
					SetSyncVar(value, ref SynchronizedPosition, 1uL);
				}
			}
		}

		public override void OnEnter(ReferenceHub player)
		{
			if (IsActive && !global::PlayerRoles.PlayerRolesUtils.IsSCP(player))
			{
				base.OnEnter(player);
				player.playerEffectsController.EnableEffect<global::CustomPlayerEffects.Stained>(1f);
			}
		}

		public override void OnStay(ReferenceHub player)
		{
			player.playerEffectsController.EnableEffect<global::CustomPlayerEffects.Stained>(1f);
		}

		public override void OnExit(ReferenceHub player)
		{
			base.OnExit(player);
			if (IsActive && !global::PlayerRoles.PlayerRolesUtils.IsSCP(player))
			{
				player.playerEffectsController.EnableEffect<global::CustomPlayerEffects.Stained>(2f);
			}
		}

		protected override void Start()
		{
			base.Start();
			if (global::Mirror.NetworkServer.active)
			{
				AllTantrums.Add(this);
				global::InventorySystem.Items.ThrowableProjectiles.ExplosionGrenade.OnExploded += CheckExplosion;
			}
		}

		[global::Mirror.Server]
		public override void ServerDestroy()
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void Hazards.TantrumEnvironmentalHazard::ServerDestroy()' called when server was not active");
				return;
			}
			base.ServerDestroy();
			RpcDespawn(PlaySizzle);
			ServerDelayedDestroy(6f);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (global::Mirror.NetworkServer.active)
			{
				AllTantrums.Remove(this);
				global::InventorySystem.Items.ThrowableProjectiles.ExplosionGrenade.OnExploded -= CheckExplosion;
			}
		}

		[global::Mirror.ClientRpc]
		private void RpcDespawn(bool playSizzle)
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, playSizzle);
			SendRPCInternal(typeof(global::Hazards.TantrumEnvironmentalHazard), "RpcDespawn", writer, 0, includeOwner: true);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		private void ServerDelayedDestroy(float waitTime)
		{
			if (global::Mirror.NetworkServer.active)
			{
				global::MEC.Timing.RunCoroutine(DelayedPuddleRemoval(waitTime).CancelWith(base.gameObject));
			}
		}

		private void LateUpdate()
		{
			SourcePosition = SynchronizedPosition.Position;
		}

		private void CheckExplosion(global::Footprinting.Footprint attacker, global::UnityEngine.Vector3 pos, global::InventorySystem.Items.ThrowableProjectiles.ExplosionGrenade grenade)
		{
			global::UnityEngine.Vector3 position = _correctPosition.position;
			if (!(global::UnityEngine.Mathf.Abs(pos.y - position.y) > 5f))
			{
				float num = _explodeDistance * _explodeDistance;
				if (!((position - pos).SqrMagnitudeIgnoreY() > num))
				{
					PlaySizzle = true;
					ServerDestroy();
				}
			}
		}

		private global::System.Collections.Generic.IEnumerator<float> DelayedPuddleRemoval(float waitTime)
		{
			yield return global::MEC.Timing.WaitForSeconds(waitTime);
			global::Mirror.NetworkServer.Destroy(base.gameObject);
		}

		static TantrumEnvironmentalHazard()
		{
			AllTantrums = new global::System.Collections.Generic.List<global::Hazards.TantrumEnvironmentalHazard>();
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(global::Hazards.TantrumEnvironmentalHazard), "RpcDespawn", InvokeUserCode_RpcDespawn);
		}

		private void MirrorProcessed()
		{
		}

		private void UserCode_RpcDespawn(bool playSizzle)
		{
		}

		protected static void InvokeUserCode_RpcDespawn(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkClient.active)
			{
				global::UnityEngine.Debug.LogError("RPC RpcDespawn called on server.");
			}
			else
			{
				((global::Hazards.TantrumEnvironmentalHazard)obj).UserCode_RpcDespawn(global::Mirror.NetworkReaderExtensions.ReadBoolean(reader));
			}
		}

		public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
		{
			bool result = base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				global::RelativePositioning.RelativePositionSerialization.WriteRelativePosition(writer, SynchronizedPosition);
				return true;
			}
			global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 1L) != 0L)
			{
				global::RelativePositioning.RelativePositionSerialization.WriteRelativePosition(writer, SynchronizedPosition);
				result = true;
			}
			return result;
		}

		public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				global::RelativePositioning.RelativePosition relativePosition = SynchronizedPosition;
				Network_003CSynchronizedPosition_003Ek__BackingField = global::RelativePositioning.RelativePositionSerialization.ReadRelativePosition(reader);
				return;
			}
			long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
			if ((num & 1L) != 0L)
			{
				global::RelativePositioning.RelativePosition relativePosition2 = SynchronizedPosition;
				Network_003CSynchronizedPosition_003Ek__BackingField = global::RelativePositioning.RelativePositionSerialization.ReadRelativePosition(reader);
			}
		}
	}
}
