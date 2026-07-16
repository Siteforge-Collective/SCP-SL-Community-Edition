namespace PlayerRoles.PlayableScps.Scp939.Ripples
{
	public class RippleTriggerBase : global::PlayerRoles.PlayableScps.Subroutines.ScpStandardSubroutine<global::PlayerRoles.PlayableScps.Scp939.Scp939Role>
	{
		private bool _playerSet;

		private global::PlayerRoles.PlayableScps.Scp939.Ripples.RipplePlayer _player;

		private static int _playerIndex;

		protected global::PlayerRoles.PlayableScps.Scp939.Ripples.RipplePlayer Player
		{
			get
			{
				if (_playerSet)
				{
					return _player;
				}
				_player = base.ScpRole.SubroutineModule.AllSubroutines[PlayerIndex] as global::PlayerRoles.PlayableScps.Scp939.Ripples.RipplePlayer;
				_playerSet = true;
				return _player;
			}
		}

		protected bool IsLocalOrSpectated
		{
			get
			{
				if (!base.Owner.isLocalPlayer)
				{
					return global::PlayerRoles.Spectating.SpectatorNetworking.IsLocallySpectated(base.Owner);
				}
				return true;
			}
		}

		private int PlayerIndex
		{
			get
			{
				if (_playerIndex > 0)
				{
					return _playerIndex;
				}
				global::PlayerRoles.PlayableScps.Subroutines.SubroutineManagerModule subroutineModule = base.ScpRole.SubroutineModule;
				for (int i = 0; i < subroutineModule.AllSubroutines.Length; i++)
				{
					if (subroutineModule.AllSubroutines[i] is global::PlayerRoles.PlayableScps.Scp939.Ripples.RipplePlayer)
					{
						return _playerIndex = i;
					}
				}
				throw new global::System.InvalidOperationException("SCP-939 has no RipplePlayer subroutine!");
			}
		}

		protected void PlayInRange(global::UnityEngine.Vector3 pos, float maxRange, global::UnityEngine.Color color)
		{
			PlayInRangeSqr(pos, maxRange * maxRange, color);
		}

		protected void PlayInRangeSqr(global::UnityEngine.Vector3 pos, float maxRangeSqr, global::UnityEngine.Color color)
		{
			if (!((pos - base.ScpRole.FpcModule.Position).sqrMagnitude > maxRangeSqr))
			{
				Player.Play(pos, color);
			}
		}
	}
}
