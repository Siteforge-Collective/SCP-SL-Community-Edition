using System.Diagnostics;

namespace InventorySystem.Items.Firearms.Modules
{
	public class SimpleInspector : IInspectorModule, IFirearmModuleBase
	{
		private const float MinimalAntispamCooldown = 0.2f;

		private readonly int _layer;

		private readonly Firearm _firearm;

		private readonly Stopwatch _cooldownStopwatch;

		public bool Standby => false;

		public bool CanInspect => false;

		public SimpleInspector(Firearm selfRef, int animatorLayer)
		{
		}

		public void OnInspect()
		{
		}
	}
}
