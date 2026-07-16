using GameCore;

namespace Achievements
{
    public readonly struct Achievement
    {
        private readonly string _steamName;
        private readonly string _steamProgress;
        private readonly long _discordId;
        private readonly int _maxValue;
        public readonly bool ActivatedByServer;

        public Achievement(string steamName, long discordId, bool byServer = false)
        {
            _steamName = steamName;
            _discordId = discordId;
            _steamProgress = string.Empty;
            _maxValue = 0;
            ActivatedByServer = byServer;
        }

        public Achievement(string steamName, string steamParameter, long discordId, int maxValue, bool byServer = false)
        {
            _steamName = steamName;
            _steamProgress = steamParameter;
            _discordId = discordId;
            _maxValue = maxValue;
            ActivatedByServer = byServer;
        }

        public void Achieve()
        {
        }

        public void AddProgress(int amt = 1)
        {
        }

        public void Reset()
        {
            switch (CentralAuthManager.Platform)
            {
                case DistributionPlatform.Steam:
                    SetAchievementSteam(false);
                    break;
                case DistributionPlatform.Discord:
                    SetAchievementDiscord(false);
                    break;
            }
        }

        private void SetAchievementDiscord(bool state)
        {
        }

        private void SetAchievementSteam(bool state)
        {
        }

        private void AddProgressDiscord(int progress)
        {
        }

        private void AddProgressSteam(int progress)
        {
        }
    }
}