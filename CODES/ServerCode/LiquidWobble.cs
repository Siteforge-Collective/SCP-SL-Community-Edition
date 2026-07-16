public class LiquidWobble : global::UnityEngine.MonoBehaviour
{
	[global::UnityEngine.SerializeField]
	private float _maxWobble = 0.03f;

	[global::UnityEngine.SerializeField]
	private float _wobbleSpeed = 1f;

	[global::UnityEngine.SerializeField]
	private float _recovery = 1f;

	private global::UnityEngine.Renderer _renderer;

	private global::UnityEngine.Vector3 _lastPosition;

	private global::UnityEngine.Vector3 _velocity;

	private global::UnityEngine.Vector3 _lastRotation;

	private global::UnityEngine.Vector3 _angularVelocity;

	private float _wobbleAmountX;

	private float _wobbleAmountZ;

	private float _wobbleAmountToAddX;

	private float _wobbleAmountToAddZ;

	private float _pulse;

	private float _time = 0.5f;

	private void Start()
	{
		_renderer = GetComponent<global::UnityEngine.Renderer>();
	}

	private void Update()
	{
		_time += global::UnityEngine.Time.deltaTime;
		_wobbleAmountToAddX = global::UnityEngine.Mathf.Lerp(_wobbleAmountToAddX, 0f, global::UnityEngine.Time.deltaTime * _recovery);
		_wobbleAmountToAddZ = global::UnityEngine.Mathf.Lerp(_wobbleAmountToAddZ, 0f, global::UnityEngine.Time.deltaTime * _recovery);
		_pulse = (float)global::System.Math.PI * 2f * _wobbleSpeed;
		_wobbleAmountX = _wobbleAmountToAddX * global::UnityEngine.Mathf.Sin(_pulse * _time);
		_wobbleAmountZ = _wobbleAmountToAddZ * global::UnityEngine.Mathf.Sin(_pulse * _time);
		_renderer.material.SetFloat("_WobbleZ", _wobbleAmountX);
		_renderer.material.SetFloat("_WobbleX", _wobbleAmountZ);
		_velocity = (_lastPosition - base.transform.position) / global::UnityEngine.Time.deltaTime;
		_angularVelocity = base.transform.rotation.eulerAngles - _lastRotation;
		_wobbleAmountToAddX += global::UnityEngine.Mathf.Clamp((_velocity.x + _angularVelocity.z * 0.2f) * _maxWobble, 0f - _maxWobble, _maxWobble);
		_wobbleAmountToAddZ += global::UnityEngine.Mathf.Clamp((_velocity.z + _angularVelocity.x * 0.2f) * _maxWobble, 0f - _maxWobble, _maxWobble);
		_lastPosition = base.transform.position;
		_lastRotation = base.transform.rotation.eulerAngles;
	}
}
