namespace PlayerRoles.RoleAssign
{
	public class ScpPreferenceSlider : global::UnityEngine.UI.Slider
	{
		private static global::PlayerRoles.RoleAssign.ScpPreferenceSlider _highlighted;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.RoleTypeId _role = global::PlayerRoles.RoleTypeId.None;

		public static bool AnyHighlighted { get; private set; }

		private void OnValueChanged(float x)
		{
			int num = global::UnityEngine.Mathf.RoundToInt(x);
			global::PlayerRoles.RoleAssign.ScpSpawnPreferences.SavePreference(_role, num);
		}

		private void Deselect()
		{
			if (!(_highlighted != this))
			{
				AnyHighlighted = false;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			value = global::PlayerRoles.RoleAssign.ScpSpawnPreferences.GetPreference(_role);
			base.onValueChanged.AddListener(OnValueChanged);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			Deselect();
		}

		public override void OnPointerDown(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			base.OnPointerDown(eventData);
			AnyHighlighted = true;
			_highlighted = this;
		}

		public override void OnPointerUp(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			base.OnPointerUp(eventData);
			Deselect();
		}
	}
}
