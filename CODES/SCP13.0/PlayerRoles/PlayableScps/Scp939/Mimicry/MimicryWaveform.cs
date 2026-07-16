using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using VoiceChat;

namespace PlayerRoles.PlayableScps.Scp939.Mimicry
{
	public class MimicryWaveform : UiWaveformVisualizer, IDragHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
	{
		private static MimicryWaveform _lastWaveform;

		private readonly Stopwatch _playbackSw;

		[SerializeField]
		private RectTransform _trimIndicator;

		[SerializeField]
		private float _minCoverage;

		[SerializeField]
		private float _maxCoverage;

		[SerializeField]
		private Vector2 _trimmerOffset;

		[SerializeField]
		private RectTransform _waveformProgressBar;

		[SerializeField]
		private RectTransform _progressBarLimiter;

		private bool _isSet;

		private bool _isDragging;

		private float _beginDragTime;

		private float _endDragTime;

		private double _startPlaybackTimeOffset;

		private double _playbackTotalDuration;

		private double _playbackMaxDuration;

		private float StopwatchElapsed => 0f;

		public bool IsPlaying => false;

		public void StartPlayback(int totalLengthSamples, out int startSample, out int lengthSamples)
		{
			startSample = default(int);
			lengthSamples = default(int);
		}

		public void StopPlayback()
		{
		}

		public void OnDrag(PointerEventData eventData)
		{
		}

		public void OnPointerUp(PointerEventData eventData)
		{
		}

		public void OnPointerDown(PointerEventData eventData)
		{
		}

		private void Apply()
		{
		}

		private bool TryGetTime(Vector2 mousePos, out float percent)
		{
			percent = default(float);
			return false;
		}

		private float SamplesToSeconds(int samples)
		{
			return 0f;
		}

		private void GetStartStop(out float startTime, out float stopTime)
		{
			startTime = default(float);
			stopTime = default(float);
		}

		private void Update()
		{
		}

		private void UpdateTrimIndicator()
		{
		}

		private void UpdateProgressBar()
		{
		}
	}
}
