using UnityEngine;

namespace OperationalGuide
{
    public class PannableObject : MonoBehaviour
    {
        private const float RotationSpeed = 400f;
        private const float IdleRotationSpeed = 25f;
        private const float InertiaDecay = 0.994f;
        private const float SlowAmount = 0.1f;
        private const float AgressiveInertiaDecay = 0.96f;

        public bool UserInput;
        public float XAxisModifer = -1f;
        public float YAxisModifer = 1f;

        private float _verticalInertia;
        private float _horizontalInertia;

        private void Start()
        {
            OnPannableStart();
        }

        private void OnEnable()
        {
            UserInput = false;
            OnPannableEnable();
        }

        private void OnDisable()
        {
            UserInput = false;
            OnPannableDisable();
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                UserInput = true;

                float mouseY = Input.GetAxis("Mouse Y");
                float sens = UserSettings.ControlsSettings
                                             .SensitivitySettings
                                             .SensMultiplier;
                _verticalInertia = mouseY * RotationSpeed * Time.deltaTime * sens;

                float mouseX = Input.GetAxis("Mouse X");
                float sens2 = UserSettings.ControlsSettings
                                             .SensitivitySettings
                                             .SensMultiplier;
                _horizontalInertia = mouseX * RotationSpeed * Time.deltaTime * sens2;

                transform.Rotate(
                    _verticalInertia * YAxisModifer,
                    _horizontalInertia * XAxisModifer,
                    0f,
                    Space.World
                );
            }
            else
            {
                _verticalInertia *= InertiaDecay;         
                _horizontalInertia *= InertiaDecay;

                _verticalInertia *= AgressiveInertiaDecay; 
                _horizontalInertia *= AgressiveInertiaDecay;

                transform.Rotate(
                    _verticalInertia,
                    _horizontalInertia,
                    0f,
                    Space.World
                );

                if (!UserInput)
                {
                    transform.Rotate(
                        0f,
                        Time.deltaTime * IdleRotationSpeed,
                        0f,
                        Space.World
                    );
                }
            }

            OnUpdate();
        }

        public virtual void OnUpdate() { }
        public virtual void OnPannableEnable() { }
        public virtual void OnPannableDisable() { }
        public virtual void OnPannableStart() { }
    }
}