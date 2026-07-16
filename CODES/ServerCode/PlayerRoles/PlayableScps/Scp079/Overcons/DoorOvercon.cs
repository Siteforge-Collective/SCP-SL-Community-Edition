namespace PlayerRoles.PlayableScps.Scp079.Overcons
{
	public class DoorOvercon : global::PlayerRoles.PlayableScps.Scp079.Overcons.StandardOvercon
	{
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Sprite _openSprite;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Sprite _closedSprite;

		private global::UnityEngine.SphereCollider _col;

		public global::Interactables.Interobjects.DoorUtils.DoorVariant Target { get; internal set; }

		private bool IsInvisible
		{
			get
			{
				if (Target is global::Interactables.Interobjects.DoorUtils.IDamageableDoor damageableDoor && damageableDoor.IsDestroyed)
				{
					return true;
				}
				if (Target is global::Interactables.Interobjects.CheckpointDoor checkpointDoor)
				{
					if (!checkpointDoor.TargetState)
					{
						return checkpointDoor.GetExactState() > 0f;
					}
					return true;
				}
				return false;
			}
		}

		private void LateUpdate()
		{
			TargetSprite.sprite = (Target.TargetState ? _openSprite : _closedSprite);
			bool flag = !IsInvisible;
			TargetSprite.enabled = flag;
			_col.enabled = flag;
		}

		protected override void Awake()
		{
			base.Awake();
			_col = GetComponent<global::UnityEngine.SphereCollider>();
		}
	}
}
