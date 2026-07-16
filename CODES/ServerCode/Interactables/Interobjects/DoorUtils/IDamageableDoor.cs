namespace Interactables.Interobjects.DoorUtils
{
	public interface IDamageableDoor
	{
		bool IsDestroyed { get; set; }

		bool ServerDamage(float hp, global::Interactables.Interobjects.DoorUtils.DoorDamageType type);

		void ClientDestroyEffects();

		float GetHealthPercent();
	}
}
