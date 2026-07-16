using System;
using UnityEngine;

public class LiquidWobble : MonoBehaviour
{
    [SerializeField]
    public float _maxWobble = 0.03f;

    [SerializeField]
    public float _wobbleSpeed = 1f;

    [SerializeField]
    public float _recovery = 1f;

    public Renderer _renderer;

    public Vector3 _lastPosition;

    public Vector3 _velocity;

    public Vector3 _lastRotation;

    public Vector3 _angularVelocity;

    public float _wobbleAmountX;

    public float _wobbleAmountZ;

    public float _wobbleAmountToAddX;

    public float _wobbleAmountToAddZ;

    public float _pulse;

    public float _time = 0.5f;

    public void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void Update()
    {
        _time += Time.deltaTime;
        _wobbleAmountToAddX = Mathf.Lerp(_wobbleAmountToAddX, 0f, Time.deltaTime * _recovery);
        _wobbleAmountToAddZ = Mathf.Lerp(_wobbleAmountToAddZ, 0f, Time.deltaTime * _recovery);
        _pulse = (float)Math.PI * 2f * _wobbleSpeed;
        _wobbleAmountX = _wobbleAmountToAddX * Mathf.Sin(_pulse * _time);
        _wobbleAmountZ = _wobbleAmountToAddZ * Mathf.Sin(_pulse * _time);
        _renderer.material.SetFloat("_WobbleZ", _wobbleAmountX);
        _renderer.material.SetFloat("_WobbleX", _wobbleAmountZ);
        _velocity = (_lastPosition - base.transform.position) / Time.deltaTime;
        _angularVelocity = base.transform.rotation.eulerAngles - _lastRotation;
        _wobbleAmountToAddX += Mathf.Clamp((_velocity.x + _angularVelocity.z * 0.2f) * _maxWobble, 0f - _maxWobble, _maxWobble);
        _wobbleAmountToAddZ += Mathf.Clamp((_velocity.z + _angularVelocity.x * 0.2f) * _maxWobble, 0f - _maxWobble, _maxWobble);
        _lastPosition = base.transform.position;
        _lastRotation = base.transform.rotation.eulerAngles;
    }
}
