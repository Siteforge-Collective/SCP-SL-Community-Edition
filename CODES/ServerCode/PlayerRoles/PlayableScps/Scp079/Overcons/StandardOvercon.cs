namespace PlayerRoles.PlayableScps.Scp079.Overcons
{
	public class StandardOvercon : global::PlayerRoles.PlayableScps.Scp079.Overcons.OverconBase
	{
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _scaleOverDistance;

		[global::UnityEngine.SerializeField]
		protected global::UnityEngine.SpriteRenderer TargetSprite;

		private const float SurfaceSizeScale = 2.5f;

		public static global::UnityEngine.Color HighlightedColor = new global::UnityEngine.Color(1f, 1f, 1f, 1f);

		public static global::UnityEngine.Color NormalColor = new global::UnityEngine.Color(1f, 1f, 1f, 0.27f);

		public override bool IsHighlighted
		{
			get
			{
				return base.IsHighlighted;
			}
			internal set
			{
				TargetSprite.color = (value ? HighlightedColor : NormalColor);
				base.IsHighlighted = value;
			}
		}

		protected virtual void Awake()
		{
			TargetSprite.color = HighlightedColor;
		}

		public void Rescale(global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera cam)
		{
			Rescale(cam, global::UnityEngine.Vector3.Distance(cam.Position, base.transform.position));
		}

		public void Rescale(global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera cam, float dis)
		{
			base.transform.LookAt(cam.Position);
			float num = _scaleOverDistance.Evaluate(dis);
			if (cam.Room.Zone == global::MapGeneration.FacilityZone.Surface)
			{
				num *= 2.5f;
			}
			base.transform.localScale = global::UnityEngine.Vector3.one * num;
		}
	}
}
