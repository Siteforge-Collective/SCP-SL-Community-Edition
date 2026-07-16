using System;
using CustomRendering;
using UnityEngine;

public class Outside : MonoBehaviour
{
    public static Outside Singleton;

    private FogSetting _fog;

    public bool LocalHostIsOutside
    {
        get
        {
            if (_fog == null) return false;
            return _fog._state;
        }
    }

    public static event Action<bool> OnSetOutside;

    private void Awake()
    {
        if (Singleton != null)
            Destroy(Singleton.gameObject);

        Singleton = this;
    }

    private void Start()
    {
        if (FogController.Singleton == null) return;

        _fog = FogController.Singleton.GetFogSetting(FogType.Outside);
    }

    private void Update()
    {
        if (_fog == null) return;

        bool inRoom = MainCameraController.TryGetCurrentRoom(out _);

        if (!inRoom)
        {
            if (!_fog._state)
                FogController.EnableFogType(FogType.Outside, 0f);
        }
        else
        {
            if (_fog._state)
                FogController.DisableFogType(FogType.Outside, 0f);
        }

        OnSetOutside?.Invoke(_fog._state);
    }

    public void RefreshCameras()
    {
        if (_fog != null)
            OnSetOutside?.Invoke(_fog._state);
    }

    private void SetOutside(bool setOutside)
    {
        if (_fog == null) return;

        if (setOutside)
        {
            if (!_fog._state)
                FogController.EnableFogType(FogType.Outside, 0f);
        }
        else
        {
            if (_fog._state)
                FogController.DisableFogType(FogType.Outside, 0f);
        }

        OnSetOutside?.Invoke(_fog._state);
    }
}