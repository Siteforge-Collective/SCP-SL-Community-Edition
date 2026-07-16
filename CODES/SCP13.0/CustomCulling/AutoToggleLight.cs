using UnityEngine;

namespace CustomCulling
{
    public class AutoToggleLight : MonoBehaviour
    {
        private Light _light;

        private void Start()
        {
            _light = GetComponent<Light>();
        }

        private void Update()
        {
            if (_light != null)
            {
                _light.enabled = false;
            }
        }
    }
}