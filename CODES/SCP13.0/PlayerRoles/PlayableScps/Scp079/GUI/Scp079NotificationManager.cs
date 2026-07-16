using System.Collections.Generic;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079NotificationManager : Scp079GuiElementBase
	{
		[SerializeField]
		private Scp079NotificationEntry _template;

		[SerializeField]
		private Vector2 _defaultSize;

		[SerializeField]
		private AudioClip[] _sounds;

		private readonly Queue<Scp079NotificationEntry> _textPool;

		private readonly List<Scp079NotificationEntry> _spawnedTexts;

		private static Scp079NotificationManager _singleton;

		private static bool _singletonSet;

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private void Update()
		{
		}

		private void SpawnNotification(IScp079Notification notification)
		{
		}

		public static void AddNotification(IScp079Notification handler)
		{
		}

		public static void AddNotification(string notification)
		{
		}

		public static void AddNotification(Scp079HudTranslation translation)
		{
		}

		public static void AddNotification(Scp079HudTranslation translation, params object[] format)
		{
		}

		public static bool TryGetTextHeight(string content, out float height)
		{
			height = default(float);
			return false;
		}
	}
}
