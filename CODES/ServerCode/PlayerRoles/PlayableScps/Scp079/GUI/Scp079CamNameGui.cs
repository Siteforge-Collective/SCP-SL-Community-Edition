namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079CamNameGui : global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079GuiElementBase
	{
		[global::UnityEngine.SerializeField]
		private global::TMPro.TextMeshProUGUI _label;

		private global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync _curCamSync;

		internal override void Init(global::PlayerRoles.PlayableScps.Scp079.Scp079Role role, ReferenceHub owner)
		{
			base.Init(role, owner);
			role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync>(out _curCamSync);
		}

		private void Update()
		{
			_label.text = _curCamSync.CurrentCamera.Label;
		}
	}
}
