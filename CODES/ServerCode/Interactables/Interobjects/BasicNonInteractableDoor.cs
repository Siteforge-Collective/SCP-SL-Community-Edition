namespace Interactables.Interobjects
{
	public class BasicNonInteractableDoor : global::Interactables.Interobjects.BasicDoor, global::Interactables.Interobjects.DoorUtils.INonInteractableDoor, global::Interactables.Interobjects.DoorUtils.IScp106PassableDoor
	{
		[global::UnityEngine.SerializeField]
		private bool _ignoreLockdowns;

		[global::UnityEngine.SerializeField]
		private bool _ignoreRemoteAdmin;

		[global::UnityEngine.SerializeField]
		private bool _blockScp106;

		public bool IgnoreLockdowns => _ignoreLockdowns;

		public bool IgnoreRemoteAdmin => _ignoreRemoteAdmin;

		public bool IsScp106Passable => !_blockScp106;

		private void MirrorProcessed()
		{
		}
	}
}
