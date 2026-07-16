namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079Nightvision : global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079GuiElementBase
	{
		[global::System.Serializable]
		private struct ZoneNightvisionPair
		{
			public global::UnityEngine.Rendering.PostProcessing.PostProcessVolume PostProcess;

			public global::MapGeneration.FacilityZone Zone;
		}

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079Nightvision.ZoneNightvisionPair[] _pairs;

		private global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync _curCam;

		private global::PlayerRoles.PlayableScps.Scp079.Scp079LostSignalHandler _lostSignal;

		private readonly global::System.Collections.Generic.HashSet<global::UnityEngine.Rendering.PostProcessing.PostProcessVolume> _volumes = new global::System.Collections.Generic.HashSet<global::UnityEngine.Rendering.PostProcessing.PostProcessVolume>();

		private readonly global::System.Collections.Generic.Dictionary<global::MapGeneration.FacilityZone, global::UnityEngine.Rendering.PostProcessing.PostProcessVolume> _zoneTargets = new global::System.Collections.Generic.Dictionary<global::MapGeneration.FacilityZone, global::UnityEngine.Rendering.PostProcessing.PostProcessVolume>();

		private global::UnityEngine.Rendering.PostProcessing.PostProcessVolume TargetVolume
		{
			get
			{
				if (_curCam.CurClientSwitchState != global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync.ClientSwitchState.None)
				{
					return null;
				}
				if (_lostSignal.Lost)
				{
					return null;
				}
				global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera currentCamera = base.Role.CurrentCamera;
				if (currentCamera == null)
				{
					return null;
				}
				if (!FlickerableLightController.IsInDarkenedRoom(currentCamera.Position))
				{
					return null;
				}
				if (!_zoneTargets.TryGetValue(currentCamera.Room.Zone, out var value))
				{
					return null;
				}
				return value;
			}
		}

		internal override void Init(global::PlayerRoles.PlayableScps.Scp079.Scp079Role role, ReferenceHub owner)
		{
			base.Init(role, owner);
			role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync>(out _curCam);
			role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp079.Scp079LostSignalHandler>(out _lostSignal);
			_curCam.OnCameraChanged += UpdateAll;
			global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079Nightvision.ZoneNightvisionPair[] pairs = _pairs;
			for (int i = 0; i < pairs.Length; i++)
			{
				global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079Nightvision.ZoneNightvisionPair zoneNightvisionPair = pairs[i];
				_volumes.Add(zoneNightvisionPair.PostProcess);
				_zoneTargets[zoneNightvisionPair.Zone] = zoneNightvisionPair.PostProcess;
			}
			UpdateAll();
		}

		private void OnDestroy()
		{
			if (!(_curCam == null))
			{
				_curCam.OnCameraChanged -= UpdateAll;
			}
		}

		private void Update()
		{
			UpdateAll();
		}

		private void UpdateAll()
		{
			global::UnityEngine.Rendering.PostProcessing.PostProcessVolume target = TargetVolume;
			global::Utils.NonAllocLINQ.HashsetExtensions.ForEach(_volumes, delegate(global::UnityEngine.Rendering.PostProcessing.PostProcessVolume x)
			{
				x.enabled = x == target;
			});
		}
	}
}
