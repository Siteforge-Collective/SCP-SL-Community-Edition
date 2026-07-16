namespace PlayerRoles.FirstPersonControl
{
	public class FpcMotor
	{
		protected enum FpcViewMode
		{
			LocalPlayer = 0,
			Server = 1,
			Thirdperson = 2
		}

		private static readonly global::UnityEngine.Vector3 Gravity = new global::UnityEngine.Vector3(0f, -19.6f, 0f);

		private static readonly global::UnityEngine.Vector3 InvisiblePosition = global::UnityEngine.Vector3.up * 6000f;

		private const float JumpToStepOffsetRatio = 0.35f;

		private const float StepDiffMultiplier = 1.6f;

		private const float StickToGroundForce = 10f;

		private const float MinMoveDiff = 0.03f;

		private const float PositionOverrideAbsTolerance = 0.5f;

		private const float PositionOverrideCooldown = 0.4f;

		private const float ThirdpersonLerpMultiplier = 2f;

		private const float ThirdpersonHeightLerp = 9f;

		private const float ThirdpersonMinSpeed = 3f;

		private const float FallDamageMinVelocity = 14.5f;

		private const float FallDamagePower = 0.8f;

		private const float FallDamageMultiplier = 31.4f;

		private const float FallDamageAbsolute = 10f;

		private const float FallDamageImmunityTime = 2.5f;

		private bool _requestedJump;

		private float _lastMaxSpeed;

		private float _maxFallSpeed;

		private bool _prevGrounded;

		private static global::UnityEngine.KeyCode _keyFwd;

		private static global::UnityEngine.KeyCode _keyBwd;

		private static global::UnityEngine.KeyCode _keyLft;

		private static global::UnityEngine.KeyCode _keyRgt;

		private static global::UnityEngine.KeyCode _keyJump;

		private readonly global::System.Diagnostics.Stopwatch _lastOverrideTime;

		private readonly global::System.Diagnostics.Stopwatch _fallDamageImmunity;

		private readonly bool _enableFallDamage;

		private readonly float _defaultStepOffset;

		private readonly float _defaultHeight;

		protected readonly ReferenceHub Hub;

		protected readonly global::UnityEngine.Transform CachedTransform;

		protected readonly global::PlayerRoles.FirstPersonControl.FpcMotor.FpcViewMode ViewMode;

		protected readonly global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule MainModule;

		public global::UnityEngine.Vector3 MoveDirection { get; private set; }

		public virtual global::UnityEngine.Vector3 Velocity { get; private set; }

		public bool IsJumping { get; private set; }

		public bool IsInvisible { get; set; }

		public global::RelativePositioning.RelativePosition ReceivedPosition { get; set; }

		public bool MovementDetected { get; set; }

		public bool RotationDetected { get; set; }

		public bool WantsToJump
		{
			get
			{
				if (!_requestedJump)
				{
					if (Hub.isLocalPlayer && global::UnityEngine.Input.GetKeyDown(_keyJump))
					{
						return !InputLocked;
					}
					return false;
				}
				return true;
			}
			set
			{
				_requestedJump = value;
			}
		}

		protected virtual float Speed => MainModule.MaxMovementSpeed;

		protected virtual global::UnityEngine.Vector3 DesiredMove
		{
			get
			{
				if (ViewMode == global::PlayerRoles.FirstPersonControl.FpcMotor.FpcViewMode.LocalPlayer)
				{
					if (TryGetOverride(out var overrideDir))
					{
						return overrideDir;
					}
					if (InputLocked)
					{
						return global::UnityEngine.Vector3.zero;
					}
					float num = 0f;
					float num2 = 0f;
					if (global::UnityEngine.Input.GetKey(_keyFwd))
					{
						num2 += 1f;
					}
					if (global::UnityEngine.Input.GetKey(_keyBwd))
					{
						num2 -= 1f;
					}
					if (global::UnityEngine.Input.GetKey(_keyRgt))
					{
						num += 1f;
					}
					if (global::UnityEngine.Input.GetKey(_keyLft))
					{
						num -= 1f;
					}
					return CachedTransform.forward * num2 + CachedTransform.right * num;
				}
				global::UnityEngine.Vector3 position = ReceivedPosition.Position;
				global::UnityEngine.Vector3 vector = position - Position;
				if (global::Mirror.NetworkServer.active)
				{
					float num3 = global::UnityEngine.Mathf.Clamp(vector.y * 1.6f, 0f, 0.35f * MainModule.JumpSpeed);
					MainModule.CharController.stepOffset = global::UnityEngine.Mathf.Min(_defaultStepOffset + num3, _defaultHeight);
					if (MainModule.Noclip.RecentlyActive)
					{
						Position = position;
						return global::UnityEngine.Vector3.zero;
					}
				}
				float num4 = vector.MagnitudeIgnoreY();
				if (num4 < 0.03f)
				{
					return global::UnityEngine.Vector3.zero;
				}
				num4 -= 0.5f;
				if (num4 < _lastMaxSpeed && global::UnityEngine.Mathf.Abs(vector.y) < global::UnityEngine.Mathf.Max(MainModule.JumpSpeed, global::UnityEngine.Mathf.Abs(MoveDirection.y)))
				{
					if (!global::Mirror.NetworkServer.active)
					{
						return vector;
					}
					return new global::UnityEngine.Vector3(vector.x, 0f, vector.z);
				}
				if (global::Mirror.NetworkServer.active)
				{
					if (_lastOverrideTime.Elapsed.TotalSeconds > 0.4000000059604645)
					{
						MainModule.ServerOverridePosition(Position, global::UnityEngine.Vector3.zero);
					}
				}
				else
				{
					Position = position;
				}
				return global::UnityEngine.Vector3.zero;
			}
		}

		private global::UnityEngine.Vector3 Position
		{
			get
			{
				return MainModule.Position;
			}
			set
			{
				MainModule.Position = value;
			}
		}

		private bool InputLocked => global::CursorManagement.CursorManager.MovementLocked;

		public FpcMotor(ReferenceHub hub, global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule module, bool enableFallDamage)
		{
			Hub = hub;
			MainModule = module;
			_enableFallDamage = enableFallDamage;
			CachedTransform = hub.transform;
			if (global::Mirror.NetworkServer.active)
			{
				_lastOverrideTime = global::System.Diagnostics.Stopwatch.StartNew();
				_fallDamageImmunity = global::System.Diagnostics.Stopwatch.StartNew();
				module.OnServerPositionOverwritten = (global::System.Action)global::System.Delegate.Combine(module.OnServerPositionOverwritten, (global::System.Action)delegate
				{
					_lastOverrideTime.Restart();
				});
				_defaultStepOffset = module.CharController.stepOffset;
				_defaultHeight = module.CharController.height;
			}
			if (Hub.isLocalPlayer)
			{
				ViewMode = global::PlayerRoles.FirstPersonControl.FpcMotor.FpcViewMode.LocalPlayer;
				ReloadInputConfigs();
			}
			else
			{
				ViewMode = (global::Mirror.NetworkServer.active ? global::PlayerRoles.FirstPersonControl.FpcMotor.FpcViewMode.Server : global::PlayerRoles.FirstPersonControl.FpcMotor.FpcViewMode.Thirdperson);
			}
		}

		public void UpdatePosition(out bool sendJump)
		{
			sendJump = false;
			_lastMaxSpeed = Speed;
			if (MainModule.Noclip.IsActive)
			{
				MoveDirection = global::UnityEngine.Vector3.zero;
				return;
			}
			if (ViewMode == global::PlayerRoles.FirstPersonControl.FpcMotor.FpcViewMode.Thirdperson)
			{
				UpdateThirdperson();
				return;
			}
			global::UnityEngine.CharacterController charController = MainModule.CharController;
			global::UnityEngine.Physics.SphereCast(Position, charController.radius, global::UnityEngine.Vector3.down, out var hitInfo, charController.height / 2f, global::InventorySystem.Items.MicroHID.MicroHIDItem.WallMask, global::UnityEngine.QueryTriggerInteraction.Ignore);
			global::UnityEngine.Vector3 normalized = global::UnityEngine.Vector3.ProjectOnPlane(DesiredMove, hitInfo.normal).normalized;
			global::UnityEngine.Vector3 moveDir = new global::UnityEngine.Vector3(normalized.x * _lastMaxSpeed, MoveDirection.y, normalized.z * _lastMaxSpeed);
			bool isGrounded = charController.isGrounded;
			if (isGrounded)
			{
				UpdateGrounded(ref moveDir, ref sendJump, MainModule.JumpSpeed);
			}
			else
			{
				UpdateFloating(ref moveDir);
			}
			_prevGrounded = isGrounded;
			MainModule.IsGrounded = isGrounded;
			MoveDirection = (MainModule.Noclip.IsActive ? global::UnityEngine.Vector3.zero : moveDir);
			global::UnityEngine.Vector3 position = Position;
			charController.Move(GetFrameMove());
			Position = CachedTransform.position;
			MovementDetected = Position != position;
			Velocity = (Position - position) / global::UnityEngine.Time.deltaTime;
		}

		public void ResetFallDamageCooldown()
		{
			_fallDamageImmunity.Restart();
		}

		protected virtual global::UnityEngine.Vector3 GetFrameMove()
		{
			global::UnityEngine.Vector3 result = MoveDirection * global::UnityEngine.Time.deltaTime;
			if (ViewMode != global::PlayerRoles.FirstPersonControl.FpcMotor.FpcViewMode.LocalPlayer)
			{
				global::UnityEngine.Vector3 position = ReceivedPosition.Position;
				global::UnityEngine.Vector3 position2 = Position;
				result.x = ClampMoveDirection(position2.x, position.x, result.x);
				result.z = ClampMoveDirection(position2.z, position.z, result.z);
			}
			return result;
		}

		protected virtual void UpdateGrounded(ref global::UnityEngine.Vector3 moveDir, ref bool sendJump, float jumpSpeed)
		{
			if (WantsToJump)
			{
				if (jumpSpeed > 0f)
				{
					moveDir.y = jumpSpeed;
				}
				_requestedJump = false;
				IsJumping = true;
				sendJump = true;
			}
			else
			{
				moveDir.y = -10f;
				IsJumping = false;
			}
			if (_maxFallSpeed > 14.5f && _enableFallDamage)
			{
				ServerProcessFall(_maxFallSpeed - 14.5f);
			}
			_maxFallSpeed = 14.5f;
		}

		protected virtual void UpdateFloating(ref global::UnityEngine.Vector3 moveDir)
		{
			if (_prevGrounded && !IsJumping)
			{
				moveDir.y = 0f;
			}
			moveDir += Gravity * global::UnityEngine.Time.deltaTime;
			_maxFallSpeed = global::UnityEngine.Mathf.Max(_maxFallSpeed, 0f - moveDir.y);
		}

		private float ClampMoveDirection(float curPos, float targetPos, float moveDir)
		{
			float num = global::UnityEngine.Mathf.Abs(curPos - targetPos);
			return global::UnityEngine.Mathf.Clamp(moveDir, 0f - num, num);
		}

		private void UpdateThirdperson()
		{
			if (IsInvisible)
			{
				Position = InvisiblePosition;
				return;
			}
			global::UnityEngine.Vector3 desiredMove = DesiredMove;
			global::UnityEngine.Vector3 position = Position;
			global::UnityEngine.Vector3 position2 = ReceivedPosition.Position;
			Velocity = new global::UnityEngine.Vector3(desiredMove.x, 0f, desiredMove.z).normalized * _lastMaxSpeed + global::UnityEngine.Vector3.up * desiredMove.y;
			MoveDirection = Velocity;
			global::UnityEngine.Vector3 b = new global::UnityEngine.Vector3(position2.x, position.y, position2.z);
			float num = global::UnityEngine.Time.deltaTime * global::UnityEngine.Mathf.Max(3f, _lastMaxSpeed);
			position = global::UnityEngine.Vector3.Lerp(position, b, num * 2f);
			position.y = global::UnityEngine.Mathf.Lerp(position.y, position2.y, 9f * global::UnityEngine.Time.deltaTime);
			Position = position;
		}

		private void ServerProcessFall(float speed)
		{
			if (global::Mirror.NetworkServer.active && !(_fallDamageImmunity.Elapsed.TotalSeconds < 2.5))
			{
				global::PlayerRoles.RoleTypeId roleId = Hub.GetRoleId();
				global::UnityEngine.Vector3 position = Position;
				float damage = global::UnityEngine.Mathf.Pow(speed, 0.8f) * 31.4f + 10f;
				Hub.playerStats.DealDamage(new global::PlayerStatsSystem.UniversalDamageHandler(damage, global::PlayerStatsSystem.DeathTranslations.Falldown));
				global::Utils.Networking.NetworkUtils.SendToAuthenticated(new global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcFallDamageMessage(Hub, position, roleId));
			}
		}

		public void OnElevatorMoved(global::UnityEngine.Bounds elevatorBounds, global::Interactables.Interobjects.ElevatorChamber chamb, global::UnityEngine.Vector3 deltaPos, global::UnityEngine.Quaternion deltaRot)
		{
			if (!elevatorBounds.Contains(Position))
			{
				return;
			}
			if (global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Vector3 position = ReceivedPosition.Position;
				global::UnityEngine.Vector3 point = new global::UnityEngine.Vector3(position.x, Position.y, position.z);
				if (elevatorBounds.Contains(point))
				{
					_lastOverrideTime.Restart();
				}
			}
			global::UnityEngine.Transform transform = chamb.transform;
			global::UnityEngine.Vector3 vector = Position + deltaPos;
			vector = deltaRot * (vector - transform.position) + transform.position;
			Position = vector;
			if (Hub.isLocalPlayer)
			{
				MainModule.MouseLook.CurrentHorizontal += deltaRot.eulerAngles.y;
			}
		}

		private bool TryGetOverride(out global::UnityEngine.Vector3 overrideDir)
		{
			bool result = false;
			overrideDir = global::UnityEngine.Vector3.zero;
			if (Hub.inventory.CurInstance is global::PlayerRoles.FirstPersonControl.IMovementInputOverride movementInputOverride && movementInputOverride != null && movementInputOverride.MovementOverrideActive)
			{
				result = true;
				overrideDir = movementInputOverride.MovementOverrideDirection;
			}
			for (int i = 0; i < Hub.playerEffectsController.EffectsLength; i++)
			{
				if (Hub.playerEffectsController.AllEffects[i] is global::PlayerRoles.FirstPersonControl.IMovementInputOverride movementInputOverride2 && movementInputOverride2.MovementOverrideActive)
				{
					result = true;
					overrideDir += movementInputOverride2.MovementOverrideDirection;
				}
			}
			return result;
		}

		private static void ReloadInputConfigs()
		{
		}
	}
}
