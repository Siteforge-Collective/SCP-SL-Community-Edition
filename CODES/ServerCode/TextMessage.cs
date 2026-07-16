public class TextMessage : global::UnityEngine.MonoBehaviour
{
	public float spacing = 15.5f;

	public float xOffset = 3f;

	public float lerpSpeed = 3f;

	public float position;

	public float remainingLife;

	private global::UnityEngine.Vector3 GetPosition()
	{
		return new global::UnityEngine.Vector3(xOffset, spacing * position, 0f);
	}

	private void Start()
	{
	}

	private void Update()
	{
		remainingLife -= global::UnityEngine.Time.deltaTime;
	}
}
