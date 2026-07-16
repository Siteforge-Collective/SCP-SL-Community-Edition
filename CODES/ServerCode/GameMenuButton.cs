public class GameMenuButton : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Vector3 normalPos;

	public global::UnityEngine.Vector3 focusedPos;

	public global::UnityEngine.AnimationCurve anim;

	private bool isFocused;

	private float status;

	private global::UnityEngine.RectTransform rectTransform;

	private void Start()
	{
		rectTransform = GetComponent<global::UnityEngine.RectTransform>();
	}

	public void Focus(bool b)
	{
		isFocused = b;
	}

	private void Update()
	{
		status += global::UnityEngine.Time.deltaTime * (float)(isFocused ? 1 : (-1));
		status = global::UnityEngine.Mathf.Clamp01(status);
		global::UnityEngine.Vector3 vector = focusedPos - normalPos;
		rectTransform.localPosition = normalPos + vector * anim.Evaluate(status);
	}
}
