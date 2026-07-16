using NorthwoodLib.Pools;
using System.Text;
using UnityEngine;

namespace LightContainmentZoneDecontamination
{
    public class DecontaminationClientTimer : MonoBehaviour
    {
        public static float RemainingTimeInSeconds;

        public static string ScreenTimeString;

        public float ClientTimer;

        [SerializeField]
        private AnimationCurve deviationSpeedup;

        [SerializeField]
        private float maxDeviation = 2f;

        [SerializeField]
        public AnimationCurve ScaledTimer;

        private const int DigitsSpacingSize = 20;

        private const int SeparatorLeftSpacingSize = 1;

        private const int ColonRightSpacingSize = 10;

        private const int DotRightSpacingSize = 1;

        private int _lastMinutes = -1;

        private int _lastSeconds = -1;

        private int _lastMilliseconds = -1;

        public void UpdateTimer(float serverTime)
        {
            float diff = serverTime - ClientTimer;
            if (diff > 0f && diff <= maxDeviation)
            {
                ClientTimer += deviationSpeedup.Evaluate(diff / maxDeviation) * Time.deltaTime;
            }
            else
            {
                ClientTimer = serverTime;
            }
        }

        private void Update()
        {
            DecontaminationController controller = DecontaminationController.Singleton;
            float scaledTime = ScaledTimer.Evaluate(ClientTimer);
            if (controller.DecontaminationOverride == DecontaminationController.DecontaminationStatus.Forced)
            {
                enabled = false;
            }
            RemainingTimeInSeconds = scaledTime;

            int minutes = Mathf.FloorToInt(RemainingTimeInSeconds / 60f);
            int seconds = Mathf.FloorToInt(RemainingTimeInSeconds % 60f);
            int milliseconds = Mathf.FloorToInt((RemainingTimeInSeconds % 1f) * 100f);

            if (minutes != _lastMinutes || seconds != _lastSeconds || milliseconds != _lastMilliseconds)
            {
                _lastMinutes = minutes;
                _lastSeconds = seconds;
                _lastMilliseconds = milliseconds;

                StringBuilder builder = StringBuilderPool.Shared.Rent();
                AppendDigits(builder, minutes);
                AppendColon(builder);
                AppendDigits(builder, seconds);
                AppendDot(builder);
                AppendDigits(builder, milliseconds);
                ScreenTimeString = StringBuilderPool.Shared.ToStringReturn(builder);
            }
        }

        internal static void AppendDigits(StringBuilder builder, int time)
        {
            if (time < 10)
            {
                builder.Append("<size=").Append(DigitsSpacingSize).Append("> </size>0");
            }
            else
            {
                builder.Append((time / 10).ToString());
            }
            builder.Append(time % 10);
        }

        internal static void AppendColon(StringBuilder builder)
        {
            builder.Append("<size=").Append(SeparatorLeftSpacingSize).Append("> </size>:")
                   .Append("<size=").Append(ColonRightSpacingSize).Append("> </size>");
        }

        internal static void AppendDot(StringBuilder builder)
        {
            builder.Append("<size=").Append(SeparatorLeftSpacingSize).Append("> </size>.")
                   .Append("<size=").Append(DotRightSpacingSize).Append("> </size>");
        }
    }
}