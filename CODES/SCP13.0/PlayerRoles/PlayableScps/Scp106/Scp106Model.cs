using Decals;
using PlayerRoles.FirstPersonControl.Thirdperson;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp106
{
	public class Scp106Model : AnimatedCharacterModel
	{
		private static readonly int SubmergeHash;

		private static readonly int SpeedHash;

		[SerializeField]
		private AnimationCurve _submergeAnim;

		[SerializeField]
		private AnimationCurve _appearAnim;

		[SerializeField]
		private GameObject[] _hiddenObjects;

		[SerializeField]
		private Transform _balanceTransform;

		[SerializeField]
		private Transform _portalTransform;

		[SerializeField]
		private ParticleSystem _stalkParticles;

		[SerializeField]
		private AnimationCurve _portalScale;

		public Transform[] VisibilityReferencePoints;

		private const float PortalRotationSpeed = 21f;

		private Decal[] _portalDecals;

		private Transform _tr;

		private Vector3 _defaultPos;

		private Scp106SinkholeController _sinkhole;

		private Scp106StalkAbility _stalkAbility;

		private Scp106MovementModule _fpc;

		[field: SerializeField]
		public Transform StalkCameraTarget { get; private set; }

		protected override bool FootstepPlayable => false;

		protected override bool LandingFootstepPlayable => false;

		private void LateUpdate()
		{
		}

		protected override void Update()
		{
		}

		protected override void Awake()
		{
		}

		public override void SpawnObject()
		{
		}
	}
}
