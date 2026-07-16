using System;
using MapGeneration;
using PlayerRoles.PlayableScps.Scp079.Cameras;
using TMPro;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079LostSignalGui : Scp079GuiElementBase
	{
		[Serializable]
		private struct ZoneClip
		{
			public FacilityZone Zone;

			public AudioClip Clip;
		}

		[SerializeField]
		private GameObject _rootObj;

		[SerializeField]
		private AudioSource _loopSource;

		[SerializeField]
		private TextMeshProUGUI _etaText;

		[SerializeField]
		private ZoneClip[] _zoneStarts;

		[SerializeField]
		private AudioClip _fallbackStart;

		[SerializeField]
		private ZoneClip[] _zoneLoops;

		[SerializeField]
		private AudioClip _fallbackLoop;

		private Scp079LostSignalHandler _handler;

		private Scp079CurrentCameraSync _curCamSync;

		private string _textFormat;

		internal override void Init(Scp079Role role, ReferenceHub owner)
		{
		}

		private void OnDestroy()
		{
		}

		private void UpdateScreen()
		{
		}

		private void Update()
		{
		}

		private void PlayStart(FacilityZone zone)
		{
		}

		private void PlayLoop(FacilityZone zone)
		{
		}

		private void PlayClip(AudioClip clip)
		{
		}
	}
}
