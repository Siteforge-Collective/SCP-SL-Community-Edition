using Interactables.Interobjects.DoorUtils;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.Overcons
{
	public class DoorOvercon : StandardOvercon
	{
		[SerializeField]
		private Sprite _openSprite;

		[SerializeField]
		private Sprite _closedSprite;

		private SphereCollider _col;

		public DoorVariant Target { get; internal set; }

		private bool IsInvisible => false;

		private bool IsCurrentDoorValid()
		{
			return false;
		}

		private void LateUpdate()
		{
		}

		protected override void Awake()
		{
		}
	}
}
