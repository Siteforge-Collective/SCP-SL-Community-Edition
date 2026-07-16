using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.Map
{
	public class Scp079DetectedPlayerIndicator : MonoBehaviour
	{
		private ReferenceHub _trackedPlayer;

		private Transform _rotTr;

		private PlayerRoleBase _role;

		private RectTransform _rt;

		private IZoneMap[] _maps;

		private float _mainTimer;

		[SerializeField]
		private CanvasGroup _mainRoot;

		[SerializeField]
		private CanvasGroup _lostRoot;

		[SerializeField]
		private UiCircle _rippleCircle;

		[SerializeField]
		private AnimationCurve _mainFadeAnim;

		[SerializeField]
		private AnimationCurve _lostFadeAnim;

		[SerializeField]
		private AnimationCurve _rippleRadius;

		[SerializeField]
		private AnimationCurve _rippleWidth;

		[SerializeField]
		private int _mainRepeats;

		[SerializeField]
		private float _deleteTime;

		public void Setup(ReferenceHub ply, IZoneMap[] maps, RectTransform rotationTransform)
		{
		}

		private void Update()
		{
		}

		private bool UpdateMain()
		{
			return false;
		}

		private void UpdateLost()
		{
		}
	}
}
