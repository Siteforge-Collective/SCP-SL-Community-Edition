namespace InventorySystem.Items.Firearms.Modules
{
	public class MultiShotHitreg : global::InventorySystem.Items.Firearms.Modules.SingleBulletHitreg
	{
		private readonly global::UnityEngine.Vector3[] _offsets;

		public MultiShotHitreg(global::InventorySystem.Items.Firearms.Firearm fa, ReferenceHub hub, global::InventorySystem.Items.Firearms.FirearmRecoilPattern pattern, global::UnityEngine.Vector3[] offsets)
			: base(fa, hub, pattern)
		{
			_offsets = offsets;
		}

		protected override void ServerPerformShot(global::UnityEngine.Ray ray)
		{
			if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerShotWeapon, Hub, Firearm))
			{
				ray = ServerRandomizeRay(ray);
				global::UnityEngine.Quaternion rot = Hub.PlayerCameraReference.rotation;
				_offsets.ForEach(delegate(global::UnityEngine.Vector3 x)
				{
					Fire(ray, rot * x);
				});
			}
		}

		private void Fire(global::UnityEngine.Ray ray, global::UnityEngine.Vector3 offset)
		{
			ray = new global::UnityEngine.Ray(ray.origin + offset, ray.direction);
			if (global::UnityEngine.Physics.Raycast(ray, out var hitInfo, Firearm.BaseStats.MaxDistance(), global::InventorySystem.Items.Firearms.Modules.StandardHitregBase.HitregMask))
			{
				ServerProcessRaycastHit(ray, hitInfo);
			}
		}
	}
}
