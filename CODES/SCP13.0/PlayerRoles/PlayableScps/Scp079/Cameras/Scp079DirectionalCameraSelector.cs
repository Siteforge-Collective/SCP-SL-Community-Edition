using UnityEngine;
using UserSettings;

namespace PlayerRoles.PlayableScps.Scp079.Cameras
{
	public class Scp079DirectionalCameraSelector : Scp079KeyAbilityBase
	{
		private static string _translationNoCamera;

		private static string _translationPaidSwitch;

		private static string _translationFreeSwitch;

		private static bool _translationsSet;

		private static readonly Vector3Int[] WorldDirections;

		private static readonly CachedUserSetting<bool> AllowKeybindZoneSwitching;

		[SerializeField]
		private ActionName _key;

		[SerializeField]
		private Vector3 _direction;

		private Scp079Camera _lastCamera;

		private bool _lastValid;

		private float _lastSwitchCost;

		private float _failMessageSwitchCost;

		protected virtual bool AllowSwitchingBetweenZones => false;

		public override bool IsReady => false;

		public override ActionName ActivationKey => default(ActionName);

		public override bool IsVisible => false;

		public override string FailMessage => null;

		public override string AbilityName => null;

		protected virtual bool TryGetCamera(out Scp079Camera targetCamera)
		{
			targetCamera = null;
			return false;
		}

		protected override void Trigger()
		{
		}

		protected override void Start()
		{
		}

		private void OnDestroy()
		{
		}

		public override void OnFailMessageAssigned()
		{
		}
	}
}
