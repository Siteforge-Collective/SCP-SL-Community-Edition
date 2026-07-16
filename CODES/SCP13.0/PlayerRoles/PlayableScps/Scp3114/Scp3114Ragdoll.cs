using System.Runtime.InteropServices;
using Mirror;
using PlayerRoles.Ragdolls;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp3114
{
	public class Scp3114Ragdoll : DynamicRagdoll
	{
		[SyncVar]
		private RoleTypeId _disguiseRole;

		[SerializeField]
		private float _revealDelay;

		[SerializeField]
		private float _revealDuration;

		[SerializeField]
		private Renderer _ownRenderer;

		private bool _playingAnimation;

		private float _revealElapsed;

		private Transform[] _trackedBones;

		private Material[] _humanMaterials;

		private Transform _humanRagdollRoot;

		private static readonly int ProgressHash;

		private static readonly int FadeHash;

		public RoleTypeId Network_disguiseRole
		{
			get
			{
				return default(RoleTypeId);
			}
			[param: In]
			set
			{
			}
		}

		protected override void Start()
		{
		}

		protected override void Update()
		{
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static void ServerOnRagdollCreated(ReferenceHub owner, BasicRagdoll ragdoll)
		{
		}
	}
}
