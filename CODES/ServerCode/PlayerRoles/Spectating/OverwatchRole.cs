namespace PlayerRoles.Spectating
{
	public class OverwatchRole : global::PlayerRoles.Spectating.SpectatorRole, global::PlayerRoles.IObfuscatedRole
	{
		[global::UnityEngine.SerializeField]
		private global::TMPro.TextMeshProUGUI _hudTemplate;

		private bool _hudSetup;

		private global::TMPro.TextMeshProUGUI _hudInstance;

		public override global::PlayerRoles.RoleTypeId RoleTypeId => global::PlayerRoles.RoleTypeId.Overwatch;

		public override global::UnityEngine.Color RoleColor => global::UnityEngine.Color.cyan;

		public override bool ReadyToRespawn => false;

		public global::PlayerRoles.RoleTypeId GetRoleForUser(ReferenceHub receiver)
		{
			if (!PermissionsHandler.IsPermitted(receiver.serverRoles.Permissions, PlayerPermissions.GameplayData))
			{
				return base.RoleTypeId;
			}
			return RoleTypeId;
		}

		private void Update()
		{
			if (SetupHud())
			{
				_hudInstance.text = (global::PlayerRoles.Spectating.SpectatorTargetTracker.TryGetTrackedPlayer(out var hub) ? $"PlayerID: {hub.PlayerId}{(global::System.Environment.NewLine)}Nickname: {hub.nicknameSync.MyNick}" : string.Empty);
			}
		}

		private bool SetupHud()
		{
			if (!base.IsLocalPlayer || _hudSetup)
			{
				return _hudSetup;
			}
			if (!global::PlayerRoles.Spectating.SpectatorTargetTracker.TrackerSet)
			{
				return false;
			}
			global::UnityEngine.GameObject gameObject = global::UnityEngine.GameObject.Find("Spectator Canvas");
			if (gameObject == null)
			{
				return false;
			}
			_hudInstance = global::UnityEngine.Object.Instantiate(_hudTemplate, gameObject.transform);
			global::UnityEngine.RectTransform rectTransform = _hudInstance.rectTransform;
			rectTransform.anchoredPosition = global::UnityEngine.Vector3.zero;
			rectTransform.localScale = global::UnityEngine.Vector3.one;
			rectTransform.localRotation = global::UnityEngine.Quaternion.identity;
			_hudSetup = true;
			return true;
		}

		public override void DisableRole(global::PlayerRoles.RoleTypeId newRole)
		{
			base.DisableRole(newRole);
			if (_hudSetup && !(_hudInstance == null))
			{
				_hudSetup = false;
				global::UnityEngine.Object.Destroy(_hudInstance.gameObject);
			}
		}
	}
}
