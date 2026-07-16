using UnityEngine;
using UnityEngine.Serialization;
using System;

public class RoomLight : MonoBehaviour
{
    public static readonly Color DefaultWarheadColor = new Color(1f, 0f, 0f, 1f);
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissiveColor");

    [FormerlySerializedAs("materialId")]
    [SerializeField]
    private int _materialId;

    [SerializeField]
    private Renderer[] _renderers;

    private Material _copy;
    private int _rendererCount;
    private bool _isDirty;
    private bool _warheadInProgress;
    private bool _overrideColorSet;
    private bool _blackoutAnimActive;
    private bool _targetBlackout;
    private float _blackoutAnimProgress;
    private Color _overrideColor;
    private Color _initialLightColor;
    private Color _initialMaterialColor;

    internal bool HasLight;

    internal Light LightSource { get; private set; }

    private void Awake()
    {
        LightSource = GetComponentInChildren<Light>();
        HasLight = LightSource != null;

        _rendererCount = _renderers.Length;

        if (_rendererCount > 0 && _renderers[0] != null)
        {
            Material sharedMat = _renderers[0].sharedMaterials[_materialId];
            _copy = new Material(sharedMat);

            for (int i = 0; i < _rendererCount; i++)
            {
                if (_renderers[i] == null) continue;

                Material[] materials = _renderers[i].sharedMaterials;
                if (_materialId < materials.Length)
                {
                    materials[_materialId] = _copy;
                    _renderers[i].materials = materials;
                }
            }
            _initialMaterialColor = _copy.GetColor(EmissionColor);
        }

        if (HasLight)
        {
            _initialLightColor = LightSource.color;
        }
    }

    private void OnEnable()
    {
        _warheadInProgress = AlphaWarheadController.InProgress;
        _isDirty = true;
        AlphaWarheadController.OnProgressChanged += UpdateWarhead;
    }

    private void OnDisable()
    {
        AlphaWarheadController.OnProgressChanged -= UpdateWarhead;
    }

    private void UpdateWarhead(bool val)
    {
        _warheadInProgress = val;
        _isDirty = true;
    }

    private void Update()
    {
        if (!_isDirty) return;

        Color targetColor;
        if (_overrideColorSet)
        {
            targetColor = _overrideColor;
        }
        else
        {
            targetColor = _warheadInProgress ? DefaultWarheadColor : _initialLightColor;
        }

        float intensity = 1f;

        if (_blackoutAnimActive)
        {
            if (_targetBlackout)
            {
                intensity = FlickerBlackout(_blackoutAnimProgress);
            }
            else
            {
                intensity = FlickerReEnabling(_blackoutAnimProgress);
            }

            _blackoutAnimProgress += Time.deltaTime;

            if (_blackoutAnimProgress >= 1f)
            {
                _blackoutAnimActive = false;
            }
        }
        else
        {
            intensity = _targetBlackout ? 0f : 1f;
        }

        SetColors(targetColor, targetColor, intensity);
        _isDirty = _blackoutAnimActive;
    }

    private float FlickerBlackout(float f)
    {
        return Mathf.Clamp01(Mathf.Sin(f * 3.1415927f * 20f));
    }

    private float FlickerReEnabling(float f)
    {
        return f;
    }

    private void SetColors(Color lightColor, Color matColor, float intensity)
    {
        if (_rendererCount > 0 && _copy != null)
        {
            _copy.SetColor(EmissionColor, matColor * intensity);
        }

        if (HasLight)
        {
            LightSource.color = lightColor;
            LightSource.intensity = intensity * 10f;
        }
    }

    internal void SetOverrideColor(Color overrideColor)
    {
        _isDirty = true;
        _overrideColorSet = true;
        _overrideColor = overrideColor;
    }

    internal void ResetOverrideColor()
    {
        _isDirty = true;
        _overrideColorSet = false;
    }

    internal void SetBlackout(bool state)
    {
        _isDirty = true;
        _targetBlackout = state;
        _blackoutAnimActive = true;
        _blackoutAnimProgress = 0f;
    }
}