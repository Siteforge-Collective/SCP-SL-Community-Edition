using CustomRendering;
using UnityEngine;

public class ExplosionCameraShake : MonoBehaviour
{
	public float force;

	public float deductSpeed;

	private CameraShake _cameraShake;

	public static ExplosionCameraShake singleton;

	private void Update()
	{
	}

	private void Awake()
	{
	}

	private void OnDestroy()
	{
	}

	private void StopShake()
	{
	}

	public void Shake(float explosionForce)
	{
	}
}
