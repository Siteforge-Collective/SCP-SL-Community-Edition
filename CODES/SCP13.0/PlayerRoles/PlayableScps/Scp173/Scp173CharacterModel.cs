using System.Diagnostics;
using System.Runtime.CompilerServices;
using PlayerRoles.FirstPersonControl.Thirdperson;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp173
{
	public class Scp173CharacterModel : CharacterModel
	{
		public delegate void ModelFrozen(Scp173Role target);

		[SerializeField]
		private float _lowestPitch;

		[SerializeField]
		private AudioSource[] _footstepSources;

		[SerializeField]
		private float _footstepOverallLoundess;

		[SerializeField]
		private float _footstepSwapSpeed;

		[SerializeField]
		private float _footstepEnableSpeed;

		[SerializeField]
		private float _footstepDisableSpeed;

		[SerializeField]
		private float _groundedSustainTime;

		[SerializeField]
		private float _footstepGroundedSustainMultiplier;

		private readonly Stopwatch _groundedSw;

		private int _sourcesCount;

		private float _stepSize;

		private bool _isFrozen;

		private float _currentVolume;

		private Quaternion _frozenRot;

		private Scp173Role _role;

		private Scp173MovementModule _fpc;

		private Scp173ObserversTracker _observers;

		public bool Frozen
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public static event ModelFrozen OnFrozen
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		private void LateUpdate()
		{
		}

		private void UpdateFootsteps(bool isMoving, bool grounded)
		{
		}

		private void OnGrounded()
		{
		}

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}
	}
}
