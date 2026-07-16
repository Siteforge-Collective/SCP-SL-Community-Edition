namespace PlayerRoles.FirstPersonControl.Spawnpoints
{
	public class RoleSpawnpointVisualizer : global::UnityEngine.MonoBehaviour
	{
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Color _gizmosColor = global::UnityEngine.Color.white;

		[global::UnityEngine.SerializeField]
		private int _numberOfTests = 64;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.RoleTypeId _role;

		private void OnDrawGizmosSelected()
		{
			global::UnityEngine.Gizmos.color = _gizmosColor;
			if (!global::PlayerRoles.PlayerRoleLoader.TryGetRoleTemplate<global::PlayerRoles.PlayerRoleBase>(_role, out var result) || !(result is global::PlayerRoles.FirstPersonControl.IFpcRole fpcRole))
			{
				return;
			}
			global::PlayerRoles.FirstPersonControl.Spawnpoints.ISpawnpointHandler spawnpointHandler = fpcRole.SpawnpointHandler;
			if (spawnpointHandler == null)
			{
				return;
			}
			for (int i = 0; i < _numberOfTests; i++)
			{
				if (spawnpointHandler.TryGetSpawnpoint(out var position, out var _))
				{
					global::UnityEngine.Gizmos.DrawWireSphere(position, 0.2f);
				}
			}
		}
	}
}
