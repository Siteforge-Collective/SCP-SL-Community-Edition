namespace InventorySystem.Items.Firearms
{
	public class FirearmWorldmodel : global::UnityEngine.MonoBehaviour
	{
		[global::System.Serializable]
		private struct AttachmentElement
		{
			[global::UnityEngine.SerializeField]
			private global::UnityEngine.GameObject[] _targetObjects;

			public void Refresh(bool state)
			{
				global::UnityEngine.GameObject[] targetObjects = _targetObjects;
				for (int i = 0; i < targetObjects.Length; i++)
				{
					targetObjects[i].SetActive(state);
				}
			}
		}

		[global::System.Serializable]
		public class RiggedAttachmentElement
		{
			public int TargetAttachmentId;

			[global::UnityEngine.SerializeField]
			private global::UnityEngine.Transform _targetBone;

			[global::UnityEngine.SerializeField]
			private Offset _disabled;

			[global::UnityEngine.SerializeField]
			private Offset _enabled;

			[global::UnityEngine.SerializeField]
			private bool _thirdpersonOnly;

			public void Refresh(bool isEnabled, bool thirdperson)
			{
				Offset offset = ((isEnabled && (thirdperson || !_thirdpersonOnly)) ? _enabled : _disabled);
				_targetBone.localPosition = offset.position;
				_targetBone.localRotation = global::UnityEngine.Quaternion.Euler(offset.rotation);
				_targetBone.localScale = offset.scale;
			}
		}

		[global::System.Serializable]
		private class MagazineElement
		{
			[global::UnityEngine.SerializeField]
			private global::UnityEngine.GameObject _targetObject;

			[global::UnityEngine.SerializeField]
			private int[] _attachmentIds;

			private uint[] _binaryCodes;

			private int _prevStatusCode;

			private uint[] BinaryCodes
			{
				get
				{
					if (_binaryCodes == null)
					{
						GenerateBinaryCodes();
					}
					return _binaryCodes;
				}
			}

			private void GenerateBinaryCodes()
			{
				_binaryCodes = new uint[_attachmentIds.Length];
				for (int i = 0; i < _attachmentIds.Length; i++)
				{
					uint num = 1u;
					for (int j = 0; j < _attachmentIds[i]; j++)
					{
						num *= 2;
					}
					_binaryCodes[i] = num;
				}
			}

			public bool Refresh(uint attachmentsCode, bool hasMag)
			{
				if (!hasMag || _attachmentIds.Length == 0)
				{
					return ApplyStatus(hasMag);
				}
				uint[] binaryCodes = BinaryCodes;
				foreach (uint num in binaryCodes)
				{
					if ((attachmentsCode & num) == num)
					{
						return ApplyStatus(status: true);
					}
				}
				return ApplyStatus(status: false);
			}

			private bool ApplyStatus(bool status)
			{
				int num = (status ? 1 : 2);
				bool result = num != _prevStatusCode;
				_targetObject.SetActive(status);
				_prevStatusCode = num;
				return result;
			}
		}

		[global::System.Serializable]
		private class FlagableElement
		{
			[global::UnityEngine.SerializeField]
			private global::UnityEngine.Transform _targetTransform;

			[global::UnityEngine.SerializeField]
			private Offset _falsePosition;

			[global::UnityEngine.SerializeField]
			private Offset _truePosition;

			[global::UnityEngine.SerializeField]
			private global::InventorySystem.Items.Firearms.FirearmStatusFlags[] _compatibleFlags;

			[global::UnityEngine.SerializeField]
			private bool _invertFlags;

			[global::UnityEngine.SerializeField]
			private bool _checkAmmo;

			[global::UnityEngine.SerializeField]
			private bool _needsAmmo;

			private int _prevValue;

			public bool Refresh(global::InventorySystem.Items.Firearms.FirearmStatusFlags flags, bool hasAmmo)
			{
				bool flag = _invertFlags;
				global::InventorySystem.Items.Firearms.FirearmStatusFlags[] compatibleFlags = _compatibleFlags;
				foreach (global::InventorySystem.Items.Firearms.FirearmStatusFlags flag2 in compatibleFlags)
				{
					if (global::InventorySystem.Items.Firearms.BasicMessages.FirearmBasicMessagesHandler.HasFlagFast(flags, flag2))
					{
						flag = !_invertFlags;
						break;
					}
				}
				if (_checkAmmo && _needsAmmo != hasAmmo)
				{
					flag = false;
				}
				global::UnityEngine.Transform targetTransform = _targetTransform;
				Offset obj = (flag ? _truePosition : _falsePosition);
				targetTransform.localPosition = obj.position;
				global::UnityEngine.Transform targetTransform2 = _targetTransform;
				Offset obj2 = (flag ? _truePosition : _falsePosition);
				targetTransform2.localRotation = global::UnityEngine.Quaternion.Euler(obj2.rotation);
				global::UnityEngine.Transform targetTransform3 = _targetTransform;
				Offset obj3 = (flag ? _truePosition : _falsePosition);
				targetTransform3.localScale = obj3.scale;
				int num = (flag ? 1 : 2);
				bool result = num != _prevValue;
				_prevValue = num;
				return result;
			}
		}

		private global::InventorySystem.Items.Firearms.FirearmStatus _prevStatus;

		private bool _alreadySetupOnce;

		[global::UnityEngine.SerializeField]
		private bool _enableColliders;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Collider[] _colliders;

		[global::UnityEngine.SerializeField]
		private global::InventorySystem.Items.Firearms.FirearmWorldmodel.AttachmentElement[] _attachments;

		[global::UnityEngine.SerializeField]
		private global::InventorySystem.Items.Firearms.FirearmWorldmodel.RiggedAttachmentElement[] _riggedElements;

		[global::UnityEngine.SerializeField]
		private global::InventorySystem.Items.Firearms.FirearmWorldmodel.MagazineElement[] _magazineElements;

		[global::UnityEngine.SerializeField]
		private global::InventorySystem.Items.Firearms.FirearmWorldmodel.FlagableElement[] _flagableElements;

		private void Awake()
		{
			global::UnityEngine.Collider[] colliders = _colliders;
			for (int i = 0; i < colliders.Length; i++)
			{
				colliders[i].enabled = _enableColliders;
			}
			global::InventorySystem.Items.Firearms.FirearmWorldmodelInitializer[] componentsInChildren = GetComponentsInChildren<global::InventorySystem.Items.Firearms.FirearmWorldmodelInitializer>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Initialize(_enableColliders);
			}
		}

		public void PlayParticleEffects()
		{
		}

		public bool ApplyStatus(global::InventorySystem.Items.Firearms.FirearmStatus status, ItemType firearmType)
		{
			bool num = !_alreadySetupOnce;
			bool result = false;
			bool flag = num || _prevStatus.Attachments != status.Attachments;
			bool flag2 = num || status.Flags != _prevStatus.Flags;
			bool flag3 = num || _prevStatus.Ammo != status.Ammo;
			bool hasMag = global::InventorySystem.Items.Firearms.BasicMessages.FirearmBasicMessagesHandler.HasFlagFast(status.Flags, global::InventorySystem.Items.Firearms.FirearmStatusFlags.MagazineInserted);
			if ((flag || flag2) && RefreshMags(status.Attachments, hasMag))
			{
				result = true;
			}
			if (flag)
			{
				RefreshAttachments(status.Attachments, firearmType);
				result = true;
			}
			if ((flag2 || flag3) && flag2 && RefreshFlagables(status.Flags, status.Ammo > 0))
			{
				result = true;
			}
			_prevStatus = status;
			_alreadySetupOnce = true;
			return result;
		}

		private bool RefreshFlagables(global::InventorySystem.Items.Firearms.FirearmStatusFlags flags, bool hasAmmo)
		{
			bool result = false;
			global::InventorySystem.Items.Firearms.FirearmWorldmodel.FlagableElement[] flagableElements = _flagableElements;
			for (int i = 0; i < flagableElements.Length; i++)
			{
				if (flagableElements[i].Refresh(flags, hasAmmo))
				{
					result = true;
				}
			}
			return result;
		}

		private bool RefreshMags(uint att, bool hasMag)
		{
			bool result = false;
			global::InventorySystem.Items.Firearms.FirearmWorldmodel.MagazineElement[] magazineElements = _magazineElements;
			for (int i = 0; i < magazineElements.Length; i++)
			{
				if (magazineElements[i].Refresh(att, hasMag))
				{
					result = true;
				}
			}
			return result;
		}

		private void RefreshAttachments(uint code, ItemType fiream)
		{
			if (!global::InventorySystem.InventoryItemLoader.AvailableItems.TryGetValue(fiream, out var value) || !(value is global::InventorySystem.Items.Firearms.Firearm firearm))
			{
				return;
			}
			uint num = 1u;
			global::System.Collections.Generic.HashSet<global::InventorySystem.Items.Firearms.Attachments.AttachmentSlot> hashSet = global::NorthwoodLib.Pools.HashSetPool<global::InventorySystem.Items.Firearms.Attachments.AttachmentSlot>.Shared.Rent();
			global::InventorySystem.Items.Firearms.Attachments.Components.Attachment[] attachments = firearm.Attachments;
			foreach (global::InventorySystem.Items.Firearms.Attachments.Components.Attachment attachment in attachments)
			{
				hashSet.Add(attachment.Slot);
			}
			global::InventorySystem.Items.Firearms.FirearmWorldmodel.AttachmentElement[] attachments2 = _attachments;
			foreach (global::InventorySystem.Items.Firearms.FirearmWorldmodel.AttachmentElement attachmentElement in attachments2)
			{
				attachmentElement.Refresh(state: false);
			}
			for (int j = 0; j < firearm.Attachments.Length; j++)
			{
				bool flag = (code & num) == num && hashSet.Remove(firearm.Attachments[j].Slot);
				if (flag)
				{
					_attachments[j].Refresh(state: true);
				}
				global::InventorySystem.Items.Firearms.FirearmWorldmodel.RiggedAttachmentElement[] riggedElements = _riggedElements;
				foreach (global::InventorySystem.Items.Firearms.FirearmWorldmodel.RiggedAttachmentElement riggedAttachmentElement in riggedElements)
				{
					if (riggedAttachmentElement.TargetAttachmentId == j)
					{
						riggedAttachmentElement.Refresh(flag, !_enableColliders);
					}
				}
				num *= 2;
			}
			foreach (global::InventorySystem.Items.Firearms.Attachments.AttachmentSlot item in hashSet)
			{
				for (int k = 0; k < firearm.Attachments.Length; k++)
				{
					if (item == firearm.Attachments[k].Slot)
					{
						_attachments[k].Refresh(state: true);
						break;
					}
				}
			}
			global::NorthwoodLib.Pools.HashSetPool<global::InventorySystem.Items.Firearms.Attachments.AttachmentSlot>.Shared.Return(hashSet);
		}
	}
}
