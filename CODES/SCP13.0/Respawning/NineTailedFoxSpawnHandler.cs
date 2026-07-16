using System.Collections.Generic;
using PlayerRoles;

namespace Respawning
{
	public class NineTailedFoxSpawnHandler : ConfigBasedTeamSpawnHandler
	{
		public override float EffectTime => 0f;

		public override void GenerateQueue(Queue<RoleTypeId> queueToFill, int playersToSpawn)
		{
		}

		public NineTailedFoxSpawnHandler(string maxWaveSizeConfig, int defaultMaxWaveSize, string startTokensConfig, int defaultStartTokens)
			: base(null, 0, null, 0)
		{
		}
	}
}
