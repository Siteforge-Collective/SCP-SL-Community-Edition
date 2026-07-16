namespace PlayerRoles.PlayableScps.Scp079.Cameras
{
	public class Scp079DirectionalCameraSelector : global::PlayerRoles.PlayableScps.Scp079.Scp079KeyAbilityBase
	{
		private static string _translationNoCamera;

		private static string _translationSwitch;

		private static readonly global::UnityEngine.Vector3Int[] WorldDirections = new global::UnityEngine.Vector3Int[4]
		{
			new global::UnityEngine.Vector3Int(-1, 0, 0),
			new global::UnityEngine.Vector3Int(0, 0, -1),
			new global::UnityEngine.Vector3Int(1, 0, 0),
			new global::UnityEngine.Vector3Int(0, 0, 1)
		};

		[global::UnityEngine.SerializeField]
		private ActionName _key;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Vector3 _direction;

		private global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera _lastCamera;

		private bool _lastValid;

		private float _lastSwitchCost;

		private float _failMessageSwitchCost;

		public override bool IsReady
		{
			get
			{
				_lastValid = TryGetCamera(out _lastCamera);
				if (!_lastValid)
				{
					return false;
				}
				_lastSwitchCost = base.CurrentCamSync.GetSwitchCost(_lastCamera);
				return _lastSwitchCost <= base.AuxManager.CurrentAux;
			}
		}

		public override ActionName ActivationKey => _key;

		public override bool IsVisible => !global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079CursorManager.LockCameras;

		public override string AbilityName
		{
			get
			{
				if (!_lastValid)
				{
					return _translationNoCamera;
				}
				return string.Format(_translationSwitch, _lastCamera.Label, _lastSwitchCost);
			}
		}

		public override string FailMessage
		{
			get
			{
				if (!(base.AuxManager.CurrentAux < _failMessageSwitchCost))
				{
					return null;
				}
				return GetNoAuxMessage(_failMessageSwitchCost);
			}
		}

		protected virtual bool TryGetCamera(out global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera targetCamera)
		{
			targetCamera = null;
			bool result = false;
			global::UnityEngine.Transform currentCamera = MainCameraController.CurrentCamera;
			global::UnityEngine.Vector3 normalized = currentCamera.TransformDirection(_direction).normalized;
			global::UnityEngine.Vector3Int vector3Int = global::UnityEngine.Vector3Int.zero;
			float num = -1f;
			global::UnityEngine.Vector3Int[] worldDirections = WorldDirections;
			foreach (global::UnityEngine.Vector3Int vector3Int2 in worldDirections)
			{
				float num2 = global::UnityEngine.Vector3.Dot(vector3Int2, normalized);
				if (!(num2 < num))
				{
					vector3Int = vector3Int2;
					num = num2;
				}
			}
			if (num <= 0f)
			{
				return false;
			}
			global::UnityEngine.Vector3Int vector3Int3 = global::MapGeneration.RoomIdUtils.PositionToCoords(currentCamera.position) + vector3Int;
			foreach (global::PlayerRoles.PlayableScps.Scp079.Overcons.CameraOvercon visibleOvercon in global::PlayerRoles.PlayableScps.Scp079.Overcons.CameraOverconRenderer.VisibleOvercons)
			{
				if (visibleOvercon.IsElevator)
				{
					continue;
				}
				global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera target = visibleOvercon.Target;
				if (!(global::MapGeneration.RoomIdUtils.PositionToCoords(target.Position) != vector3Int3))
				{
					targetCamera = target;
					result = true;
					if (targetCamera.IsMain)
					{
						return true;
					}
				}
			}
			return result;
		}

		protected override void Trigger()
		{
			base.CurrentCamSync.ClientSwitchTo(_lastCamera);
		}

		protected override void Start()
		{
			base.Start();
			_translationNoCamera = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.NoCamera);
			_translationSwitch = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.GoToCamera);
			base.CurrentCamSync.OnCameraChanged += delegate
			{
				_failMessageSwitchCost = 0f;
			};
		}

		public override void OnFailMessageAssigned()
		{
			_failMessageSwitchCost = (_lastValid ? _lastSwitchCost : 0f);
		}
	}
}
