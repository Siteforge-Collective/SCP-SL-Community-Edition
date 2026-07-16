namespace PlayerRoles.PlayableScps.Subroutines
{
	public class SubroutineManagerModule : global::UnityEngine.MonoBehaviour
	{
		public global::PlayerRoles.PlayableScps.Subroutines.ScpSubroutineBase[] AllSubroutines;

		private void OnValidate()
		{
			AllSubroutines = GetComponentsInChildren<global::PlayerRoles.PlayableScps.Subroutines.ScpSubroutineBase>();
		}

		public bool TryGetSubroutine<T>(out T subroutine) where T : global::PlayerRoles.PlayableScps.Subroutines.ScpSubroutineBase
		{
			for (int i = 0; i < AllSubroutines.Length; i++)
			{
				if (AllSubroutines[i] is T val)
				{
					subroutine = val;
					return true;
				}
			}
			subroutine = null;
			return false;
		}

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			CustomNetworkManager.OnClientReady += delegate
			{
				global::Mirror.NetworkServer.ReplaceHandler(delegate(global::Mirror.NetworkConnection conn, global::PlayerRoles.PlayableScps.Subroutines.SubroutineMessage msg)
				{
					msg.ServerApplyTrigger(conn);
				});
				global::Mirror.NetworkClient.ReplaceHandler(delegate(global::Mirror.NetworkConnection conn, global::PlayerRoles.PlayableScps.Subroutines.SubroutineMessage msg)
				{
					msg.ClientApplyConfirmation();
				});
			};
		}
	}
}
