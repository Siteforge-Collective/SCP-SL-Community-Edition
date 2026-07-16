using System.Linq;
using Mirror;
using TMPro;
using UnityEngine;

namespace PlayerRoles.Voice
{
    public class IntercomDisplay : NetworkBehaviour
    {
        private enum IcomText
        {
            Ready = 0,
            Transmitting = 1,
            TrasmittingBypass = 2,
            Restarting = 3,
            AdminUsing = 4,
            Muted = 5,
            Unknown = 6,
            Wait = 7
        }

        private static IntercomDisplay _singleton;

        [SerializeField]
        private TMP_Text _text;

        [SyncVar]
        private string _overrideText;

        private Intercom _icom;

        private string[] _translations;

        private bool[] _translationsSet;

        private void Awake()
        {
            _icom = GetComponent<Intercom>();
            _singleton = this;

            int maxVal = 0;
            foreach (var value in System.Enum.GetValues(typeof(IcomText)))
            {
                maxVal = Mathf.Max(maxVal, (int)value + 1);
            }

            _translations = new string[maxVal];
            _translationsSet = new bool[maxVal];
        }

        private void Update()
        {
            if (!string.IsNullOrEmpty(_overrideText))
            {
                _text.text = _overrideText;
                return;
            }

            IntercomState state = Intercom.State;

            switch (state)
            {
                case IntercomState.Ready:
                    _text.text = GetTranslation(IcomText.Ready);
                    break;

                case IntercomState.InUse:
                    IcomText icomType = _icom.BypassMode ? IcomText.TrasmittingBypass : IcomText.Transmitting;
                    string trans = GetTranslation(icomType);
                    _text.text = string.Format(trans, Mathf.CeilToInt(_icom.RemainingTime));
                    break;

                case IntercomState.Cooldown:
                    string cooldownTrans = GetTranslation(IcomText.Restarting);
                    _text.text = string.Format(cooldownTrans, Mathf.CeilToInt(_icom.RemainingTime));
                    break;

                default:
                    _text.text = GetTranslation(IcomText.Unknown);
                    break;
            }
        }

        private string GetTranslation(IcomText val)
        {
            int index = (int)val;
            if (!_translationsSet[index])
            {
                string key = val.ToString();
                _translations[index] = TranslationReader.Get("Intercom", index, key);
                _translationsSet[index] = true;
            }
            return _translations[index];
        }

        [Server]
        public static bool TrySetDisplay(string str)
        {
            if (_singleton == null)
                return false;

            _singleton._overrideText = str;
            return true;
        }
    }
}