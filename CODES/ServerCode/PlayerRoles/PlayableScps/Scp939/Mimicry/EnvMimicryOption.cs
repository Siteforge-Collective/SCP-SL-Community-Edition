namespace PlayerRoles.PlayableScps.Scp939.Mimicry
{
	[global::System.Serializable]
	public class EnvMimicryOption
	{
		private global::PlayerRoles.PlayableScps.Scp939.Mimicry.EnvMimicrySequence _currentlyPlayed;

		private bool _hasSound;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp939.Mimicry.EnvMimicrySequence[] _randomSequences;

		[field: global::UnityEngine.SerializeField]
		public global::PlayerRoles.RoleTypeId RoleCondition { get; private set; } = global::PlayerRoles.RoleTypeId.None;

		[field: global::UnityEngine.SerializeField]
		public global::UnityEngine.Texture MainIcon { get; private set; }

		[field: global::UnityEngine.SerializeField]
		public global::UnityEngine.Texture CategoryIcon { get; private set; }

		public void Play(byte id)
		{
			_currentlyPlayed = global::UnityEngine.Object.Instantiate(_randomSequences[id % _randomSequences.Length]);
			if (_currentlyPlayed == null)
			{
				_hasSound = false;
				return;
			}
			_hasSound = true;
			_currentlyPlayed.EnqueueAll();
		}

		public void UpdateSequence(global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicPointController mimicPoint)
		{
			if (_hasSound && !(_currentlyPlayed == null) && !_currentlyPlayed.UpdateSequence(mimicPoint.MimicPointTransform))
			{
				_hasSound = false;
				_currentlyPlayed = null;
				global::UnityEngine.Object.Destroy(_currentlyPlayed);
			}
		}
	}
}
