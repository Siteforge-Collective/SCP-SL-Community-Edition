using UnityEngine;
using UnityEngine.UI;
using ZXing;

namespace RemoteAdmin
{
	public class LargeDataPrinter : MonoBehaviour
	{
		public GameObject Panel;

		public RawImage QrDisplay;

		private const int Size = 800;

		private static LargeDataPrinter _singleton;

		private static BarcodeWriter<Texture2D> _barcodeWriter;

		private void OnEnable()
		{
		}

		private void Update()
		{
		}

		public static void Display(string content)
		{
		}

		public static void Hide()
		{
		}
	}
}
