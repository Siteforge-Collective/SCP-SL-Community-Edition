using PlayerRoles.PlayableScps.Scp079.Cameras;
using TMPro;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079CamNameGui : Scp079GuiElementBase
	{
		[SerializeField]
		private TextMeshProUGUI _label;

		private Scp079CurrentCameraSync _curCamSync;

		internal override void Init(Scp079Role role, ReferenceHub owner)
		{
		}

		private void Update()
		{
		}
	}
}
