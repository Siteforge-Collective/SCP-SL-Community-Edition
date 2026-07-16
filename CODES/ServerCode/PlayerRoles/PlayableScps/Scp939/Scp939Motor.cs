namespace PlayerRoles.PlayableScps.Scp939
{
	public class Scp939Motor : global::PlayerRoles.FirstPersonControl.FpcMotor
	{
		private const float MinDot = 0.4f;

		private const float LungeRadius = 0.6f;

		private const float FocusToleranceOffset = 0.13f;

		private const float FocusTimeTolerance = 3.5f;

		private const int MaxDetections = 32;

		private const float MinDistanceSqr = 0.4f;

		private readonly global::PlayerRoles.PlayableScps.Scp939.Scp939Role _role;

		private readonly global::PlayerRoles.PlayableScps.Scp939.Scp939FocusAbility _focus;

		private readonly global::PlayerRoles.PlayableScps.Scp939.Scp939LungeAbility _lunge;

		private readonly global::PlayerRoles.PlayableScps.Scp939.Scp939AmnesticCloudAbility _cloud;

		private static readonly global::UnityEngine.Collider[] Detections = new global::UnityEngine.Collider[32];

		private static readonly CachedLayerMask Mask = new CachedLayerMask("Hitbox", "Glass");

		private static readonly global::System.Collections.Generic.HashSet<global::PlayerRoles.HumanRole> DetectedHumans = new global::System.Collections.Generic.HashSet<global::PlayerRoles.HumanRole>();

		private bool IsLocalPlayer => ViewMode == global::PlayerRoles.FirstPersonControl.FpcMotor.FpcViewMode.LocalPlayer;

		private bool WantsToLunge
		{
			get
			{
				if (_lunge.IsReady)
				{
					if (!_lunge.LungeRequested)
					{
						return base.WantsToJump;
					}
					return true;
				}
				return false;
			}
		}

		private bool IsLunging => _lunge.State == global::PlayerRoles.PlayableScps.Scp939.Scp939LungeState.Triggered;

		protected override float Speed
		{
			get
			{
				if (IsLunging)
				{
					return _lunge.LungeForwardSpeed;
				}
				if (IsLocalPlayer && _cloud.TargetState)
				{
					return base.Speed * global::UnityEngine.Mathf.Clamp01(1f - _cloud.HoldDuration);
				}
				float num = _focus.State;
				if (global::Mirror.NetworkServer.active && !IsLocalPlayer && _focus.TargetState)
				{
					if (_focus.FrozenTime > 3.5f)
					{
						return 0f;
					}
					num -= 0.13f;
				}
				return base.Speed * global::UnityEngine.Mathf.Clamp01(1f - num);
			}
		}

		protected override global::UnityEngine.Vector3 DesiredMove
		{
			get
			{
				if (!IsLocalPlayer || !IsLunging)
				{
					return base.DesiredMove;
				}
				return CachedTransform.forward;
			}
		}

		public override global::UnityEngine.Vector3 Velocity
		{
			get
			{
				if (!global::Mirror.NetworkServer.active || Hub.isLocalPlayer || !IsLunging)
				{
					return base.Velocity;
				}
				return CachedTransform.forward * _lunge.LungeForwardSpeed;
			}
		}

		public bool MovingForwards
		{
			get
			{
				if (IsLocalPlayer)
				{
					return global::UnityEngine.Vector3.Dot(base.MoveDirection.NormalizeIgnoreY(), CachedTransform.forward) > 0.4f;
				}
				return true;
			}
		}

		private void ProcessHitboxCollision(HitboxIdentity hid)
		{
			if (IsLocalPlayer && hid.TargetHub.roleManager.CurrentRole is global::PlayerRoles.HumanRole humanRole && !((MainModule.Position - humanRole.FpcModule.Position).SqrMagnitudeIgnoreY() < 0.4f))
			{
				DetectedHumans.Add(humanRole);
			}
		}

		private void ProcessWindowCollision(BreakableWindow window)
		{
			if (global::Mirror.NetworkServer.active)
			{
				window.Damage(window.health, new global::PlayerRoles.PlayableScps.Scp939.Scp939DamageHandler(_role, global::PlayerRoles.PlayableScps.Scp939.Scp939DamageType.LungeTarget), default(global::UnityEngine.Vector3));
				return;
			}
			global::UnityEngine.Collider[] componentsInChildren = window.GetComponentsInChildren<global::UnityEngine.Collider>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
		}

		private void OverlapCapsule(global::UnityEngine.Vector3 point1, global::UnityEngine.Vector3 point2)
		{
			int num = global::UnityEngine.Physics.OverlapCapsuleNonAlloc(point1, point2, 0.6f, Detections, Mask);
			for (int i = 0; i < num; i++)
			{
				if (Detections[i].TryGetComponent<IDestructible>(out var component))
				{
					if (component is HitboxIdentity hid)
					{
						ProcessHitboxCollision(hid);
					}
					else if (component is BreakableWindow window)
					{
						ProcessWindowCollision(window);
					}
				}
			}
		}

		protected override void UpdateFloating(ref global::UnityEngine.Vector3 moveDir)
		{
			if (_focus.State == 1f && _lunge.State != global::PlayerRoles.PlayableScps.Scp939.Scp939LungeState.Triggered)
			{
				moveDir.y = global::UnityEngine.Mathf.Min(moveDir.y, 0f - _lunge.LungeJumpSpeed);
			}
			base.UpdateFloating(ref moveDir);
		}

		protected override void UpdateGrounded(ref global::UnityEngine.Vector3 moveDir, ref bool sendJump, float jumpSpeed)
		{
			if (WantsToLunge)
			{
				_lunge.TriggerLunge();
				jumpSpeed = _lunge.LungeJumpSpeed;
				base.WantsToJump = true;
			}
			else if (_focus.State > 0f || _cloud.TargetState)
			{
				jumpSpeed = 0f;
			}
			base.UpdateGrounded(ref moveDir, ref sendJump, jumpSpeed);
		}

		protected override global::UnityEngine.Vector3 GetFrameMove()
		{
			global::UnityEngine.Vector3 frameMove = base.GetFrameMove();
			if (!IsLunging || (!global::Mirror.NetworkServer.active && !IsLocalPlayer))
			{
				return frameMove;
			}
			DetectedHumans.Clear();
			global::UnityEngine.Vector3 position = MainModule.Position;
			global::UnityEngine.Vector3 vector = position + frameMove;
			global::UnityEngine.Vector3 vector2 = global::UnityEngine.Vector3.down * 0.6f;
			OverlapCapsule(position, vector);
			OverlapCapsule(position + vector2, vector + vector2);
			if (!IsLocalPlayer)
			{
				return frameMove;
			}
			global::PlayerRoles.HumanRole human = null;
			float num = float.MaxValue;
			bool flag = false;
			foreach (global::PlayerRoles.HumanRole detectedHuman in DetectedHumans)
			{
				float sqrMagnitude = (detectedHuman.FpcModule.Position - vector).sqrMagnitude;
				if (!(sqrMagnitude >= num))
				{
					human = detectedHuman;
					num = sqrMagnitude;
					flag = true;
				}
			}
			if (!flag)
			{
				return frameMove;
			}
			_lunge.ClientSendHit(human);
			return _lunge.LungeJumpSpeed * global::UnityEngine.Time.deltaTime * global::UnityEngine.Vector3.down;
		}

		public Scp939Motor(ReferenceHub hub, global::PlayerRoles.PlayableScps.Scp939.Scp939Role role)
			: base(hub, role.FpcModule, enableFallDamage: false)
		{
			_role = role;
			role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp939.Scp939FocusAbility>(out _focus);
			role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp939.Scp939LungeAbility>(out _lunge);
			role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp939.Scp939AmnesticCloudAbility>(out _cloud);
		}
	}
}
