using UnityEngine;

namespace Respawning.Misc
{
	public class ChaosBrakeLights : MonoBehaviour
	{
		private bool _prevState;

		public bool State;

		[SerializeField]
		private MeshRenderer _renderer;

		[SerializeField]
		private Material _onMat;

		[SerializeField]
		private Material _offMat;

		[SerializeField]
		private Light _leftBrake;

		[SerializeField]
		private Light _rightBrake;

		private void Update()
		{
		}
	}
}
