namespace InventorySystem.Items.Firearms.Modules
{
	public class BuckshotHitreg : global::InventorySystem.Items.Firearms.Modules.StandardHitregBase
	{
		[global::System.Serializable]
		public struct BuckshotSettings
		{
			public global::UnityEngine.Vector2[] PredefinedPellets;

			public int MaxHits;

			public float Randomness;

			public float OverallScale;
		}

		private struct ShotgunHit
		{
			public readonly float Damage;

			public readonly global::UnityEngine.Ray RcRay;

			public readonly global::UnityEngine.RaycastHit RcResult;

			public ShotgunHit(float damage, global::UnityEngine.Ray ray, global::UnityEngine.RaycastHit hit)
			{
				Damage = damage;
				RcRay = ray;
				RcResult = hit;
			}
		}

		private readonly global::InventorySystem.Items.Firearms.Modules.BuckshotHitreg.BuckshotSettings _buckshotSettings;

		public const float TotalInaccuracyScale = 0.4f;

		private static readonly global::System.Collections.Generic.Dictionary<IDestructible, global::System.Collections.Generic.List<global::InventorySystem.Items.Firearms.Modules.BuckshotHitreg.ShotgunHit>> Hits = new global::System.Collections.Generic.Dictionary<IDestructible, global::System.Collections.Generic.List<global::InventorySystem.Items.Firearms.Modules.BuckshotHitreg.ShotgunHit>>();

		protected override global::InventorySystem.Items.Firearms.Firearm Firearm { get; set; }

		protected override ReferenceHub Hub { get; set; }

		public float BuckshotScale => _buckshotSettings.OverallScale * global::InventorySystem.Items.Firearms.Attachments.AttachmentsUtils.AttachmentsValue(Firearm, global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.SpreadMultiplier);

		private global::UnityEngine.Vector2 GenerateRandomPelletDirection => (new global::UnityEngine.Vector2(global::UnityEngine.Random.value, global::UnityEngine.Random.value) - global::UnityEngine.Vector2.one / 2f).normalized * global::UnityEngine.Random.value;

		private float BuckshotRandomness => 1f - global::UnityEngine.Mathf.Clamp01((1f - _buckshotSettings.Randomness) * global::InventorySystem.Items.Firearms.Attachments.AttachmentsUtils.AttachmentsValue(Firearm, global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.SpreadPredictability));

		private int LastFiredAmount => (Firearm.ActionModule as global::InventorySystem.Items.Firearms.Modules.PumpAction).LastFiredAmount;

		public BuckshotHitreg(global::InventorySystem.Items.Firearms.Firearm firearm, ReferenceHub hub, global::InventorySystem.Items.Firearms.Modules.BuckshotHitreg.BuckshotSettings buckshotSettings)
		{
			Firearm = firearm;
			Hub = hub;
			_buckshotSettings = buckshotSettings;
		}

		protected override void ServerPerformShot(global::UnityEngine.Ray shootRay)
		{
			bool isGrounded;
			float movementSpeed;
			if (Hub.roleManager.CurrentRole is global::PlayerRoles.FirstPersonControl.IFpcRole fpcRole)
			{
				isGrounded = fpcRole.FpcModule.IsGrounded;
				movementSpeed = fpcRole.FpcModule.Motor.Velocity.magnitude;
			}
			else
			{
				isGrounded = true;
				movementSpeed = 0f;
			}
			float num = Firearm.BaseStats.GetInaccuracy(Firearm, Firearm.AdsModule.ServerAds, movementSpeed, isGrounded) * 0.4f;
			global::UnityEngine.Vector2 offsetVector = (new global::UnityEngine.Vector2(global::UnityEngine.Random.value, global::UnityEngine.Random.value) - global::UnityEngine.Vector2.one / 2f).normalized * global::UnityEngine.Random.value * num;
			Hits.Clear();
			for (int i = 0; i < LastFiredAmount; i++)
			{
				global::UnityEngine.Vector2[] predefinedPellets = _buckshotSettings.PredefinedPellets;
				foreach (global::UnityEngine.Vector2 pelletSettings in predefinedPellets)
				{
					ShootPellet(pelletSettings, shootRay, offsetVector);
				}
			}
			float num2 = 0f;
			foreach (global::System.Collections.Generic.KeyValuePair<IDestructible, global::System.Collections.Generic.List<global::InventorySystem.Items.Firearms.Modules.BuckshotHitreg.ShotgunHit>> hit in Hits)
			{
				num2 += ApplyHits(hit.Key, hit.Value);
			}
			if (num2 > 0f)
			{
				Hitmarker.SendHitmarker(base.Conn, num2 / 50f + 0.5f);
			}
		}

		private float ApplyHits(IDestructible target, global::System.Collections.Generic.List<global::InventorySystem.Items.Firearms.Modules.BuckshotHitreg.ShotgunHit> hits)
		{
			float num = 0f;
			ReferenceHub hub;
			bool flag = !ReferenceHub.TryGetHubNetID(target.NetworkId, out hub) || !hub.playerEffectsController.GetEffect<global::CustomPlayerEffects.Invisible>().IsEnabled;
			foreach (global::InventorySystem.Items.Firearms.Modules.BuckshotHitreg.ShotgunHit hit in hits)
			{
				float damage = hit.Damage;
				if (target.Damage(damage, new global::PlayerStatsSystem.FirearmDamageHandler(Firearm, damage, useHumanMutlipliers: false), hit.RcResult.point))
				{
					PlaceBloodDecal(hit.RcRay, hit.RcResult, target);
					if (flag)
					{
						num += damage;
					}
				}
			}
			ShowHitIndicator(target.NetworkId, num, Hub.transform.position);
			return num;
		}

		private bool CanShoot(IDestructible dest)
		{
			if (!Hits.TryGetValue(dest, out var value))
			{
				Hits.Add(dest, new global::System.Collections.Generic.List<global::InventorySystem.Items.Firearms.Modules.BuckshotHitreg.ShotgunHit>());
				return true;
			}
			return value.Count < _buckshotSettings.MaxHits * LastFiredAmount;
		}

		private void ShootPellet(global::UnityEngine.Vector2 pelletSettings, global::UnityEngine.Ray originalRay, global::UnityEngine.Vector2 offsetVector)
		{
			if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerShotWeapon, Hub, Firearm))
			{
				return;
			}
			global::UnityEngine.Vector2 vector = global::UnityEngine.Vector2.Lerp(pelletSettings, GenerateRandomPelletDirection, BuckshotRandomness) * BuckshotScale;
			global::UnityEngine.Vector3 direction = originalRay.direction;
			direction = global::UnityEngine.Quaternion.AngleAxis(vector.x + offsetVector.x, Hub.PlayerCameraReference.up) * direction;
			direction = global::UnityEngine.Quaternion.AngleAxis(vector.y + offsetVector.y, Hub.PlayerCameraReference.right) * direction;
			global::UnityEngine.Ray ray = new global::UnityEngine.Ray(originalRay.origin, direction);
			if (global::UnityEngine.Physics.Raycast(ray, out var hitInfo, Firearm.BaseStats.MaxDistance(), global::InventorySystem.Items.Firearms.Modules.StandardHitregBase.HitregMask))
			{
				if (!hitInfo.collider.TryGetComponent<IDestructible>(out var component))
				{
					PlaceBulletholeDecal(ray, hitInfo);
				}
				else if (CanShoot(component))
				{
					float damage = Firearm.BaseStats.DamageAtDistance(Firearm, hitInfo.distance) / (float)_buckshotSettings.MaxHits;
					Hits[component].Add(new global::InventorySystem.Items.Firearms.Modules.BuckshotHitreg.ShotgunHit(damage, ray, hitInfo));
				}
			}
		}
	}
}
