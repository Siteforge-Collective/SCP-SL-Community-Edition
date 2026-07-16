using Decals;
using UnityEngine;

namespace InventorySystem.Items.Firearms.Modules
{
	public class SingleBulletHitreg : StandardHitregBase
	{
		private readonly bool _usesRecoilPattern;

		public readonly FirearmRecoilPattern RecoilPattern;

		protected override Firearm Firearm { get; set; }

		protected override ReferenceHub Hub { get; set; }

		protected override DecalPoolType BulletHoleDecal => default(DecalPoolType);

		public SingleBulletHitreg(Firearm firearm, ReferenceHub hub, FirearmRecoilPattern recoilPattern = null)
		{
		}

		protected virtual Ray ServerRandomizeRay(Ray ray)
		{
			return default(Ray);
		}

		protected virtual void ServerProcessRaycastHit(Ray ray, RaycastHit hit)
		{
		}

		protected override void ServerPerformShot(Ray ray)
		{
		}

		private bool CheckInaccurateFriendlyFire(IDestructible target)
		{
			return false;
		}
	}
}
