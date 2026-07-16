using System;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem.Items.Firearms
{
	public class FirearmWorldmodel : MonoBehaviour
	{
		[Serializable]
		private struct AttachmentElement
		{
			[SerializeField]
			private GameObject[] _targetObjects;

			public void Refresh(bool state)
			{
			}
		}

		[Serializable]
		public class RiggedAttachmentElement
		{
			public int TargetAttachmentId;

			[SerializeField]
			private Transform _targetBone;

			[SerializeField]
			private Offset _disabled;

			[SerializeField]
			private Offset _enabled;

			[SerializeField]
			private bool _thirdpersonOnly;

			public void Refresh(bool isEnabled, bool thirdperson)
			{
			}
		}

		[Serializable]
		private class MagazineElement
		{
			[SerializeField]
			private GameObject _targetObject;

			[SerializeField]
			private int[] _attachmentIds;

			private uint[] _binaryCodes;

			private int _prevStatusCode;

			private uint[] BinaryCodes => null;

			private void GenerateBinaryCodes()
			{
			}

			public bool Refresh(uint attachmentsCode, bool hasMag)
			{
				return false;
			}

			private bool ApplyStatus(bool status)
			{
				return false;
			}
		}

		[Serializable]
		private class FlagableElement
		{
			[SerializeField]
			private Transform _targetTransform;

			[SerializeField]
			private Offset _falsePosition;

			[SerializeField]
			private Offset _truePosition;

			[SerializeField]
			private FirearmStatusFlags[] _compatibleFlags;

			[SerializeField]
			private bool _invertFlags;

			[SerializeField]
			private bool _checkAmmo;

			[SerializeField]
			private bool _needsAmmo;

			private int _prevValue;

			public bool Refresh(FirearmStatusFlags flags, bool hasAmmo)
			{
				return false;
			}
		}

		[Serializable]
		private class AmmoCounterElement
		{
			[SerializeField]
			private Text _targetText;

			[SerializeField]
			private string _format;

			private static readonly string[] UncockedPerFormatLength;

			public void Refresh(bool cocked, byte ammo)
			{
			}
		}

		private FirearmStatus _prevStatus;

		private bool _alreadySetupOnce;

		[SerializeField]
		private bool _enableColliders;

		[SerializeField]
		private Collider[] _colliders;

		[SerializeField]
		private AttachmentElement[] _attachments;

		[SerializeField]
		private RiggedAttachmentElement[] _riggedElements;

		[SerializeField]
		private MagazineElement[] _magazineElements;

		[SerializeField]
		private FlagableElement[] _flagableElements;

		[SerializeField]
		private AmmoCounterElement[] _ammoCounterElements;

		[SerializeField]
		private ParticleSystem[] _shootingEffects;

		[SerializeField]
		private GameObject[] _bullets;

		public ItemIdentifier Identifier { get; private set; }

		private void Awake()
		{
		}

		public void PlayParticleEffects()
		{
		}

		public bool ApplyStatus(FirearmStatus status, ItemIdentifier id)
		{
			return false;
		}

		private bool RefreshFlagables(FirearmStatusFlags flags, bool hasAmmo)
		{
			return false;
		}

		private bool RefreshMags(uint att, bool hasMag)
		{
			return false;
		}

		private void RefreshAttachments(uint code, ItemType fiream)
		{
		}
	}
}
