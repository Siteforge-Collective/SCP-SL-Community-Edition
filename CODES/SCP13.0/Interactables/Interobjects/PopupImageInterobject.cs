using UnityEngine;
using UnityEngine.UI;

namespace Interactables.Interobjects
{
    public class PopupImageInterobject : PopupInterobject
    {
        [SerializeField]
        public Sprite ImageToDisplay;

        [SerializeField]
        public Vector2 Resolution;

        private static UserMainInterface UserGUI => UserMainInterface.Singleton;

        protected override void OnClientStateChange()
        {
            TrackedPosition = transform.position;
            if (UserGUI != null)
            {
                UserGUI.imageDisplayGroup.gameObject.SetActive(true);
                UserGUI.imageToDisplay.sprite = ImageToDisplay;
                UserGUI.imageToDisplay.rectTransform.sizeDelta = Resolution;
                UserGUI.imageToDisplay.color = Color.white;
            }
        }

        protected override void OnClientUpdate(float enableRatio)
        {
            if (UserGUI != null)
            {
                UserGUI.imageDisplayGroup.alpha = enableRatio;
            }
        }

        public PopupImageInterobject()
        {
            FadeSpeedSeconds = 0.2f;
        }
    }
}