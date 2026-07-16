using UnityEngine;

namespace Targeting
{
	public class Scp096Target : TargetComponent
	{
		[SerializeField]
		private GameObject _targetParticles;

		private bool _isTarget;

		public override bool IsTarget
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		private void Start()
		{
		}
	}
}
