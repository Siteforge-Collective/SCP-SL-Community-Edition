using System.Text;
using PlayerRoles.PlayableScps.Scp079.GUI;
using PlayerRoles.PlayableScps.Scp079.Map;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079ScannerMenuToggler : Scp079ToggleMenuAbilityBase<Scp079ScannerMenuToggler>, IScp079LevelUpNotifier
	{
		[SerializeField]
		private int _minimalTierIndex;

		public bool IsUnlocked => false;

		public override ActionName ActivationKey => default(ActionName);

		public override bool IsVisible => false;

		protected override Scp079HudTranslation OpenTranslation => default(Scp079HudTranslation);

		protected override Scp079HudTranslation CloseTranslation => default(Scp079HudTranslation);

		public bool WriteLevelUpNotification(StringBuilder sb, int newLevel)
		{
			return false;
		}

		protected override void Update()
		{
		}
	}
}
