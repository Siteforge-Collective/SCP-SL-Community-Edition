namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079GeneratorWarningGui : global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079GuiElementBase
	{
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _warningSound;

		[global::UnityEngine.SerializeField]
		private float _headerCooldown;

		private int _lastAmount;

		private string _headerText;

		private readonly global::System.Diagnostics.Stopwatch _headerStopwatchTimer = global::System.Diagnostics.Stopwatch.StartNew();

		private const string HeaderFormat = "<color=red>{0}</color>";

		private void Awake()
		{
			_headerText = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.YouAreBeingAttacked);
		}

		private void Update()
		{
			int num = 0;
			bool flag = _headerStopwatchTimer.Elapsed.TotalSeconds < (double)_headerCooldown;
			bool flag2 = flag && !global::PlayerRoles.PlayableScps.Scp079.Scp079Role.LocalInstanceActive;
			foreach (global::MapGeneration.Distributors.Scp079Generator allGenerator in global::PlayerRoles.PlayableScps.Scp079.Scp079Recontainer.AllGenerators)
			{
				if (allGenerator.Activating && global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079GeneratorNotification.TrackedGens.Add(allGenerator))
				{
					num++;
					if (_lastAmount == 0 && !flag)
					{
						global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079NotificationManager.AddNotification($"<color=red>{_headerText}</color>");
						_headerStopwatchTimer.Restart();
					}
					global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079NotificationManager.AddNotification(new global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079GeneratorNotification(allGenerator, flag2));
				}
			}
			if (num > _lastAmount && !flag2)
			{
				PlaySound(_warningSound);
			}
			_lastAmount = num;
		}
	}
}
