using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tooltips
{
	public class TooltipManager : MonoBehaviour
	{
		[SerializeField]
		private Tooltip _tooltip;

		private float _showTimer;

		private float _hideTimer;

		public Dictionary<GameObject, string> StoredTips { get; }

		public bool IsEnabled { get; set; }

		public bool IsVisible { get; private set; }

		public Tooltip Tooltip => null;

		[field: SerializeField]
		public float ShowDelay { get; set; }

		[field: SerializeField]
		public float HideDelay { get; set; }

		private float HideTimer
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		private float ShowTimer
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		private void Update()
		{
		}

		private void SetState(bool isVisible)
		{
		}

		private void TickState(bool isHoveringElement)
		{
		}

		private bool IsCursorOverUI(out RaycastResult result)
		{
			result = default(RaycastResult);
			return false;
		}
	}
}
