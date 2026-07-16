using Mirror;
using PlayerStatsSystem;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp106
{
    public class VigorStat : SyncedStatBase
    {
        private const float StartAmount = 0f;

        public override SyncMode Mode => SyncMode.PrivateAndSpectators;

        public override float MinValue => 0f;

        public override float MaxValue => 1f;

        public override bool CheckDirty(float prevValue, float newValue)
        {
            return ToByte(prevValue) != ToByte(newValue);
        }

        public override float ReadValue(NetworkReader reader)
        {
            return ToFloat(reader.ReadByte());
        }

        public override void WriteValue(NetworkWriter writer)
        {
            writer.WriteByte(ToByte(CurValue));
        }

        internal override void ClassChanged()
        {
            base.ClassChanged();
            if (NetworkServer.active)
            {
                CurValue = 0f;
            }
        }

        private byte ToByte(float val)
        {
            return (byte)Mathf.CeilToInt(val * 255f);
        }

        private float ToFloat(byte val)
        {
            return (float)(int)val / 255f;
        }
    }
}
