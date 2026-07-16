using System;
using System.Collections.Generic;
using Decals;
using UnityEngine;

namespace InventorySystem.Items.Firearms.Modules
{
	public class BuckshotHitreg : StandardHitregBase
	{
		[Serializable]
		public struct BuckshotSettings
		{
			public Vector2[] PredefinedPellets;

			public int MaxHits;

			public float Randomness;

			public float OverallScale;
		}

		private readonly struct ShotgunHit
		{
			public readonly IDestructible Target;

			public readonly float Damage;

			public readonly Ray RcRay;

			public readonly RaycastHit RcResult;

			public ShotgunHit(IDestructible dest, float damage, Ray ray, RaycastHit hit)
			{
				Target = null;
				Damage = 0f;
				RcRay = default(Ray);
				RcResult = default(RaycastHit);
			}
		}

		public const float TotalInaccuracyScale = 0.4f;

		private static readonly List<ShotgunHit> Hits;

		private readonly Func<BuckshotSettings> _buckshotSettingsProvider;

		protected override Firearm Firearm { get; set; }

		protected override ReferenceHub Hub { get; set; }

		protected override DecalPoolType BulletHoleDecal => default(DecalPoolType);

		public float BuckshotScale => 0f;

		private Vector2 GenerateRandomPelletDirection => default(Vector2);

		private float BuckshotRandomness => 0f;

		private int LastFiredAmount => 0;

		private BuckshotSettings CurBuckshotSettings => default(BuckshotSettings);

		public BuckshotHitreg(Firearm firearm, ReferenceHub hub, Func<BuckshotSettings> buckshotSettingsProvider)
		{
		}

		protected override void ServerPerformShot(Ray shootRay)
		{
		}

		private void ApplyHit(ShotgunHit hit, out float displayedDamage)
		{
			displayedDamage = default(float);
		}

		private bool CanShoot(IDestructible dest)
		{
			return false;
		}

		private void ShootPellet(Vector2 pelletSettings, Ray originalRay, Vector2 offsetVector)
		{
		}
	}
}
