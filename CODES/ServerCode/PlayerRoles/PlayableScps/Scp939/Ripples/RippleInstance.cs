namespace PlayerRoles.PlayableScps.Scp939.Ripples
{
	public class RippleInstance : global::UnityEngine.MonoBehaviour
	{
		private bool _inElevator;

		private global::UnityEngine.Vector3 _pos;

		private global::UnityEngine.Transform _t;

		private float _setTime;

		private const float MinDuration = 3f;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.ParticleSystem _colorParticle;

		[field: global::UnityEngine.SerializeField]
		public global::UnityEngine.ParticleSystem MainParticleSystem { get; private set; }

		public bool InUse => global::UnityEngine.Time.timeSinceLevelLoad - _setTime <= 0f;

		public void Set(global::UnityEngine.Vector3 pos, global::UnityEngine.Color col)
		{
			if (_inElevator)
			{
				_t.SetParent(null);
				_inElevator = false;
			}
			_pos = pos;
			_t.position = pos;
			_setTime = global::UnityEngine.Time.timeSinceLevelLoad + 3f;
			global::UnityEngine.ParticleSystem.MainModule main = _colorParticle.main;
			main.startColor = col;
			MainParticleSystem.Play(withChildren: true);
		}

		private void OnDisable()
		{
			_setTime = 0f;
		}

		private void OnDestroy()
		{
			global::Interactables.Interobjects.ElevatorChamber.OnElevatorMoved -= OnElevatorMoved;
		}

		private void Awake()
		{
			_t = base.transform;
			global::Interactables.Interobjects.ElevatorChamber.OnElevatorMoved += OnElevatorMoved;
		}

		private void OnElevatorMoved(global::UnityEngine.Bounds elevatorBounds, global::Interactables.Interobjects.ElevatorChamber chamber, global::UnityEngine.Vector3 deltaPos, global::UnityEngine.Quaternion deltaRot)
		{
			if (!_inElevator && elevatorBounds.Contains(_pos))
			{
				_t.SetParent(chamber.transform);
				_t.position += deltaPos;
				_inElevator = true;
			}
		}
	}
}
