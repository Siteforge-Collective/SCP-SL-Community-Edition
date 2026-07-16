namespace PlayerRoles.PlayableScps
{
	public readonly struct VisionInformation
	{
		public enum FailReason
		{
			NotOnSameFloor = 0,
			NotInDistance = 1,
			NotInView = 2,
			NotInLineOfSight = 3,
			InDarkRoom = 4,
			IsLooking = 5,
			UnkownReason = 6
		}

		public static readonly int VisionLayerMask = global::UnityEngine.LayerMask.GetMask("Door", "Default", "Locker");

		public static readonly global::UnityEngine.RaycastHit[] RaycastResult = new global::UnityEngine.RaycastHit[1];

		public float LookingAmount { get; }

		public ReferenceHub SourceHub { get; }

		public global::UnityEngine.Vector3 TargetPosition { get; }

		public float Distance { get; }

		public bool IsOnSameFloor { get; }

		public bool IsLooking { get; }

		public bool IsInDistance { get; }

		public bool IsInDarkness { get; }

		public bool IsInLineOfSight { get; }

		public VisionInformation(ReferenceHub sourceHub, global::UnityEngine.Vector3 targetHub, bool isLooking, bool isOnSameFloor, float lookingAmount, float distance, bool isInLineOfSight, bool isInDarkness, bool isInDistance)
		{
			SourceHub = sourceHub;
			TargetPosition = targetHub;
			IsLooking = isLooking;
			LookingAmount = lookingAmount;
			Distance = distance;
			IsInLineOfSight = isInLineOfSight;
			IsInDarkness = isInDarkness;
			IsInDistance = isInDistance;
			IsOnSameFloor = isOnSameFloor;
		}

		public static global::PlayerRoles.PlayableScps.VisionInformation GetVisionInformation(ReferenceHub source, global::UnityEngine.Transform sourceCam, global::UnityEngine.Vector3 target, float targetRadius = 0f, float visionTriggerDistance = 0f, bool checkFog = true, bool checkLineOfSight = true, int maskLayer = 0)
		{
			bool isOnSameFloor = false;
			bool flag = false;
			if (global::UnityEngine.Mathf.Abs(target.y - sourceCam.position.y) < 100f)
			{
				isOnSameFloor = true;
				flag = true;
			}
			bool flag2 = visionTriggerDistance == 0f;
			global::UnityEngine.Vector3 vector = target - sourceCam.position;
			float magnitude = vector.magnitude;
			if (flag && visionTriggerDistance > 0f)
			{
				float num = ((!checkFog) ? visionTriggerDistance : ((target.y > 980f) ? visionTriggerDistance : (visionTriggerDistance / 2f)));
				if (magnitude <= num)
				{
					flag2 = true;
				}
				flag = flag2;
			}
			float lookingAmount = 1f;
			if (flag)
			{
				flag = false;
				if (magnitude < targetRadius)
				{
					if (global::UnityEngine.Vector3.Dot(source.transform.forward, (target - source.transform.position).normalized) > 0f)
					{
						flag = true;
						lookingAmount = 1f;
					}
				}
				else if (global::InventorySystem.Items.Usables.Scp244.Scp244Utils.CheckVisibility(sourceCam.position, target))
				{
					global::UnityEngine.Vector3 vector2 = sourceCam.InverseTransformPoint(target);
					if (targetRadius != 0f)
					{
						vector2.x = global::UnityEngine.Mathf.MoveTowards(vector2.x, 0f, targetRadius);
						vector2.y = global::UnityEngine.Mathf.MoveTowards(vector2.y, 0f, targetRadius);
					}
					AspectRatioSync aspectRatioSync = source.aspectRatioSync;
					float num2 = global::UnityEngine.Vector2.Angle(global::UnityEngine.Vector2.up, new global::UnityEngine.Vector2(vector2.x, vector2.z));
					if (num2 < aspectRatioSync.XScreenEdge)
					{
						float num3 = global::UnityEngine.Vector2.Angle(global::UnityEngine.Vector2.up, new global::UnityEngine.Vector2(vector2.y, vector2.z));
						if (num3 < AspectRatioSync.YScreenEdge)
						{
							lookingAmount = (num2 + num3) / aspectRatioSync.XplusY;
							flag = true;
						}
					}
				}
			}
			bool flag3 = !checkLineOfSight;
			if (flag && checkLineOfSight)
			{
				if (maskLayer == 0)
				{
					maskLayer = VisionLayerMask;
				}
				flag3 = global::UnityEngine.Physics.RaycastNonAlloc(new global::UnityEngine.Ray(sourceCam.position, vector.normalized), RaycastResult, flag2 ? magnitude : vector.magnitude, maskLayer) == 0;
				flag = flag3;
			}
			bool flag4 = !CheckAttachments(source) && FlickerableLightController.IsInDarkenedRoom(target);
			flag = flag && !flag4;
			return new global::PlayerRoles.PlayableScps.VisionInformation(source, target, flag, isOnSameFloor, lookingAmount, magnitude, flag2, flag3, flag4);
		}

		private static bool CheckAttachments(ReferenceHub source)
		{
			global::InventorySystem.Items.ItemBase curInstance = source.inventory.CurInstance;
			if (curInstance != null && curInstance is global::InventorySystem.Items.ILightEmittingItem lightEmittingItem)
			{
				return lightEmittingItem.IsEmittingLight;
			}
			return false;
		}

		public global::PlayerRoles.PlayableScps.VisionInformation.FailReason GetFailReason()
		{
			if (!IsOnSameFloor)
			{
				return global::PlayerRoles.PlayableScps.VisionInformation.FailReason.NotOnSameFloor;
			}
			if (!IsInDistance)
			{
				return global::PlayerRoles.PlayableScps.VisionInformation.FailReason.NotInDistance;
			}
			if (LookingAmount >= 1f)
			{
				return global::PlayerRoles.PlayableScps.VisionInformation.FailReason.NotInView;
			}
			if (!IsInLineOfSight)
			{
				return global::PlayerRoles.PlayableScps.VisionInformation.FailReason.NotInLineOfSight;
			}
			if (IsInDarkness)
			{
				return global::PlayerRoles.PlayableScps.VisionInformation.FailReason.InDarkRoom;
			}
			if (!IsLooking)
			{
				return global::PlayerRoles.PlayableScps.VisionInformation.FailReason.UnkownReason;
			}
			return global::PlayerRoles.PlayableScps.VisionInformation.FailReason.IsLooking;
		}
	}
}
