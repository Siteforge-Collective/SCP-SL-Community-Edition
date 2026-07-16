namespace Targeting
{
	public class Scp096Target : global::Targeting.TargetComponent
	{
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _targetParticles;

		private bool _isTarget;

		public override bool IsTarget
		{
			get
			{
				return _isTarget;
			}
			set
			{
				_targetParticles.SetActive(value);
				_isTarget = value;
			}
		}

		private void Start()
		{
			IsTarget = false;
		}
	}
}
