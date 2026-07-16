using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class CrashDetector : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003C_IShow_003Ed__6 : IEnumerator<float>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private float _003C_003E2__current;

		public CrashDetector _003C_003E4__this;

		private string _003Cok_003E5__2;

		private int _003Ci_003E5__3;

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
		public _003C_IShow_003Ed__6(int _003C_003E1__state)
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

	public static CrashDetector singleton;

	[SerializeField]
	private GameObject image;

	[SerializeField]
	private Button button;

	[SerializeField]
	private Text text;

	private void Awake()
	{
	}

	public bool Show()
	{
		return false;
	}

	[IteratorStateMachine(typeof(_003C_IShow_003Ed__6))]
	private IEnumerator<float> _IShow()
	{
		return null;
	}
}
