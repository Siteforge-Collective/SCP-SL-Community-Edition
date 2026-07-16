using System.Text;
using PlayerRoles.Spectating;
using TMPro;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079
{
    public class Scp079SpectatableListElement : FullSizeSpectatableListElement
    {
        [SerializeField]
        private TextMeshProUGUI _info;

        private bool _isSet;
        private string _formatTier;
        private Scp079TierManager _tierMng;
        private readonly StringBuilder _sb = new StringBuilder();

        private void Awake()
        {
            _isSet = false;
            _formatTier = Translations.Get<Scp079HudTranslation>((Scp079HudTranslation)1);
        }

        protected override void OnTargetChanged(SpectatableModuleBase prevTarget, SpectatableModuleBase newTarget)
        {
            base.OnTargetChanged(prevTarget, newTarget);

            if (newTarget == null)
                return;

            Scp079Role mainRole = newTarget.MainRole as Scp079Role;
            if (mainRole == null)
                return;

            if (mainRole is Scp079Role && mainRole.SubroutineModule.TryGetSubroutine(out _tierMng))
            {
                _isSet = true;
            }
        }

        protected override void Update()
        {
            base.Update();

            if (!_isSet)
                return;

            _sb.Clear();

            int accessTier = _tierMng.AccessTierLevel;
            int thresholdsCount = _tierMng.NextLevelThreshold;
            int displayTier = Mathf.Clamp(accessTier, 0, thresholdsCount) + 1;

            _sb.AppendFormat(_formatTier, displayTier);

            int nextLevelThreshold = _tierMng.NextLevelThreshold;
            if (nextLevelThreshold > 0)
            {
                _sb.Append(" (");

                int clampedTier = Mathf.Clamp(accessTier, 0, thresholdsCount);
                int index = clampedTier - 1;
                int expProgress;

                if (index >= 0)
                {
                    expProgress = _tierMng.TotalExp - _tierMng.AbsoluteThresholds[index];
                }
                else
                {
                    expProgress = _tierMng.TotalExp;
                }

                int progress = Mathf.Min(nextLevelThreshold, Mathf.FloorToInt(expProgress));
                
                _sb.Append(progress);
                _sb.Append("/");
                _sb.Append(nextLevelThreshold);
                _sb.Append(")");
            }

            _info.text = _sb.ToString();
        }
    }
}
