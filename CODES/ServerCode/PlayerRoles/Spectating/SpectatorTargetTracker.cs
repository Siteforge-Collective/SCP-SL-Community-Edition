namespace PlayerRoles.Spectating
{
	public class SpectatorTargetTracker : global::UnityEngine.MonoBehaviour
	{
		private static bool _trackedTransformSet;

		private static global::UnityEngine.Transform _trackedTransform;

		private static global::UnityEngine.Vector3 _trackedTransformPositionOffset;

		private static global::UnityEngine.Quaternion _trackedTransformRotationOffset;

		private static global::PlayerRoles.Spectating.SpectatableModuleBase _curTracked;

		private static Offset _offsetCache;

		public static global::System.Action OnTargetChanged;

		private static global::UnityEngine.Transform TrackedTransform
		{
			get
			{
				int num;
				if (_trackedTransformSet)
				{
					num = ((_trackedTransform == null) ? 1 : 0);
					if (num == 0)
					{
						goto IL_001e;
					}
				}
				else
				{
					num = 1;
				}
				_trackedTransformSet = false;
				goto IL_001e;
				IL_001e:
				if (num == 0)
				{
					return _trackedTransform;
				}
				return MainCameraController.CurrentCamera;
			}
		}

		public static bool TrackerSet { get; private set; }

		public static global::PlayerRoles.Spectating.SpectatorTargetTracker Singleton { get; private set; }

		public static ReferenceHub LastTrackedPlayer { get; private set; }

		public static global::PlayerRoles.Spectating.SpectatableModuleBase CurrentTarget
		{
			get
			{
				return _curTracked;
			}
			set
			{
				if (!(_curTracked == value))
				{
					if (_curTracked != null)
					{
						_curTracked.OnStoppedSpectating();
					}
					_curTracked = value;
					MainCameraController.ForceUpdatePosition();
					if (value != null)
					{
						LastTrackedPlayer = (value.MainRole.TryGetOwner(out var hub) ? hub : null);
						value.OnBeganSpectating();
					}
					OnTargetChanged?.Invoke();
				}
			}
		}

		public static Offset CurrentOffset
		{
			get
			{
				if (_trackedTransformSet || !TrackerSet)
				{
					global::UnityEngine.Transform trackedTransform = TrackedTransform;
					_offsetCache.position = trackedTransform.TransformPoint(_trackedTransformPositionOffset);
					_offsetCache.rotation = (trackedTransform.rotation * _trackedTransformRotationOffset).eulerAngles;
				}
				else if (CurrentTarget != null)
				{
					_offsetCache.position = CurrentTarget.CameraPosition;
					_offsetCache.rotation = CurrentTarget.CameraRotation;
				}
				return _offsetCache;
			}
		}

		private void OnEnable()
		{
			Singleton = this;
			TrackerSet = true;
		}

		private void OnDisable()
		{
			CurrentTarget = null;
			if (Singleton == this)
			{
				TrackerSet = false;
			}
		}

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			global::PlayerRoles.PlayerRoleManager.OnRoleChanged += OnRoleChanged;
		}

		private static void OnRoleChanged(ReferenceHub hub, global::PlayerRoles.PlayerRoleBase prevRole, global::PlayerRoles.PlayerRoleBase newRole)
		{
			if (TrackerSet && prevRole is global::PlayerRoles.Spectating.ISpectatableRole spectatableRole && CurrentTarget == spectatableRole.SpectatorModule)
			{
				CurrentTarget = ((newRole is global::PlayerRoles.Spectating.ISpectatableRole spectatableRole2) ? spectatableRole2.SpectatorModule : null);
			}
			if (!hub.isLocalPlayer)
			{
				return;
			}
			if (newRole is global::PlayerRoles.Spectating.SpectatorRole spectatorRole)
			{
				LastTrackedPlayer = hub;
				if (Singleton != null)
				{
					Singleton.gameObject.SetActive(value: true);
				}
				else
				{
					Singleton = global::UnityEngine.Object.Instantiate(spectatorRole.TrackerPrefab, global::UnityEngine.Vector3.zero, global::UnityEngine.Quaternion.identity, null);
				}
			}
			else if (TrackerSet)
			{
				Singleton.gameObject.SetActive(value: false);
			}
		}

		public static void SetTrackedTransform(global::UnityEngine.Transform trackedTransform, global::UnityEngine.Vector3 localPosOffset, global::UnityEngine.Quaternion localRotOffset)
		{
			_trackedTransform = trackedTransform;
			_trackedTransformSet = trackedTransform != null;
			_trackedTransformPositionOffset = localPosOffset;
			_trackedTransformRotationOffset = localRotOffset;
		}

		public static void SetTrackedTransform(global::UnityEngine.Transform trackedTransform)
		{
			SetTrackedTransform(trackedTransform, global::UnityEngine.Vector3.zero, global::UnityEngine.Quaternion.identity);
		}

		public static bool TryGetTrackedPlayer(out ReferenceHub hub)
		{
			if (TrackerSet && CurrentTarget != null && CurrentTarget.MainRole.TryGetOwner(out hub))
			{
				return true;
			}
			hub = null;
			return false;
		}
	}
}
