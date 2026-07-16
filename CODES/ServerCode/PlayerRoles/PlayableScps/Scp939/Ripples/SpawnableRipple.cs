namespace PlayerRoles.PlayableScps.Scp939.Ripples
{
	public class SpawnableRipple : global::UnityEngine.MonoBehaviour
	{
		[field: global::UnityEngine.SerializeField]
		public float Range { get; private set; }

		public static event global::System.Action<global::PlayerRoles.PlayableScps.Scp939.Ripples.SpawnableRipple> OnSpawned;

		private void OnEnabled()
		{
			global::PlayerRoles.PlayableScps.Scp939.Ripples.SpawnableRipple.OnSpawned?.Invoke(this);
		}
	}
}
