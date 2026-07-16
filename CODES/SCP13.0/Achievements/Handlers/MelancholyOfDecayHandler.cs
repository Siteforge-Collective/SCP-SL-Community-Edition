using System.Collections.Generic;
using System.Diagnostics;
using PlayerRoles.PlayableScps.Scp106;

namespace Achievements.Handlers
{
    public class MelancholyOfDecayHandler : AchievementHandlerBase
    {
        private const int MinTime = 5;

        private static Dictionary<ReferenceHub, Stopwatch> Timers = new Dictionary<ReferenceHub, Stopwatch>();

        internal override void OnInitialize()
        {
            Scp106SinkholeController.OnSubmergeStateChange += OnSubmergeStateChange;
            Scp106Attack.OnPlayerTeleported += OnPlayerTeleported;
        }

        internal override void OnRoundStarted()
        {
            Timers.Clear();
        }

        private void OnPlayerTeleported(ReferenceHub scp106, ReferenceHub hub)
        {
            if (Timers.TryGetValue(scp106, out var value) && value.Elapsed.TotalSeconds < 6.900000095367432)
            {
                AchievementHandlerBase.ServerAchieve(scp106.connectionToClient, AchievementName.MelancholyOfDecay);
                Timers.Remove(scp106);
            }
        }

        private void OnSubmergeStateChange(ReferenceHub scp106, bool newState)
        {
            if (!newState)
            {
                Timers[scp106] = Stopwatch.StartNew();
            }
        }
    }
}
