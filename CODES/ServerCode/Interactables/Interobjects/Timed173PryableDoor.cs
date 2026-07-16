namespace Interactables.Interobjects
{
	public class Timed173PryableDoor : global::Interactables.Interobjects.PryableDoor
	{
		private readonly global::System.Diagnostics.Stopwatch _stopwatch = new global::System.Diagnostics.Stopwatch();

		[global::UnityEngine.SerializeField]
		private float _timeMark = 25f;

		[global::UnityEngine.Tooltip("Automatically opens the gate when the time is over and SCP-173 is spawned.")]
		[global::UnityEngine.SerializeField]
		private bool _smartOpen = true;

		protected override void Start()
		{
			base.Start();
			if (global::Mirror.NetworkServer.active)
			{
				CharacterClassManager.OnRoundStarted += _stopwatch.Start;
				ServerChangeLock(global::Interactables.Interobjects.DoorUtils.DoorLockReason.SpecialDoorFeature, newState: true);
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (global::Mirror.NetworkServer.active)
			{
				CharacterClassManager.OnRoundStarted -= _stopwatch.Start;
			}
		}

		protected override void Update()
		{
			base.Update();
			if (!_stopwatch.IsRunning || _stopwatch.Elapsed.TotalSeconds < (double)_timeMark)
			{
				return;
			}
			_stopwatch.Stop();
			ServerChangeLock(global::Interactables.Interobjects.DoorUtils.DoorLockReason.SpecialDoorFeature, newState: false);
			if (!_smartOpen)
			{
				return;
			}
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				if (global::PlayerRoles.PlayerRolesUtils.GetRoleId(allHub) == global::PlayerRoles.RoleTypeId.Scp173)
				{
					base.NetworkTargetState = true;
					break;
				}
			}
		}

		private void MirrorProcessed()
		{
		}
	}
}
