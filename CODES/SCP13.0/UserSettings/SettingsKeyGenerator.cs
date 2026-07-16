using System;
using System.Collections.Generic;
using System.Text;

namespace UserSettings
{
    public static class SettingsKeyGenerator
    {
        private static readonly Dictionary<Type, ushort> TypeToHash = new Dictionary<Type, ushort>();
        private static readonly Dictionary<ushort, Type> HashToType = new Dictionary<ushort, Type>();

        private static readonly char[] HexNonAlloc32 = new char[4];
        private static readonly char[] IntToHexArr =
        {
            '0', '1', '2', '3', '4', '5', '6', '7',
            '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'
        };

        private static readonly StringBuilder KeyBuilder = new StringBuilder("UserSettings_FFFF_FFFF");

        private const int HexLength = 4;
        private const int SbHashStartIndex = 13;  
        private const int SbValueStartIndex = 18;

        public static string EnumToKey<T>(T val) where T : Enum
        {
            Type type = typeof(T);
            ushort typeHash = GetStableTypeHash(type);
            ushort enumValue = (ushort)Convert.ToInt32(val);
            return TypeValueToKey(typeHash, enumValue);
        }

        public static string TypeValueToKey(ushort typeHash, ushort value)
        {
            UshortToHex(typeHash);
            KeyBuilder[SbHashStartIndex + 0] = HexNonAlloc32[0];
            KeyBuilder[SbHashStartIndex + 1] = HexNonAlloc32[1];
            KeyBuilder[SbHashStartIndex + 2] = HexNonAlloc32[2];
            KeyBuilder[SbHashStartIndex + 3] = HexNonAlloc32[3];

            UshortToHex(value);
            KeyBuilder[SbValueStartIndex + 0] = HexNonAlloc32[0];
            KeyBuilder[SbValueStartIndex + 1] = HexNonAlloc32[1];
            KeyBuilder[SbValueStartIndex + 2] = HexNonAlloc32[2];
            KeyBuilder[SbValueStartIndex + 3] = HexNonAlloc32[3];

            return KeyBuilder.ToString();
        }

        public static void KeyToTypeValue(string key, out ushort typeHash, out ushort value)
        {
            if (key == null || key.Length < 22 || !key.StartsWith("UserSettings_"))
            {
                typeHash = 0;
                value = 0;
                return;
            }

            typeHash = HexToUshort(key, 13);
            value = HexToUshort(key, 18);
        }

        public static ushort GetStableTypeHash(Type type, bool preventCaching = false)
        {
            if (!preventCaching && TypeToHash.TryGetValue(type, out ushort cached))
            {
                return cached;
            }

            uint hash = 0;
            string name = type.FullName ?? type.Name;
            foreach (char c in name)
            {
                hash = (hash * 31) + c;
            }

            ushort shortHash = (ushort)(hash ^ (hash >> 16));

            if (!preventCaching)
            {
                TypeToHash[type] = shortHash;
                HashToType[shortHash] = type;
            }

            return shortHash;
        }

        private static void UshortToHex(ushort val)
        {
            HexNonAlloc32[0] = IntToHexArr[(val >> 12) & 0xF];
            HexNonAlloc32[1] = IntToHexArr[(val >> 8) & 0xF];
            HexNonAlloc32[2] = IntToHexArr[(val >> 4) & 0xF];
            HexNonAlloc32[3] = IntToHexArr[val & 0xF];
        }

        private static ushort HexToUshort(string str, int startIndex)
        {
            ushort result = 0;
            for (int i = 0; i < 4; i++)
            {
                char c = str[startIndex + i];
                ushort digit = c switch
                {
                    >= '0' and <= '9' => (ushort)(c - '0'),
                    >= 'A' and <= 'F' => (ushort)(c - 'A' + 10),
                    >= 'a' and <= 'f' => (ushort)(c - 'a' + 10),
                    _ => (ushort)0
                };
                result = (ushort)((result << 4) | digit);
            }
            return result;
        }
    }
}