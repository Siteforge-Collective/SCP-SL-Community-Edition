namespace InventorySystem.Items
{
	public class SharedHandsController : global::UnityEngine.MonoBehaviour
	{
		public static global::InventorySystem.Items.SharedHandsController Singleton { get; private set; }

		private void Awake()
		{
			Singleton = this;
		}
	}
}
