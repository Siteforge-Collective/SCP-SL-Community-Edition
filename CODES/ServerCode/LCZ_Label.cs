public class LCZ_Label : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.MeshRenderer chRend;

	public global::UnityEngine.MeshRenderer numRend;

	public void Refresh(global::UnityEngine.Material ch, global::UnityEngine.Material num)
	{
		chRend.sharedMaterial = ch;
		numRend.sharedMaterial = num;
	}
}
