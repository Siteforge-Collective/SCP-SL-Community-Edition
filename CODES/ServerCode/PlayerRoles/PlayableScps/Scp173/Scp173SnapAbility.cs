namespace PlayerRoles.PlayableScps.Scp173
{
	public class Scp173SnapAbility : global::PlayerRoles.PlayableScps.Subroutines.ScpKeySubroutine<global::PlayerRoles.PlayableScps.Scp173.Scp173Role>
	{
		private const float SnapRayDistance = 1.5f;

		private const float TargetBacktrackingTime = 0.4f;

		private const float OwnerBacktrackingTime = 0.1f;

		private const float ForetrackingTime = 0.2f;

		private global::PlayerRoles.PlayableScps.Scp173.Scp173ObserversTracker _observersTracker;

		private ReferenceHub _targetHub;

		private static int _snapMask;

		private static int SnapMask
		{
			get
			{
				if (_snapMask == 0)
				{
					_snapMask = global::UnityEngine.LayerMask.GetMask("Default", "Hitbox", "Glass", "Door", "BreakableGlass", "Locker");
				}
				return _snapMask;
			}
		}

		public bool IsSpeeding
		{
			get
			{
				if (base.ScpRole.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp173.Scp173BreakneckSpeedsAbility>(out var subroutine))
				{
					return subroutine.IsActive;
				}
				return false;
			}
		}

		protected override ActionName TargetKey => ActionName.Shoot;

		protected override void OnKeyDown()
		{
			base.OnKeyDown();
			if (!IsSpeeding && TryHitTarget(base.Owner.PlayerCameraReference, out _targetHub))
			{
				ClientSendCmd();
			}
		}

		private static bool TryHitTarget(global::UnityEngine.Transform origin, out ReferenceHub target)
		{
			target = null;
			if (!global::UnityEngine.Physics.Raycast(origin.position, origin.forward, out var hitInfo, 1.5f, SnapMask))
			{
				return false;
			}
			if (!hitInfo.collider.TryGetComponent<IDestructible>(out var component) || !(component is HitboxIdentity hitboxIdentity))
			{
				return false;
			}
			if (!(hitboxIdentity.TargetHub.roleManager.CurrentRole is global::PlayerRoles.HumanRole))
			{
				return false;
			}
			target = hitboxIdentity.TargetHub;
			return true;
		}

		protected override void Awake()
		{
			base.Awake();
			GetSubroutine<global::PlayerRoles.PlayableScps.Scp173.Scp173ObserversTracker>(out _observersTracker);
		}

		public override void ClientWriteCmd(global::Mirror.NetworkWriter writer)
		{
			base.ClientWriteCmd(writer);
			global::Utils.Networking.ReferenceHubReaderWriter.WriteReferenceHub(writer, _targetHub);
			global::RelativePositioning.RelativePositionSerialization.WriteRelativePosition(writer, new global::RelativePositioning.RelativePosition(_targetHub.transform.position));
			global::RelativePositioning.RelativePositionSerialization.WriteRelativePosition(writer, new global::RelativePositioning.RelativePosition(base.Owner.transform.position));
			writer.WriteLowPrecisionQuaternion(new LowPrecisionQuaternion(base.Owner.PlayerCameraReference.rotation));
		}

		public override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			base.ServerProcessCmd(reader);
			_targetHub = global::Utils.Networking.ReferenceHubReaderWriter.ReadReferenceHub(reader);
			if (_observersTracker.IsObserved || _targetHub == null || !(_targetHub.roleManager.CurrentRole is global::PlayerRoles.FirstPersonControl.IFpcRole fpcRole) || IsSpeeding || !global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp173SnapPlayer, base.Owner, _targetHub))
			{
				return;
			}
			global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule fpcModule = base.ScpRole.FpcModule;
			global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule fpcModule2 = fpcRole.FpcModule;
			global::UnityEngine.Transform playerCameraReference = base.Owner.PlayerCameraReference;
			global::UnityEngine.Vector3 position = fpcModule2.Position;
			global::UnityEngine.Vector3 position2 = fpcModule.Position;
			global::UnityEngine.Quaternion rotation = playerCameraReference.rotation;
			fpcModule2.Position = fpcModule2.Tracer.GenerateBounds(0.4f, ignoreTeleports: true).ClosestPoint(global::RelativePositioning.RelativePositionSerialization.ReadRelativePosition(reader).Position);
			global::UnityEngine.Bounds bounds = fpcModule.Tracer.GenerateBounds(0.1f, ignoreTeleports: true);
			bounds.Encapsulate(fpcModule.Position + fpcModule.Motor.Velocity * 0.2f);
			fpcModule.Position = bounds.ClosestPoint(global::RelativePositioning.RelativePositionSerialization.ReadRelativePosition(reader).Position);
			playerCameraReference.rotation = reader.ReadLowPrecisionQuaternion().Value;
			if (TryHitTarget(playerCameraReference, out var target) && target.playerStats.DealDamage(base.ScpRole.DamageHandler))
			{
				Hitmarker.SendHitmarker(base.Owner, 1f);
				if (base.ScpRole.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp173.Scp173AudioPlayer>(out var subroutine))
				{
					subroutine.ServerSendSound(global::PlayerRoles.PlayableScps.Scp173.Scp173AudioPlayer.Scp173SoundId.Snap);
				}
			}
			fpcModule2.Position = position;
			fpcModule.Position = position2;
			playerCameraReference.rotation = rotation;
		}
	}
}
