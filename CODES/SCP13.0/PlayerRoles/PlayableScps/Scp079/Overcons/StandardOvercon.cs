using PlayerRoles.PlayableScps.Scp079.Cameras;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.Overcons
{
	public class StandardOvercon : OverconBase
	{
		[SerializeField]
		private AnimationCurve _scaleOverDistance;

		[SerializeField]
		protected SpriteRenderer TargetSprite;

		private const float SurfaceSizeScale = 2.5f;

		public static Color HighlightedColor;

		public static Color NormalColor;

		public override bool IsHighlighted
		{
			get
			{
				return false;
			}
			internal set
			{
			}
		}

		protected virtual void Awake()
		{
		}

		public void Rescale(Scp079Camera cam)
		{
		}

		public void Rescale(Scp079Camera cam, float dis)
		{
		}
	}
}
