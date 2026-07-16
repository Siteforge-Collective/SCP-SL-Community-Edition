using System;
using System.Runtime.CompilerServices;
using Mirror;
using PlayerRoles.Subroutines;

namespace PlayerRoles.PlayableScps.Scp3114
{
	public class Scp3114Reveal : KeySubroutine<Scp3114Role>
	{
		private const float HoldDuration = 0.65f;

		public const ActionName RevealKey = ActionName.Reload;

		private float _holdTimer;

		protected override ActionName TargetKey => default(ActionName);

		protected override bool KeyPressable => false;

		public static event Action OnRevealFail
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		protected override void Update()
		{
		}

		protected override void OnKeyUp()
		{
		}

		public override void ServerProcessCmd(NetworkReader reader)
		{
		}
	}
}
