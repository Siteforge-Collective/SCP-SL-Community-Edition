namespace Interactables
{
	public class InteractableCollider : global::UnityEngine.MonoBehaviour
	{
		public global::UnityEngine.MonoBehaviour Target;

		public byte ColliderId;

		public global::UnityEngine.Vector3 VerificationOffset;

		public static readonly global::System.Collections.Generic.Dictionary<global::Interactables.IInteractable, global::System.Collections.Generic.Dictionary<byte, global::Interactables.InteractableCollider>> AllInstances = new global::System.Collections.Generic.Dictionary<global::Interactables.IInteractable, global::System.Collections.Generic.Dictionary<byte, global::Interactables.InteractableCollider>>();

		protected virtual void Awake()
		{
			if (Target is global::Interactables.IInteractable key)
			{
				if (!AllInstances.ContainsKey(key))
				{
					AllInstances[key] = new global::System.Collections.Generic.Dictionary<byte, global::Interactables.InteractableCollider>();
				}
				AllInstances[key][ColliderId] = this;
			}
			else
			{
				global::UnityEngine.Debug.LogError("Fatal error: '" + Target.name + "' is not IInteractable.");
			}
		}

		public static bool TryGetCollider(global::Interactables.IInteractable target, byte colliderId, out global::Interactables.InteractableCollider res)
		{
			if (AllInstances.TryGetValue(target, out var value) && value.TryGetValue(colliderId, out res))
			{
				return true;
			}
			res = null;
			return false;
		}
	}
}
