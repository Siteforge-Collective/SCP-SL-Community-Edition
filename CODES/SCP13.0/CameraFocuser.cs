using UnityEngine;

public class CameraFocuser : MonoBehaviour
{
	public Transform lookTarget;

	public float targetFovScale;

	public float minimumAngle;

	private void OnTriggerStay(Collider other)
	{
	}
}
