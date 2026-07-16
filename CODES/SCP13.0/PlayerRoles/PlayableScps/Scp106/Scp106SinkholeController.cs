using System.Runtime.CompilerServices;
using CursorManagement;
using GameObjectPools;
using Mirror;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp106
{
	public class Scp106SinkholeController : SubroutineBase, ICursorOverride, IPoolResettable, IPoolSpawnable
	{
		public delegate void SubmergeStateChanged(ReferenceHub scp106, bool newState);

		public const float DefaultEmergeAnimTime = 3.908f;

		public const float DefaultSubmergeAnimTime = 3.333f;

		private const float CooldownDuration = 5f;

		private const float AudioFadeIntensity = 8f;

		private const float AudioFadeAbs = 0.07f;

		private bool _state;

		private float _toggleTime;

		private int _vigorAbilitiesCount;

		private Scp106VigorAbilityBase[] _vigorAbilities;

		[SerializeField]
		private AudioClip _emergeSound;

		[SerializeField]
		private AudioClip _submergeSound;

		[SerializeField]
		private AudioSource _toggleAudioSource;

		public readonly InconsistentAbilityCooldown Cooldown;

		private float CurTime => 0f;

		public CursorOverrideMode CursorOverride => default(CursorOverrideMode);

		public bool LockMovement => false;

		public float ElapsedToggle => 0f;

		public bool IsDuringAnimation => false;

		public bool IsHidden => false;

		public float NormalizedState => 0f;

		public bool State
		{
			get
			{
				return false;
			}
			private set
			{
			}
		}

		public float TargetDuration => 0f;

		public float SpeedMultiplier => 0f;

		public static event SubmergeStateChanged OnSubmergeStateChange
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

		public void SpawnObject()
		{
		}

		public void ResetObject()
		{
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		protected override void Awake()
		{
		}

		private void Update()
		{
		}
	}
}
