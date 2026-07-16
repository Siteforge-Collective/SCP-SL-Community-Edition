using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PlayerRoles.Spectating
{
    public class OverwatchHelpButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public GameObject HelpDialog;
        public TextMeshProUGUI HelpText;
        public TextLanguageReplacer HelpTextReplacer;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (HelpDialog != null)
                HelpDialog.SetActive(true);

            if (HelpText != null && HelpTextReplacer != null)
            {
                HelpText.text = HelpTextReplacer.DisplayText;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (HelpDialog != null)
                HelpDialog.SetActive(false);
        }
    }
}