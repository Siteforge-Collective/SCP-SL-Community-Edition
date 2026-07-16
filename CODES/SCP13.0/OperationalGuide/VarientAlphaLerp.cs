using UnityEngine;
using UnityEngine.UI;

namespace OperationalGuide
{
	internal class VarientAlphaLerp : MonoBehaviour
	{
		public const float MinAlpha = 0.1f;

		public const float DistanceDivider = 350f;

		public RawImage VarientA;

		public RawImage VarientB;

		public bool IsASelected;

		private float _lerpA;

		private float _lerpB;

		private void Update()
		{
		}

		public Color LerpAlpha(Color startingColor, float lerpAmount)
		{
			return default(Color);
		}

		public void SetVarientStatus(bool isA)
		{
		}
	}
}
