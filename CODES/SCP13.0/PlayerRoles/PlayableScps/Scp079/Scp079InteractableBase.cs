using System.Collections.Generic;
using MapGeneration;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079
{
	public abstract class Scp079InteractableBase : MonoBehaviour
	{
		public static readonly List<Scp079InteractableBase> OrderedInstances;

		public static readonly HashSet<Scp079InteractableBase> AllInstances;

		private static int _instancesCount;

		public ushort SyncId { get; private set; }

		public Vector3 Position { get; private set; }

		public virtual RoomIdentifier Room { get; private set; }

		protected virtual void OnRegistered()
		{
		}

		protected virtual void Awake()
		{
		}

		protected virtual void OnDestroy()
		{
		}

		public override string ToString()
		{
			return null;
		}

		public static bool TryGetInteractable(ushort syncId, out Scp079InteractableBase result)
		{
			result = null;
			return false;
		}

		public static bool TryGetInteractable<T>(ushort syncId, out T result) where T : Scp079InteractableBase
		{
			result = null;
			return false;
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static void RegisterIds()
		{
		}

		private static void HandleInstance(Scp079InteractableBase instance)
		{
		}

		private static bool CheckPriority(Scp079InteractableBase target, Scp079InteractableBase other)
		{
			return false;
		}
	}
}
