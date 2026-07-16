using System;
using System.Collections.Generic;
using System.Diagnostics;
using PlayerRoles.FirstPersonControl;
using UnityEngine;
using VoiceChat;

namespace PlayerRoles.Voice
{
	public class VoiceBubble : MonoBehaviour
	{
		[Serializable]
		private struct Bubble
		{
			public Sprite Icon;

			public float MaxDistance;

			public VoiceChatChannel Channel;
		}

		[SerializeField]
		private Bubble[] _serializedOverrides;

		[SerializeField]
		private VoiceModuleBase _voiceModule;

		[SerializeField]
		private FirstPersonMovementModule _fpc;

		[SerializeField]
		private SpriteRenderer _rend;

		[SerializeField]
		private Gradient _colorOverNormalizedDistance;

		private const float SustainTime = 0.15f;

		private readonly Dictionary<VoiceChatChannel, Bubble> _overrides;

		private readonly Stopwatch _sustainSw;

		private Transform _t;

		private bool _isCulled;

		private bool IsHidden => false;

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private void UpdateIcon()
		{
		}
	}
}
