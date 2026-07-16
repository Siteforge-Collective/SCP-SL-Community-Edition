using UnityEngine;

namespace Tooltips
{
	public class CustomTooltipData : MonoBehaviour, ITooltipHolder
	{
		[field: SerializeField]
		public TooltipData[] StoredInfo { get; set; }

		[field: SerializeField]
		public TooltipManager Manager { get; set; }

		private void Awake()
		{
		}
	}
}
