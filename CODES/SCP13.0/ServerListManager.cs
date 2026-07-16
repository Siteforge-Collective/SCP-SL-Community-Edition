using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ServerListManager : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CDownloadList_003Ed__13 : IEnumerator<float>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private float _003C_003E2__current;

		public ServerListManager _003C_003E4__this;

		private byte _003Ci_003E5__2;

		private UnityWebRequest _003Cwww_003E5__3;

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
		public _003CDownloadList_003Ed__13(int _003C_003E1__state)
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

	[CompilerGenerated]
	private sealed class _003CDisplayList_003Ed__14 : IEnumerator<float>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private float _003C_003E2__current;

		public ServerListManager _003C_003E4__this;

		private int _003CdownloadCode_003E5__2;

		private bool _003CfilterServers_003E5__3;

		private bool _003Cany_003E5__4;

		private ServerListItem[] _003C_003E7__wrap4;

		private int _003C_003E7__wrap5;

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
		public _003CDisplayList_003Ed__14(int _003C_003E1__state)
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
		}
	}

	public RectTransform contentParent;

	public RectTransform element;

	public Text loadingText;

	private List<GameObject> spawns;

	private ServerListItem[] _servers;

	private int _downloading;

	private static string _filter;

	private static int _filterTolerance;

	private void Awake()
	{
	}

	private GameObject AddRecord()
	{
		return null;
	}

	private void OnEnable()
	{
	}

	public void ApplyNameFilter(string nameFilter)
	{
	}

	public void Refresh()
	{
	}

	[IteratorStateMachine(typeof(_003CDownloadList_003Ed__13))]
	private IEnumerator<float> DownloadList()
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CDisplayList_003Ed__14))]
	private IEnumerator<float> DisplayList()
	{
		return null;
	}
}
