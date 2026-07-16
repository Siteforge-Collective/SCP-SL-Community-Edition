using UnityEngine;
using UnityEngine.UI;

namespace OperationalGuide
{
	public class OperationalGuide : MonoBehaviour
	{
		public static readonly float MinZoom;

		public static readonly float MaxZoom;

		public static Vector3 ScaleModifier;

		public static OperationalGuide Instance;

		public OperationalTabController[] Tabs;

		public Camera Camera;

		public Animator FadeAnimator;

		public GameObject Back;

		public GameObject FullscreenPannable;

		public RawImage FullscreenPannableImage;

		[SerializeField]
		private RenderTexture _regularRenderTexture;

		[SerializeField]
		private RenderTexture _fullscreenRenderTexture;

		private const float ZoomIn = 1f;

		private const float ZoomOut = -1f;

		public void ButtonChangeTab(int index)
		{
		}

		public static void ChangeZoom(Transform transform, bool increase, bool panImage = false)
		{
		}

		private void Update()
		{
		}

		public void SetFullscreen(bool state)
		{
		}

		public OperationalTabController GetActiveTab()
		{
			return null;
		}

		public void BackCurrentPage()
		{
		}

		public void ToggleCurrentPage()
		{
		}

		private void Awake()
		{
		}

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}
	}
}
