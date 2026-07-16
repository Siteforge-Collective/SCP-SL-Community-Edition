public class ScpInterfaces : global::UnityEngine.MonoBehaviour
{
	public static ScpInterfaces singleton;

	public global::UnityEngine.GameObject Scp106_eq;

	public global::UnityEngine.GameObject Scp049_eq;

	public global::UnityEngine.GameObject Scp096_eq;

	public global::UnityEngine.GameObject Scp173InterfaceObj;

	public static int remTargs;

	private void Awake()
	{
		singleton = this;
	}
}
