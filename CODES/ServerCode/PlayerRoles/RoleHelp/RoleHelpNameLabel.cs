namespace PlayerRoles.RoleHelp
{
	public class RoleHelpNameLabel : global::UnityEngine.MonoBehaviour
	{
		[global::UnityEngine.SerializeField]
		private global::TMPro.TMP_Text _text;

		private const string NicknameFile = "Class_Nicknames";

		private void Awake()
		{
			if (ReferenceHub.TryGetLocalHub(out var hub))
			{
				global::PlayerRoles.PlayerRoleBase currentRole = hub.roleManager.CurrentRole;
				_text.text = string.Format(_text.text, currentRole.RoleName, TranslationReader.Get("Class_Nicknames", (int)currentRole.RoleTypeId));
			}
		}
	}
}
