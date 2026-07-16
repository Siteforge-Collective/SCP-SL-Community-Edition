public class WeaponShootAnimation : global::UnityEngine.MonoBehaviour
{
	public float curPosition;

	public global::UnityEngine.Vector3 maxRecoilPos;

	public global::UnityEngine.Vector3 maxRecoilRot;

	public float backSpeed;

	public float backY_Speed;

	private float yOverride;

	private float curY;

	private void LateUpdate()
	{
		if ((double)curPosition > 0.03)
		{
			curPosition = global::UnityEngine.Mathf.Lerp(curPosition, 0f, global::UnityEngine.Time.deltaTime * backSpeed * curPosition);
		}
		else
		{
			curPosition -= global::UnityEngine.Time.deltaTime * 0.1f;
		}
		if (curPosition < 0f)
		{
			curPosition = 0f;
		}
		yOverride = global::UnityEngine.Mathf.Lerp(0f, yOverride, curPosition);
		curY = global::UnityEngine.Mathf.Lerp(curY, yOverride, global::UnityEngine.Time.deltaTime * backY_Speed * curPosition);
		base.transform.localPosition = global::UnityEngine.Vector3.Lerp(global::UnityEngine.Vector3.zero, maxRecoilPos, curPosition);
		base.transform.localRotation = global::UnityEngine.Quaternion.Lerp(global::UnityEngine.Quaternion.Euler(global::UnityEngine.Vector3.zero), global::UnityEngine.Quaternion.Euler(maxRecoilRot + global::UnityEngine.Vector3.up * curY), curPosition);
	}

	public void Recoil(float f)
	{
		curPosition = global::UnityEngine.Mathf.Clamp01(curPosition + f);
		yOverride = global::UnityEngine.Random.Range(-10f, 10f) * f;
	}
}
