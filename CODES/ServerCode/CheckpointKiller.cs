public class CheckpointKiller : global::UnityEngine.MonoBehaviour
{
	private const float MinAliveDuration = 1f;

	private void OnTriggerEnter(global::UnityEngine.Collider other)
	{
		if (global::Mirror.NetworkServer.active && ReferenceHub.TryGetHub(other.transform.root.gameObject, out var hub) && !(hub.roleManager.CurrentRole.ActiveTime < 1f))
		{
			hub.playerStats.DealDamage(new global::PlayerStatsSystem.UniversalDamageHandler(-1f, global::PlayerStatsSystem.DeathTranslations.Crushed));
		}
	}
}
