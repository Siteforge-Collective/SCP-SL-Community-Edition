using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public static class CustomMusicLoader
{
	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CProcessLocalFile_003Ed__2 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncVoidMethodBuilder _003C_003Et__builder;

		public string file;

		public AudioType type;

		private UnityWebRequest _003Cwww_003E5__2;

		private YieldAwaitable.YieldAwaiter _003C_003Eu__1;

		private void MoveNext()
		{
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	private static string _musicPath;

	public static void LoadCustomAudio()
	{
	}

	[AsyncStateMachine(typeof(_003CProcessLocalFile_003Ed__2))]
	private static void ProcessLocalFile(string file, AudioType type)
	{
	}
}
