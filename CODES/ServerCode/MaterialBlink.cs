public class MaterialBlink : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Material materal;

	public global::UnityEngine.Color lowestColor = global::UnityEngine.Color.white;

	public global::UnityEngine.Color highestColor = global::UnityEngine.Color.white;

	public float speed = 1f;

	public float colorMultiplier = 1f;

	private float time;

	private void Update()
	{
		time += global::UnityEngine.Time.deltaTime * speed;
		if (time > 1f)
		{
			time -= 1f;
		}
		materal.SetColor("_EmissionColor", global::UnityEngine.Color.Lerp(lowestColor, highestColor, global::UnityEngine.Mathf.Abs(global::UnityEngine.Mathf.Lerp(-1f, 1f, time))) * colorMultiplier);
	}

	private void OnDisable()
	{
		materal.SetColor("_EmissionColor", highestColor);
	}
}
