using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp939.Mimicry
{
	public class EnvMimicryTeammateButton : EnvMimicryStandardButton
	{
		[SerializeField]
		private RoleTypeId _targetRole;

		private bool _isActive;

		private bool _eventAssigned;

		private bool EventAssigned
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		protected override bool IsAvailable => false;

		protected override void Awake()
		{
		}

		protected override void OnDestroy()
		{
		}

		private void OnRoleChanged(ReferenceHub hub, PlayerRoleBase prevRole, PlayerRoleBase newRole)
		{
		}
	}
}
