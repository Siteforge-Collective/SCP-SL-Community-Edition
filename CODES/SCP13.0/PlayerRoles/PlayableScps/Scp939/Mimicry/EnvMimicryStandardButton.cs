using UnityEngine;
using UnityEngine.UI;

namespace PlayerRoles.PlayableScps.Scp939.Mimicry
{
	[RequireComponent(typeof(Button))]
	public class EnvMimicryStandardButton : MonoBehaviour
	{
		[SerializeField]
		private EnvMimicrySequence[] _randomSequences;

		private bool _prevState;

		private Button _button;

		private GameObject _buttonGameObject;

		private bool _cacheSet;

		private EnvironmentalMimicry _cachedSubroutine;

		protected virtual bool IsAvailable => false;

		protected virtual void Awake()
		{
		}

		protected virtual void OnDestroy()
		{
		}

		protected virtual void AlwaysUpdate()
		{
		}

		protected virtual void OnButtonPressed()
		{
		}

		private bool TryGetLocalSubroutine(out EnvironmentalMimicry localSubroutine)
		{
			localSubroutine = null;
			return false;
		}
	}
}
