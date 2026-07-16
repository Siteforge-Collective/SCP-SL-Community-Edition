using InventorySystem.Items.Firearms.Modules;

namespace InventorySystem.Items.Firearms
{
	public class ShotgunAnimatorEvents : FirearmAnimatorEventsBase
	{
		private void MarkAsEquipped()
		{
		}

		private void InsertShell()
		{
		}

		private void UnloadShells()
		{
		}

		private void Pump(int isReload)
		{
		}

		private bool GetPumpAction(out PumpAction pa)
		{
			pa = null;
			return false;
		}
	}
}
