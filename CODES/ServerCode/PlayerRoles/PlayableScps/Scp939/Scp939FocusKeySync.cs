namespace PlayerRoles.PlayableScps.Scp939
{
	public class Scp939FocusKeySync : global::PlayerRoles.PlayableScps.Subroutines.ScpKeySubroutine<global::PlayerRoles.PlayableScps.Scp939.Scp939Role>
	{
		private global::PlayerRoles.PlayableScps.Scp939.Scp939FocusAbility _focus;

		protected override ActionName TargetKey => ActionName.Sneak;

		protected override bool IsKeyHeld
		{
			get
			{
				return base.IsKeyHeld;
			}
			set
			{
				if (IsKeyHeld != value)
				{
					base.IsKeyHeld = value;
					ClientSendCmd();
				}
			}
		}

		protected override bool KeyPressable
		{
			get
			{
				if (base.Owner.isLocalPlayer)
				{
					if (global::UnityEngine.Cursor.visible)
					{
						return _focus.TargetState;
					}
					return true;
				}
				return false;
			}
		}

		public bool FocusKeyHeld { get; private set; }

		public override void ClientWriteCmd(global::Mirror.NetworkWriter writer)
		{
			base.ClientWriteCmd(writer);
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, IsKeyHeld);
		}

		public override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			base.ServerProcessCmd(reader);
			FocusKeyHeld = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
		}

		public override void ResetObject()
		{
			base.ResetObject();
			FocusKeyHeld = false;
		}

		protected override void Awake()
		{
			base.Awake();
			GetSubroutine<global::PlayerRoles.PlayableScps.Scp939.Scp939FocusAbility>(out _focus);
		}
	}
}
