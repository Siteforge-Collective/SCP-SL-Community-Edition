namespace RadialMenus
{
	public abstract class RadialMenuBase : global::UnityEngine.MonoBehaviour
	{
		private int _slotsNum;

		private float _slotsAngleStep;

		[global::UnityEngine.SerializeField]
		private global::RadialMenus.RadialMenuSettings _settings;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Vector2 _ringWidth = new global::UnityEngine.Vector2(0.41f, 1.1f);

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.UI.Image _slotTemplate;

		[global::UnityEngine.SerializeField]
		protected global::UnityEngine.UI.Image RingImage;

		protected global::UnityEngine.UI.Image[] Highlights = new global::UnityEngine.UI.Image[32];

		public abstract int Slots { get; }

		public int HighlightedSlot { get; private set; }

		protected virtual void OnSlotsNumberChanged(int prev, int cur)
		{
			for (int i = 0; i < prev; i++)
			{
				Highlights[i].enabled = false;
			}
			for (int j = 0; j < cur; j++)
			{
				global::UnityEngine.UI.Image image;
				if (Highlights[j] == null)
				{
					image = global::UnityEngine.Object.Instantiate(_slotTemplate, RingImage.transform);
					Highlights[j] = image;
				}
				else
				{
					image = Highlights[j];
				}
				image.rectTransform.SetAsFirstSibling();
				image.rectTransform.localPosition = global::UnityEngine.Vector3.zero;
				image.rectTransform.localEulerAngles = _slotsAngleStep * (float)j * global::UnityEngine.Vector3.back;
				image.sprite = _settings.HighlightTemplates[cur];
				image.enabled = true;
			}
			RingImage.sprite = _settings.MainRings[cur];
		}

		protected bool InRingRange(out float angle)
		{
			float num = (float)global::UnityEngine.Screen.width / (float)global::UnityEngine.Screen.height;
			global::UnityEngine.Vector2 vector = new global::UnityEngine.Vector2(global::UnityEngine.Mathf.Lerp(-1f, 1f, global::UnityEngine.Mathf.Clamp01(global::UnityEngine.Input.mousePosition.x / (float)global::UnityEngine.Screen.width)) * num, global::UnityEngine.Mathf.Lerp(-1f, 1f, global::UnityEngine.Input.mousePosition.y / (float)global::UnityEngine.Screen.height));
			angle = global::UnityEngine.Vector2.Angle(global::UnityEngine.Vector2.up, vector.normalized);
			if (vector.x < 0f)
			{
				angle = 360f - angle;
			}
			float magnitude = vector.magnitude;
			if (magnitude < _ringWidth.y)
			{
				return magnitude > _ringWidth.x;
			}
			return false;
		}

		protected virtual void Update()
		{
			int num = global::UnityEngine.Mathf.Clamp(Slots, 1, _settings.HighlightTemplates.Length + 1);
			if (num != _slotsNum)
			{
				int slotsNum = _slotsNum;
				_slotsNum = num;
				_slotsAngleStep = 360f / (float)num;
				OnSlotsNumberChanged(slotsNum, num);
			}
			if (InRingRange(out var angle))
			{
				HighlightedSlot = 0;
				while (angle > _slotsAngleStep)
				{
					angle -= _slotsAngleStep;
					HighlightedSlot++;
				}
			}
			else
			{
				HighlightedSlot = -1;
			}
		}
	}
}
