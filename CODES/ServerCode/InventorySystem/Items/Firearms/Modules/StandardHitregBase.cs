namespace InventorySystem.Items.Firearms.Modules
{
	public abstract class StandardHitregBase : global::InventorySystem.Items.Firearms.Modules.IHitregModule, global::InventorySystem.Items.Firearms.Modules.IFirearmModuleBase
	{
		public static readonly global::UnityEngine.LayerMask HitregMask = global::UnityEngine.LayerMask.GetMask("Default", "Hitbox", "Glass", "CCTV", "Door", "Locker");

		private const float MinDot = 0.5f;

		private const float MaxHeightDiff = 50f;

		public bool Standby => true;

		protected global::Mirror.NetworkConnection Conn => Firearm.OwnerInventory.connectionToClient;

		protected abstract global::InventorySystem.Items.Firearms.Firearm Firearm { get; set; }

		protected abstract ReferenceHub Hub { get; set; }

		public static bool DebugMode { get; internal set; }

		protected uint PrimaryTargetNetId { get; private set; }

		private void SetHitboxes(ReferenceHub target, bool state)
		{
			if (target.roleManager.CurrentRole is global::PlayerRoles.FirstPersonControl.IFpcRole fpcRole)
			{
				HitboxIdentity[] hitboxes = fpcRole.FpcModule.CharacterModelInstance.Hitboxes;
				for (int i = 0; i < hitboxes.Length; i++)
				{
					hitboxes[i].SetColliders(state);
				}
			}
		}

		protected void SendDebug(string msg)
		{
			Hub.gameConsoleTransmission.SendToClient("[HITREG DEBUG] " + msg, "gray");
		}

		public bool ClientCalculateHit(out global::InventorySystem.Items.Firearms.BasicMessages.ShotMessage message)
		{
			message = default(global::InventorySystem.Items.Firearms.BasicMessages.ShotMessage);
			global::UnityEngine.Vector3 forward = Hub.PlayerCameraReference.forward;
			global::UnityEngine.Vector3 position = Hub.PlayerCameraReference.position;
			float maxDistance = Firearm.BaseStats.MaxDistance();
			uint netId = Hub.inventory.netId;
			float num = 0.5f;
			IDestructible component = null;
			if (!global::UnityEngine.Physics.Raycast(position, forward, out var hitInfo, maxDistance, HitregMask) || !hitInfo.collider.TryGetComponent<IDestructible>(out component))
			{
				foreach (HitboxIdentity instance in HitboxIdentity.Instances)
				{
					if (((IDestructible)instance).NetworkId != netId && !(global::UnityEngine.Mathf.Abs(((IDestructible)instance).CenterOfMass.y - position.y) > 50f))
					{
						float num2 = global::UnityEngine.Vector3.Dot(forward, (((IDestructible)instance).CenterOfMass - position).normalized);
						if (!(num2 < num) && (num2 != num || component == null || !(global::UnityEngine.Vector3.Distance(position, component.CenterOfMass) < global::UnityEngine.Vector3.Distance(position, ((IDestructible)instance).CenterOfMass))))
						{
							num = num2;
							component = instance;
						}
					}
				}
			}
			if (component != null && global::Mirror.NetworkIdentity.spawned.TryGetValue(component.NetworkId, out var value))
			{
				message.TargetNetId = value.netId;
				global::RelativePositioning.RelativePosition relativePosition = (message.TargetPosition = new global::RelativePositioning.RelativePosition(value.transform.position));
				message.TargetRotation = global::RelativePositioning.WaypointBase.GetRelativeRotation(relativePosition.WaypointId, value.transform.rotation);
			}
			else
			{
				message.TargetNetId = 0u;
			}
			global::RelativePositioning.RelativePosition relativePosition2 = (message.ShooterPosition = new global::RelativePositioning.RelativePosition(position));
			message.ShooterCameraRotation = global::RelativePositioning.WaypointBase.GetRelativeRotation(relativePosition2.WaypointId, Hub.PlayerCameraReference.rotation);
			return true;
		}

		public void ServerProcessShot(global::InventorySystem.Items.Firearms.BasicMessages.ShotMessage message)
		{
			if (!global::RelativePositioning.WaypointBase.TryGetWaypoint(message.ShooterPosition.WaypointId, out var wp))
			{
				return;
			}
			SetHitboxes(Hub, state: false);
			global::UnityEngine.Vector3 worldspacePosition = wp.GetWorldspacePosition(message.ShooterPosition.Relative);
			global::UnityEngine.Quaternion worldspaceRotation = wp.GetWorldspaceRotation(message.ShooterCameraRotation);
			using (global::PlayerRoles.FirstPersonControl.FpcBacktracker fpcBacktracker = new global::PlayerRoles.FirstPersonControl.FpcBacktracker(Hub, worldspacePosition, worldspaceRotation))
			{
				if (DebugMode)
				{
					SendDebug($"Moved shooter {fpcBacktracker.MoveAmount} meters away from claimed position.");
				}
				ReferenceHub hub;
				bool num = ReferenceHub.TryGetHubNetID(message.TargetNetId, out hub);
				global::PlayerRoles.FirstPersonControl.FpcBacktracker fpcBacktracker2 = null;
				global::UnityEngine.Quaternion rotation = default(global::UnityEngine.Quaternion);
				global::UnityEngine.Transform transform = null;
				if (num && global::RelativePositioning.WaypointBase.TryGetWaypoint(message.TargetPosition.WaypointId, out var wp2))
				{
					transform = hub.transform;
					rotation = transform.rotation;
					fpcBacktracker2 = new global::PlayerRoles.FirstPersonControl.FpcBacktracker(hub, wp2.GetWorldspacePosition(message.TargetPosition.Relative));
					transform.rotation = wp2.GetWorldspaceRotation(message.TargetRotation);
					if (DebugMode)
					{
						SendDebug($"Target PlayerID#{hub.PlayerId} moved {fpcBacktracker2.MoveAmount} meters away from claimed position.");
					}
					if (hub.isLocalPlayer)
					{
						SetHitboxes(hub, state: true);
					}
				}
				PrimaryTargetNetId = message.TargetNetId;
				ServerPerformShot(new global::UnityEngine.Ray(Hub.PlayerCameraReference.position, Hub.PlayerCameraReference.forward));
				SetHitboxes(Hub, !Hub.isLocalPlayer);
				if (num)
				{
					fpcBacktracker2.RestorePosition();
					transform.rotation = rotation;
				}
			}
		}

		protected void ShowHitIndicator(uint netId, float damage, global::UnityEngine.Vector3 origin)
		{
			if (ReferenceHub.TryGetHubNetID(netId, out var hub))
			{
				hub.connectionToClient.Send(new global::InventorySystem.Items.Firearms.BasicMessages.GunHitMessage(drawBlood: false, damage, origin));
			}
		}

		protected void PlaceBulletholeDecal(global::UnityEngine.Ray ray, global::UnityEngine.RaycastHit hit)
		{
			if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlaceBulletHole, hit.point))
			{
				global::Utils.Networking.NetworkUtils.SendToAuthenticated(new global::InventorySystem.Items.Firearms.BasicMessages.GunHitMessage(hit.point + (ray.origin - hit.point).normalized, ray.direction, isBlood: false));
			}
		}

		protected void PlaceBloodDecal(global::UnityEngine.Ray ray, global::UnityEngine.RaycastHit hit, IDestructible target)
		{
			if (ReferenceHub.TryGetHubNetID(target.NetworkId, out var hub) && global::PlayerRoles.PlayerRolesUtils.IsHuman(hub) && global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlaceBlood, hub, hit.point))
			{
				global::Utils.Networking.NetworkUtils.SendToAuthenticated(new global::InventorySystem.Items.Firearms.BasicMessages.GunHitMessage(hit.point + (ray.origin - hit.point).normalized, ray.direction, isBlood: true));
			}
		}

		protected abstract void ServerPerformShot(global::UnityEngine.Ray ray);
	}
}
