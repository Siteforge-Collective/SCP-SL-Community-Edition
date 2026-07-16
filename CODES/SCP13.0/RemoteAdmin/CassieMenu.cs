using RemoteAdmin.Menus;
using TMPro;
using UnityEngine;

namespace RemoteAdmin
{
	public class CassieMenu : RaCommandMenu
	{
		[SerializeField]
		private Color _resetColor;

		[SerializeField]
		private Color _errorColor;

		[SerializeField]
		private TMP_Text[] _buttons;

		[SerializeField]
		private TMP_Text _pageCount;

		[SerializeField]
		private TMP_Text _elementCount;

		[SerializeField]
		private CustomSlider _pitchSlider;

		[SerializeField]
		private CustomSlider _jamDelaySlider;

		[SerializeField]
		private CustomSlider _jamStutterSlider;

		[SerializeField]
		private CustomSlider _yieldSlider;

		private readonly string[] _validSuffixes;

		private int _pageIndex;

		private int _maxPages;

		public void AddJam()
		{
		}

		public void AddYield()
		{
		}

		public void AddPitch()
		{
		}

		public void ResetPages()
		{
		}

		public void JumpPages(int i = 1)
		{
		}

		public void RefreshButtons()
		{
		}

		public void AddElement(string text)
		{
		}

		public void AddElement(TMP_Text text)
		{
		}

		public void TestCassie()
		{
		}

		public void CheckEasterEgg()
		{
		}

		public void CopyToClipboard()
		{
		}

		protected override void OnStart()
		{
		}

		private void UpdateCounters()
		{
		}

		private int Index(int increment = 1)
		{
			return 0;
		}

		private void OnValueChange(string newValue)
		{
		}

		private bool HasValidWord(string word)
		{
			return false;
		}

		private bool IsValidWord(string word)
		{
			return false;
		}

		private void OnDestroy()
		{
		}
	}
}
