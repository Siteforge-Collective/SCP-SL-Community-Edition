using InventorySystem.Items.Thirdperson;
using PlayerRoles.FirstPersonControl.Thirdperson;
using UnityEngine;
using VoiceChat.Playbacks;

namespace InventorySystem.Items.Radio
{
	public class RadioThirdperson : ThirdpersonItemBase
	{
		[SerializeField]
		private AnimationClip _animation;

		[SerializeField]
		private SpatializedRadioPlaybackBase _playback;

		private uint _netId;

		private sbyte _prevVal;

		private void Update()
		{
		}

		internal override void Initialize(HumanCharacterModel model, ItemIdentifier id)
		{
		}
	}
}
