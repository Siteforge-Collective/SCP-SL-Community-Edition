namespace Interactables.Interobjects.DoorUtils
{
	public class DoorNametagExtension : global::Interactables.Interobjects.DoorUtils.DoorVariantExtension
	{
		public static readonly global::System.Collections.Generic.Dictionary<string, global::Interactables.Interobjects.DoorUtils.DoorNametagExtension> NamedDoors = new global::System.Collections.Generic.Dictionary<string, global::Interactables.Interobjects.DoorUtils.DoorNametagExtension>();

		[global::UnityEngine.SerializeField]
		private string _nametag;

		public string GetName => _nametag;

		private void Start()
		{
			UpdateName(_nametag);
		}

		private void FixedUpdate()
		{
		}

		public void UpdateName(string newName)
		{
			if (string.IsNullOrEmpty(newName))
			{
				global::UnityEngine.Debug.LogError("Nametag of " + base.transform.parent.name + "/" + base.name + " has not been set");
			}
			else
			{
				_nametag = newName;
				NamedDoors[newName] = this;
			}
		}
	}
}
