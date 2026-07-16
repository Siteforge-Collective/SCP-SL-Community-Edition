using System;
using UnityEngine;

namespace InventorySystem.Items.Firearms
{
	[Serializable]
	public struct FirearmAudioClip
	{
		public AudioClip Sound;

		public float MaxDistance;

		[SerializeField]
		private FirearmAudioFlags _flags;

		public bool HasFlag(FirearmAudioFlags flag)
		{
			return false;
		}
	}
}
