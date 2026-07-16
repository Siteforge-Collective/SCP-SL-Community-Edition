namespace OperationalGuide
{
	public class OperationalFirearmPage : OperationalPannablePage
	{
		public FirearmObject FirearmObject;

		public ItemType DefaultItem;

		public override void ToggleDescriptionMenu()
		{
		}

		public virtual void UpdateHeldItem(int item)
		{
		}
	}
}
