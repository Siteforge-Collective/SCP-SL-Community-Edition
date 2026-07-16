using Mirror;

namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079DoorLockReleaser : Scp079KeyAbilityBase
	{
		private static string _releaseMessage;

		private Scp079DoorLockChanger _lockChanger;

		private const string ColorFormat = "<color=#ffffff{0}>{1}</color>";

		private const float BlinkRate = 2.8f;

		public override ActionName ActivationKey => default(ActionName);

		public override bool IsReady => false;

		public override bool IsVisible => false;

		public override string AbilityName => null;

		public override string FailMessage => null;

		private string Transparency => null;

		protected override void Trigger()
		{
		}

		protected override void Start()
		{
		}

		public override void ServerProcessCmd(NetworkReader reader)
		{
		}
	}
}
