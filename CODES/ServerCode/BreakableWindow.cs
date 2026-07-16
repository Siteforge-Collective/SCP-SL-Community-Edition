public class BreakableWindow : global::Mirror.NetworkBehaviour, IDestructible
{
	public struct BreakableWindowStatus : global::System.IEquatable<BreakableWindow.BreakableWindowStatus>
	{
		public global::UnityEngine.Vector3 position;

		public global::UnityEngine.Quaternion rotation;

		public bool broken;

		public bool IsEqual(BreakableWindow.BreakableWindowStatus stat)
		{
			if (position == stat.position && rotation == stat.rotation)
			{
				return broken == stat.broken;
			}
			return false;
		}

		public bool Equals(BreakableWindow.BreakableWindowStatus other)
		{
			if (position == other.position && rotation == other.rotation)
			{
				return broken == other.broken;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is BreakableWindow.BreakableWindowStatus other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (((position.GetHashCode() * 397) ^ rotation.GetHashCode()) * 397) ^ broken.GetHashCode();
		}

		public static bool operator ==(BreakableWindow.BreakableWindowStatus left, BreakableWindow.BreakableWindowStatus right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(BreakableWindow.BreakableWindowStatus left, BreakableWindow.BreakableWindowStatus right)
		{
			return !left.Equals(right);
		}
	}

	public global::UnityEngine.GameObject template;

	public global::UnityEngine.Transform parent;

	public global::UnityEngine.Vector3 size;

	[global::UnityEngine.SerializeField]
	private bool _preventScpDamage;

	public global::Footprinting.Footprint LastAttacker;

	private BreakableWindow.BreakableWindowStatus prevStatus;

	[global::Mirror.SyncVar]
	public BreakableWindow.BreakableWindowStatus syncStatus;

	public float health = 30f;

	public bool isBroken;

	private global::System.Collections.Generic.List<global::UnityEngine.MeshRenderer> meshRenderers = new global::System.Collections.Generic.List<global::UnityEngine.MeshRenderer>();

	private global::UnityEngine.Transform _transform;

	public uint NetworkId => base.netId;

	public global::UnityEngine.Vector3 CenterOfMass => base.transform.position;

	public BreakableWindow.BreakableWindowStatus NetworksyncStatus
	{
		get
		{
			return syncStatus;
		}
		[param: global::System.Runtime.InteropServices.In]
		set
		{
			if (!SyncVarEqual(value, ref syncStatus))
			{
				BreakableWindow.BreakableWindowStatus breakableWindowStatus = syncStatus;
				SetSyncVar(value, ref syncStatus, 1uL);
			}
		}
	}

	[global::Mirror.ServerCallback]
	private void UpdateStatus(BreakableWindow.BreakableWindowStatus s)
	{
		if (global::Mirror.NetworkServer.active)
		{
			NetworksyncStatus = s;
		}
	}

	[global::Mirror.ServerCallback]
	private void ServerDamageWindow(float damage)
	{
		if (global::Mirror.NetworkServer.active)
		{
			health -= damage;
			if (health <= 0f)
			{
				StartCoroutine(BreakWindow());
			}
		}
	}

	private void Awake()
	{
		meshRenderers.AddRange(GetComponentsInChildren<global::UnityEngine.MeshRenderer>());
		_transform = base.transform;
		GetComponent<global::UnityEngine.Collider>().enabled = false;
		Invoke("EnableColliders", 1f);
	}

	private void EnableColliders()
	{
		GetComponent<global::UnityEngine.Collider>().enabled = true;
	}

	private void Update()
	{
		global::UnityEngine.Vector3 position = _transform.position;
		global::UnityEngine.Quaternion rotation = _transform.rotation;
		if (position == syncStatus.position && rotation == syncStatus.rotation && isBroken == syncStatus.broken)
		{
			return;
		}
		if (global::Mirror.NetworkServer.active)
		{
			BreakableWindow.BreakableWindowStatus s = new BreakableWindow.BreakableWindowStatus
			{
				position = position,
				rotation = rotation,
				broken = isBroken
			};
			UpdateStatus(s);
			return;
		}
		if (!isBroken && syncStatus.broken)
		{
			StartCoroutine(BreakWindow());
		}
		_transform.position = syncStatus.position;
		_transform.rotation = syncStatus.rotation;
		isBroken = syncStatus.broken;
	}

	private void LateUpdate()
	{
		for (int num = meshRenderers.Count - 1; num >= 0; num--)
		{
			global::UnityEngine.MeshRenderer meshRenderer = meshRenderers[num];
			meshRenderer.shadowCastingMode = (isBroken ? global::UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly : global::UnityEngine.Rendering.ShadowCastingMode.Off);
			if (isBroken)
			{
				meshRenderers.RemoveAt(num);
				global::UnityEngine.Object.Destroy(meshRenderer);
			}
			meshRenderer.gameObject.layer = (isBroken ? 28 : 14);
		}
	}

	private global::System.Collections.IEnumerator BreakWindow()
	{
		isBroken = true;
		if (ServerStatic.IsDedicated)
		{
			yield break;
		}
		global::UnityEngine.Collider[] componentsInChildren = GetComponentsInChildren<global::UnityEngine.Collider>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = false;
		}
		global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(template, parent);
		gameObject.transform.localScale = global::UnityEngine.Vector3.one;
		gameObject.transform.localPosition = global::UnityEngine.Vector3.zero;
		gameObject.transform.localRotation = global::UnityEngine.Quaternion.Euler(global::UnityEngine.Vector3.zero);
		global::UnityEngine.Rigidbody[] rbs = gameObject.GetComponentsInChildren<global::UnityEngine.Rigidbody>();
		global::System.Collections.Generic.List<global::UnityEngine.Vector3> scales = global::NorthwoodLib.Pools.ListPool<global::UnityEngine.Vector3>.Shared.Rent();
		global::UnityEngine.Rigidbody[] array = rbs;
		foreach (global::UnityEngine.Rigidbody rigidbody in array)
		{
			rigidbody.angularVelocity = new global::UnityEngine.Vector3(global::UnityEngine.Random.Range(-360, 360), global::UnityEngine.Random.Range(-360, 360), global::UnityEngine.Random.Range(-360, 360));
			rigidbody.velocity = new global::UnityEngine.Vector3(global::UnityEngine.Random.Range(-2, 2), global::UnityEngine.Random.Range(-2, 2), global::UnityEngine.Random.Range(-2, 2));
			scales.Add(rigidbody.transform.localScale);
		}
		for (int j = 0; j < 250; j++)
		{
			for (int k = 0; k < scales.Count; k++)
			{
				rbs[k].transform.localScale = global::UnityEngine.Vector3.Lerp(scales[k], scales[k] / 2f, (float)j / 75f);
			}
			yield return null;
		}
		for (float i2 = 0f; i2 < 150f; i2 += 1f)
		{
			for (int l = 0; l < scales.Count; l++)
			{
				rbs[l].transform.localScale = global::UnityEngine.Vector3.Lerp(scales[l] / 2f, global::UnityEngine.Vector3.zero, i2 / 150f);
			}
			yield return null;
		}
		global::NorthwoodLib.Pools.ListPool<global::UnityEngine.Vector3>.Shared.Return(scales);
		array = rbs;
		for (int i = 0; i < array.Length; i++)
		{
			global::UnityEngine.Object.Destroy(array[i].gameObject, 1f);
		}
	}

	private bool CheckDamagePerms(global::PlayerRoles.RoleTypeId roleType)
	{
		if (!_preventScpDamage)
		{
			return true;
		}
		if (!global::PlayerRoles.PlayerRoleLoader.TryGetRoleTemplate<global::PlayerRoles.PlayerRoleBase>(roleType, out var result))
		{
			return false;
		}
		return result.Team != global::PlayerRoles.Team.SCPs;
	}

	public bool Damage(float damage, global::PlayerStatsSystem.DamageHandlerBase handler, global::UnityEngine.Vector3 pos)
	{
		if (handler is global::PlayerStatsSystem.AttackerDamageHandler attackerDamageHandler)
		{
			if (!CheckDamagePerms(attackerDamageHandler.Attacker.Role))
			{
				return false;
			}
			if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerDamagedWindow, attackerDamageHandler.Attacker.Hub, this, handler, damage))
			{
				return false;
			}
			LastAttacker = attackerDamageHandler.Attacker;
		}
		ServerDamageWindow(damage);
		return true;
	}

	private void MirrorProcessed()
	{
	}

	public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
	{
		bool result = base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteBreakableWindowStatus(syncStatus);
			return true;
		}
		global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1L) != 0L)
		{
			writer.WriteBreakableWindowStatus(syncStatus);
			result = true;
		}
		return result;
	}

	public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			BreakableWindow.BreakableWindowStatus breakableWindowStatus = syncStatus;
			NetworksyncStatus = reader.ReadBreakableWindowStatus();
			return;
		}
		long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
		if ((num & 1L) != 0L)
		{
			BreakableWindow.BreakableWindowStatus breakableWindowStatus2 = syncStatus;
			NetworksyncStatus = reader.ReadBreakableWindowStatus();
		}
	}
}
