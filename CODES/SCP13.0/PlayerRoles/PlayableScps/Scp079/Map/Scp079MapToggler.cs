using Mirror;
using ToggleableMenus;

namespace PlayerRoles.PlayableScps.Scp079.Map
{
	public class Scp079MapToggler : Scp079ToggleMenuAbilityBase<Scp079MapToggler>, IRegisterableMenu
	{
		public override ActionName ActivationKey => default(ActionName);

		protected override Scp079HudTranslation OpenTranslation => default(Scp079HudTranslation);

		protected override Scp079HudTranslation CloseTranslation => default(Scp079HudTranslation);

		public override bool IsVisible => false;

		public bool IsEnabled
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		private void LateUpdate()
		{
		}

		protected override void Trigger()
		{
		}

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}

		public override void ClientWriteCmd(NetworkWriter writer)
		{
		}

		public override void ServerProcessCmd(NetworkReader reader)
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
