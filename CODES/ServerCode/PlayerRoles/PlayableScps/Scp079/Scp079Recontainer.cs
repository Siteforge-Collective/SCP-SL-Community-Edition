namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079Recontainer : global::UnityEngine.MonoBehaviour
	{
		public static readonly global::System.Collections.Generic.HashSet<global::MapGeneration.Distributors.Scp079Generator> AllGenerators = new global::System.Collections.Generic.HashSet<global::MapGeneration.Distributors.Scp079Generator>();

		[global::UnityEngine.SerializeField]
		private global::Interactables.Interobjects.DoorUtils.DoorVariant[] _containmentGates;

		[global::UnityEngine.SerializeField]
		private float _activationDelay;

		[global::UnityEngine.SerializeField]
		private float _lockdownDuration;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Transform _activatorButton;

		[global::UnityEngine.SerializeField]
		private BreakableWindow _activatorGlass;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Vector3 _activatorPos;

		[global::UnityEngine.SerializeField]
		private float _activatorLerpSpeed;

		[global::UnityEngine.SerializeField]
		private string _announcementProgress;

		[global::UnityEngine.SerializeField]
		private string _announcementAllActivated;

		[global::UnityEngine.SerializeField]
		private string _announcementCountdown;

		[global::UnityEngine.SerializeField]
		private string _announcementSuccess;

		[global::UnityEngine.SerializeField]
		private string _announcementFailure;

		private const float AnnouncementGlitchChance = 0.035f;

		private const float AnnouncementJamChance = 0.03f;

		private bool _alreadyRecontained;

		private bool _success;

		private int _prevEngaged;

		private float _recontainLater;

		private readonly global::System.Diagnostics.Stopwatch _delayStopwatch = new global::System.Diagnostics.Stopwatch();

		private readonly global::System.Diagnostics.Stopwatch _unlockStopwatch = new global::System.Diagnostics.Stopwatch();

		private readonly global::System.Collections.Generic.HashSet<global::Interactables.Interobjects.DoorUtils.DoorVariant> _lockedDoors = new global::System.Collections.Generic.HashSet<global::Interactables.Interobjects.DoorUtils.DoorVariant>();

		private bool CassieBusy => NineTailedFoxAnnouncer.singleton.queue.Count > 0;

		private void Start()
		{
			SetContainmentDoors(opened: false, locked: true);
			global::PlayerRoles.PlayerRoleManager.OnServerRoleSet += OnServerRoleChanged;
		}

		private void OnDestroy()
		{
			global::PlayerRoles.PlayerRoleManager.OnServerRoleSet -= OnServerRoleChanged;
		}

		private void Update()
		{
			if (!global::Mirror.NetworkServer.active)
			{
				return;
			}
			RefreshActivator();
			RefreshAmount();
			if (_unlockStopwatch.IsRunning && _unlockStopwatch.Elapsed.TotalSeconds > (double)_lockdownDuration)
			{
				EndOvercharge();
				_unlockStopwatch.Stop();
			}
			if (_recontainLater > 0f)
			{
				_delayStopwatch.Stop();
				if (!CassieBusy)
				{
					_recontainLater -= global::UnityEngine.Time.deltaTime;
				}
				if (_recontainLater <= 0f)
				{
					Recontain();
				}
			}
		}

		private void OnServerRoleChanged(ReferenceHub hub, global::PlayerRoles.RoleTypeId newRole, global::PlayerRoles.RoleChangeReason reason)
		{
			if (newRole != global::PlayerRoles.RoleTypeId.Spectator || !IsScpButNot079(hub.roleManager.CurrentRole) || global::PlayerRoles.PlayableScps.Scp079.Scp079Role.ActiveInstances.Count == 0 || global::Utils.NonAllocLINQ.HashsetExtensions.Count(ReferenceHub.AllHubs, (ReferenceHub x) => x != hub && IsScpButNot079(x.roleManager.CurrentRole)) > 0)
			{
				return;
			}
			SetContainmentDoors(opened: true, locked: true);
			_alreadyRecontained = true;
			_recontainLater = 3f;
			foreach (global::MapGeneration.Distributors.Scp079Generator allGenerator in AllGenerators)
			{
				allGenerator.Engaged = true;
			}
		}

		private bool IsScpButNot079(global::PlayerRoles.PlayerRoleBase prb)
		{
			if (prb.Team == global::PlayerRoles.Team.SCPs)
			{
				return prb.RoleTypeId != global::PlayerRoles.RoleTypeId.Scp079;
			}
			return false;
		}

		private void RefreshActivator()
		{
			if (_delayStopwatch.Elapsed.TotalSeconds > (double)_activationDelay)
			{
				if (_delayStopwatch.IsRunning)
				{
					BeginOvercharge();
					_delayStopwatch.Stop();
					_unlockStopwatch.Start();
				}
			}
			else if (_activatorGlass.isBroken)
			{
				_activatorButton.transform.localPosition = global::UnityEngine.Vector3.Lerp(_activatorButton.transform.localPosition, _activatorPos, _activatorLerpSpeed * global::UnityEngine.Time.deltaTime);
				if (!_alreadyRecontained && !CassieBusy)
				{
					Recontain();
				}
			}
		}

		private void Recontain()
		{
			_delayStopwatch.Restart();
			PlayAnnouncement(_announcementCountdown, 0f);
			global::Utils.Networking.NetworkUtils.SendToAuthenticated(new global::Subtitles.SubtitleMessage(new global::Subtitles.SubtitlePart(global::Subtitles.SubtitleType.OverchargeIn, (string[])null)));
			_alreadyRecontained = true;
		}

		private void RefreshAmount()
		{
			if (_alreadyRecontained)
			{
				return;
			}
			int num = 0;
			foreach (global::MapGeneration.Distributors.Scp079Generator allGenerator in AllGenerators)
			{
				if (allGenerator.Engaged)
				{
					num++;
				}
			}
			if (num > _prevEngaged)
			{
				UpdateStatus(num);
				_prevEngaged = num;
			}
		}

		private void SetContainmentDoors(bool opened, bool locked)
		{
			if (global::Mirror.NetworkServer.active)
			{
				global::Interactables.Interobjects.DoorUtils.DoorVariant[] containmentGates = _containmentGates;
				foreach (global::Interactables.Interobjects.DoorUtils.DoorVariant obj in containmentGates)
				{
					obj.NetworkTargetState = opened;
					obj.ServerChangeLock(global::Interactables.Interobjects.DoorUtils.DoorLockReason.SpecialDoorFeature, locked);
				}
			}
		}

		private void UpdateStatus(int engagedGenerators)
		{
			int count = AllGenerators.Count;
			string text = string.Format(_announcementProgress, engagedGenerators, count);
			global::System.Collections.Generic.List<global::Subtitles.SubtitlePart> list = new global::System.Collections.Generic.List<global::Subtitles.SubtitlePart>();
			list.Add(new global::Subtitles.SubtitlePart(global::Subtitles.SubtitleType.GeneratorsActivated, engagedGenerators.ToString(), count.ToString()));
			global::System.Collections.Generic.List<global::Subtitles.SubtitlePart> list2 = list;
			if (engagedGenerators >= count)
			{
				text += _announcementAllActivated;
				SetContainmentDoors(opened: true, global::PlayerRoles.PlayableScps.Scp079.Scp079Role.ActiveInstances.Count > 0);
				list2.Add(new global::Subtitles.SubtitlePart(global::Subtitles.SubtitleType.AllGeneratorsEngaged, (string[])null));
			}
			global::Utils.Networking.NetworkUtils.SendToAuthenticated(new global::Subtitles.SubtitleMessage(list2.ToArray()));
			PlayAnnouncement(text, 1f);
		}

		private void BeginOvercharge()
		{
			_success = TryKill079();
			bool inProgress = AlphaWarheadController.InProgress;
			foreach (global::System.Collections.Generic.KeyValuePair<global::Interactables.IInteractable, global::System.Collections.Generic.Dictionary<byte, global::Interactables.InteractableCollider>> allInstance in global::Interactables.InteractableCollider.AllInstances)
			{
				if (allInstance.Key is global::Interactables.Interobjects.BasicDoor basicDoor && !(basicDoor == null) && basicDoor.RequiredPermissions.RequiredPermissions == global::Interactables.Interobjects.DoorUtils.KeycardPermissions.None && global::MapGeneration.RoomIdentifier.RoomsByCoordinates.TryGetValue(global::MapGeneration.RoomIdUtils.PositionToCoords(basicDoor.transform.position), out var value) && value.Zone == global::MapGeneration.FacilityZone.HeavyContainment)
				{
					basicDoor.NetworkTargetState = basicDoor.TargetState && inProgress;
					basicDoor.ServerChangeLock(global::Interactables.Interobjects.DoorUtils.DoorLockReason.NoPower, newState: true);
					_lockedDoors.Add(basicDoor);
				}
			}
			foreach (FlickerableLightController instance in FlickerableLightController.Instances)
			{
				if (global::MapGeneration.RoomIdentifier.RoomsByCoordinates.TryGetValue(global::MapGeneration.RoomIdUtils.PositionToCoords(instance.transform.position), out var value2) && value2.Zone == global::MapGeneration.FacilityZone.HeavyContainment)
				{
					instance.ServerFlickerLights(_lockdownDuration);
				}
			}
			SetContainmentDoors(opened: true, locked: false);
		}

		private void EndOvercharge()
		{
			if (!_success)
			{
				PlayAnnouncement(_announcementFailure, 1f);
				global::Utils.Networking.NetworkUtils.SendToAuthenticated(new global::Subtitles.SubtitleMessage(new global::Subtitles.SubtitlePart(global::Subtitles.SubtitleType.OperationalMode, (string[])null)));
			}
			foreach (global::Interactables.Interobjects.DoorUtils.DoorVariant lockedDoor in _lockedDoors)
			{
				lockedDoor.ServerChangeLock(global::Interactables.Interobjects.DoorUtils.DoorLockReason.NoPower, newState: false);
			}
		}

		private bool TryKill079()
		{
			bool result = false;
			global::System.Collections.Generic.HashSet<ReferenceHub> hashSet = new global::System.Collections.Generic.HashSet<ReferenceHub>();
			foreach (global::PlayerRoles.PlayableScps.Scp079.Scp079Role activeInstance in global::PlayerRoles.PlayableScps.Scp079.Scp079Role.ActiveInstances)
			{
				if (activeInstance.TryGetOwner(out var hub))
				{
					hashSet.Add(hub);
				}
			}
			foreach (ReferenceHub item in hashSet)
			{
				result = true;
				if (_activatorGlass.LastAttacker.IsSet)
				{
					item.playerStats.DealDamage(new global::PlayerStatsSystem.RecontainmentDamageHandler(_activatorGlass.LastAttacker));
				}
				else
				{
					item.playerStats.DealDamage(new global::PlayerStatsSystem.UniversalDamageHandler(-1f, global::PlayerStatsSystem.DeathTranslations.Recontained));
				}
			}
			return result;
		}

		private void PlayAnnouncement(string annc, float glitchyMultiplier)
		{
			NineTailedFoxAnnouncer.singleton.ServerOnlyAddGlitchyPhrase(annc, 0.035f * glitchyMultiplier, 0.03f * glitchyMultiplier);
		}
	}
}
