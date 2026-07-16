using System.Collections.Generic;
using PlayerRoles;

namespace Respawning
{
	public class ChaosInsurgencySpawnHandler : ConfigBasedTeamSpawnHandler
	{
		private const float LogicerPercent = 0.2f;

		private const float ShotgunPercent = 0.3f;

		public override float EffectTime => 0f;

		public override void GenerateQueue(Queue<RoleTypeId> queueToFill, int playersToSpawn)
		{
		}

		public ChaosInsurgencySpawnHandler(string maxWaveSizeConfig, int defaultMaxWaveSize, string startTokensConfig, int defaultStartTokens)
			: base(null, 0, null, 0)
		{
		}
	}
}
