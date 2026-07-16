namespace PlayerRoles.PlayableScps.Scp939
{
	public class Scp939VisibilityController : global::PlayerRoles.FirstPersonControl.FpcVisibilityController, global::GameObjectPools.IPoolResettable
	{
		private struct LastSeenInfo
		{
			public double Time;

			public global::RelativePositioning.RelativePosition RelPos;

			public global::UnityEngine.Vector3 Velocity;

			public global::UnityEngine.Vector3 WorldPos => RelPos.Position + Velocity * Elapsed;

			public float Elapsed => (float)(global::Mirror.NetworkTime.time - Time);
		}

		[global::UnityEngine.SerializeField]
		private float _pingTolerance;

		[global::UnityEngine.SerializeField]
		private float _defaultRange;

		[global::UnityEngine.SerializeField]
		private float _recentFootstepRangeMultiplier;

		[global::UnityEngine.SerializeField]
		private float _recentFootstepTime;

		[global::UnityEngine.SerializeField]
		private float _focusMultiplier;

		[global::UnityEngine.SerializeField]
		private float _exhaustionMultiplier;

		[global::UnityEngine.SerializeField]
		private float _fadeSpeed;

		[global::UnityEngine.SerializeField]
		private float _sustain;

		private global::PlayerRoles.PlayableScps.Scp939.Scp939Role _scpRole;

		private global::PlayerStatsSystem.StaminaStat _stamina;

		private global::PlayerRoles.PlayableScps.Scp939.Scp939FocusAbility _focus;

		private bool _wasFaded;

		private static readonly global::System.Collections.Generic.Dictionary<uint, global::PlayerRoles.PlayableScps.Scp939.Scp939VisibilityController.LastSeenInfo> LastSeen = new global::System.Collections.Generic.Dictionary<uint, global::PlayerRoles.PlayableScps.Scp939.Scp939VisibilityController.LastSeenInfo>();

		private readonly global::System.Collections.Generic.Dictionary<uint, double> _lastFootstepSounds = new global::System.Collections.Generic.Dictionary<uint, double>();

		public float CurrentDetectionRange
		{
			get
			{
				float defaultRange = _defaultRange;
				return (defaultRange + defaultRange * _focusMultiplier * _focus.State) * global::UnityEngine.Mathf.Lerp(_exhaustionMultiplier, 1f, _stamina.NormalizedValue);
			}
		}

		private float DetectionRangeForPlayer(ReferenceHub hub)
		{
			float num = CurrentDetectionRange;
			if (_lastFootstepSounds.TryGetValue(hub.netId, out var value) && global::Mirror.NetworkTime.time - value < (double)_recentFootstepTime)
			{
				num *= _recentFootstepRangeMultiplier;
			}
			return num;
		}

		private void OnDestroy()
		{
			if (_wasFaded)
			{
				ResetFade();
			}
		}

		private void LateUpdate()
		{
			if (base.Owner.isLocalPlayer || global::PlayerRoles.Spectating.SpectatorNetworking.IsLocallySpectated(base.Owner))
			{
				global::PlayerRoles.PlayerRolesUtils.ForEachRole<global::PlayerRoles.HumanRole>(UpdateHuman);
			}
		}

		private void UpdateHuman(ReferenceHub ply, global::PlayerRoles.HumanRole human)
		{
			global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule fpcModule = human.FpcModule;
			global::PlayerRoles.FirstPersonControl.FpcMotor motor = fpcModule.Motor;
			global::PlayerRoles.FirstPersonControl.Thirdperson.CharacterModel characterModelInstance = fpcModule.CharacterModelInstance;
			global::PlayerRoles.PlayableScps.Scp939.Scp939VisibilityController.LastSeenInfo value;
			bool flag = LastSeen.TryGetValue(ply.netId, out value);
			bool flag2 = !motor.IsInvisible;
			bool flag3 = flag2 && (global::UnityEngine.Vector3.Distance(fpcModule.Position, _scpRole.FpcModule.Position) <= DetectionRangeForPlayer(ply) || (flag && value.Elapsed < _sustain));
			float fade = characterModelInstance.Fade;
			characterModelInstance.Fade += global::UnityEngine.Time.deltaTime * (flag3 ? _fadeSpeed : (0f - _fadeSpeed));
			_wasFaded = true;
			if (global::Mirror.NetworkServer.active || !base.Owner.isLocalPlayer)
			{
				return;
			}
			if (characterModelInstance.Fade == 0f)
			{
				if (fade != 0f)
				{
					characterModelInstance.Hitboxes.ForEach(delegate(HitboxIdentity x)
					{
						x.SetColliders(newState: false);
					});
				}
			}
			else if (!flag2)
			{
				fpcModule.Position = (flag ? value.WorldPos : motor.ReceivedPosition.Position);
			}
			else
			{
				if (!flag3)
				{
					return;
				}
				if (fade == 0f)
				{
					fpcModule.Position = motor.ReceivedPosition.Position;
					(characterModelInstance as global::PlayerRoles.FirstPersonControl.Thirdperson.AnimatedCharacterModel).ForceUpdate();
					characterModelInstance.Hitboxes.ForEach(delegate(HitboxIdentity x)
					{
						x.SetColliders(newState: true);
					});
				}
				LastSeen[ply.netId] = new global::PlayerRoles.PlayableScps.Scp939.Scp939VisibilityController.LastSeenInfo
				{
					RelPos = new global::RelativePositioning.RelativePosition(fpcModule.Position),
					Time = global::Mirror.NetworkTime.time,
					Velocity = motor.Velocity
				};
			}
		}

		private void ResetFade()
		{
			global::PlayerRoles.PlayerRolesUtils.ForEachRole(delegate(global::PlayerRoles.HumanRole x)
			{
				global::PlayerRoles.FirstPersonControl.Thirdperson.CharacterModel characterModelInstance = x.FpcModule.CharacterModelInstance;
				if (characterModelInstance.Fade != 1f)
				{
					characterModelInstance.Fade = 1f;
					characterModelInstance.Hitboxes.ForEach(delegate(HitboxIdentity hitbox)
					{
						hitbox.SetColliders(newState: true);
					});
				}
			});
		}

		private void OnFootstepPlayed(global::PlayerRoles.FirstPersonControl.Thirdperson.AnimatedCharacterModel model, float range)
		{
			ReferenceHub ownerHub = model.OwnerHub;
			if (ownerHub.roleManager.CurrentRole is global::PlayerRoles.HumanRole humanRole && !((humanRole.FpcModule.Position - _scpRole.FpcModule.Position).sqrMagnitude > range * range))
			{
				_lastFootstepSounds[ownerHub.netId] = global::Mirror.NetworkTime.time;
			}
		}

		private void OnSpectatorTargetChanged()
		{
			if (_wasFaded)
			{
				ResetFade();
			}
		}

		private void OnRoleChanged(ReferenceHub hub, global::PlayerRoles.PlayerRoleBase oldRole, global::PlayerRoles.PlayerRoleBase newRole)
		{
			if (_wasFaded && hub.isLocalPlayer && !(newRole is global::PlayerRoles.Spectating.SpectatorRole))
			{
				ResetFade();
			}
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			_scpRole = base.Role as global::PlayerRoles.PlayableScps.Scp939.Scp939Role;
			base.Owner.playerStats.TryGetModule<global::PlayerStatsSystem.StaminaStat>(out _stamina);
			_scpRole.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp939.Scp939FocusAbility>(out _focus);
			global::PlayerRoles.Spectating.SpectatorTargetTracker.OnTargetChanged = (global::System.Action)global::System.Delegate.Combine(global::PlayerRoles.Spectating.SpectatorTargetTracker.OnTargetChanged, new global::System.Action(OnSpectatorTargetChanged));
			global::PlayerRoles.PlayerRoleManager.OnRoleChanged += OnRoleChanged;
			global::PlayerRoles.FirstPersonControl.Thirdperson.AnimatedCharacterModel.OnFootstepPlayed = (global::System.Action<global::PlayerRoles.FirstPersonControl.Thirdperson.AnimatedCharacterModel, float>)global::System.Delegate.Combine(global::PlayerRoles.FirstPersonControl.Thirdperson.AnimatedCharacterModel.OnFootstepPlayed, new global::System.Action<global::PlayerRoles.FirstPersonControl.Thirdperson.AnimatedCharacterModel, float>(OnFootstepPlayed));
			if (base.Owner.isLocalPlayer)
			{
				LastSeen.Clear();
			}
		}

		public override bool ValidateVisibility(ReferenceHub hub)
		{
			if (!base.ValidateVisibility(hub))
			{
				return false;
			}
			if (!(hub.roleManager.CurrentRole is global::PlayerRoles.HumanRole humanRole))
			{
				return true;
			}
			global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule fpcModule = _scpRole.FpcModule;
			float a = BaseRangeForPlayer(hub, humanRole);
			a = global::UnityEngine.Mathf.Max(a, DetectionRangeForPlayer(hub));
			a += fpcModule.MaxMovementSpeed * _pingTolerance;
			bool num = (humanRole.FpcModule.Position - fpcModule.Position).sqrMagnitude <= a * a;
			global::PlayerRoles.PlayableScps.Scp939.Scp939VisibilityController.LastSeenInfo value;
			bool result = num || (LastSeen.TryGetValue(hub.netId, out value) && value.Elapsed < _sustain);
			if (!num || _scpRole.IsLocalPlayer)
			{
				return result;
			}
			LastSeen[hub.netId] = new global::PlayerRoles.PlayableScps.Scp939.Scp939VisibilityController.LastSeenInfo
			{
				Time = global::Mirror.NetworkTime.time
			};
			return true;
		}

		private float BaseRangeForPlayer(ReferenceHub hub, global::PlayerRoles.HumanRole human)
		{
			float num = 0f;
			global::InventorySystem.Items.ItemBase curInstance = hub.inventory.CurInstance;
			global::InventorySystem.Items.Usables.PlayerHandler value;
			if (curInstance is global::InventorySystem.Items.MicroHID.MicroHIDItem microHIDItem)
			{
				if (microHIDItem != null && microHIDItem.State != global::InventorySystem.Items.MicroHID.HidState.Idle && microHIDItem.State != global::InventorySystem.Items.MicroHID.HidState.StopSound)
				{
					num = 30f;
				}
			}
			else if (curInstance is global::InventorySystem.Items.Usables.UsableItem && global::InventorySystem.Items.Usables.UsableItemsController.Handlers.TryGetValue(hub, out value) && curInstance != null && value.CurrentUsable.ItemSerial == curInstance.ItemSerial)
			{
				num = 15f;
			}
			if (human.VoiceModule is global::PlayerRoles.Voice.HumanVoiceModule humanVoiceModule && humanVoiceModule.ServerIsSending)
			{
				num = global::UnityEngine.Mathf.Max(num, humanVoiceModule.ProximityPlayback.Source.maxDistance);
			}
			return num;
		}

		public void ResetObject()
		{
			global::PlayerRoles.Spectating.SpectatorTargetTracker.OnTargetChanged = (global::System.Action)global::System.Delegate.Remove(global::PlayerRoles.Spectating.SpectatorTargetTracker.OnTargetChanged, new global::System.Action(OnSpectatorTargetChanged));
			global::PlayerRoles.PlayerRoleManager.OnRoleChanged -= OnRoleChanged;
			global::PlayerRoles.FirstPersonControl.Thirdperson.AnimatedCharacterModel.OnFootstepPlayed = (global::System.Action<global::PlayerRoles.FirstPersonControl.Thirdperson.AnimatedCharacterModel, float>)global::System.Delegate.Remove(global::PlayerRoles.FirstPersonControl.Thirdperson.AnimatedCharacterModel.OnFootstepPlayed, new global::System.Action<global::PlayerRoles.FirstPersonControl.Thirdperson.AnimatedCharacterModel, float>(OnFootstepPlayed));
			if (_wasFaded)
			{
				ResetFade();
			}
		}
	}
}
