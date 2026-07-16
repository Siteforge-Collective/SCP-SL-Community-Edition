using System.Collections.Generic;
using GameObjectPools;
using InventorySystem.Items.Firearms;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.FirstPersonControl.Thirdperson;
using PlayerStatsSystem;
using RelativePositioning;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp939
{
	public class Scp939VisibilityController : FpcVisibilityController, IPoolResettable
	{
		private struct LastSeenInfo
		{
			public double Time;

			public RelativePosition RelPos;

			public Vector3 Velocity;

			public Vector3 WorldPos => default(Vector3);

			public float Elapsed => 0f;
		}

		private const float DetectionRangeForShootingCrouchingOrJumping = 4f;

		[SerializeField]
		private float _pingTolerance;

		[SerializeField]
		private float _defaultRange;

		[SerializeField]
		private float _recentFootstepRangeMultiplier;

		[SerializeField]
		private float _recentFootstepTime;

		[SerializeField]
		private float _focusMultiplier;

		[SerializeField]
		private float _exhaustionMultiplier;

		[SerializeField]
		private float _fadeSpeed;

		[SerializeField]
		private float _sustain;

		private Scp939Role _scpRole;

		private StaminaStat _stamina;

		private Scp939FocusAbility _focus;

		private bool _wasFaded;

		private static readonly Dictionary<uint, LastSeenInfo> LastSeen;

		private readonly Dictionary<uint, double> _lastFootstepSounds;

		private readonly Dictionary<uint, double> _lastShotSound;

		public float CurrentDetectionRange => 0f;

		private float DetectionRangeForPlayer(ReferenceHub hub)
		{
			return 0f;
		}

		private void OnDestroy()
		{
		}

		private void LateUpdate()
		{
		}

		private void UpdateEnemies(ReferenceHub ply, FpcStandardRoleBase human)
		{
		}

		private void ResetFade()
		{
		}

		private void OnFootstepPlayed(AnimatedCharacterModel model, float range)
		{
		}

		private void OnSpectatorTargetChanged()
		{
		}

		private void OnRoleChanged(ReferenceHub hub, PlayerRoleBase oldRole, PlayerRoleBase newRole)
		{
		}

		public override void SpawnObject()
		{
		}

		public override bool ValidateVisibility(ReferenceHub hub)
		{
			return false;
		}

		private float BaseRangeForPlayer(ReferenceHub hub, FpcStandardRoleBase targetRole)
		{
			return 0f;
		}

		private void OnServerPlayedFirearmSound(Firearm firearm, byte audioId, float dis)
		{
		}

		private void OnClientReceivedAudioMessage(GunAudioMessage message)
		{
		}

		private void OnPlayedFirearmSound(ReferenceHub hub)
		{
		}

		public void ResetObject()
		{
		}
	}
}
