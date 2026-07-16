namespace PlayerRoles.PlayableScps.Scp079.Overcons
{
	public class CameraOvercon : global::PlayerRoles.PlayableScps.Scp079.Overcons.StandardOvercon
	{
		[global::System.Serializable]
		private struct ZoneOverrride
		{
			public global::MapGeneration.FacilityZone Zone;

			public global::UnityEngine.Sprite Icon;

			public global::UnityEngine.Vector3 Offset;
		}

		private const float ColorSelectorTarget = 0.3f;

		private const float ExternalCamHeight = 3.2f;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Sprite _defaultIcon;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Vector3 _defaultOffset;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp079.Overcons.CameraOvercon.ZoneOverrride[] _zoneOverrides;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _externalIcon;

		private global::MapGeneration.FacilityZone _prevZone;

		private global::UnityEngine.Vector3 _prevOffset;

		private global::UnityEngine.Vector3 _position;

		public global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera Target { get; private set; }

		public bool IsElevator { get; private set; }

		public global::UnityEngine.Vector3 Position
		{
			get
			{
				return _position;
			}
			set
			{
				_position = value;
				base.transform.position = value;
			}
		}

		internal void Setup(global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera newCam, global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera target, bool isElevator)
		{
			Target = target;
			IsElevator = isElevator;
			global::MapGeneration.FacilityZone zone = Target.Room.Zone;
			if (zone != _prevZone)
			{
				GetZoneOverrides(zone, out var icon, out _prevOffset);
				TargetSprite.sprite = icon;
				_prevZone = zone;
			}
			global::UnityEngine.Vector3 position = target.transform.TransformPoint(_prevOffset);
			if (newCam.Room == Target.Room)
			{
				_externalIcon.SetActive(isElevator);
				Position = position;
				Rescale(newCam);
				return;
			}
			if (global::Interactables.Interobjects.DoorUtils.DoorVariant.DoorsByRoom.TryGetValue(Target.Room, out var value) && global::Utils.NonAllocLINQ.HashsetExtensions.TryGetFirst(value, (global::Interactables.Interobjects.DoorUtils.DoorVariant x) => x.Rooms.Contains(newCam.Room), out var first))
			{
				Position = first.transform.position + global::UnityEngine.Vector3.up * 3.2f;
				Rescale(newCam, 255f);
			}
			else
			{
				Position = position;
				Rescale(newCam);
			}
			_externalIcon.SetActive(value: true);
		}

		private void GetZoneOverrides(global::MapGeneration.FacilityZone zone, out global::UnityEngine.Sprite icon, out global::UnityEngine.Vector3 offset)
		{
			global::PlayerRoles.PlayableScps.Scp079.Overcons.CameraOvercon.ZoneOverrride[] zoneOverrides = _zoneOverrides;
			for (int i = 0; i < zoneOverrides.Length; i++)
			{
				global::PlayerRoles.PlayableScps.Scp079.Overcons.CameraOvercon.ZoneOverrride zoneOverrride = zoneOverrides[i];
				if (zoneOverrride.Zone == zone)
				{
					icon = zoneOverrride.Icon;
					offset = zoneOverrride.Offset;
					return;
				}
			}
			icon = _defaultIcon;
			offset = _defaultOffset;
		}

		private void LateUpdate()
		{
			TargetSprite.color = global::UnityEngine.Color.Lerp(global::PlayerRoles.PlayableScps.Scp079.Overcons.StandardOvercon.NormalColor, global::PlayerRoles.PlayableScps.Scp079.Overcons.StandardOvercon.HighlightedColor, IsHighlighted ? 1f : ((global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079ForwardCameraSelector.HighlightedCamera == Target) ? 0.3f : 0f));
		}
	}
}
