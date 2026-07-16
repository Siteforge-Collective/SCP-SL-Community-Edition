using System.Collections.Generic;
using Mirror;
using PlayerRoles.Ragdolls;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp3114
{
	public class Scp3114RagdollToBonesConverter : SubroutineBase
	{
		[SerializeField]
		private GameObject[] _boneParts;

		private DynamicRagdoll _syncRagdoll;

		private static bool _cacheSet;

		private static string[] _cachedNames;

		private static GameObject[] _cachedTemplates;

		private static readonly List<Transform> ReplacementTransforms;

		private static readonly List<Rigidbody> ReplacementRbs;

		public static void ConvertExisting(DynamicRagdoll ragdoll)
		{
		}

		public static void ServerConvertNew(Scp3114Role scp, DynamicRagdoll ragdoll)
		{
		}

		private static bool TryPrepCache()
		{
			return false;
		}

		private static void ProcessNameMatch(DynamicRagdoll ragdoll, Transform ragdollPart, GameObject matchedBone)
		{
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}
	}
}
