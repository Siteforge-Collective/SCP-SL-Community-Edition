namespace PlayerRoles.PlayableScps.Scp096
{
	public class Scp096MovementModule : global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule
	{
		[global::UnityEngine.SerializeField]
		private float _jumpSpeedRage;

		private const float SlowedSpeed = 2.55f;

		private const float NormalSpeed = 3.64f;

		private const float RageSpeed = 8f;

		private const float ChargeSpeed = 18.5f;

		private const float PryGateDuration = 2f;

		private const float AngleAdjustSpeed = 120f;

		private global::PlayerRoles.PlayableScps.Scp096.Scp096StateController _stateController;

		private float _gateLookAngle;

		private float _normalJumpSpeed;

		private readonly global::System.Diagnostics.Stopwatch _gatePrySw = global::System.Diagnostics.Stopwatch.StartNew();

		private readonly global::UnityEngine.AnimationCurve _gatePryX = new global::UnityEngine.AnimationCurve(TemplateKeyframes);

		private readonly global::UnityEngine.AnimationCurve _gatePryZ = new global::UnityEngine.AnimationCurve(TemplateKeyframes);

		private readonly global::System.Collections.Generic.List<global::UnityEngine.Transform> _pryablePoints = new global::System.Collections.Generic.List<global::UnityEngine.Transform>();

		private static readonly global::UnityEngine.Keyframe[] TemplateKeyframes = new global::UnityEngine.Keyframe[3]
		{
			new global::UnityEngine.Keyframe(0f, 1f, 0f, 0f, 0f, 0.3f),
			new global::UnityEngine.Keyframe(0.35f, 0.2f, -0f, -0f, 0.5f, 0.4f),
			new global::UnityEngine.Keyframe(1f, 0f, -0f, -0f, 0.2f, 0f)
		};

		private float MovementSpeed
		{
			set
			{
				SneakSpeed = value;
				WalkSpeed = value;
				SprintSpeed = value;
			}
		}

		public override bool LockMovement
		{
			get
			{
				if (!base.Role.IsLocalPlayer)
				{
					return false;
				}
				global::PlayerRoles.PlayableScps.Scp096.Scp096AbilityState abilityState = _stateController.AbilityState;
				if (abilityState - 3 <= global::PlayerRoles.PlayableScps.Scp096.Scp096AbilityState.TryingNotToCry)
				{
					return true;
				}
				return base.LockMovement;
			}
		}

		protected override void UpdateMovement()
		{
			UpdateSpeedAndOverrides();
			base.UpdateMovement();
		}

		private void UpdateSpeedAndOverrides()
		{
			switch (_stateController.AbilityState)
			{
			case global::PlayerRoles.PlayableScps.Scp096.Scp096AbilityState.Charging:
				MovementSpeed = 18.5f;
				(base.Motor as global::PlayerRoles.PlayableScps.Scp096.Scp096Motor).SetOverride(base.transform.forward);
				return;
			case global::PlayerRoles.PlayableScps.Scp096.Scp096AbilityState.PryingGate:
				MovementSpeed = 3.64f;
				UpdateGatePrying();
				return;
			}
			switch (_stateController.RageState)
			{
			case global::PlayerRoles.PlayableScps.Scp096.Scp096RageState.Distressed:
			case global::PlayerRoles.PlayableScps.Scp096.Scp096RageState.Calming:
				MovementSpeed = 2.55f;
				JumpSpeed = _normalJumpSpeed;
				break;
			case global::PlayerRoles.PlayableScps.Scp096.Scp096RageState.Enraged:
				MovementSpeed = 8f;
				JumpSpeed = _jumpSpeedRage;
				break;
			default:
				MovementSpeed = 3.64f;
				JumpSpeed = _normalJumpSpeed;
				break;
			}
		}

		private void UpdateGatePrying()
		{
			float num = (float)_gatePrySw.Elapsed.TotalSeconds;
			if (num > 2f)
			{
				if (global::Mirror.NetworkServer.active)
				{
					_stateController.SetAbilityState(global::PlayerRoles.PlayableScps.Scp096.Scp096AbilityState.None);
				}
			}
			else if (base.Role.IsLocalPlayer)
			{
				num /= 2f;
				base.Position = new global::UnityEngine.Vector3(_gatePryX.Evaluate(num), base.Position.y, _gatePryZ.Evaluate(num));
				base.MouseLook.CurrentHorizontal = global::UnityEngine.Mathf.MoveTowardsAngle(base.MouseLook.CurrentHorizontal, _gateLookAngle, global::UnityEngine.Time.deltaTime * 120f);
				base.MouseLook.CurrentVertical = global::UnityEngine.Mathf.MoveTowardsAngle(base.MouseLook.CurrentVertical, 0f, global::UnityEngine.Time.deltaTime * 120f);
			}
		}

		private void SetGatePryCurves(int index, global::UnityEngine.Vector3 pos)
		{
			global::UnityEngine.Keyframe[] keys = _gatePryX.keys;
			keys[index].value = pos.x;
			_gatePryX.keys = keys;
			global::UnityEngine.Keyframe[] keys2 = _gatePryZ.keys;
			keys2[index].value = pos.z;
			_gatePryZ.keys = keys2;
		}

		private void Awake()
		{
			_normalJumpSpeed = JumpSpeed;
		}

		protected override void SetupModules()
		{
			base.Motor = new global::PlayerRoles.PlayableScps.Scp096.Scp096Motor(base.Hub, base.Role as global::PlayerRoles.PlayableScps.Scp096.Scp096Role);
			base.Noclip = new global::PlayerRoles.FirstPersonControl.FpcNoclip(base.Hub, this);
			base.MouseLook = new global::PlayerRoles.FirstPersonControl.FpcMouseLook(base.Hub, this);
			base.StateProcessor = new global::PlayerRoles.FirstPersonControl.FpcStateProcessor(base.Hub, this);
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			_stateController = (base.Role as global::PlayerRoles.PlayableScps.Scp096.Scp096Role).StateController;
		}

		public void SetTargetGate(global::Interactables.Interobjects.PryableDoor door)
		{
			if (door.PryPositions.Length == 0)
			{
				return;
			}
			_gatePrySw.Restart();
			if (base.Role.IsLocalPlayer)
			{
				SetGatePryCurves(0, base.Position);
				_pryablePoints.Clear();
				_pryablePoints.AddRange(door.PryPositions);
				_pryablePoints.Sort(delegate(global::UnityEngine.Transform x, global::UnityEngine.Transform y)
				{
					float sqrMagnitude = (base.Position - x.position).sqrMagnitude;
					float sqrMagnitude2 = (base.Position - y.position).sqrMagnitude;
					return sqrMagnitude.CompareTo(sqrMagnitude2);
				});
				global::UnityEngine.Transform transform = _pryablePoints[0];
				global::UnityEngine.Transform transform2 = _pryablePoints[_pryablePoints.Count - 1];
				SetGatePryCurves(1, transform.position);
				SetGatePryCurves(2, transform2.position);
				global::UnityEngine.Vector3 normalized = (transform2.position - transform.position).normalized;
				_gateLookAngle = global::UnityEngine.Vector3.Angle(normalized, global::UnityEngine.Vector3.forward) * global::UnityEngine.Mathf.Sign(global::UnityEngine.Vector3.Dot(normalized, global::UnityEngine.Vector3.right));
			}
			if (global::Mirror.NetworkServer.active)
			{
				_stateController.SetAbilityState(global::PlayerRoles.PlayableScps.Scp096.Scp096AbilityState.PryingGate);
			}
		}
	}
}
