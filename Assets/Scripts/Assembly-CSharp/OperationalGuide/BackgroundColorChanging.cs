using UnityEngine;
using UnityEngine.UI;

namespace OperationalGuide
{
    public class BackgroundColorChanging : MonoBehaviour
    {
        [Header("Target & Colors")]
        public Image Background;
        
        public Color ColorIni = Color.white;
        
        public Color ColorFin = Color.red;

        [Header("Trigger Settings")]
        [Range(0f, 1f)]
        public float Chance = 0.8f;

        private int _framesActive;

        private void Start()
        {

            if (ClutterSpawner.IsHolidayActive(Holidays.Christmas))
            {
                ColorIni = Color.red;    
                ColorFin = Color.green; 
            }

            if (Background != null)
                Background.color = ColorIni;
        }

        private void Update()
        {
            int previousFrames = _framesActive;
            _framesActive = 0; 

            if (previousFrames != 0)
            {
                _framesActive = previousFrames + 1;
                return;
            }

            if (Random.value <= Chance) return;

            _framesActive = 1;
            if (Background != null)
                Background.color = ColorFin;
        }
    }
}