using UnityEngine;

namespace InventorySystem.Items.Firearms
{
	public abstract class FirearmAnimatorEventsBase : MonoBehaviour
	{
		private Firearm _cachedFirearm;

		private bool _cacheSet;

		private bool _isServerControlled;

		private AnimatedFirearmViewmodel _afv;

		protected AnimatedFirearmViewmodel ViewModel => null;

		protected Firearm TargetFirearm => null;

		protected bool IsServerController => false;

		private void SetupCache()
		{
		}

		protected void ModifyUserAmmo(int ammoToModify)
		{
		}

		protected virtual void PlaySound(int soundId)
		{
		}
	}
}
