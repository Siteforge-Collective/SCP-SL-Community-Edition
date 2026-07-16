public class CameraFocuser : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Transform lookTarget;

	public float targetFovScale = 1f;

	public float minimumAngle;

	private void OnTriggerStay(global::UnityEngine.Collider other)
	{
		ReferenceHub hub = ReferenceHub.GetHub(other.transform.root.gameObject);
		if (hub != null && hub.characterClassManager.isLocalPlayer)
		{
			base.transform.LookAt(lookTarget);
			global::UnityEngine.Mathf.Clamp(global::UnityEngine.Quaternion.Angle(hub.PlayerCameraReference.rotation, base.transform.rotation), minimumAngle, 70f);
		}
	}
}
