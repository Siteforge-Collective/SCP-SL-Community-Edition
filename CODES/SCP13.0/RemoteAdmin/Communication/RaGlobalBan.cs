using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RemoteAdmin.Interfaces;
using UnityEngine.Networking;

namespace RemoteAdmin.Communication
{
	public class RaGlobalBan : IServerCommunication, IClientCommunication
	{
		[CompilerGenerated]
		private sealed class _003CIssueGlobalBan_003Ed__9 : IEnumerator<float>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private float _003C_003E2__current;

			private UnityWebRequest _003Cwww_003E5__2;

			float IEnumerator<float>.Current
			{
				[DebuggerHidden]
				get
				{
					return 0f;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return null;
				}
			}

			[DebuggerHidden]
			public _003CIssueGlobalBan_003Ed__9(int _003C_003E1__state)
			{
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				return false;
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			private void _003C_003Em__Finally1()
			{
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
			}
		}

		private static string _toBan;

		private static string _toBanUserId;

		private static string _toBanAuth;

		public int DataId => 0;

		public void ReceiveData(CommandSender sender, string data)
		{
		}

		public static void Request(string key, int keytype)
		{
		}

		public void ReceiveData(string data, bool secure)
		{
		}

		internal static void ConfirmGlobalBanning()
		{
		}

		[IteratorStateMachine(typeof(_003CIssueGlobalBan_003Ed__9))]
		private static IEnumerator<float> IssueGlobalBan()
		{
			return null;
		}
	}
}
