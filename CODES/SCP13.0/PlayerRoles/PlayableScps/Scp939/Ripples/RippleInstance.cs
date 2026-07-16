using Interactables.Interobjects;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp939.Ripples
{
	public class RippleInstance : MonoBehaviour
	{
		private bool _inElevator;

		private Vector3 _pos;

		private Transform _t;

		private float _setTime;

		private const float MinDuration = 3f;

		[field: SerializeField]
		public ParticleSystem MainParticleSystem { get; private set; }

		public bool InUse => false;

		public void Set(Vector3 pos, Color col)
		{
		}

		private void OnDisable()
		{
		}

		private void OnDestroy()
		{
		}

		private void Awake()
		{
		}

		private void OnElevatorMoved(Bounds elevatorBounds, ElevatorChamber chamber, Vector3 deltaPos, Quaternion deltaRot)
		{
		}
	}
}
