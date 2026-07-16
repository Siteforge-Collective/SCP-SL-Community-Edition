namespace Windows
{
	public class HeadlessConsole
	{
		private global::System.IO.TextWriter oldOutput;

		private const int STD_OUTPUT_HANDLE = -11;

		public void Initialize()
		{
			if (!AttachConsole(uint.MaxValue))
			{
				AllocConsole();
			}
			oldOutput = global::System.Console.Out;
			try
			{
				global::System.IO.FileStream stream = new global::System.IO.FileStream(GetStdHandle(-11), global::System.IO.FileAccess.Write);
				global::System.Text.Encoding aSCII = global::System.Text.Encoding.ASCII;
				global::System.Console.SetOut(new global::System.IO.StreamWriter(stream, aSCII)
				{
					AutoFlush = true
				});
			}
			catch (global::System.Exception ex)
			{
				global::UnityEngine.Debug.Log("Couldn't redirect output: " + ex.Message);
			}
		}

		public void Shutdown()
		{
			global::System.Console.SetOut(oldOutput);
			FreeConsole();
		}

		public void SetTitle(string strName)
		{
			SetConsoleTitle(strName);
		}

		[global::System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool AttachConsole(uint dwProcessId);

		[global::System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool AllocConsole();

		[global::System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool FreeConsole();

		[global::System.Runtime.InteropServices.DllImport("kernel32.dll", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.StdCall, CharSet = global::System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
		private static extern global::System.IntPtr GetStdHandle(int nStdHandle);

		[global::System.Runtime.InteropServices.DllImport("kernel32.dll")]
		private static extern bool SetConsoleTitle(string lpConsoleTitle);
	}
}
