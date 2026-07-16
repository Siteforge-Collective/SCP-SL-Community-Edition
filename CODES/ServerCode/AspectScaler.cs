public class AspectScaler : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.UI.CanvasScaler Scaler;

	private void Update()
	{
		float num = (float)global::UnityEngine.Screen.width / (float)global::UnityEngine.Screen.height;
		if (num > 1.8f)
		{
			Scaler.matchWidthOrHeight = 1f;
		}
		if (num < 1.65f)
		{
			Scaler.matchWidthOrHeight = 0f;
		}
	}
}
