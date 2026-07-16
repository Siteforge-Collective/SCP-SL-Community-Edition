namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079NotificationManager : global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079GuiElementBase
	{
		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079NotificationEntry _template;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Vector2 _defaultSize;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip[] _sounds;

		private readonly global::System.Collections.Generic.Queue<global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079NotificationEntry> _textPool = new global::System.Collections.Generic.Queue<global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079NotificationEntry>();

		private readonly global::System.Collections.Generic.List<global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079NotificationEntry> _spawnedTexts = new global::System.Collections.Generic.List<global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079NotificationEntry>();

		private static global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079NotificationManager _singleton;

		private static bool _singletonSet;

		private void Awake()
		{
			_singleton = this;
			_singletonSet = true;
		}

		private void OnDestroy()
		{
			if (!(_singleton != this))
			{
				_singletonSet = false;
			}
		}

		private void Update()
		{
			for (int i = 0; i < _spawnedTexts.Count; i++)
			{
				global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079NotificationEntry scp079NotificationEntry = _spawnedTexts[i];
				global::PlayerRoles.PlayableScps.Scp079.GUI.IScp079Notification content = scp079NotificationEntry.Content;
				scp079NotificationEntry.Text.text = content.DisplayedText;
				scp079NotificationEntry.Text.alpha = global::UnityEngine.Mathf.Clamp01(content.Opacity);
				scp079NotificationEntry.Text.rectTransform.sizeDelta = new global::UnityEngine.Vector2(_defaultSize.x, content.Height);
				global::PlayerRoles.PlayableScps.Scp079.GUI.NotificationSound sound = content.Sound;
				if (sound != global::PlayerRoles.PlayableScps.Scp079.GUI.NotificationSound.None)
				{
					PlaySound(_sounds[(int)sound]);
				}
				if (content.Delete)
				{
					scp079NotificationEntry.gameObject.SetActive(value: false);
					_textPool.Enqueue(scp079NotificationEntry);
					_spawnedTexts.RemoveAt(i);
					i--;
				}
			}
		}

		private void SpawnNotification(global::PlayerRoles.PlayableScps.Scp079.GUI.IScp079Notification notification)
		{
			if (!CollectionExtensions.TryDequeue(_textPool, out var element))
			{
				element = global::UnityEngine.Object.Instantiate(_template, _template.transform.parent);
			}
			_spawnedTexts.Add(element);
			element.Content = notification;
			element.Text.text = string.Empty;
			element.Text.rectTransform.sizeDelta = _defaultSize;
			element.gameObject.SetActive(value: true);
			element.transform.SetAsLastSibling();
		}

		public static void AddNotification(global::PlayerRoles.PlayableScps.Scp079.GUI.IScp079Notification handler)
		{
			if (_singletonSet)
			{
				_singleton.SpawnNotification(handler);
			}
		}

		public static void AddNotification(string notification)
		{
			AddNotification(new global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079SimpleNotification(notification));
		}

		public static void AddNotification(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation translation)
		{
			AddNotification(Translations.Get(translation));
		}

		public static void AddNotification(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation translation, params object[] format)
		{
			AddNotification(string.Format(Translations.Get(translation), format));
		}
	}
}
