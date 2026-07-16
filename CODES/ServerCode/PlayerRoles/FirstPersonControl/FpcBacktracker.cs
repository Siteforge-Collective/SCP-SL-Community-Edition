namespace PlayerRoles.FirstPersonControl
{
	public class FpcBacktracker : global::System.IDisposable
	{
		private readonly global::UnityEngine.Vector3 _prevPos;

		private readonly global::UnityEngine.Quaternion _prevRot;

		private readonly global::UnityEngine.Vector3 _newPos;

		private readonly ReferenceHub _movedHub;

		private bool _moved;

		private bool _restoreUponDeath;

		public float MoveAmount => global::UnityEngine.Vector3.Distance(_newPos, _prevPos);

		public FpcBacktracker(ReferenceHub attacker, global::UnityEngine.Vector3 claimedPos, global::UnityEngine.Quaternion claimedRot, float backtrack = 0.1f, float forecast = 0.15f)
			: this(attacker, claimedPos, claimedRot, backtrack, forecast, ignoreTp: true, restoreUponDeath: true)
		{
		}

		public FpcBacktracker(ReferenceHub target, global::UnityEngine.Vector3 claimedPos, float backtrack = 0.4f)
			: this(target, claimedPos, target.PlayerCameraReference.rotation, backtrack, 0f, ignoreTp: true, restoreUponDeath: true)
		{
		}

		public FpcBacktracker(ReferenceHub hub, global::UnityEngine.Vector3 claimedPos, global::UnityEngine.Quaternion claimedRot, float backtrack, float forecast, bool ignoreTp, bool restoreUponDeath)
		{
			if (hub.roleManager.CurrentRole is global::PlayerRoles.FirstPersonControl.IFpcRole fpcRole)
			{
				global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule fpcModule = fpcRole.FpcModule;
				_moved = true;
				_movedHub = hub;
				_prevPos = fpcModule.Position;
				_prevRot = hub.PlayerCameraReference.rotation;
				global::UnityEngine.Bounds bounds = ((backtrack <= 0f) ? new global::UnityEngine.Bounds(fpcModule.Position, global::UnityEngine.Vector3.zero) : fpcModule.Tracer.GenerateBounds(backtrack, ignoreTp));
				if (forecast > 0f)
				{
					bounds.Encapsulate(_prevPos + fpcModule.Motor.Velocity * forecast);
				}
				_newPos = bounds.ClosestPoint(claimedPos);
				fpcModule.Position = _newPos;
				hub.PlayerCameraReference.rotation = claimedRot;
			}
			else
			{
				_moved = false;
			}
			if (restoreUponDeath)
			{
				global::PlayerStatsSystem.PlayerStats.OnAnyPlayerDied += OnDied;
				_restoreUponDeath = true;
			}
			else
			{
				_restoreUponDeath = false;
			}
		}

		public void Cancel()
		{
			_moved = true;
			_restoreUponDeath = false;
		}

		public void Dispose()
		{
			RestorePosition();
		}

		public void RestorePosition()
		{
			if (_moved)
			{
				if (_movedHub.roleManager.CurrentRole is global::PlayerRoles.FirstPersonControl.IFpcRole fpcRole)
				{
					fpcRole.FpcModule.Position = _prevPos;
					_movedHub.PlayerCameraReference.rotation = _prevRot;
				}
				_moved = false;
			}
			if (_restoreUponDeath)
			{
				global::PlayerStatsSystem.PlayerStats.OnAnyPlayerDied -= OnDied;
				_restoreUponDeath = false;
			}
		}

		private void OnDied(ReferenceHub ply, global::PlayerStatsSystem.DamageHandlerBase dhb)
		{
			if (!(ply != _movedHub))
			{
				RestorePosition();
			}
		}
	}
}
