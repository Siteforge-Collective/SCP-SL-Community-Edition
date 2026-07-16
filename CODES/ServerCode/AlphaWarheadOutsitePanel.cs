public class AlphaWarheadOutsitePanel : global::Mirror.NetworkBehaviour
{
	public global::UnityEngine.Animator panelButtonCoverAnim;

	public static AlphaWarheadNukesitePanel nukeside;

	public global::UnityEngine.GameObject[] inevitable;

	[global::Mirror.SyncVar]
	public bool keycardEntered;

	private static readonly int _enabled = global::UnityEngine.Animator.StringToHash("enabled");

	public bool NetworkkeycardEntered
	{
		get
		{
			return keycardEntered;
		}
		[param: global::System.Runtime.InteropServices.In]
		set
		{
			if (!SyncVarEqual(value, ref keycardEntered))
			{
				bool flag = keycardEntered;
				SetSyncVar(value, ref keycardEntered, 1uL);
			}
		}
	}

	private void Update()
	{
		base.transform.localPosition = new global::UnityEngine.Vector3(0f, 0f, 9f);
		global::UnityEngine.GameObject[] array = inevitable;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(AlphaWarheadController.InProgress && AlphaWarheadController.TimeUntilDetonation <= 10f && AlphaWarheadController.TimeUntilDetonation > 0f);
		}
		panelButtonCoverAnim.SetBool(_enabled, keycardEntered);
	}

	private void MirrorProcessed()
	{
	}

	public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
	{
		bool result = base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, keycardEntered);
			return true;
		}
		global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1L) != 0L)
		{
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, keycardEntered);
			result = true;
		}
		return result;
	}

	public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			bool flag = keycardEntered;
			NetworkkeycardEntered = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
			return;
		}
		long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
		if ((num & 1L) != 0L)
		{
			bool flag2 = keycardEntered;
			NetworkkeycardEntered = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
		}
	}
}
