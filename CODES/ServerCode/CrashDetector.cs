public class CrashDetector : global::UnityEngine.MonoBehaviour
{
	public static CrashDetector singleton;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.GameObject image;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.UI.Button button;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.UI.Text text;

	private void Awake()
	{
		if (image == null)
		{
			global::UnityEngine.Object.Destroy(this);
			return;
		}
		singleton = this;
		base.gameObject.SetActive(Show());
	}

	public bool Show()
	{
		return false;
	}
}
