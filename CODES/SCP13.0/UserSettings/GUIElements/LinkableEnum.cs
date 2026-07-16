using System;
using UnityEngine;

namespace UserSettings.GUIElements
{
    [Serializable]
    public struct LinkableEnum
    {
        [SerializeField] private ushort _typeHash;
        [SerializeField] private ushort _value;

        public ushort TypeHash => _typeHash;
        public ushort Value => _value;

        private LinkableEnum(ushort typeHash, ushort value)
        {
            _typeHash = typeHash;
            _value = value;
        }

        public static LinkableEnum FromEnum<TEnum>(TEnum enumValue) where TEnum : struct, Enum
        {
            ushort typeHash = SettingsKeyGenerator.GetStableTypeHash(typeof(TEnum));
            ushort numValue = (ushort)Convert.ToInt32(enumValue);

            return new LinkableEnum(typeHash, numValue);
        }

        public static LinkableEnum ForType<TEnum>() where TEnum : struct, Enum
        {
            ushort typeHash = SettingsKeyGenerator.GetStableTypeHash(typeof(TEnum));
            return new LinkableEnum(typeHash, 0);
        }

        public TEnum GetValue<TEnum>() where TEnum : struct, Enum
        {
            return (TEnum)Enum.ToObject(typeof(TEnum), _value);
        }

        public static implicit operator int(LinkableEnum linkable) => linkable._value;

        public override bool Equals(object obj)
        {
            return obj is LinkableEnum other && _typeHash == other._typeHash && _value == other._value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_typeHash, _value);
        }

        public override string ToString()
        {
            return $"{{TypeHash: {_typeHash}, Value: {_value}}}";
        }
    }
}