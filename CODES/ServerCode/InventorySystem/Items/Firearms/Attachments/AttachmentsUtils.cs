namespace InventorySystem.Items.Firearms.Attachments
{
	public static class AttachmentsUtils
	{
		private static int _paramNumberCache;

		private static global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterDefinition[] _cachedDefitionons;

		private static bool[] _readyMixingModes;

		private static bool _mixingModesCacheSet;

		public static int TotalNumberOfParams
		{
			get
			{
				if (_paramNumberCache <= 0)
				{
					_paramNumberCache = global::System.Enum.GetValues(typeof(global::InventorySystem.Items.Firearms.Attachments.AttachmentParam)).Length;
				}
				return _paramNumberCache;
			}
		}

		public static uint GetCurrentAttachmentsCode(this global::InventorySystem.Items.Firearms.Firearm firearm)
		{
			uint num = 1u;
			uint num2 = 0u;
			for (int i = 0; i < firearm.Attachments.Length; i++)
			{
				if (firearm.Attachments[i].IsEnabled)
				{
					num2 += num;
				}
				num *= 2;
			}
			return num2;
		}

		public static uint GetRandomAttachmentsCode(ItemType firearmType)
		{
			if (!global::InventorySystem.InventoryItemLoader.AvailableItems.TryGetValue(firearmType, out var value) || !(value is global::InventorySystem.Items.Firearms.Firearm firearm))
			{
				return 0u;
			}
			int num = firearm.Attachments.Length;
			bool[] array = new bool[num];
			int num2 = 0;
			while (num2 < num)
			{
				global::InventorySystem.Items.Firearms.Attachments.AttachmentSlot slot = firearm.Attachments[num2].Slot;
				for (int i = num2; i < num; i++)
				{
					if (i + 1 >= num || firearm.Attachments[i + 1].Slot != slot)
					{
						array[global::UnityEngine.Random.Range(num2, i + 1)] = true;
						num2 = i + 1;
						break;
					}
				}
			}
			uint num3 = 1u;
			uint num4 = 0u;
			for (int j = 0; j < num; j++)
			{
				if (array[j])
				{
					num4 += num3;
				}
				num3 *= 2;
			}
			return num4;
		}

		public static float AttachmentsValue(this global::InventorySystem.Items.Firearms.Firearm firearm, global::InventorySystem.Items.Firearms.Attachments.AttachmentParam param)
		{
			global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterDefinition definitionOfParam = GetDefinitionOfParam((int)param);
			float num = definitionOfParam.DefaultValue;
			int num2 = firearm.Attachments.Length;
			for (int i = 0; i < num2; i++)
			{
				global::InventorySystem.Items.Firearms.Attachments.Components.Attachment attachment = firearm.Attachments[i];
				if (attachment.IsEnabled && attachment.TryGetValue((int)param, out var val))
				{
					num = MixValue(num, val, definitionOfParam.MixingMode);
				}
			}
			if (firearm.SimulatedInstanceMode)
			{
				return num;
			}
			for (int j = 0; j < firearm.Owner.playerEffectsController.EffectsLength; j++)
			{
				if (firearm.Owner.playerEffectsController.AllEffects[j] is global::CustomPlayerEffects.IWeaponModifierPlayerEffect weaponModifierPlayerEffect && weaponModifierPlayerEffect.ParamsActive && weaponModifierPlayerEffect.TryGetWeaponParam(param, out var val2))
				{
					num = MixValue(num, val2, definitionOfParam.MixingMode);
				}
			}
			return ClampValue(num, definitionOfParam);
		}

		public static float ProcessValue(this global::InventorySystem.Items.Firearms.Firearm firearm, float value, global::InventorySystem.Items.Firearms.Attachments.AttachmentParam param)
		{
			float num = firearm.AttachmentsValue(param);
			switch (GetDefinitionOfParam((int)param).MixingMode)
			{
			case global::InventorySystem.Items.Firearms.Attachments.ParameterMixingMode.Additive:
				return value + num;
			case global::InventorySystem.Items.Firearms.Attachments.ParameterMixingMode.Percent:
				return value * num;
			case global::InventorySystem.Items.Firearms.Attachments.ParameterMixingMode.Override:
				return num;
			default:
				return value;
			}
		}

		public static bool HasAdvantageFlag(this global::InventorySystem.Items.Firearms.Firearm firearm, global::InventorySystem.Items.Firearms.Attachments.AttachmentDescriptiveAdvantages flag)
		{
			int num = firearm.Attachments.Length;
			for (int i = 0; i < num; i++)
			{
				global::InventorySystem.Items.Firearms.Attachments.Components.Attachment attachment = firearm.Attachments[i];
				if (attachment.IsEnabled && attachment.DescriptivePros.HasFlagFast(flag))
				{
					return true;
				}
			}
			return false;
		}

		public static bool HasDownsideFlag(this global::InventorySystem.Items.Firearms.Firearm firearm, global::InventorySystem.Items.Firearms.Attachments.AttachmentDescriptiveDownsides flag)
		{
			int num = firearm.Attachments.Length;
			for (int i = 0; i < num; i++)
			{
				global::InventorySystem.Items.Firearms.Attachments.Components.Attachment attachment = firearm.Attachments[i];
				if (attachment.IsEnabled && attachment.DescriptiveCons.HasFlagFast(flag))
				{
					return true;
				}
			}
			return false;
		}

		private static float MixValue(float originalValue, float paraValue, global::InventorySystem.Items.Firearms.Attachments.ParameterMixingMode mixMode)
		{
			switch (mixMode)
			{
			case global::InventorySystem.Items.Firearms.Attachments.ParameterMixingMode.Additive:
				originalValue += paraValue;
				break;
			case global::InventorySystem.Items.Firearms.Attachments.ParameterMixingMode.Percent:
				originalValue += paraValue - 1f;
				break;
			case global::InventorySystem.Items.Firearms.Attachments.ParameterMixingMode.Override:
				originalValue = paraValue;
				break;
			}
			return originalValue;
		}

		private static float ClampValue(float f, global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterDefinition definition)
		{
			return global::UnityEngine.Mathf.Clamp(f, definition.MinValue, definition.MaxValue);
		}

		private static global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterDefinition GetDefinitionOfParam(int paramId)
		{
			if (!_mixingModesCacheSet)
			{
				_cachedDefitionons = new global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterDefinition[TotalNumberOfParams];
				_readyMixingModes = new bool[TotalNumberOfParams];
				_mixingModesCacheSet = true;
			}
			if (_readyMixingModes[paramId])
			{
				return _cachedDefitionons[paramId];
			}
			if (!global::InventorySystem.Items.Firearms.Attachments.AttachmentParameterDefinition.Definitions.TryGetValue((global::InventorySystem.Items.Firearms.Attachments.AttachmentParam)paramId, out var value))
			{
				throw new global::System.Exception($"Parameter {(global::InventorySystem.Items.Firearms.Attachments.AttachmentParam)paramId} is not defined!");
			}
			_readyMixingModes[paramId] = true;
			_cachedDefitionons[paramId] = value;
			return value;
		}

		public static void ApplyAttachmentsCode(this global::InventorySystem.Items.Firearms.Firearm firearm, uint code, bool reValidate)
		{
			if (reValidate)
			{
				code = firearm.ValidateAttachmentsCode(code);
			}
			uint num = 1u;
			for (int i = 0; i < firearm.Attachments.Length; i++)
			{
				firearm.Attachments[i].IsEnabled = (code & num) == num;
				num *= 2;
			}
		}

		public static uint ValidateAttachmentsCode(this global::InventorySystem.Items.Firearms.Firearm firearm, uint code)
		{
			uint num = 0u;
			uint num2 = 1u;
			global::System.Collections.Generic.HashSet<global::InventorySystem.Items.Firearms.Attachments.AttachmentSlot> hashSet = global::NorthwoodLib.Pools.HashSetPool<global::InventorySystem.Items.Firearms.Attachments.AttachmentSlot>.Shared.Rent();
			global::InventorySystem.Items.Firearms.Attachments.Components.Attachment[] attachments = firearm.Attachments;
			foreach (global::InventorySystem.Items.Firearms.Attachments.Components.Attachment attachment in attachments)
			{
				hashSet.Add(attachment.Slot);
			}
			for (int j = 0; j < firearm.Attachments.Length; j++)
			{
				if ((code & num2) == num2 && hashSet.Remove(firearm.Attachments[j].Slot))
				{
					num += num2;
				}
				num2 *= 2;
			}
			foreach (global::InventorySystem.Items.Firearms.Attachments.AttachmentSlot item in hashSet)
			{
				for (int k = 0; k < firearm.Attachments.Length; k++)
				{
					if (item == firearm.Attachments[k].Slot)
					{
						uint num3 = 1u;
						for (int l = 0; l < k; l++)
						{
							num3 *= 2;
						}
						num += num3;
						break;
					}
				}
			}
			global::NorthwoodLib.Pools.HashSetPool<global::InventorySystem.Items.Firearms.Attachments.AttachmentSlot>.Shared.Return(hashSet);
			return num;
		}

		public static void GetDefaultLengthAndWeight(this global::InventorySystem.Items.Firearms.Firearm fa, out float length, out float weight)
		{
			global::System.Collections.Generic.HashSet<global::InventorySystem.Items.Firearms.Attachments.AttachmentSlot> hashSet = global::NorthwoodLib.Pools.HashSetPool<global::InventorySystem.Items.Firearms.Attachments.AttachmentSlot>.Shared.Rent();
			length = fa.BaseLength;
			weight = fa.BaseWeight;
			for (int i = 0; i < fa.Attachments.Length; i++)
			{
				if (hashSet.Add(fa.Attachments[i].Slot))
				{
					length += fa.Attachments[i].Length;
					weight += fa.Attachments[i].Weight;
				}
			}
			global::NorthwoodLib.Pools.HashSetPool<global::InventorySystem.Items.Firearms.Attachments.AttachmentSlot>.Shared.Return(hashSet);
		}

		public static global::InventorySystem.Items.Firearms.FirearmStatusFlags OverrideFlashlightFlags(this global::InventorySystem.Items.Firearms.Firearm fa, bool overrideFlashlight)
		{
			global::InventorySystem.Items.Firearms.FirearmStatusFlags flags = fa.Status.Flags;
			if (overrideFlashlight)
			{
				return flags | global::InventorySystem.Items.Firearms.FirearmStatusFlags.FlashlightEnabled;
			}
			return flags & ~global::InventorySystem.Items.Firearms.FirearmStatusFlags.FlashlightEnabled;
		}

		public static bool HasFlagFast(this global::InventorySystem.Items.Firearms.Attachments.AttachmentDescriptiveAdvantages flags, global::InventorySystem.Items.Firearms.Attachments.AttachmentDescriptiveAdvantages flag)
		{
			return (flags & flag) == flag;
		}

		public static bool HasFlagFast(this global::InventorySystem.Items.Firearms.Attachments.AttachmentDescriptiveDownsides flags, global::InventorySystem.Items.Firearms.Attachments.AttachmentDescriptiveDownsides flag)
		{
			return (flags & flag) == flag;
		}
	}
}
