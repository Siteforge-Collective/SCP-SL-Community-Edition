using TMPro;
using UnityEngine;

namespace LightContainmentZoneDecontamination
{
    public class DecontaminationScreen : MonoBehaviour
    {
        public Animator AnimationController;

        public TextMeshPro CountdownText;

        public bool InsideLCZ;

        private int _animHash;

        private int _lczHash;

        private bool _singletonSet;

        private void Start()
        {
            _animHash = Animator.StringToHash("Time");
            _lczHash = Animator.StringToHash("InsideLCZ");
        }

        private void Update()
        {
            if (!_singletonSet)
            {
                if (DecontaminationController.Singleton == null)
                {
                    return;
                }
                _singletonSet = true;
            }

            DecontaminationController controller = DecontaminationController.Singleton;
            if (!controller.ClientTimer.enabled)
            {
                enabled = false;
                return;
            }

            if (controller.DecontaminationOverride == DecontaminationController.DecontaminationStatus.Forced)
            {
                enabled = false;
                return;
            }

            AnimationController.SetBool(_lczHash, InsideLCZ);
            AnimationController.SetFloat(_animHash, DecontaminationClientTimer.RemainingTimeInSeconds);
            CountdownText.text = DecontaminationClientTimer.ScreenTimeString;
        }
    }
}