public class FastRoundRestartController : global::Mirror.NetworkBehaviour
{
	private void Start()
	{
		global::UnityEngine.Object.Destroy(this);
	}

	private void MirrorProcessed()
	{
	}
}
