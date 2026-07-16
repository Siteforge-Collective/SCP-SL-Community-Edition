using System.Collections.Generic;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public class BlastDoor : NetworkBehaviour
{
	public static readonly HashSet<BlastDoor> Instances;

	private static readonly int _close;

	[SyncVar(hook = "SetClosed")]
	public bool isClosed;

	public bool NetworkisClosed
	{
		get
		{
			return false;
		}
		[param: In]
		set
		{
		}
	}

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}

	public void SetClosed(bool prev, bool b)
	{
	}
}
