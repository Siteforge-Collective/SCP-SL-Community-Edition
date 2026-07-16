namespace ServerOutput
{
	[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Size = 1)]
	public struct RoundRestartedEntry : global::ServerOutput.IOutputEntry
	{
		public string GetString()
		{
			return ((byte)16).ToString();
		}

		public int GetBytesLength()
		{
			return 1;
		}

		public void GetBytes(ref byte[] buffer, out int length)
		{
			length = 1;
			buffer[0] = 16;
		}
	}
}
