public static class BreakableWindowStatusSerializer
{
	public unsafe static void WriteBreakableWindowStatus(this global::Mirror.NetworkWriter writer, BreakableWindow.BreakableWindowStatus value)
	{
		writer.EnsureLength(sizeof(BreakableWindow.BreakableWindowStatus));
		fixed (byte* ptr = &writer.buffer[writer.Position])
		{
			*(BreakableWindow.BreakableWindowStatus*)ptr = value;
		}
		writer.Position += sizeof(BreakableWindow.BreakableWindowStatus);
	}

	public unsafe static BreakableWindow.BreakableWindowStatus ReadBreakableWindowStatus(this global::Mirror.NetworkReader reader)
	{
		if (reader.Position + sizeof(BreakableWindow.BreakableWindowStatus) > reader.buffer.Count)
		{
			throw new global::System.IO.EndOfStreamException("ReadByte out of range:" + reader);
		}
		BreakableWindow.BreakableWindowStatus result;
		fixed (byte* ptr = &reader.buffer.Array[reader.buffer.Offset + reader.Position])
		{
			result = *(BreakableWindow.BreakableWindowStatus*)ptr;
		}
		reader.Position += sizeof(BreakableWindow.BreakableWindowStatus);
		return result;
	}
}
