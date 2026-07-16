using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using ToggleableMenus;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameMenu : SimpleToggleableMenu
{
	[CompilerGenerated]
	private sealed class _003C_ShowServerInfo_003Ed__10 : IEnumerator<float>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private float _003C_003E2__current;

		public GameMenu _003C_003E4__this;

		public string id;

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
		public _003C_ShowServerInfo_003Ed__10(int _003C_003E1__state)
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

	public static GameMenu singleton;

	public GameObject[] minors;

	public Graphic[] colorableElements;

	public TextMeshProUGUI infoText;

	public GameObject hideHUDText;

	private bool _pastebinDisplayed;

	public override bool LockMovement => false;

	protected override void Awake()
	{
	}

	private void Update()
	{
	}

	[IteratorStateMachine(typeof(_003C_ShowServerInfo_003Ed__10))]
	private IEnumerator<float> _ShowServerInfo(string id)
	{
		return null;
	}

	protected override void OnToggled()
	{
	}

	public void SelectMinor(int id)
	{
	}

	public void Disconnect()
	{
	}

	public void Exit()
	{
	}
}
