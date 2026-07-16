using PlayerRoles.FirstPersonControl;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp049
{
	public class Scp049MovementModule : FirstPersonMovementModule
	{
		[SerializeField]
		private Scp049Role _role;

		private float _normalSpeed;

		private float _enragedSpeed;

		private Scp049SenseAbility _senseAbility;

		private float MovementSpeed
		{
			set
			{
			}
		}

		private void Awake()
		{
		}

		protected override void UpdateMovement()
		{
		}
	}
}
