using System.Collections.Generic;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp939.Mimicry
{
	public class MimicryRecordingsMenu : MimicryMenuBase
	{
		[SerializeField]
		private MimicryRecordingIcon _iconTemplate;

		[SerializeField]
		private RectTransform _iconsParent;

		[SerializeField]
		private float _spacing;

		[SerializeField]
		private float _lerpSpeed;

		[SerializeField]
		private float _dragIconOffset;

		[SerializeField]
		private float _dragPositionOffset;

		private bool _updateNextFrame;

		private MimicryRecorder _recorder;

		private MimicryRecordingIcon[] _instancesUserOrder;

		private MimicryRecordingIcon _draggedInstance;

		private readonly List<MimicryRecordingIcon> _instancesChronological;

		protected override void Setup(Scp939Role role)
		{
		}

		private void OnDestroy()
		{
		}

		private void OnVoicesModified()
		{
		}

		private bool GetDragData(out int occupiedIndex, out int targetIndex, out float targetHeight)
		{
			occupiedIndex = default(int);
			targetIndex = default(int);
			targetHeight = default(float);
			return false;
		}

		private void UpdateInstancePositions(float interpolant)
		{
		}

		private int GetPositionOffset(int instanceId, int origin, int target)
		{
			return 0;
		}

		private void AddInstance(int recordingId)
		{
		}

		private void RemoveInstance(MimicryRecordingIcon instance)
		{
		}

		private void Update()
		{
		}

		internal void BeginDrag(MimicryRecordingIcon icon)
		{
		}
	}
}
