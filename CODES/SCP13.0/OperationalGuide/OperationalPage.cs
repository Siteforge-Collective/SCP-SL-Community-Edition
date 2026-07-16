using UnityEngine;
using UnityEngine.UI;

namespace OperationalGuide
{
	public class OperationalPage : MonoBehaviour
	{
		public GameObject MainPage;

		public GameObject DescriptionPage;

		public Image BackgroundImage;

		public bool F1PageActive;

		public Animator PageAnimator;

		public virtual void ToggleDescriptionMenu()
		{
		}

		public virtual void TurnOn()
		{
		}

		public void ForceTurnOff()
		{
		}

		public virtual void OnPageEnable()
		{
		}

		public virtual void OnPageDisable()
		{
		}

		private void OnDisable()
		{
		}

		private void OnEnable()
		{
		}
	}
}
