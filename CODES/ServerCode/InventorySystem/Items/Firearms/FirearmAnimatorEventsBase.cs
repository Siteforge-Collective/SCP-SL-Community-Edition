namespace InventorySystem.Items.Firearms
{
	public abstract class FirearmAnimatorEventsBase : global::UnityEngine.MonoBehaviour
	{
		private global::InventorySystem.Items.Firearms.Firearm _cachedFirearm;

		private bool _cacheSet;

		protected bool IsServerController;

		protected global::InventorySystem.Items.Firearms.Firearm TargetFirearm
		{
			get
			{
				if (_cacheSet)
				{
					return _cachedFirearm;
				}
				if (TryGetComponent<global::UnityEngine.Animator>(out var component))
				{
					component.cullingMode = global::UnityEngine.AnimatorCullingMode.AlwaysAnimate;
				}
				else
				{
					global::UnityEngine.Debug.LogError("Firearm " + base.name + " does not have an animator.");
				}
				if (TryGetComponent<global::InventorySystem.Items.Firearms.Firearm>(out var component2))
				{
					TargetFirearm = component2;
					IsServerController = true;
				}
				else
				{
					IsServerController = false;
				}
				return _cachedFirearm;
			}
			set
			{
				_cachedFirearm = value;
				_cacheSet = true;
			}
		}

		protected void ModifyUserAmmo(int ammoToModify)
		{
			TargetFirearm.OwnerInventory.ServerAddAmmo(TargetFirearm.AmmoType, ammoToModify);
		}

		protected virtual void PlaySound(int soundId)
		{
			if (soundId < TargetFirearm.AudioClips.Length)
			{
				global::InventorySystem.Items.Firearms.FirearmAudioClip firearmAudioClip = TargetFirearm.AudioClips[soundId];
				bool flag = firearmAudioClip.HasFlag(global::InventorySystem.Items.Firearms.FirearmAudioFlags.SendToPlayers);
				if (global::Mirror.NetworkServer.active && IsServerController && flag)
				{
					TargetFirearm.ServerSendAudioMessage((byte)soundId);
				}
			}
		}
	}
}
