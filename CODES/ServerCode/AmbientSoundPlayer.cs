public class AmbientSoundPlayer : global::Mirror.NetworkBehaviour
{
	[global::System.Serializable]
	public class AmbientClip
	{
		public global::UnityEngine.AudioClip clip;

		public bool repeatable = true;

		public bool is3D = true;

		public bool played;

		public int index;
	}

	public global::UnityEngine.GameObject audioPrefab;

	public int minTime = 30;

	public int maxTime = 60;

	public AmbientSoundPlayer.AmbientClip[] clips;

	private global::System.Collections.Generic.List<AmbientSoundPlayer.AmbientClip> list = new global::System.Collections.Generic.List<AmbientSoundPlayer.AmbientClip>();

	private global::Security.RateLimit _ambientSoundRateLimit = new global::Security.RateLimit(4, 3f);

	private void Start()
	{
		if (base.isLocalPlayer && base.isServer)
		{
			for (int i = 0; i < clips.Length; i++)
			{
				clips[i].index = i;
			}
			Invoke("GenerateRandom", 10f);
		}
	}

	private void GenerateRandom()
	{
		list.Clear();
		int num = 0;
		AmbientSoundPlayer.AmbientClip[] array = clips;
		foreach (AmbientSoundPlayer.AmbientClip ambientClip in array)
		{
			if (!ambientClip.played)
			{
				list.Add(ambientClip);
			}
		}
		num = global::UnityEngine.Random.Range(0, list.Count);
		int index = list[num].index;
		if (!clips[index].repeatable)
		{
			clips[index].played = true;
		}
		RpcPlaySound(index);
		Invoke("GenerateRandom", global::UnityEngine.Random.Range(minTime, maxTime));
	}

	[global::Mirror.ClientRpc]
	private void RpcPlaySound(int id)
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		global::Mirror.NetworkWriterExtensions.WriteInt32(writer, id);
		SendRPCInternal(typeof(AmbientSoundPlayer), "RpcPlaySound", writer, 0, includeOwner: true);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	private void MirrorProcessed()
	{
	}

	private void UserCode_RpcPlaySound(int id)
	{
	}

	protected static void InvokeUserCode_RpcPlaySound(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkClient.active)
		{
			global::UnityEngine.Debug.LogError("RPC RpcPlaySound called on server.");
		}
		else
		{
			((AmbientSoundPlayer)obj).UserCode_RpcPlaySound(global::Mirror.NetworkReaderExtensions.ReadInt32(reader));
		}
	}

	static AmbientSoundPlayer()
	{
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(AmbientSoundPlayer), "RpcPlaySound", InvokeUserCode_RpcPlaySound);
	}
}
