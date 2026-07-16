namespace PlayerRoles.PlayableScps.Scp049
{
	public class Scp049AttackAbility : global::PlayerRoles.PlayableScps.Subroutines.ScpKeySubroutine<global::PlayerRoles.PlayableScps.Scp049.Scp049Role>
	{
		private const float CooldownTime = 1.5f;

		private const float LagBacktrackingCompensation = 0.4f;

		private static int _attackLayerMask;

		private const float AttackDistance = 1.728f;

		[global::UnityEngine.SerializeField]
		private float _statusEffectDuration = 20f;

		private ReferenceHub _target;

		private global::PlayerRoles.PlayableScps.Scp049.Scp049ResurrectAbility _resurrect;

		public readonly global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown Cooldown = new global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown();

		internal static global::UnityEngine.LayerMask AttackMask
		{
			get
			{
				if (_attackLayerMask == 0)
				{
					_attackLayerMask = global::UnityEngine.LayerMask.GetMask("Hitbox") | (int)global::PlayerRoles.FirstPersonControl.FpcStateProcessor.Mask;
				}
				return _attackLayerMask;
			}
		}

		protected override ActionName TargetKey => ActionName.Shoot;

		public event global::System.Action<ReferenceHub> OnServerHit;

		public override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			if (!Cooldown.IsReady || _resurrect.IsInProgress)
			{
				return;
			}
			_target = global::Utils.Networking.ReferenceHubReaderWriter.ReadReferenceHub(reader);
			if (!(_target == null) && IsTargetValid(_target))
			{
				Cooldown.Trigger(1.5f);
				global::CustomPlayerEffects.CardiacArrest effect = _target.playerEffectsController.GetEffect<global::CustomPlayerEffects.CardiacArrest>();
				if (effect.IsEnabled)
				{
					_target.playerStats.DealDamage(new global::PlayerStatsSystem.Scp049DamageHandler(base.Owner, -1f, global::PlayerStatsSystem.Scp049DamageHandler.AttackType.Instakill));
				}
				else
				{
					effect.SetAttacker(base.Owner);
					effect.Intensity = 1;
					effect.ServerChangeDuration(_statusEffectDuration);
				}
				this.OnServerHit?.Invoke(_target);
				ServerSendRpc(toAll: true);
				Hitmarker.SendHitmarker(base.Owner, 1f);
			}
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			Cooldown.WriteCooldown(writer);
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			Cooldown.ReadCooldown(reader);
		}

		public override void ClientWriteCmd(global::Mirror.NetworkWriter writer)
		{
			global::Utils.Networking.ReferenceHubReaderWriter.WriteReferenceHub(writer, _target);
		}

		public override void ResetObject()
		{
			base.ResetObject();
			Cooldown.Clear();
		}

		protected override void Awake()
		{
			base.Awake();
			GetSubroutine<global::PlayerRoles.PlayableScps.Scp049.Scp049ResurrectAbility>(out _resurrect);
		}

		protected override void OnKeyDown()
		{
			base.OnKeyDown();
			if (CanFindTarget(base.Owner.PlayerCameraReference, out _target))
			{
				ClientSendCmd();
			}
		}

		private bool IsTargetValid(ReferenceHub target)
		{
			if (!(target.roleManager.CurrentRole is global::PlayerRoles.HumanRole humanRole))
			{
				return false;
			}
			if (base.Owner.isLocalPlayer)
			{
				return true;
			}
			global::UnityEngine.Bounds bounds = humanRole.FpcModule.Tracer.GenerateBounds(0.4f, ignoreTeleports: true);
			global::UnityEngine.Vector3 position = base.ScpRole.FpcModule.Position;
			global::UnityEngine.Vector3 vector = bounds.ClosestPoint(position);
			if (global::UnityEngine.Vector3.Distance(position, vector) >= 1.728f)
			{
				return false;
			}
			return !global::UnityEngine.Physics.Linecast(position, vector, global::PlayerRoles.FirstPersonControl.FpcStateProcessor.Mask);
		}

		private bool CanFindTarget(global::UnityEngine.Transform camera, out ReferenceHub target)
		{
			target = null;
			if (!global::UnityEngine.Physics.Raycast(camera.position, camera.forward, out var hitInfo, 1.728f, AttackMask))
			{
				return false;
			}
			if (!hitInfo.collider.TryGetComponent<HitboxIdentity>(out var component))
			{
				return false;
			}
			target = component.TargetHub;
			return true;
		}
	}
}
