namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079DoorLockReleaser : global::PlayerRoles.PlayableScps.Scp079.Scp079KeyAbilityBase
	{
		private static string _format;

		private global::PlayerRoles.PlayableScps.Scp079.Scp079DoorLockChanger _lockChanger;

		private const string ColorFormat = "<color=#ffffff{0}>{1}</color>";

		private const float BlinkRate = 2.8f;

		public override ActionName ActivationKey => ActionName.Scp079UnlockAll;

		public override bool IsReady => true;

		public override bool IsVisible => _lockChanger.TotalLocked > 0;

		public override string AbilityName => $"<color=#ffffff{Transparency}>{string.Format(_format, _lockChanger.TotalLocked)}</color>";

		public override string FailMessage => null;

		private string Transparency
		{
			get
			{
				float f = global::UnityEngine.Time.timeSinceLevelLoad * 2.8f * (float)global::System.Math.PI;
				return global::UnityEngine.Mathf.RoundToInt(global::UnityEngine.Mathf.InverseLerp(-1f, 1f, global::UnityEngine.Mathf.Sin(f)) * 255f).ToString("X2");
			}
		}

		protected override void Trigger()
		{
			ClientSendCmd();
		}

		protected override void Start()
		{
			base.Start();
			GetSubroutine<global::PlayerRoles.PlayableScps.Scp079.Scp079DoorLockChanger>(out _lockChanger);
			_format = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.ReleaseDoorLocks);
		}

		public override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			base.ServerProcessCmd(reader);
			_lockChanger.ServerUnlockAll();
		}
	}
}
