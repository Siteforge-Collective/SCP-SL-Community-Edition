namespace PlayerRoles.PlayableScps.Scp106
{
	public class Scp106StalkVisibilityController : global::PlayerRoles.PlayableScps.Subroutines.ScpStandardSubroutine<global::PlayerRoles.PlayableScps.Scp106.Scp106Role>
	{
		private const float AbsoluteDistance = 4f;

		private const float HealthToDistance = 0.3f;

		private const float InvisibleHeight = 8000f;

		private const float TransitionSpeed = 11.5f;

		private const float ServerTolerance = 5f;

		private const float SendCooldown = 0.08f;

		private const float SubmergeTolerance = 0.8f;

		private global::PlayerRoles.PlayableScps.Scp106.Scp106StalkAbility _stalk;

		private bool _anyFaded;

		private readonly global::System.Diagnostics.Stopwatch _sendStopwatch = global::System.Diagnostics.Stopwatch.StartNew();

		private readonly global::System.Collections.Generic.HashSet<global::PlayerRoles.FirstPersonControl.Thirdperson.CharacterModel> _affectedModels = new global::System.Collections.Generic.HashSet<global::PlayerRoles.FirstPersonControl.Thirdperson.CharacterModel>();

		public readonly global::System.Collections.Generic.Dictionary<int, byte> SyncDamage = new global::System.Collections.Generic.Dictionary<int, byte>();

		private void UpdateAll()
		{
			if (base.Owner.isLocalPlayer)
			{
				UpdateClient();
			}
			else if (global::PlayerRoles.Spectating.SpectatorNetworking.IsLocallySpectated(base.Owner))
			{
				UpdateSpectator();
			}
			else if (_anyFaded)
			{
				CleanupFade();
			}
			if (global::Mirror.NetworkServer.active)
			{
				UpdateServer();
			}
		}

		private void UpdateClient()
		{
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				if (!(allHub.roleManager.CurrentRole is global::PlayerRoles.HumanRole humanRole))
				{
					continue;
				}
				float num = (GetVisibilityForPlayer(allHub, humanRole) ? 11.5f : (-11.5f));
				global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule fpcModule = humanRole.FpcModule;
				global::PlayerRoles.FirstPersonControl.Thirdperson.CharacterModel characterModelInstance = fpcModule.CharacterModelInstance;
				bool flag = characterModelInstance.Fade == 0f;
				characterModelInstance.Fade += num * global::UnityEngine.Time.deltaTime;
				if (characterModelInstance.Fade < 1f)
				{
					_affectedModels.Add(characterModelInstance);
				}
				else
				{
					_affectedModels.Remove(characterModelInstance);
				}
				_anyFaded = true;
				if (!global::Mirror.NetworkServer.active)
				{
					if (characterModelInstance.Fade == 0f)
					{
						fpcModule.Position = global::UnityEngine.Vector3.up * 8000f;
					}
					else if (flag || fpcModule.Motor.IsInvisible)
					{
						fpcModule.Position = fpcModule.Motor.ReceivedPosition.Position;
					}
				}
			}
		}

		private void UpdateSpectator()
		{
			RefreshDamageDictionary();
			UpdateClient();
		}

		private void CleanupFade()
		{
			global::Utils.NonAllocLINQ.HashsetExtensions.ForEach(_affectedModels, delegate(global::PlayerRoles.FirstPersonControl.Thirdperson.CharacterModel x)
			{
				x.Fade = 1f;
			});
			_affectedModels.Clear();
			_anyFaded = false;
		}

		private void UpdateServer()
		{
			if (_stalk.IsActive)
			{
				RefreshDamageDictionary();
				if (!(_sendStopwatch.Elapsed.TotalSeconds < 0.07999999821186066))
				{
					_sendStopwatch.Restart();
					ServerSendRpc(toAll: false);
				}
			}
		}

		private bool GetVisibilityForPlayer(ReferenceHub hub, global::PlayerRoles.HumanRole role)
		{
			global::PlayerRoles.FirstPersonControl.FpcMotor motor = role.FpcModule.Motor;
			if (motor.IsInvisible)
			{
				return false;
			}
			if (!_stalk.IsActive || base.ScpRole.Sinkhole.NormalizedState < 0.8f)
			{
				return true;
			}
			if (hub.playerEffectsController.GetEffect<global::CustomPlayerEffects.Invigorated>().IsEnabled)
			{
				return false;
			}
			if (!SyncDamage.TryGetValue(hub.PlayerId, out var value))
			{
				value = 0;
			}
			return global::UnityEngine.Vector3.Distance(base.ScpRole.FpcModule.Position, motor.ReceivedPosition.Position) < (float)(int)value * 0.3f + 4f;
		}

		private void RefreshDamageDictionary()
		{
			SyncDamage.Clear();
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				if (!(allHub.roleManager.CurrentRole is global::PlayerRoles.HumanRole humanRole))
				{
					continue;
				}
				if (allHub.playerEffectsController.GetEffect<global::CustomPlayerEffects.Traumatized>().IsEnabled)
				{
					SyncDamage[allHub.PlayerId] = 0;
					continue;
				}
				global::PlayerStatsSystem.HealthStat module = allHub.playerStats.GetModule<global::PlayerStatsSystem.HealthStat>();
				int num = global::UnityEngine.Mathf.FloorToInt(module.MaxValue - module.CurValue);
				if (num != 0 && !(global::UnityEngine.Vector3.Distance(humanRole.FpcModule.Position, base.ScpRole.FpcModule.Position) - 5f > (float)num * 0.3f + 4f))
				{
					SyncDamage[allHub.PlayerId] = (byte)global::UnityEngine.Mathf.Clamp(num, 0, 255);
				}
			}
		}

		protected override void Awake()
		{
			base.Awake();
			GetSubroutine<global::PlayerRoles.PlayableScps.Scp106.Scp106StalkAbility>(out _stalk);
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			writer.WriteByte((byte)SyncDamage.Count);
			foreach (global::System.Collections.Generic.KeyValuePair<int, byte> item in SyncDamage)
			{
				writer.WriteRecyclablePlayerId(new RecyclablePlayerId(item.Key));
				writer.WriteByte(item.Value);
			}
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			SyncDamage.Clear();
			byte b = reader.ReadByte();
			for (int i = 0; i < b; i++)
			{
				int value = reader.ReadRecyclablePlayerId().Value;
				SyncDamage[value] = reader.ReadByte();
			}
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule.OnPositionUpdated += UpdateAll;
		}

		public override void ResetObject()
		{
			base.ResetObject();
			CleanupFade();
			global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule.OnPositionUpdated -= UpdateAll;
		}
	}
}
