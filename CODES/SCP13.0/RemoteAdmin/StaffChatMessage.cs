using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RemoteAdmin
{
	public class StaffChatMessage : MonoBehaviour
	{
		[SerializeField]
		private Image _image;

		[SerializeField]
		private TextMeshProUGUI _text;

		public Color Color
		{
			get
			{
				return default(Color);
			}
			set
			{
			}
		}

		public string Content
		{
			get
			{
				return null;
			}
			set
			{
			}
		}
	}
}
