namespace RemoteAdmin
{
	public class LargeDataPrinter : global::UnityEngine.MonoBehaviour
	{
		private const int Size = 700;

		internal static global::RemoteAdmin.LargeDataPrinter Singleton;

		private static global::ZXing.BarcodeWriter _barcodeWriter;

		public global::UnityEngine.GameObject Panel;

		public void OnEnable()
		{
		}

		private void Update()
		{
		}
	}
}
