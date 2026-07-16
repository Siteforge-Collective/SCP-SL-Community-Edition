namespace FacilitySoundtrack
{
	public class Scp173InsanitySoundtrack : global::FacilitySoundtrack.SoundtrackLayerBase
	{
		[global::System.Serializable]
		private class EncounterTrack
		{
			private float _nextUse;

			public global::UnityEngine.AudioClip Clip;

			public float Cooldown;

			public bool TryUse(float currentTime)
			{
				if (currentTime < _nextUse)
				{
					return false;
				}
				_nextUse = currentTime + Cooldown;
				return true;
			}

			public void ResetCooldown()
			{
				_nextUse = 0f;
			}
		}

		public const float PanLimit = 0.65f;

		[global::UnityEngine.SerializeField]
		private float _fadeInLerp;

		[global::UnityEngine.SerializeField]
		private float _fadeOutLerp;

		[global::UnityEngine.SerializeField]
		private float _sustainTime;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioSource[] _distanceAmbients;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioSource _encounterSource;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _goneClip;

		[global::UnityEngine.SerializeField]
		private global::FacilitySoundtrack.Scp173InsanitySoundtrack.EncounterTrack _closeEncounter;

		[global::UnityEngine.SerializeField]
		private global::FacilitySoundtrack.Scp173InsanitySoundtrack.EncounterTrack _farEncounter;

		[global::UnityEngine.SerializeField]
		private float _closeEncounterDistanceThreshold;

		[global::UnityEngine.SerializeField]
		private float _distanceLerp;

		[global::UnityEngine.SerializeField]
		private float _distanceCap;

		private readonly global::System.Collections.Generic.HashSet<global::PlayerRoles.PlayableScps.Scp173.Scp173Role> _observed173s = new global::System.Collections.Generic.HashSet<global::PlayerRoles.PlayableScps.Scp173.Scp173Role>();

		private global::UnityEngine.AnimationCurve[] _volumeCurves;

		private int _observed173sCount;

		private int _ambientsCount;

		private bool _isActive;

		private bool _prevPlay;

		private float _stopAmbientTime;

		private float _weight;

		private float _lastDistance;

		private bool _cameraSet;

		private global::UnityEngine.Camera _camera;

		private float _lastScreenPosition;

		private float CurTime => global::UnityEngine.Time.timeSinceLevelLoad;

		private bool IsObserved => _observed173sCount > 0;

		public override float Weight => _weight;

		public override bool Additive => false;

		private void Awake()
		{
			global::PlayerRoles.PlayerRoleManager.OnRoleChanged += OnRoleChanged;
			global::PlayerRoles.PlayableScps.Scp173.Scp173CharacterModel.OnFrozen += OnFrozen;
			_ambientsCount = _distanceAmbients.Length;
			_volumeCurves = new global::UnityEngine.AnimationCurve[_ambientsCount];
			for (int i = 0; i < _ambientsCount; i++)
			{
				_volumeCurves[i] = _distanceAmbients[i].GetCustomCurve(global::UnityEngine.AudioSourceCurveType.CustomRolloff);
			}
		}

		private void OnDestroy()
		{
			global::PlayerRoles.PlayerRoleManager.OnRoleChanged -= OnRoleChanged;
			global::PlayerRoles.PlayableScps.Scp173.Scp173CharacterModel.OnFrozen -= OnFrozen;
		}

		private void Update()
		{
			if (!_isActive || !ReferenceHub.TryGetLocalHub(out var lhub))
			{
				return;
			}
			if (_observed173sCount > 0)
			{
				_observed173sCount -= _observed173s.RemoveWhere((global::PlayerRoles.PlayableScps.Scp173.Scp173Role x) => !IsObservedBy(x, lhub));
				if (!IsObserved)
				{
					_stopAmbientTime = CurTime + _sustainTime;
				}
			}
			bool flag = IsObserved || _stopAmbientTime > CurTime;
			bool num = flag != _prevPlay;
			_weight = global::UnityEngine.Mathf.Lerp(_weight, flag ? 1 : 0, (flag ? _fadeInLerp : _fadeOutLerp) * global::UnityEngine.Time.deltaTime);
			_prevPlay = flag;
			if (num && !flag)
			{
				_encounterSource.PlayOneShot(_goneClip);
				_closeEncounter.ResetCooldown();
				_farEncounter.ResetCooldown();
			}
			if (!flag)
			{
				return;
			}
			float num2 = _distanceCap;
			foreach (global::PlayerRoles.PlayableScps.Scp173.Scp173Role observed in _observed173s)
			{
				float num3 = DistanceTo(observed);
				if (num3 < num2)
				{
					num2 = num3;
				}
			}
			_lastDistance = global::UnityEngine.Mathf.Lerp(_lastDistance, num2, global::UnityEngine.Time.deltaTime * _distanceLerp);
		}

		private void OnRoleChanged(ReferenceHub userHub, global::PlayerRoles.PlayerRoleBase prevRole, global::PlayerRoles.PlayerRoleBase newRole)
		{
			if (userHub.isLocalPlayer)
			{
				_isActive = global::PlayerRoles.PlayerRolesUtils.IsHuman(userHub);
				_prevPlay = false;
				_weight = 0f;
				_observed173sCount = 0;
				_stopAmbientTime = 0f;
			}
		}

		private void OnFrozen(global::PlayerRoles.PlayableScps.Scp173.Scp173Role target)
		{
			if (_observed173s.Add(target))
			{
				_observed173sCount++;
			}
			if (DistanceTo(target) < _closeEncounterDistanceThreshold && _closeEncounter.TryUse(CurTime))
			{
				_encounterSource.PlayOneShot(_closeEncounter.Clip);
			}
			else if (_farEncounter.TryUse(CurTime))
			{
				_encounterSource.PlayOneShot(_farEncounter.Clip);
			}
		}

		private float DistanceTo(global::PlayerRoles.PlayableScps.Scp173.Scp173Role role)
		{
			return global::UnityEngine.Vector3.Distance(role.FpcModule.Position, MainCameraController.CurrentCamera.position);
		}

		private bool IsObservedBy(global::PlayerRoles.PlayableScps.Scp173.Scp173Role scp173, ReferenceHub lhub)
		{
			if (scp173.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp173.Scp173ObserversTracker>(out var subroutine))
			{
				return subroutine.IsObservedBy(lhub);
			}
			return false;
		}

		public override void UpdateVolume(float volumeScale)
		{
			for (int i = 0; i < _ambientsCount; i++)
			{
				float num = volumeScale;
				if (num > 0f)
				{
					float time = _lastDistance / _distanceAmbients[i].maxDistance;
					num *= _volumeCurves[i].Evaluate(time);
				}
				_distanceAmbients[i].volume = num;
				_distanceAmbients[i].panStereo = _lastScreenPosition;
			}
		}
	}
}
