using Decals;
using InventorySystem.Items.ThrowableProjectiles;
using Mirror;
using UnityEngine;

namespace InventorySystem.Items.Firearms.Modules
{
	public class DisruptorHitreg : StandardHitregBase
	{
		public struct DisruptorHitMessage : NetworkMessage
		{
			public Vector3 Position;

			public LowPrecisionQuaternion Rotation;
		}

		private readonly ExplosionGrenade _explosionSettings;

		private const float ExplosionThrowback = 0.1f;

		protected override Firearm Firearm { get; set; }

		protected override ReferenceHub Hub { get; set; }

		protected override DecalPoolType BulletHoleDecal => default(DecalPoolType);

		public DisruptorHitreg(Firearm firearm, ReferenceHub hub, ExplosionGrenade explosionSettings)
		{
		}

		protected override void ServerPerformShot(Ray ray)
		{
		}

		private void CreateExplosion(Vector3 hitPoint)
		{
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static void ProcessHitMessage(DisruptorHitMessage msg)
		{
		}
	}
}
