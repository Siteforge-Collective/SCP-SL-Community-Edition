namespace Interactables.Interobjects.DoorUtils
{
	public class RegularDoorButton : global::Interactables.InteractableCollider
	{
		[global::UnityEngine.SerializeField]
		private global::TMPro.TextMeshProUGUI _mainText;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.MeshRenderer _mainRenderer;

		private bool _useText;

		protected override void Awake()
		{
			base.Awake();
			_useText = _mainText != null;
		}

		public void SetupButton(string text, global::UnityEngine.Material mat)
		{
			if (_useText)
			{
				_mainText.text = text;
			}
			_mainRenderer.sharedMaterial = mat;
		}
	}
}
