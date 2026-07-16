namespace Interactables.Interobjects
{
	public class SqueakInteraction : global::Interactables.Interobjects.PopupInterobject, IDestructible
	{
		private SqueakSpawner _spawner;

		public uint NetworkId => _spawner.netId;

		public global::UnityEngine.Vector3 CenterOfMass => global::UnityEngine.Vector3.zero;

		public bool Damage(float damage, global::PlayerStatsSystem.DamageHandlerBase handler, global::UnityEngine.Vector3 exactHitPos)
		{
			if (!(handler is global::PlayerStatsSystem.AttackerDamageHandler attackerDamageHandler) || attackerDamageHandler.Attacker.Hub == null)
			{
				return false;
			}
			_spawner.TargetHitMouse(attackerDamageHandler.Attacker.Hub.networkIdentity.connectionToClient);
			return true;
		}

		private void Awake()
		{
			_spawner = GetComponentInParent<SqueakSpawner>();
		}
	}
}
