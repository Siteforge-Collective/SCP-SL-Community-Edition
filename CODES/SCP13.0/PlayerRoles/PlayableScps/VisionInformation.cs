using PlayerRoles.FirstPersonControl;
using UnityEngine;

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

		public const float MaximumVisionDistance = 30f;

		public const float SurfaceMaximumVisionDistance = 60f;

		public static readonly int VisionLayerMask;

		public static readonly RaycastHit[] RaycastResult;

		public float LookingAmount { get; }

		public ReferenceHub SourceHub { get; }

		public Vector3 TargetPosition { get; }

		public float Distance { get; }

		public bool IsOnSameFloor { get; }

		public bool IsLooking { get; }

		public bool IsInDistance { get; }

		public bool IsInDarkness { get; }

		public bool IsInLineOfSight { get; }

		public VisionInformation(ReferenceHub sourceHub, Vector3 targetHub, bool isLooking, bool isOnSameFloor, float lookingAmount, float distance, bool isInLineOfSight, bool isInDarkness, bool isInDistance)
		{
			LookingAmount = 0f;
			SourceHub = null;
			TargetPosition = default(Vector3);
			Distance = 0f;
			IsOnSameFloor = false;
			IsLooking = false;
			IsInDistance = false;
			IsInDarkness = false;
			IsInLineOfSight = false;
		}

		public static VisionInformation GetVisionInformation(ReferenceHub source, Transform sourceCam, Vector3 target, float targetRadius = 0f, float visionTriggerDistance = 0f, bool checkFog = true, bool checkLineOfSight = true, int maskLayer = 0, bool checkInDarkness = true)
		{
			return default(VisionInformation);
		}

		private static bool CheckAttachments(ReferenceHub source)
		{
			return false;
		}

		public FailReason GetFailReason()
		{
			return default(FailReason);
		}

		public static bool IsInView(ReferenceHub originHub, ReferenceHub targetHub)
		{
			return false;
		}

		public static bool IsInView(ReferenceHub originHub, IFpcRole fpcRole)
		{
			return false;
		}
	}
}
