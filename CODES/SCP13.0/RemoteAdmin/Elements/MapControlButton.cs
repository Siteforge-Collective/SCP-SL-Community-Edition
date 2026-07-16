using RemoteAdmin.Menus;
using UnityEngine;
using UnityEngine.UI;

namespace RemoteAdmin.Elements
{
	public abstract class MapControlButton : ValueButton
	{
		protected MapControlMenu MapMenu;

		private bool _menuInitialized;

		[SerializeField]
		private Image _image;

		public virtual void Setup()
		{
		}

		public void UpdateGraphics()
		{
		}

		protected abstract Color BackgroundColor();

		protected abstract Color OutlineColor();
	}
}
