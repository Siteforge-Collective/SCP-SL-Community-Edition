using PlayerRoles.PlayableScps.Scp079.Cameras;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079NicknameGui : Scp079GuiElementBase
	{
		[SerializeField]
		private Camera _cam;

		[SerializeField]
		private RectTransform _fullscreenRect;

		[SerializeField]
		private RectTransform _template;

		private Scp079CurrentCameraSync _camSync;

		private NicknameGuiDrawer _nicknameGuiDrawer;

		private void Start()
		{
		}

		private void LateUpdate()
		{
		}

		private void Redraw()
		{
		}

		internal override void Init(Scp079Role role, ReferenceHub owner)
		{
		}
	}
}
