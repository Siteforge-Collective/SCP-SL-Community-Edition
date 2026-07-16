using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class DebugLogReader : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CScrollToDown_003Ed__9 : IEnumerator<float>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private float _003C_003E2__current;

		public ScrollRect scrollRect;

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
		public _003CScrollToDown_003Ed__9(int _003C_003E1__state)
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

	public GameObject Parent;

	public Text Prefab;

	private static readonly List<Text> Lines;

	private static readonly List<string> Linesstring;

	public ScrollRect Scroll;

	private static int _lineLimit;

	public static bool SuccesfullyInitialized()
	{
		return false;
	}

	public void OnEnable()
	{
	}

	public void OnDisable()
	{
	}

	[IteratorStateMachine(typeof(_003CScrollToDown_003Ed__9))]
	private static IEnumerator<float> ScrollToDown(ScrollRect scrollRect)
	{
		return null;
	}
}
