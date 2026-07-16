using System;
using System.Runtime.CompilerServices;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp939.Ripples
{
	public class RippleTriggerBase : StandardSubroutine<Scp939Role>
	{
		private bool _playerSet;

		private RipplePlayer _player;

		private static int _playerIndex;

		protected RipplePlayer Player => null;

		protected bool IsLocalOrSpectated => false;

		private int PlayerIndex => 0;

		public static event Action<ReferenceHub> OnPlayedRippleLocally
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		protected void PlayInRange(Vector3 pos, float maxRange, Color color)
		{
		}

		protected void PlayInRangeSqr(Vector3 pos, float maxRangeSqr, Color color)
		{
		}

		protected void OnPlayedRipple(ReferenceHub hub)
		{
		}

		protected void ServerSendRpcToObservers()
		{
		}

		protected bool CheckVisibility(ReferenceHub ply)
		{
			return false;
		}
	}
}
