public class TeslaGate : global::Mirror.NetworkBehaviour
{
	public global::UnityEngine.Vector3 localPosition;

	public global::UnityEngine.Vector3 localRotation;

	public global::UnityEngine.GameObject[] killers;

	public global::UnityEngine.LayerMask killerMask;

	public bool InProgress;

	public global::UnityEngine.Animator ledLights;

	public global::UnityEngine.ParticleSystem[] windupParticles;

	public global::UnityEngine.ParticleSystem[] shockParticles;

	public global::UnityEngine.ParticleSystem[] smokeParticles;

	[global::UnityEngine.Header("Parameters")]
	public global::UnityEngine.Vector3 sizeOfKiller;

	public float sizeOfTrigger;

	public float distanceToIdle;

	public float windupTime;

	public float cooldownTime;

	[global::UnityEngine.Header("Idle Loop")]
	public bool isIdling;

	public global::UnityEngine.AudioSource loopSource;

	public global::UnityEngine.AudioClip idleStart;

	public global::UnityEngine.AudioClip idleLoop;

	public global::UnityEngine.AudioClip idleEnd;

	[global::UnityEngine.Header("Audio")]
	public global::UnityEngine.AudioSource source;

	public global::UnityEngine.AudioClip[] clipsWarmup;

	public global::UnityEngine.AudioClip[] clipsShock;

	public bool showGizmos;

	[global::Mirror.SyncVar]
	public float InactiveTime;

	public global::System.Collections.Generic.List<global::Hazards.TantrumEnvironmentalHazard> TantrumsToBeDestroyed = new global::System.Collections.Generic.List<global::Hazards.TantrumEnvironmentalHazard>();

	private bool next079burst;

	private static readonly int _animatorShockHash;

	private static readonly int _animatorIdleHash;

	public float NetworkInactiveTime
	{
		get
		{
			return InactiveTime;
		}
		[param: global::System.Runtime.InteropServices.In]
		set
		{
			if (!SyncVarEqual(value, ref InactiveTime))
			{
				float inactiveTime = InactiveTime;
				SetSyncVar(value, ref InactiveTime, 1uL);
			}
		}
	}

	public static event global::System.Action<TeslaGate> OnBursted;

	public void ServerSideCode()
	{
		if (!InProgress)
		{
			global::MEC.Timing.RunCoroutine(ServerSideWaitForAnimation());
			RpcPlayAnimation();
		}
	}

	private global::System.Collections.Generic.IEnumerator<float> ServerSideWaitForAnimation()
	{
		InProgress = true;
		yield return global::MEC.Timing.WaitForSeconds(windupTime);
		if (TantrumsToBeDestroyed.Count > 0)
		{
			TantrumsToBeDestroyed.ForEach(delegate(global::Hazards.TantrumEnvironmentalHazard tantrum)
			{
				if (tantrum != null)
				{
					tantrum.PlaySizzle = true;
					tantrum.ServerDestroy();
				}
			});
			TantrumsToBeDestroyed.Clear();
		}
		yield return global::MEC.Timing.WaitForSeconds(cooldownTime);
		InProgress = false;
	}

	public void ServerSideIdle(bool shouldIdle)
	{
		if (shouldIdle)
		{
			RpcDoIdle();
		}
		else
		{
			RpcDoneIdling();
		}
	}

	private void Update()
	{
	}

	private void Start()
	{
	}

	public void ClientSideCode()
	{
		base.transform.localPosition = localPosition;
		base.transform.localRotation = global::UnityEngine.Quaternion.Euler(localRotation);
		if (ledLights != null)
		{
			ledLights.SetBool(_animatorShockHash, InProgress);
			ledLights.SetBool(_animatorIdleHash, isIdling);
		}
	}

	[global::Mirror.ClientRpc]
	private void RpcDoIdle()
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		SendRPCInternal(typeof(TeslaGate), "RpcDoIdle", writer, 0, includeOwner: true);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	[global::Mirror.ClientRpc]
	private void RpcDoneIdling()
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		SendRPCInternal(typeof(TeslaGate), "RpcDoneIdling", writer, 0, includeOwner: true);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	[global::Mirror.ClientRpc]
	private void RpcPlayAnimation()
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		SendRPCInternal(typeof(TeslaGate), "RpcPlayAnimation", writer, 0, includeOwner: true);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	[global::Mirror.ClientRpc]
	public void RpcInstantBurst()
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		SendRPCInternal(typeof(TeslaGate), "RpcInstantBurst", writer, 0, includeOwner: true);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	private global::System.Collections.Generic.IEnumerator<float> _DoShock()
	{
		TeslaGate.OnBursted?.Invoke(this);
		source.Stop();
		global::UnityEngine.AudioClip[] array = clipsShock;
		foreach (global::UnityEngine.AudioClip audioClip in array)
		{
			if (audioClip != null)
			{
				source.PlayOneShot(audioClip);
			}
		}
		global::UnityEngine.ParticleSystem[] array2 = windupParticles;
		foreach (global::UnityEngine.ParticleSystem particleSystem in array2)
		{
			if (particleSystem != null)
			{
				particleSystem.Play();
			}
		}
		array2 = shockParticles;
		foreach (global::UnityEngine.ParticleSystem particleSystem2 in array2)
		{
			if (particleSystem2 != null)
			{
				particleSystem2.Play();
			}
		}
		ReferenceHub hub;
		while (!ReferenceHub.TryGetLocalHub(out hub))
		{
			yield return float.NegativeInfinity;
		}
		_ = hub.gameObject;
		yield return global::MEC.Timing.WaitForSeconds(0.25f);
		yield return global::MEC.Timing.WaitForSeconds(0.25f);
		array2 = smokeParticles;
		foreach (global::UnityEngine.ParticleSystem particleSystem3 in array2)
		{
			if (particleSystem3 != null)
			{
				particleSystem3.Play();
			}
		}
		if (isIdling)
		{
			yield break;
		}
		array2 = windupParticles;
		foreach (global::UnityEngine.ParticleSystem particleSystem4 in array2)
		{
			if (particleSystem4 != null)
			{
				particleSystem4.Stop();
			}
		}
	}

	private global::System.Collections.Generic.IEnumerator<float> _PlayAnimation()
	{
		bool is079 = next079burst;
		next079burst = false;
		if (!is079)
		{
			global::UnityEngine.AudioClip[] array = clipsWarmup;
			foreach (global::UnityEngine.AudioClip clip in array)
			{
				source.PlayOneShot(clip);
			}
			global::UnityEngine.ParticleSystem[] array2 = windupParticles;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].Play();
			}
			yield return global::MEC.Timing.WaitForSeconds(windupTime);
		}
		global::MEC.Timing.RunCoroutine(_DoShock());
		yield return global::MEC.Timing.WaitForSeconds(is079 ? 0.5f : cooldownTime);
	}

	public bool PlayerInRange(ReferenceHub player)
	{
		if (player.roleManager.CurrentRole is global::PlayerRoles.ITeslaControllerRole teslaControllerRole && !teslaControllerRole.CanActivateShock)
		{
			return false;
		}
		return InRange(player.transform.position);
	}

	private bool InRange(global::UnityEngine.Vector3 position)
	{
		return global::UnityEngine.Vector3.Distance(base.transform.position, position) < sizeOfTrigger;
	}

	public bool PlayerInIdleRange(ReferenceHub player)
	{
		if (!global::PlayerRoles.PlayerRolesUtils.IsAlive(player) || (player.roleManager.CurrentRole is global::PlayerRoles.ITeslaControllerRole teslaControllerRole && !teslaControllerRole.CanActivateIdle))
		{
			return false;
		}
		global::UnityEngine.Vector3 position = base.transform.position;
		if (player.roleManager.CurrentRole is global::PlayerRoles.FirstPersonControl.IFpcRole fpcRole)
		{
			return global::UnityEngine.Vector3.Distance(position, fpcRole.FpcModule.Position) < distanceToIdle;
		}
		if (player.roleManager.CurrentRole is global::PlayerRoles.PlayableScps.Scp079.Scp079Role scp079Role)
		{
			return global::MapGeneration.RoomIdUtils.IsTheSameRoom(scp079Role.CurrentCamera.Position, position);
		}
		return false;
	}

	private void OnDrawGizmosSelected()
	{
		if (showGizmos)
		{
			global::UnityEngine.Gizmos.color = new global::UnityEngine.Color(1f, 0f, 0f, 0.2f);
			global::UnityEngine.GameObject[] array = killers;
			for (int i = 0; i < array.Length; i++)
			{
				global::UnityEngine.Gizmos.DrawCube(array[i].transform.position + global::UnityEngine.Vector3.up * (sizeOfKiller.y / 2f), sizeOfKiller);
			}
			global::UnityEngine.Gizmos.color = new global::UnityEngine.Color(1f, 1f, 0f, 0.2f);
			global::UnityEngine.Gizmos.DrawSphere(base.transform.position, sizeOfTrigger);
		}
	}

	static TeslaGate()
	{
		_animatorShockHash = global::UnityEngine.Animator.StringToHash("ShockActive");
		_animatorIdleHash = global::UnityEngine.Animator.StringToHash("IdleActive");
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(TeslaGate), "RpcDoIdle", InvokeUserCode_RpcDoIdle);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(TeslaGate), "RpcDoneIdling", InvokeUserCode_RpcDoneIdling);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(TeslaGate), "RpcPlayAnimation", InvokeUserCode_RpcPlayAnimation);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(TeslaGate), "RpcInstantBurst", InvokeUserCode_RpcInstantBurst);
	}

	private void MirrorProcessed()
	{
	}

	private void UserCode_RpcDoIdle()
	{
		if (!isIdling)
		{
			isIdling = true;
			loopSource.PlayOneShot(idleStart);
			loopSource.PlayDelayed(idleStart.length);
		}
		global::UnityEngine.ParticleSystem[] array = windupParticles;
		foreach (global::UnityEngine.ParticleSystem particleSystem in array)
		{
			if (!particleSystem.isPlaying)
			{
				particleSystem.Play();
			}
		}
	}

	protected static void InvokeUserCode_RpcDoIdle(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkClient.active)
		{
			global::UnityEngine.Debug.LogError("RPC RpcDoIdle called on server.");
		}
		else
		{
			((TeslaGate)obj).UserCode_RpcDoIdle();
		}
	}

	private void UserCode_RpcDoneIdling()
	{
		if (isIdling)
		{
			isIdling = false;
			loopSource.Stop();
			loopSource.PlayOneShot(idleEnd);
			global::UnityEngine.ParticleSystem[] array = windupParticles;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Stop();
			}
		}
	}

	protected static void InvokeUserCode_RpcDoneIdling(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkClient.active)
		{
			global::UnityEngine.Debug.LogError("RPC RpcDoneIdling called on server.");
		}
		else
		{
			((TeslaGate)obj).UserCode_RpcDoneIdling();
		}
	}

	private void UserCode_RpcPlayAnimation()
	{
		global::MEC.Timing.RunCoroutine(_PlayAnimation(), global::MEC.Segment.FixedUpdate);
	}

	protected static void InvokeUserCode_RpcPlayAnimation(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkClient.active)
		{
			global::UnityEngine.Debug.LogError("RPC RpcPlayAnimation called on server.");
		}
		else
		{
			((TeslaGate)obj).UserCode_RpcPlayAnimation();
		}
	}

	public void UserCode_RpcInstantBurst()
	{
		next079burst = true;
		global::MEC.Timing.RunCoroutine(_PlayAnimation(), global::MEC.Segment.FixedUpdate);
	}

	protected static void InvokeUserCode_RpcInstantBurst(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkClient.active)
		{
			global::UnityEngine.Debug.LogError("RPC RpcInstantBurst called on server.");
		}
		else
		{
			((TeslaGate)obj).UserCode_RpcInstantBurst();
		}
	}

	public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
	{
		bool result = base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			global::Mirror.NetworkWriterExtensions.WriteSingle(writer, InactiveTime);
			return true;
		}
		global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1L) != 0L)
		{
			global::Mirror.NetworkWriterExtensions.WriteSingle(writer, InactiveTime);
			result = true;
		}
		return result;
	}

	public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			float inactiveTime = InactiveTime;
			NetworkInactiveTime = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
			return;
		}
		long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
		if ((num & 1L) != 0L)
		{
			float inactiveTime2 = InactiveTime;
			NetworkInactiveTime = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
		}
	}
}
