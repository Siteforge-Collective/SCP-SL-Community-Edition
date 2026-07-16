public class ExplosionCameraShake : global::UnityEngine.MonoBehaviour
{
	public float force;

	public float deductSpeed = 2f;

	private global::PostProcessing.CameraShake _cameraShake;

	public static ExplosionCameraShake singleton;

	private void Update()
	{
		force -= global::UnityEngine.Time.deltaTime / deductSpeed;
		force = global::UnityEngine.Mathf.Clamp01(force);
		_cameraShake.scanLineJitter.value = force;
		_cameraShake.horizontalShake.value = force;
		_cameraShake.colorDrift.value = force;
	}

	private void Awake()
	{
		singleton = this;
		_cameraShake = GetComponent<global::UnityEngine.Rendering.PostProcessing.PostProcessVolume>().profile.GetSetting<global::PostProcessing.CameraShake>();
	}

	public void Shake(float explosionForce)
	{
		if (explosionForce > force)
		{
			force = explosionForce;
		}
	}
}
