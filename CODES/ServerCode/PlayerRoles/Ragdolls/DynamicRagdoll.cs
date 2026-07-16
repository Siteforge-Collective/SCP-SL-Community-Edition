namespace PlayerRoles.Ragdolls
{
	public class DynamicRagdoll : BasicRagdoll
	{
		public global::UnityEngine.Rigidbody[] LinkedRigidbodies;

		public global::PlayerRoles.Ragdolls.HitboxData[] Hitboxes;

		protected override void OnCleanup()
		{
			base.OnCleanup();
			global::UnityEngine.Rigidbody[] linkedRigidbodies = LinkedRigidbodies;
			for (int i = 0; i < linkedRigidbodies.Length; i++)
			{
				linkedRigidbodies[i].gameObject.SetActive(value: false);
			}
		}

		private void MirrorProcessed()
		{
		}
	}
}
