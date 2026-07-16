using PlayerRoles.FirstPersonControl.Thirdperson;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp3114
{
	public class Scp3114Model : HumanCharacterModel
	{
		private Scp3114Strangle _strangle;

		private Scp3114Slap _slap;

		private Scp3114Dance _dance;

		private bool _hadIdentity;

		[SerializeField]
		private int _strangleLayer;

		[SerializeField]
		private int _danceLayer;

		[SerializeField]
		private float _layerWeightAdjustSpeed;

		[SerializeField]
		private ParticleSystem _revealParticles;

		[SerializeField]
		private GameObject _skeletonFormItemRoot;

		private static readonly int HashStrangling;

		private static readonly int HashDanceVariant;

		private static readonly int HashSlapTrigger;

		private static readonly int HashSlapMirror;

		private static readonly int HashStealing;

		private static readonly int HashReveal;

		protected override bool FootstepPlayable => false;

		protected override bool LandingFootstepPlayable => false;

		private Scp3114Role ScpRole => null;

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}

		protected override void Update()
		{
		}

		private void AdjustWeight(int layer, bool isActive)
		{
		}

		private void OnIdentityChanged()
		{
		}

		private void OnSlapTriggered()
		{
		}
	}
}
