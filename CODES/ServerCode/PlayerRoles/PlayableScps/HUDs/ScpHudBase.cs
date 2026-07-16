namespace PlayerRoles.PlayableScps.HUDs
{
	public abstract class ScpHudBase : global::UnityEngine.MonoBehaviour
	{
		public ReferenceHub Hub { get; private set; }

		[field: global::UnityEngine.SerializeField]
		public global::TMPro.TMP_Text TargetCounter { get; private set; }

		protected virtual void Update()
		{
		}

		protected virtual void OnDestroy()
		{
		}

		protected virtual void UpdateCounter()
		{
		}

		internal virtual void OnDied()
		{
		}

		internal virtual void Init(ReferenceHub hub)
		{
			Hub = hub;
		}
	}
}
