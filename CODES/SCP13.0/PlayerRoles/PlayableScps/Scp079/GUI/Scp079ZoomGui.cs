using PlayerRoles.PlayableScps.Scp079.Cameras;
using TMPro;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079ZoomGui : Scp079GuiElementBase
	{
		private const float RoundingAccuracy = 10f;

		private Scp079CurrentCameraSync _curCamSync;

		private string _format;

		[SerializeField]
		private Vector2 _minPos;

		[SerializeField]
		private Vector2 _maxPos;

		[SerializeField]
		private RectTransform _slider;

		[SerializeField]
		private TextMeshProUGUI _text;

		[SerializeField]
		private GameObject _root;

		internal override void Init(Scp079Role role, ReferenceHub owner)
		{
		}

		private void Update()
		{
		}
	}
}
