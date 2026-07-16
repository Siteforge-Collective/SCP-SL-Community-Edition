public class SpectatorHidden : global::UnityEngine.MonoBehaviour, global::GameObjectPools.IPoolResettable
{
	public global::UnityEngine.Renderer AttachedRenderer { get; private set; }

	private void Awake()
	{
		AttachedRenderer = GetComponent<global::UnityEngine.Renderer>();
	}

	public void ResetObject()
	{
		AttachedRenderer.shadowCastingMode = global::UnityEngine.Rendering.ShadowCastingMode.On;
	}
}
