public class ContentFitter : global::UnityEngine.MonoBehaviour
{
	public bool continuousUpdate;

	public global::UnityEngine.RectTransform targetTransform;

	private global::System.Collections.Generic.List<global::UnityEngine.RectTransform> transforms = new global::System.Collections.Generic.List<global::UnityEngine.RectTransform>();

	private void LateUpdate()
	{
		if (continuousUpdate)
		{
			continuousUpdate = false;
			Fit();
		}
	}

	public void Fit()
	{
		transforms.Clear();
		global::UnityEngine.RectTransform[] componentsInChildren = GetComponentsInChildren<global::UnityEngine.RectTransform>();
		foreach (global::UnityEngine.RectTransform rectTransform in componentsInChildren)
		{
			if (rectTransform != GetComponent<global::UnityEngine.RectTransform>())
			{
				transforms.Add(rectTransform);
			}
		}
		global::UnityEngine.Vector2 vector = new global::UnityEngine.Vector2(1E+09f, -1E+09f);
		global::UnityEngine.Vector2 vector2 = new global::UnityEngine.Vector2(-1E+09f, 1E+09f);
		foreach (global::UnityEngine.RectTransform transform in transforms)
		{
			global::UnityEngine.Vector2 vector3 = new global::UnityEngine.Vector2(transform.localPosition.x - transform.sizeDelta.x * transform.pivot.x, transform.localPosition.y + transform.sizeDelta.y * transform.pivot.y);
			global::UnityEngine.Vector2 vector4 = new global::UnityEngine.Vector2(transform.localPosition.x + transform.sizeDelta.x * (1f - transform.pivot.x), transform.localPosition.y - transform.sizeDelta.y * (1f - transform.pivot.y));
			if (vector3.x < vector.x)
			{
				vector.x = vector3.x;
			}
			if (vector3.y > vector.y)
			{
				vector.y = vector3.y;
			}
			if (vector4.y < vector2.y)
			{
				vector2.y = vector4.y;
			}
			if (vector4.x > vector2.x)
			{
				vector2.x = vector4.x;
			}
		}
		global::UnityEngine.Vector2 sizeDelta = new global::UnityEngine.Vector2(global::UnityEngine.Mathf.Abs(vector.x - vector2.x), global::UnityEngine.Mathf.Abs(vector.y - vector2.y));
		targetTransform.localPosition = vector;
		targetTransform.sizeDelta = sizeDelta;
	}
}
