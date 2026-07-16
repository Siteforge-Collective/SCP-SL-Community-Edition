using UnityEngine;

namespace InventorySystem.Items.Firearms.FunctionalParts
{
	public class AnimatedLaser : FunctionalFirearmPart
	{
		[SerializeField]
		private Transform _forwardTransform;

		[SerializeField]
		private Light _targetLight;

		[SerializeField]
		private AnimationCurve _truthnessOverAngle;

		[SerializeField]
		private Color _laserColor;

		private Transform _camForward;

		private void Start()
		{
		}

		private void LateUpdate()
		{
		}
	}
}
