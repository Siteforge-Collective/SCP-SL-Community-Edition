using InventorySystem.Items.SwayControllers;
using UnityEngine;

namespace PlayerRoles.PlayableScps.HUDs
{
	public abstract class ScpViewmodelBase : MonoBehaviour
	{
		private bool _destroyed;

		private GoopSway _sway;

		[SerializeField]
		private Vector3 _localRotation;

		[SerializeField]
		private Vector3 _trackerOffset;

		[SerializeField]
		private Transform _cameraBone;

		[SerializeField]
		private GoopSway.GoopSwaySettings _swaySettings;

		public abstract float CamFOV { get; }

		protected ScpHudBase Hud { get; private set; }

		protected ReferenceHub Owner { get; private set; }

		protected PlayerRoleBase Role { get; private set; }

		[field: SerializeField]
		protected Animator Anim { get; private set; }

		protected virtual void Start()
		{
		}

		protected virtual void SkipAnimations(float totalTime, int steps = 3)
		{
		}

		protected virtual void OnDestroy()
		{
		}

		protected virtual void LateUpdate()
		{
		}

		protected abstract void UpdateAnimations();

		private void OnRoleChanged(ReferenceHub hub, PlayerRoleBase oldRole, PlayerRoleBase newRole)
		{
		}

		private void DestroySelf()
		{
		}
	}
}
