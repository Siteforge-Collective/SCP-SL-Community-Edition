using PlayerRoles.PlayableScps.HumeShield;

namespace PlayerRoles.PlayableScps.Scp106
{
	public class Scp106HumeShieldController : DynamicHumeShieldController
	{
		private Scp106Role _role106;

		private Scp106StalkAbility _stalk;

		public override float HsRegeneration => 0f;

		public override void SpawnObject()
		{
		}
	}
}
