using System;
using System.Collections.Generic;
using MapGeneration;
using PlayerRoles.PlayableScps.Scp079.Cameras;
using UnityEngine;
using UnityEngine.Rendering;

namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079Nightvision : Scp079GuiElementBase
	{
		[Serializable]
		private struct ZoneNightvisionPair
		{
			public Volume PostProcess;

			public FacilityZone Zone;
		}

		[SerializeField]
		private ZoneNightvisionPair[] _pairs;

		private Scp079CurrentCameraSync _curCam;

		private Scp079LostSignalHandler _lostSignal;

		private readonly HashSet<Volume> _volumes;

		private readonly Dictionary<FacilityZone, Volume> _zoneTargets;

		private Volume TargetVolume => null;

		internal override void Init(Scp079Role role, ReferenceHub owner)
		{
		}

		private void OnDestroy()
		{
		}

		private void Update()
		{
		}

		private void UpdateAll()
		{
		}
	}
}
