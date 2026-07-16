using Mirror;
using PlayerRoles.Subroutines;

namespace PlayerRoles.PlayableScps.Scp939
{
	public class Scp939FocusKeySync : KeySubroutine<Scp939Role>
	{
		private Scp939FocusAbility _focus;

		protected override ActionName TargetKey => default(ActionName);

		protected override bool IsKeyHeld
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		protected override bool KeyPressable => false;

		public bool FocusKeyHeld { get; private set; }

		public override void ClientWriteCmd(NetworkWriter writer)
		{
		}

		public override void ServerProcessCmd(NetworkReader reader)
		{
		}

		public override void ResetObject()
		{
		}

		protected override void Awake()
		{
		}
	}
}
