using System;
using MapGeneration;
using PlayerRoles.PlayableScps.Scp079.Cameras;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.Overcons
{
	public class CameraOvercon : StandardOvercon
	{
		[Serializable]
		private struct ZoneOverrride
		{
			public FacilityZone Zone;

			public Sprite Icon;

			public Vector3 Offset;
		}

		private const float ColorSelectorTarget = 0.3f;

		private const float ExternalCamHeight = 3.2f;

		[SerializeField]
		private Sprite _defaultIcon;

		[SerializeField]
		private Vector3 _defaultOffset;

		[SerializeField]
		private ZoneOverrride[] _zoneOverrides;

		[SerializeField]
		private GameObject _externalIcon;

		private FacilityZone _prevZone;

		private Vector3 _prevOffset;

		private Vector3 _position;

		public Scp079Camera Target { get; private set; }

		public bool IsElevator { get; private set; }

		public Vector3 Position
		{
			get
			{
				return default(Vector3);
			}
			set
			{
			}
		}

		internal void Setup(Scp079Camera newCam, Scp079Camera target, bool isElevator)
		{
		}

		private void GetZoneOverrides(FacilityZone zone, out Sprite icon, out Vector3 offset)
		{
			icon = null;
			offset = default(Vector3);
		}

		private void LateUpdate()
		{
		}
	}
}
