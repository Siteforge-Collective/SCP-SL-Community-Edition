using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RemoteAdmin
{
	public class PlayerRecord : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
	{
		[SerializeField]
		private TMP_Text _textComponent;

		private bool _hasLink;

		private bool _isSelected;

		public static string LastSelectedId { get; internal set; }

		public static List<PlayerRecord> Instances { get; }

		public string PlayerId { get; set; }

		public bool IsSelected
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public void OnPointerDown(PointerEventData _)
		{
		}

		internal void SetState(bool selected, bool allowMultipleSelection = false, bool changeLastSelected = true)
		{
		}

		internal void SetText(string content)
		{
		}

		private void OnMouseSelect(bool newState, bool pressingControl = false, bool pressingShift = false)
		{
		}

		private IEnumerable<PlayerRecord> ToSelect(int oldIndex, int newIndex)
		{
			return null;
		}

		private void Update()
		{
		}

		private void UpdateGraphic()
		{
		}

		private void Start()
		{
		}
	}
}
