public class AlphaWarheadNukesitePanel : global::Mirror.NetworkBehaviour
{
	public global::UnityEngine.Transform lever;

	public BlastDoor blastDoor;

	public global::UnityEngine.Material led_blastdoors;

	public global::UnityEngine.Material led_outsidedoor;

	public global::UnityEngine.Material led_detonationinprogress;

	public global::UnityEngine.Material led_cancel;

	public global::UnityEngine.Material[] onOffMaterial;

	private float _leverStatus;

	[global::Mirror.SyncVar]
	public new bool enabled;

	private const string OutsideDoorName = "SURFACE_NUKE";

	private static readonly int _emissionColor = global::UnityEngine.Shader.PropertyToID("_EmissionColor");

	private bool _doorFound;

	private global::Interactables.Interobjects.DoorUtils.DoorNametagExtension _outsideDoor;

	private bool OutsideDoorOpen
	{
		get
		{
			if (!_doorFound)
			{
				_doorFound = global::Interactables.Interobjects.DoorUtils.DoorNametagExtension.NamedDoors.TryGetValue("SURFACE_NUKE", out _outsideDoor);
			}
			if (_doorFound)
			{
				return _outsideDoor.TargetDoor.TargetState;
			}
			return false;
		}
	}

	public bool Networkenabled
	{
		get
		{
			return enabled;
		}
		[param: global::System.Runtime.InteropServices.In]
		set
		{
			if (!SyncVarEqual(value, ref enabled))
			{
				bool flag = enabled;
				SetSyncVar(value, ref enabled, 1uL);
			}
		}
	}

	private void Awake()
	{
		AlphaWarheadOutsitePanel.nukeside = this;
	}

	private void FixedUpdate()
	{
		UpdateLeverStatus();
	}

	public bool AllowChangeLevelState()
	{
		if (!(global::System.Math.Abs(_leverStatus) < 0.001f))
		{
			return global::System.Math.Abs(_leverStatus - 1f) < 0.001f;
		}
		return true;
	}

	private void UpdateLeverStatus()
	{
		if (!(AlphaWarheadController.Singleton == null))
		{
			global::UnityEngine.Color color = new global::UnityEngine.Color(0.2f, 0.3f, 0.5f);
			led_detonationinprogress.SetColor(_emissionColor, AlphaWarheadController.Singleton.Info.InProgress ? color : global::UnityEngine.Color.black);
			led_outsidedoor.SetColor(_emissionColor, OutsideDoorOpen ? color : global::UnityEngine.Color.black);
			led_blastdoors.SetColor(_emissionColor, blastDoor.isClosed ? color : global::UnityEngine.Color.black);
			led_cancel.SetColor(_emissionColor, (AlphaWarheadController.TimeUntilDetonation > 10f && AlphaWarheadController.InProgress) ? global::UnityEngine.Color.red : global::UnityEngine.Color.black);
			_leverStatus += (enabled ? 0.04f : (-0.04f));
			_leverStatus = global::UnityEngine.Mathf.Clamp01(_leverStatus);
			for (int i = 0; i < 2; i++)
			{
				onOffMaterial[i].SetColor(_emissionColor, (i == global::UnityEngine.Mathf.RoundToInt(_leverStatus)) ? new global::UnityEngine.Color(1.2f, 1.2f, 1.2f, 1f) : global::UnityEngine.Color.black);
			}
			lever.localRotation = global::UnityEngine.Quaternion.Euler(new global::UnityEngine.Vector3(global::UnityEngine.Mathf.Lerp(10f, -170f, _leverStatus), -90f, 90f));
		}
	}

	private void MirrorProcessed()
	{
	}

	public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
	{
		bool result = base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, enabled);
			return true;
		}
		global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1L) != 0L)
		{
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, enabled);
			result = true;
		}
		return result;
	}

	public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			bool flag = enabled;
			Networkenabled = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
			return;
		}
		long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
		if ((num & 1L) != 0L)
		{
			bool flag2 = enabled;
			Networkenabled = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
		}
	}
}
