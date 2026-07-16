using PlayerRoles;
using RemoteAdmin.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace RemoteAdmin
{
	public class RoleElement : ValueButton
	{
		[SerializeField]
		private RawImage _avatar;

		[SerializeField]
		private Image _background;

		[SerializeField]
		private Image _border;

		[SerializeField]
		private float _backgroundAlpha;

		private RoleTypeId _roleTypeId;

		public PlayerRoleBase Role { get; set; }

		public RoleTypeId RoleTypeId
		{
			get
			{
				return default(RoleTypeId);
			}
			set
			{
			}
		}

		public void SetupInterface(PlayerRoleBase role, RoleTypeId roleType)
		{
		}

		public override void SetState(bool isSelected)
		{
		}

		private Color GenerateBackground(Color originalColor)
		{
			return default(Color);
		}
	}
}
