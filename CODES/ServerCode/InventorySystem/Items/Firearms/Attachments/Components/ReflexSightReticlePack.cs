namespace InventorySystem.Items.Firearms.Attachments.Components
{
	[global::UnityEngine.CreateAssetMenu(fileName = "New Sight Pack", menuName = "ScriptableObject/Firearms/Reflex Sight Pack")]
	public class ReflexSightReticlePack : global::UnityEngine.ScriptableObject
	{
		public global::UnityEngine.Texture[] Reticles;

		public global::UnityEngine.Texture this[int index] => Reticles[index];

		public int Length => Reticles.Length;
	}
}
