namespace PlayerRoles.PlayableScps.Scp079.Overcons
{
	public abstract class OverconBase : global::UnityEngine.MonoBehaviour
	{
		public static readonly global::System.Collections.Generic.HashSet<global::PlayerRoles.PlayableScps.Scp079.Overcons.OverconBase> ActiveInstances = new global::System.Collections.Generic.HashSet<global::PlayerRoles.PlayableScps.Scp079.Overcons.OverconBase>();

		public virtual bool IsHighlighted { get; internal set; }

		protected virtual void OnEnable()
		{
			ActiveInstances.Add(this);
		}

		protected virtual void OnDisable()
		{
			ActiveInstances.Remove(this);
		}
	}
}
