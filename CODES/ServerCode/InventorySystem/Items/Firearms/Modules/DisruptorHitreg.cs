namespace InventorySystem.Items.Firearms.Modules
{
	public class DisruptorHitreg : global::InventorySystem.Items.Firearms.Modules.StandardHitregBase
	{
		public struct DisruptorHitMessage : global::Mirror.NetworkMessage
		{
			public global::UnityEngine.Vector3 Position;

			public LowPrecisionQuaternion Rotation;
		}

		private readonly global::InventorySystem.Items.ThrowableProjectiles.ExplosionGrenade _explosionSettings;

		private const float ExplosionThrowback = 0.1f;

		protected override global::InventorySystem.Items.Firearms.Firearm Firearm { get; set; }

		protected override ReferenceHub Hub { get; set; }

		public DisruptorHitreg(global::InventorySystem.Items.Firearms.Firearm firearm, ReferenceHub hub, global::InventorySystem.Items.ThrowableProjectiles.ExplosionGrenade explosionSettings)
		{
			Firearm = firearm;
			Hub = hub;
			_explosionSettings = explosionSettings;
		}

		protected override void ServerPerformShot(global::UnityEngine.Ray ray)
		{
			global::InventorySystem.Items.Firearms.FirearmBaseStats baseStats = Firearm.BaseStats;
			if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerShotWeapon, Hub, Firearm))
			{
				return;
			}
			bool isGrounded;
			global::UnityEngine.Vector3 vector;
			if (Hub.roleManager.CurrentRole is global::PlayerRoles.FirstPersonControl.IFpcRole fpcRole)
			{
				isGrounded = fpcRole.FpcModule.IsGrounded;
				vector = fpcRole.FpcModule.Motor.Velocity;
			}
			else
			{
				isGrounded = true;
				vector = global::UnityEngine.Vector3.zero;
			}
			global::UnityEngine.Vector3 vector2 = (new global::UnityEngine.Vector3(global::UnityEngine.Random.value, global::UnityEngine.Random.value, global::UnityEngine.Random.value) - global::UnityEngine.Vector3.one / 2f).normalized * global::UnityEngine.Random.value;
			float inaccuracy = baseStats.GetInaccuracy(Firearm, Firearm.AdsModule.ServerAds, vector.magnitude, isGrounded);
			ray.direction = global::UnityEngine.Quaternion.Euler(inaccuracy * vector2) * ray.direction;
			global::UnityEngine.LayerMask layerMask = global::UnityEngine.LayerMask.GetMask("Default", "Hitbox", "CCTV", "Door", "Locker", "Pickup");
			if (!global::UnityEngine.Physics.Raycast(ray, out var hitInfo, baseStats.MaxDistance(), layerMask))
			{
				return;
			}
			if (!hitInfo.collider.TryGetComponent<IDestructible>(out var component))
			{
				global::Utils.Networking.NetworkUtils.SendToAuthenticated(new global::InventorySystem.Items.Firearms.Modules.DisruptorHitreg.DisruptorHitMessage
				{
					Position = hitInfo.point + hitInfo.normal * 0.1f,
					Rotation = new LowPrecisionQuaternion(global::UnityEngine.Quaternion.LookRotation(-hitInfo.normal))
				});
				CreateExplosion(hitInfo.point);
				return;
			}
			float damage = baseStats.DamageAtDistance(Firearm, hitInfo.distance);
			if (component.Damage(damage, new global::PlayerStatsSystem.DisruptorDamageHandler(Firearm.Footprint, damage), hitInfo.point))
			{
				if (!ReferenceHub.TryGetHubNetID(component.NetworkId, out var hub) || !hub.playerEffectsController.GetEffect<global::CustomPlayerEffects.Invisible>().IsEnabled)
				{
					Hitmarker.SendHitmarker(base.Conn, 2f);
				}
				ShowHitIndicator(component.NetworkId, damage, ray.origin);
			}
			CreateExplosion(hitInfo.point);
		}

		private void CreateExplosion(global::UnityEngine.Vector3 hitPoint)
		{
			global::UnityEngine.Vector3 vector = (Firearm.Owner.PlayerCameraReference.position - hitPoint).normalized * 0.1f;
			global::InventorySystem.Items.ThrowableProjectiles.ExplosionGrenade.Explode(Firearm.Footprint, hitPoint + vector, _explosionSettings);
		}

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			CustomNetworkManager.OnClientReady += delegate
			{
				global::Mirror.NetworkClient.ReplaceHandler<global::InventorySystem.Items.Firearms.Modules.DisruptorHitreg.DisruptorHitMessage>(ProcessHitMessage);
			};
		}

		private static void ProcessHitMessage(global::InventorySystem.Items.Firearms.Modules.DisruptorHitreg.DisruptorHitMessage msg)
		{
		}
	}
}
