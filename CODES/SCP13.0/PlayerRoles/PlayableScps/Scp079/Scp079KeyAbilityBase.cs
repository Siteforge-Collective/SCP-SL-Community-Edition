using PlayerRoles.PlayableScps.Scp079.GUI;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079
{
	public abstract class Scp079KeyAbilityBase : Scp079AbilityBase, IScp079FailMessageProvider
	{
		private enum Category
		{
			Movement = 0,
			SpecialAbility = 1,
			OverconInteraction = 2
		}

		[SerializeField]
		private Category _category;

		private static string _translationNoAux;

		private static Object TrackedFailMessage
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public abstract ActionName ActivationKey { get; }

		public abstract bool IsReady { get; }

		public abstract bool IsVisible { get; }

		public abstract string AbilityName { get; }

		public abstract string FailMessage { get; }

		public int CategoryId => 0;

		[field: SerializeField]
		public bool UseLeftMenu { get; private set; }

		protected string GetNoAuxMessage(float cost)
		{
			return null;
		}

		protected virtual void Start()
		{
		}

		protected virtual void Update()
		{
		}

		protected abstract void Trigger();

		public virtual void OnFailMessageAssigned()
		{
		}
	}
}
