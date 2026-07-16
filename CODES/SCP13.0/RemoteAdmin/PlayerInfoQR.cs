using UnityEngine;
using UnityEngine.UI;
using ZXing;

namespace RemoteAdmin
{
	internal class PlayerInfoQR : MonoBehaviour
	{
		public RawImage QrDisplay;

		private static PlayerInfoQR _singleton;

		private static BarcodeWriter<Texture2D> _barcodeWriter;

		private static Texture2D _emptyCode;

		private static bool _clear;

		private const int Size = 125;

		public void OnEnable()
		{
		}

		public static void Display(string userId)
		{
		}

		public static void Clear()
		{
		}
	}
}
