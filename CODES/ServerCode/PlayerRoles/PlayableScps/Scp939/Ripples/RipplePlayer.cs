namespace PlayerRoles.PlayableScps.Scp939.Ripples
{
	public class RipplePlayer : global::PlayerRoles.PlayableScps.Subroutines.ScpSubroutineBase
	{
		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp939.Ripples.RippleInstance _rippleTemplate;

		private global::PlayerRoles.PlayableScps.Scp939.Scp939FocusAbility _focus;

		private int _poolCount;

		private readonly global::System.Collections.Generic.Queue<global::PlayerRoles.PlayableScps.Scp939.Ripples.RippleInstance> _pool = new global::System.Collections.Generic.Queue<global::PlayerRoles.PlayableScps.Scp939.Ripples.RippleInstance>();

		public void Play(global::UnityEngine.Vector3 position, global::UnityEngine.Color color)
		{
			bool flag = false;
			global::PlayerRoles.PlayableScps.Scp939.Ripples.RippleInstance rippleInstance = null;
			while (_poolCount != 0 && !(flag = (rippleInstance = _pool.Peek()) != null))
			{
				_pool.Dequeue();
				_poolCount--;
			}
			if (flag && !rippleInstance.InUse)
			{
				_pool.Dequeue();
				_poolCount--;
			}
			else
			{
				rippleInstance = global::UnityEngine.Object.Instantiate(_rippleTemplate);
			}
			rippleInstance.Set(position, (_focus.State < 1f) ? global::UnityEngine.Color.red : color);
			_pool.Enqueue(rippleInstance);
			_poolCount++;
		}

		public void Play(global::PlayerRoles.HumanRole human)
		{
			Play(human.FpcModule.Position, human.RoleColor);
		}

		protected override void Awake()
		{
			base.Awake();
			(base.Role as global::PlayerRoles.PlayableScps.Subroutines.ISubroutinedScpRole).SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp939.Scp939FocusAbility>(out _focus);
		}
	}
}
