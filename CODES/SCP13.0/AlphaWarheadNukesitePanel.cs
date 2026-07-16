using System;
using System.Runtime.InteropServices;
using Interactables.Interobjects.DoorUtils;
using Mirror;
using UnityEngine;

public class AlphaWarheadNukesitePanel : NetworkBehaviour
{
	private enum DiodeType
	{
		InProgress = 0,
		BlastDoor = 1,
		OutsideDoor = 2
	}

	public Transform lever;

	public BlastDoor blastDoor;

	[SerializeField]
	private MeshRenderer _ledRenderer;

	[SerializeField]
	private Material _onMat;

	[SerializeField]
	private Material _offMat;

	private float _leverStatus;

	private Material[] _matSet;

	private bool[] _prevMats;

	private bool _anyModified;

	[SyncVar]
	public bool enabled;

	private const string OutsideDoorName = "SURFACE_NUKE";

	private bool _doorFound;

	private DoorNametagExtension _outsideDoor;
    private bool OutsideDoorOpen
    {
        get
        {
            if (!_doorFound)
            {
                _doorFound = DoorNametagExtension.NamedDoors.TryGetValue(OutsideDoorName, out _outsideDoor);
            }
            if (_doorFound)
            {
                return _outsideDoor.TargetDoor.TargetState;
            }
            return false;
        }
    }

    private void Awake()
    {
        int num = _ledRenderer.sharedMaterials.Length;
        _matSet = new Material[num];
        _prevMats = new bool[num];
        for (int i = 0; i < num; i++)
        {
            _matSet[i] = _offMat;
        }
        AlphaWarheadOutsitePanel.nukeside = this;
    }

    public bool AllowChangeLevelState()
    {
        if (!(Math.Abs(_leverStatus) < 0.001f))
        {
            return Math.Abs(_leverStatus - 1f) < 0.001f;
        }
        return true;
    }

    private void SetDiode(DiodeType diode, bool status)
    {
        if (_prevMats[(int)diode] != status)
        {
            _matSet[(int)diode] = (status ? _onMat : _offMat);
            _prevMats[(int)diode] = status;
            _anyModified = true;
        }
    }

    private void Update()
    {
        _anyModified = false;
        SetDiode(DiodeType.InProgress, AlphaWarheadController.InProgress);
        SetDiode(DiodeType.OutsideDoor, OutsideDoorOpen);
        SetDiode(DiodeType.BlastDoor, blastDoor.isClosed);
        if (_anyModified)
        {
            _ledRenderer.sharedMaterials = _matSet;
        }
        _leverStatus += (enabled ? 0.04f : (-0.04f));
        _leverStatus = Mathf.Clamp01(_leverStatus);
        lever.localRotation = Quaternion.Euler(new Vector3(Mathf.Lerp(10f, -170f, _leverStatus), -90f, 90f));
    }

}
