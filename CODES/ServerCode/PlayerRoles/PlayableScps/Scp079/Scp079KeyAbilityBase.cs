namespace PlayerRoles.PlayableScps.Scp079
{
	public abstract class Scp079KeyAbilityBase : global::PlayerRoles.PlayableScps.Scp079.Scp079AbilityBase, global::PlayerRoles.PlayableScps.Scp079.GUI.IScp079FailMessageProvider
	{
		private enum Category
		{
			Movement = 0,
			SpecialAbility = 1,
			OverconInteraction = 2
		}

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp079.Scp079KeyAbilityBase.Category _category;

		private static string _translationNoAux;

		private static global::UnityEngine.Object TrackedFailMessage
		{
			get
			{
				return global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079AbilityList.Singleton.TrackedFailMessage as global::UnityEngine.Object;
			}
			set
			{
				global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079AbilityList.Singleton.TrackedFailMessage = value as global::PlayerRoles.PlayableScps.Scp079.GUI.IScp079FailMessageProvider;
			}
		}

		public abstract ActionName ActivationKey { get; }

		public abstract bool IsReady { get; }

		public abstract bool IsVisible { get; }

		public abstract string AbilityName { get; }

		public abstract string FailMessage { get; }

		public int CategoryId => (int)_category;

		protected string GetNoAuxMessage(float cost)
		{
			return _translationNoAux + "\n" + base.AuxManager.GenerateETA(cost);
		}

		protected virtual void Start()
		{
			_translationNoAux = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.NotEnoughAux);
		}

		protected virtual void Update()
		{
			if (!base.Role.IsLocalPlayer || !IsVisible || !global::UnityEngine.Input.GetKeyDown(NewInput.GetKey(ActivationKey)) || base.LostSignalHandler.Lost)
			{
				return;
			}
			if (IsReady)
			{
				if (TrackedFailMessage == this)
				{
					TrackedFailMessage = null;
				}
				Trigger();
			}
			else
			{
				TrackedFailMessage = this;
			}
		}

		protected abstract void Trigger();

		public virtual void OnFailMessageAssigned()
		{
		}
	}
}
