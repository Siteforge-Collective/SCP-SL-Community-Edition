namespace ToggleableMenus
{
	public class SimpleToggleableMenu : global::ToggleableMenus.ToggleableMenuBase
	{
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _targetRoot;

		public override bool CanToggle => true;

		protected override void OnToggled()
		{
			_targetRoot.SetActive(IsEnabled);
		}
	}
}
