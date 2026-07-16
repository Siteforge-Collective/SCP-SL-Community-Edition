using UnityEngine;

namespace OperationalGuide
{
	public class OperationalPannablePage : OperationalPage
	{
		public PannableObject PannableObject;

		public GameObject PannableImage;

		private Vector3 _defaultScale;

		private Quaternion _defaultRotation;

		public override void OnPageEnable()
		{
		}

		public override void OnPageDisable()
		{
		}

		public void ResetUserInput()
		{
		}
	}
}
