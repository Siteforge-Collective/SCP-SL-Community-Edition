public class DecalDestroyer : global::UnityEngine.MonoBehaviour
{
	public float lifeTime = 5f;

	private global::System.Collections.IEnumerator Start()
	{
		yield return new global::UnityEngine.WaitForSeconds(lifeTime);
		global::UnityEngine.Object.Destroy(base.gameObject);
	}
}
