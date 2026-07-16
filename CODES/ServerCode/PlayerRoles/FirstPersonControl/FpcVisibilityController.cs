namespace PlayerRoles.FirstPersonControl
{
	public class FpcVisibilityController : global::PlayerRoles.Visibility.VisibilityController
	{
		private const int SurfaceHeight = 800;

		private global::CustomPlayerEffects.Invisible _invisibleEffect;

		protected virtual int NormalMaxRangeSqr => 1100;

		protected virtual int SurfaceMaxRangeSqr => 4900;

		public override global::PlayerRoles.Visibility.InvisibilityFlags GetActiveFlags(ReferenceHub observer)
		{
			global::PlayerRoles.Visibility.InvisibilityFlags invisibilityFlags = base.GetActiveFlags(observer);
			if (_invisibleEffect.IsEnabled)
			{
				invisibilityFlags |= global::PlayerRoles.Visibility.InvisibilityFlags.Scp268;
			}
			if (!(observer.roleManager.CurrentRole is global::PlayerRoles.FirstPersonControl.IFpcRole fpcRole) || !(base.Owner.roleManager.CurrentRole is global::PlayerRoles.FirstPersonControl.IFpcRole fpcRole2))
			{
				return invisibilityFlags;
			}
			global::UnityEngine.Vector3 position = fpcRole.FpcModule.Position;
			global::UnityEngine.Vector3 position2 = fpcRole2.FpcModule.Position;
			float num = ((global::UnityEngine.Mathf.Min(position.y, position2.y) > 800f) ? SurfaceMaxRangeSqr : NormalMaxRangeSqr);
			if ((position - position2).sqrMagnitude > num)
			{
				invisibilityFlags |= global::PlayerRoles.Visibility.InvisibilityFlags.OutOfRange;
			}
			return invisibilityFlags;
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			_invisibleEffect = base.Owner.playerEffectsController.GetEffect<global::CustomPlayerEffects.Invisible>();
		}
	}
}
