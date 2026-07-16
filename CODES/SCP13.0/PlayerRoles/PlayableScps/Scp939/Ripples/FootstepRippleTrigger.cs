using Mirror;
using PlayerRoles.FirstPersonControl.Thirdperson;
using RelativePositioning;

namespace PlayerRoles.PlayableScps.Scp939.Ripples
{
	public class FootstepRippleTrigger : RippleTriggerBase
	{
		private ReferenceHub _syncPlayer;

		private RelativePosition _syncPos;

		private byte _syncDistance;

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}

		private void OnFootstepPlayed(AnimatedCharacterModel model, float dis)
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
