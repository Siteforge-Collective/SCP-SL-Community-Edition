namespace InventorySystem.Items.ThrowableProjectiles
{
	public interface IExplosionTrigger
	{
		void OnExplosionDetected(global::Footprinting.Footprint attacker, global::UnityEngine.Vector3 source, float range);
	}
}
