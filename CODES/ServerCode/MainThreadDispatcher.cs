public class MainThreadDispatcher : global::UnityEngine.MonoBehaviour
{
	public enum DispatchTime
	{
		Update = 0,
		LateUpdate = 1,
		FixedUpdate = 2
	}

	private static readonly global::NorthwoodLib.ActionDispatcher UpdateDispatcher = new global::NorthwoodLib.ActionDispatcher();

	private static readonly global::NorthwoodLib.ActionDispatcher LateUpdateDispatcher = new global::NorthwoodLib.ActionDispatcher();

	private static readonly global::NorthwoodLib.ActionDispatcher FixedUpdateDispatcher = new global::NorthwoodLib.ActionDispatcher();

	public static void Dispatch(global::System.Action action, MainThreadDispatcher.DispatchTime dispatchTime = MainThreadDispatcher.DispatchTime.Update)
	{
		switch (dispatchTime)
		{
		case MainThreadDispatcher.DispatchTime.Update:
			UpdateDispatcher.Dispatch(action);
			break;
		case MainThreadDispatcher.DispatchTime.LateUpdate:
			LateUpdateDispatcher.Dispatch(action);
			break;
		case MainThreadDispatcher.DispatchTime.FixedUpdate:
			FixedUpdateDispatcher.Dispatch(action);
			break;
		}
	}

	private void Update()
	{
		UpdateDispatcher.Invoke();
	}

	private void LateUpdate()
	{
		LateUpdateDispatcher.Invoke();
	}

	private void FixedUpdate()
	{
		LateUpdateDispatcher.Invoke();
	}
}
