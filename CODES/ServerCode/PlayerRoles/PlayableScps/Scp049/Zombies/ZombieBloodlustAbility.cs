namespace PlayerRoles.PlayableScps.Scp049.Zombies
{
	public class ZombieBloodlustAbility : global::PlayerRoles.PlayableScps.Subroutines.ScpSubroutineBase, global::GameObjectPools.IPoolResettable
	{
		[global::UnityEngine.SerializeField]
		private float _maxViewDistance;

		private float _simulatedStareTime;

		private readonly global::System.Diagnostics.Stopwatch _simulatedStareSw = global::System.Diagnostics.Stopwatch.StartNew();

		public bool LookingAtTarget { get; private set; }

		public float SimulatedStare
		{
			get
			{
				return global::UnityEngine.Mathf.Max(0f, _simulatedStareTime - (float)_simulatedStareSw.Elapsed.TotalSeconds);
			}
			set
			{
				_simulatedStareTime = value;
				_simulatedStareSw.Restart();
			}
		}

		private void Update()
		{
			RefreshChaseState();
		}

		public void RefreshChaseState()
		{
			if (global::Mirror.NetworkServer.active && base.Role.TryGetOwner(out var hub))
			{
				bool flag = SimulatedStare > 0f;
				LookingAtTarget = flag || AnyTargets(hub, hub.PlayerCameraReference);
				ServerSendRpc(toAll: true);
			}
		}

		private bool AnyTargets(ReferenceHub owner, global::UnityEngine.Transform camera)
		{
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				if (allHub.IsHuman() && !allHub.playerEffectsController.GetEffect<global::CustomPlayerEffects.Invisible>().IsEnabled && allHub.roleManager.CurrentRole is global::PlayerRoles.FirstPersonControl.IFpcRole fpcRole && global::PlayerRoles.PlayableScps.VisionInformation.GetVisionInformation(owner, camera, fpcRole.FpcModule.Position, fpcRole.FpcModule.CharacterControllerSettings.Radius, _maxViewDistance).IsLooking)
				{
					return true;
				}
			}
			return false;
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, LookingAtTarget);
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			LookingAtTarget = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
		}

		public void ResetObject()
		{
			_simulatedStareTime = 0f;
		}
	}
}
