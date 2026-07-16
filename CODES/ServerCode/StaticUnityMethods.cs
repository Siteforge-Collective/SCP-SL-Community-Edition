public class StaticUnityMethods : global::UnityEngine.MonoBehaviour
{
	public static bool IsPlaying
	{
		get
		{
			if (ReferenceHub.TryGetLocalHub(out var _))
			{
				if (!global::Mirror.NetworkClient.active)
				{
					return global::Mirror.NetworkServer.active;
				}
				return true;
			}
			return false;
		}
	}

	public static event global::System.Action OnUpdate;

	public static event global::System.Action OnLateUpdate;

	public static event global::System.Action OnFixedUpdate;

	private void Update()
	{
		StaticUnityMethods.OnUpdate?.Invoke();
	}

	private void FixedUpdate()
	{
		StaticUnityMethods.OnFixedUpdate?.Invoke();
	}

	private void LateUpdate()
	{
		StaticUnityMethods.OnLateUpdate?.Invoke();
	}
}
