namespace PlayerRoles.FirstPersonControl
{
	public class FpcSpectatableModule : global::PlayerRoles.Spectating.SpectatableModuleBase, IViewmodelRole
	{
		private global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule FpcModule => (base.MainRole as global::PlayerRoles.FirstPersonControl.IFpcRole).FpcModule;

		public override global::UnityEngine.Vector3 CameraPosition
		{
			get
			{
				if (!(base.MainRole is global::PlayerRoles.ICameraController cameraController))
				{
					return base.TargetHub.PlayerCameraReference.position;
				}
				return cameraController.CameraPosition;
			}
		}

		public override global::UnityEngine.Vector3 CameraRotation
		{
			get
			{
				if (base.MainRole is global::PlayerRoles.ICameraController cameraController)
				{
					float z = ((cameraController is global::PlayerRoles.IAdvancedCameraController advancedCameraController) ? advancedCameraController.RollRotation : 0f);
					return new global::UnityEngine.Vector3(cameraController.VerticalRotation, cameraController.HorizontalRotation, z);
				}
				return base.TargetHub.PlayerCameraReference.rotation.eulerAngles;
			}
		}

		internal override void OnBeganSpectating()
		{
			FpcModule.CharacterModelInstance.SetVisibility(newState: false);
			global::InventorySystem.Inventory.OnCurrentItemChanged += OnCurrentItemChanged;
			global::InventorySystem.Items.ItemIdentifier curItem = base.TargetHub.inventory.CurItem;
			OnCurrentItemChanged(base.TargetHub, curItem, curItem);
		}

		internal override void OnStoppedSpectating()
		{
			FpcModule.CharacterModelInstance.SetVisibility(newState: true);
			global::InventorySystem.Inventory.OnCurrentItemChanged -= OnCurrentItemChanged;
		}

		private void OnCurrentItemChanged(ReferenceHub hub, global::InventorySystem.Items.ItemIdentifier oldItem, global::InventorySystem.Items.ItemIdentifier newItem)
		{
		}
	}
}
