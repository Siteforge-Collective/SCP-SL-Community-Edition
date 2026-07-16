namespace InventorySystem.Items.Firearms.Modules
{
	public class SimpleInspector : global::InventorySystem.Items.Firearms.Modules.IInspectorModule, global::InventorySystem.Items.Firearms.Modules.IFirearmModuleBase
	{
		private const float MinimalAntispamCooldown = 0.2f;

		private readonly int _layer;

		private readonly global::InventorySystem.Items.Firearms.Firearm _firearm;

		private readonly global::System.Diagnostics.Stopwatch _cooldownStopwatch;

		public bool Standby => true;

		public SimpleInspector(global::InventorySystem.Items.Firearms.Firearm selfRef, int animatorLayer)
		{
			_firearm = selfRef;
			_layer = animatorLayer;
			_cooldownStopwatch = new global::System.Diagnostics.Stopwatch();
			_cooldownStopwatch.Start();
		}
	}
}
