namespace PlayerRoles.PlayableScps.Scp173
{
	public class Scp173CharacterModel : global::PlayerRoles.FirstPersonControl.Thirdperson.CharacterModel
	{
		public delegate void ModelFrozen(global::PlayerRoles.PlayableScps.Scp173.Scp173Role target);

		[global::UnityEngine.SerializeField]
		private float _lowestPitch;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioSource[] _footstepSources;

		[global::UnityEngine.SerializeField]
		private float _footstepOverallLoundess;

		[global::UnityEngine.SerializeField]
		private float _footstepSwapSpeed;

		[global::UnityEngine.SerializeField]
		private float _footstepEnableSpeed;

		[global::UnityEngine.SerializeField]
		private float _footstepDisableSpeed;

		[global::UnityEngine.SerializeField]
		private float _groundedSustainTime;

		[global::UnityEngine.SerializeField]
		private float _footstepGroundedSustainMultiplier;

		private readonly global::System.Diagnostics.Stopwatch _groundedSw = global::System.Diagnostics.Stopwatch.StartNew();

		private int _sourcesCount;

		private float _stepSize;

		private bool _isFrozen;

		private float _currentVolume;

		private global::UnityEngine.Quaternion _frozenRot;

		private global::PlayerRoles.PlayableScps.Scp173.Scp173Role _role;

		private global::PlayerRoles.PlayableScps.Scp173.Scp173MovementModule _fpc;

		private global::PlayerRoles.PlayableScps.Scp173.Scp173ObserversTracker _observers;

		public bool Frozen
		{
			get
			{
				return _isFrozen;
			}
			set
			{
				if (value != _isFrozen)
				{
					_isFrozen = value;
					if (_isFrozen)
					{
						_frozenRot = base.transform.rotation;
						global::PlayerRoles.PlayableScps.Scp173.Scp173CharacterModel.OnFrozen?.Invoke(_role);
					}
					else
					{
						base.transform.localRotation = global::UnityEngine.Quaternion.identity;
					}
				}
			}
		}

		public static event global::PlayerRoles.PlayableScps.Scp173.Scp173CharacterModel.ModelFrozen OnFrozen;

		private void LateUpdate()
		{
			if (ReferenceHub.TryGetLocalHub(out var hub))
			{
				Frozen = hub.IsHuman() && _observers.IsObservedBy(hub);
				UpdateFootsteps(!Frozen && _fpc.Motor.Velocity != global::UnityEngine.Vector3.zero, _fpc.IsGrounded);
				if (Frozen)
				{
					base.transform.rotation = _frozenRot;
				}
			}
		}

		private void UpdateFootsteps(bool isMoving, bool grounded)
		{
			float num = (isMoving ? _footstepEnableSpeed : _footstepDisableSpeed);
			if (grounded)
			{
				_groundedSw.Restart();
			}
			else if (isMoving && _groundedSw.Elapsed.TotalSeconds < (double)_groundedSustainTime)
			{
				num *= _footstepGroundedSustainMultiplier;
			}
			float num2 = global::UnityEngine.Mathf.MoveTowards(_currentVolume, (isMoving && grounded) ? 1 : 0, global::UnityEngine.Time.deltaTime * num);
			float pitch = global::UnityEngine.Mathf.Lerp(_lowestPitch, 1f, num2);
			float num3 = global::UnityEngine.Time.timeSinceLevelLoad * _footstepSwapSpeed;
			_currentVolume = num2;
			num2 *= _footstepOverallLoundess;
			for (int i = 0; i < _sourcesCount; i++)
			{
				global::UnityEngine.AudioSource obj = _footstepSources[i];
				float f = global::UnityEngine.Mathf.Sin(num3 + (float)global::System.Math.PI * _stepSize * (float)i);
				obj.pitch = pitch;
				obj.volume = num2 * global::UnityEngine.Mathf.Abs(f);
			}
		}

		private void OnGrounded()
		{
			_currentVolume = 1f;
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			_role = base.OwnerHub.roleManager.CurrentRole as global::PlayerRoles.PlayableScps.Scp173.Scp173Role;
			_fpc = _role.FpcModule as global::PlayerRoles.PlayableScps.Scp173.Scp173MovementModule;
			global::PlayerRoles.PlayableScps.Scp173.Scp173MovementModule fpc = _fpc;
			fpc.OnGrounded = (global::System.Action)global::System.Delegate.Combine(fpc.OnGrounded, new global::System.Action(OnGrounded));
			_role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp173.Scp173ObserversTracker>(out _observers);
			_sourcesCount = _footstepSources.Length;
			_stepSize = 1f / (float)_sourcesCount;
			for (int i = 0; i < _sourcesCount; i++)
			{
				global::UnityEngine.AudioSource obj = _footstepSources[i];
				obj.volume = 0f;
				obj.PlayDelayed(obj.clip.length * _stepSize * (float)i);
			}
		}

		public override void ResetObject()
		{
			base.ResetObject();
			global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule fpcModule = _role.FpcModule;
			fpcModule.OnGrounded = (global::System.Action)global::System.Delegate.Remove(fpcModule.OnGrounded, new global::System.Action(OnGrounded));
			for (int i = 0; i < _sourcesCount; i++)
			{
				_footstepSources[i].Stop();
			}
		}
	}
}
