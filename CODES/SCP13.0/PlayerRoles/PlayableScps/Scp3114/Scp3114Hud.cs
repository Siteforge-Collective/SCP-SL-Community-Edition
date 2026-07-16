using InventorySystem.Items;
using PlayerRoles.PlayableScps.HUDs;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerRoles.PlayableScps.Scp3114
{
	public class Scp3114Hud : ViewmodelScpHud
	{
		[SerializeField]
		private GameObject _hudRoot;

		[SerializeField]
		private ScpWarningHud _warning;

		[SerializeField]
		private AbilityHud _strangleCooldown;

		[SerializeField]
		private AbilityHud _identityCooldown;

		[SerializeField]
		private LoadingCircleHud _stealSkinBar;

		[SerializeField]
		private RawImage _avatarImage;

		private Scp3114Strangle _strangle;

		private Scp3114Identity _identity;

		private Scp3114Disguise _disguise;

		private Scp3114Role _scpRole;

		private bool _wasLocalPlayer;

		private Texture CurAvatar => null;

		internal override void Init(ReferenceHub hub)
		{
		}

		internal override void OnDied()
		{
		}

		protected override void OnDestroy()
		{
		}

		protected override void Update()
		{
		}

		private void OnIdentityChanged()
		{
		}

		private void OnDisguiseErrorReceived(byte errorCode)
		{
		}

		private void OnAttackWhileDisguised()
		{
		}

		private void OnRevealFail()
		{
		}

		private void OnVcHintTriggered()
		{
		}

		private void OnCurrentItemChanged(ReferenceHub ply, ItemIdentifier prevItem, ItemIdentifier newItem)
		{
		}

		private void PlayWarning(Scp3114HudTranslation text)
		{
		}

		private void PlayWarning(Scp3114HudTranslation text, string formatArg0)
		{
		}

		private void PlayWarning(Scp3114HudTranslation text, ActionName action)
		{
		}
	}
}
