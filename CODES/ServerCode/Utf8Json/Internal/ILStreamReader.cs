namespace Utf8Json.Internal
{
	internal class ILStreamReader : global::System.IO.BinaryReader
	{
		private static readonly global::System.Reflection.Emit.OpCode[] oneByteOpCodes;

		private static readonly global::System.Reflection.Emit.OpCode[] twoByteOpCodes;

		private int endPosition;

		public int CurrentPosition => (int)BaseStream.Position;

		public bool EndOfStream => (int)BaseStream.Position >= endPosition;

		static ILStreamReader()
		{
			oneByteOpCodes = new global::System.Reflection.Emit.OpCode[256];
			twoByteOpCodes = new global::System.Reflection.Emit.OpCode[256];
			global::System.Reflection.FieldInfo[] fields = typeof(global::System.Reflection.Emit.OpCodes).GetFields(global::System.Reflection.BindingFlags.Static | global::System.Reflection.BindingFlags.Public);
			for (int i = 0; i < fields.Length; i++)
			{
				global::System.Reflection.Emit.OpCode opCode = (global::System.Reflection.Emit.OpCode)fields[i].GetValue(null);
				ushort num = (ushort)opCode.Value;
				if (num < 256)
				{
					oneByteOpCodes[num] = opCode;
				}
				else if ((num & 0xFF00) == 65024)
				{
					twoByteOpCodes[num & 0xFF] = opCode;
				}
			}
		}

		public ILStreamReader(byte[] ilByteArray)
			: base(new global::System.IO.MemoryStream(ilByteArray))
		{
			endPosition = ilByteArray.Length;
		}

		public global::System.Reflection.Emit.OpCode ReadOpCode()
		{
			byte b = ReadByte();
			if (b != 254)
			{
				return oneByteOpCodes[b];
			}
			b = ReadByte();
			return twoByteOpCodes[b];
		}

		public int ReadMetadataToken()
		{
			return ReadInt32();
		}
	}
}
