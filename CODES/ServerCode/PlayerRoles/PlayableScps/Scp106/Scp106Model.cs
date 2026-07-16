namespace PlayerRoles.PlayableScps.Scp106
{
	public class Scp106Model : global::PlayerRoles.FirstPersonControl.Thirdperson.AnimatedCharacterModel
	{
		private static readonly int SubmergeHash = global::UnityEngine.Animator.StringToHash("IsSubmerged");

		private static readonly int SpeedHash = global::UnityEngine.Animator.StringToHash("TransitionSpeed");

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _submergeAnim;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _appearAnim;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject[] _hiddenObjects;

		private global::UnityEngine.Transform _tr;

		private global::UnityEngine.Vector3 _defaultPos;

		private global::PlayerRoles.PlayableScps.Scp106.Scp106SinkholeController _sinkhole;

		private global::PlayerRoles.PlayableScps.Scp106.Scp106StalkAbility _stalkAbility;

		private global::PlayerRoles.PlayableScps.Scp106.Scp106MovementModule _fpc;

		protected override bool FootstepPlayable
		{
			get
			{
				if (base.FpcModule.IsGrounded && base.FpcModule.Motor.MovementDetected)
				{
					return LandingFootstepPlayable;
				}
				return false;
			}
		}

		protected override bool LandingFootstepPlayable => _sinkhole.NormalizedState == 0f;

		private void LateUpdate()
		{
			if (!base.Pooled)
			{
				base.Animator.SetFloat(SpeedHash, _sinkhole.SpeedMultiplier);
				bool isHidden = _sinkhole.IsHidden;
				global::UnityEngine.GameObject[] hiddenObjects = _hiddenObjects;
				for (int i = 0; i < hiddenObjects.Length; i++)
				{
					hiddenObjects[i].SetActive(!isHidden);
				}
				if (base.IsTracked)
				{
					SetVisibility(_sinkhole.IsDuringAnimation);
				}
				base.Animator.SetBool(SubmergeHash, _sinkhole.State);
				float normalizedState = _sinkhole.NormalizedState;
				global::UnityEngine.AnimationCurve animationCurve = (_sinkhole.State ? _submergeAnim : _appearAnim);
				global::UnityEngine.Vector3 vector = global::UnityEngine.Vector3.up * animationCurve.Evaluate(normalizedState);
				_tr.localPosition = _defaultPos + vector;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			_tr = base.transform;
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			global::PlayerRoles.PlayableScps.Scp106.Scp106Role scp106Role = base.OwnerHub.roleManager.CurrentRole as global::PlayerRoles.PlayableScps.Scp106.Scp106Role;
			scp106Role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp106.Scp106StalkAbility>(out _stalkAbility);
			_fpc = scp106Role.FpcModule as global::PlayerRoles.PlayableScps.Scp106.Scp106MovementModule;
			_defaultPos = _fpc.CharacterModelTemplate.transform.localPosition;
			_sinkhole = scp106Role.Sinkhole;
		}
	}
}
