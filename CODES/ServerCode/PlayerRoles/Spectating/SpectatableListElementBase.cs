namespace PlayerRoles.Spectating
{
	public class SpectatableListElementBase : global::GameObjectPools.PoolObject
	{
		private global::UnityEngine.RectTransform _cachedTransform;

		private global::PlayerRoles.Spectating.SpectatableModuleBase _target;

		private bool _transformCacheSet;

		protected global::UnityEngine.RectTransform CachedRectTransform
		{
			get
			{
				if (!_transformCacheSet)
				{
					if (!TryGetComponent<global::UnityEngine.RectTransform>(out _cachedTransform))
					{
						throw new global::System.InvalidOperationException("SpectatableListElementBase of name '" + base.name + "' does not have a rect transform!");
					}
					_transformCacheSet = true;
				}
				return _cachedTransform;
			}
		}

		public global::PlayerRoles.Spectating.SpectatableModuleBase Target
		{
			get
			{
				return _target;
			}
			internal set
			{
				global::PlayerRoles.Spectating.SpectatableModuleBase target = _target;
				if (value != target)
				{
					_target = value;
					OnTargetChanged(target, value);
				}
			}
		}

		public int Index { get; internal set; }

		public float Height => CachedRectTransform.sizeDelta.y;

		public bool IsCurrent
		{
			get
			{
				if (!base.Pooled)
				{
					return Target == global::PlayerRoles.Spectating.SpectatorTargetTracker.CurrentTarget;
				}
				return false;
			}
		}

		protected virtual void OnTargetChanged(global::PlayerRoles.Spectating.SpectatableModuleBase prevTarget, global::PlayerRoles.Spectating.SpectatableModuleBase newTarget)
		{
		}

		public void BeginSpectating()
		{
			global::PlayerRoles.Spectating.SpectatorTargetTracker.CurrentTarget = Target;
		}
	}
}
