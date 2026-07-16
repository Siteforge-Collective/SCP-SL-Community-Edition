public class CreditText : global::UnityEngine.MonoBehaviour
{
	public bool move;

	public float speed;

	private void FixedUpdate()
	{
		if (move)
		{
			base.transform.Translate(global::UnityEngine.Vector3.up * speed);
		}
	}
}
