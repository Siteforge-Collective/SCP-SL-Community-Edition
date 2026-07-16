using UnityEngine;

namespace InventorySystem.Items.Jailbird
{
	public class JailbirdMaterialController : MonoBehaviour
	{
		private ushort _serial;

		[SerializeField]
		private Material _almostDepletedMat;

		[SerializeField]
		private Material _normalMat;

		[SerializeField]
		private Renderer _emissionRend;

		private void Update()
		{
		}

		public void SetSerial(ushort serial)
		{
		}
	}
}
