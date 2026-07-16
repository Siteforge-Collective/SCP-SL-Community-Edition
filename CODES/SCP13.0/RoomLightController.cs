using System.Collections.Generic;
using MapGeneration;
using Mirror;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class RoomLightController : NetworkBehaviour
{
    private List<RoomLight> _cachedLights;
    private List<HDProbe> _cachedReflectionProbes;
    private bool _cacheSet;

    public const float HeightRadius = 100f;
    private float _flickerDuration;

    [SyncVar(hook = nameof(LightsEnabledHook))]
    public bool LightsEnabled;

    [SyncVar(hook = nameof(OverrideColorHook))]
    public Color OverrideColor;

    public static readonly List<RoomLightController> Instances = new List<RoomLightController>();

    public List<RoomLight> LightsInRoom
    {
        get
        {
            PrepareCache();
            return _cachedLights;
        }
    }

    public List<HDProbe> ReflectionProbes
    {
        get
        {
            PrepareCache();
            return _cachedReflectionProbes;
        }
    }

    public RoomIdentifier Room { get; private set; }

    private void Start()
    {
        if (NetworkServer.active)
        {
            LightsEnabled = true;
        }
    }

    private void OnEnable()
    {
        Instances.Add(this);
    }

    private void OnDisable()
    {
        Instances.Remove(this);
    }

    private void PrepareCache()
    {
        if (_cacheSet) return;

        _cachedLights = new List<RoomLight>();
        _cachedReflectionProbes = new List<HDProbe>();

        Transform parent = transform.parent != null ? transform.parent : transform;

        foreach (RoomLight light in parent.GetComponentsInChildren<RoomLight>(true))
        {
            if (Mathf.Abs(light.transform.position.y - transform.position.y) <= 100f)
            {
                _cachedLights.Add(light);
            }
        }

        foreach (HDProbe probe in parent.GetComponentsInChildren<HDProbe>(true))
        {
            if (Mathf.Abs(probe.transform.position.y - transform.position.y) <= 100f)
            {
                if (probe.mode != ProbeSettings.Mode.Baked)
                {
                    _cachedReflectionProbes.Add(probe);
                }
            }
        }

        _cacheSet = true;
    }

    private void Update()
    {
        if (NetworkServer.active && _flickerDuration > 0f)
        {
            _flickerDuration -= Time.deltaTime;
            if (_flickerDuration <= 0f)
            {
                SetLights(true);
            }
        }
    }

    private void SetLights(bool state)
    {
        if (NetworkServer.active)
        {
            LightsEnabled = state;
        }

        SetProbeIntensity(state);
        PrepareCache();

        foreach (RoomLight light in _cachedLights)
        {
            if (light != null)
            {
                light.gameObject.SetActive(state);
            }
        }
    }

    private void SetProbeIntensity(bool state)
    {
        PrepareCache();
        float weight = state ? 1f : 0f;
        foreach (HDProbe probe in _cachedReflectionProbes)
        {
            if (probe != null)
            {
                probe.weight = weight;
            }
        }
    }

    private void LightsEnabledHook(bool oldValue, bool newValue)
    {
        SetLights(newValue);
    }

    private void OverrideColorHook(Color oldValue, Color newValue)
    {
        PrepareCache();
        foreach (RoomLight light in _cachedLights)
        {
            if (light != null)
            {
                light.SetOverrideColor(newValue);
            }
        }
    }

    [Server]
    public void ServerFlickerLights(float dur)
    {
        if (dur <= 0f)
        {
            _flickerDuration = 0f;
            SetLights(true);
        }
        else
        {
            _flickerDuration = dur;
            SetLights(false);
        }
    }

    [RuntimeInitializeOnLoadMethod]
    private static void Init()
    {
        SeedSynchronizer.OnMapGenerated += delegate
        {
            foreach (RoomLightController instance in Instances)
            {
                instance.Room = RoomIdUtils.RoomAtPosition(instance.transform.position);
            }
        };
    }

    public static bool IsInDarkenedRoom(Vector3 positionToCheck)
    {
        RoomIdentifier roomIdentifier = RoomIdUtils.RoomAtPosition(positionToCheck);
        if (roomIdentifier == null)
        {
            roomIdentifier = RoomIdUtils.RoomAtPositionRaycasts(positionToCheck);
        }

        foreach (RoomLightController instance in Instances)
        {
            if (!instance.LightsEnabled && instance.Room == roomIdentifier)
            {
                if (Mathf.Abs(instance.transform.position.y - positionToCheck.y) <= 100f)
                {
                    return true;
                }
            }
        }
        return false;
    }
}