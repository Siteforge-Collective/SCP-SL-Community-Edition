using UnityEngine;

namespace InventorySystem.Items.Firearms.Modules
{
	public class MultiShotHitreg : SingleBulletHitreg
	{
		private readonly Vector3[] _offsets;

		public MultiShotHitreg(Firearm fa, ReferenceHub hub, FirearmRecoilPattern pattern, Vector3[] offsets)
			: base(null, null)
		{
		}

		protected override void ServerPerformShot(Ray ray)
		{
		}

		private void Fire(Ray ray, Vector3 offset)
		{
		}
	}
}
