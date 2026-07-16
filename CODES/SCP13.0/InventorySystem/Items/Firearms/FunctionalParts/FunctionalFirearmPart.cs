using UnityEngine;

namespace InventorySystem.Items.Firearms.FunctionalParts
{
	public class FunctionalFirearmPart : MonoBehaviour
	{
		private Firearm _fa;

		private bool _faSet;

		private bool? _hasViewmodel;

		[SerializeField]
		private AnimatedFirearmViewmodel _viewmodel;

		protected Firearm Firearm => null;

		protected bool TryGetViewmodel(out AnimatedFirearmViewmodel viewmodel)
		{
			viewmodel = null;
			return false;
		}
	}
}
