using System;
using UnityEngine;

namespace InventorySystem.Items.Firearms.Attachments.Components
{
	public class ReflexSightAttachment : SerializableAttachment, ICustomizableAttachment
	{
		public static readonly float[] Sizes;

		public static readonly Color[] Colors;

		public Action OnValuesChanged;

		public ReflexSightReticlePack TextureOptions;

		private const int DefaultSize = 4;

		[SerializeField]
		private int _defaultColorId;

		[SerializeField]
		private int _defaultReticle;

		[SerializeField]
		private Vector2 _configIconOffset;

		[SerializeField]
		private float _configIconSize;

		[SerializeField]
		private AttachmentConfigWindow _configWindow;

		private bool _wasEverActive;

		public int CurTexture { get; private set; }

		public int CurSize { get; private set; }

		public int CurColor { get; private set; }

		public Vector2 ConfigIconOffset => default(Vector2);

		public AttachmentConfigWindow ConfigWindow => null;

		public float ConfigIconScale => 0f;

		public void SetValues(int texture, int color, int size)
		{
		}

		public void SaveValues()
		{
		}

		internal void SetTexture(int i)
		{
		}

		internal void SetColor(int i)
		{
		}

		internal void ChangeSize(int i)
		{
		}

		private void Awake()
		{
		}

		private void Update()
		{
		}

		private void OnDestroy()
		{
		}

		private string GetPrefsKey(Firearm parent, int preset, string setting)
		{
			return null;
		}

		private void LoadFirstTime()
		{
		}

		private void LoadFromPrefs()
		{
		}

		private void LoadFromPreset()
		{
		}

		private void SetDefaults()
		{
		}

		private bool TryGetDatabaseEntry(Firearm fa, out ReflexSightSyncMessage msg)
		{
			msg = default(ReflexSightSyncMessage);
			return false;
		}

		private void SendNewSettings()
		{
		}
	}
}
