using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScpInterfaces : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CHideDecayedPopup_003Ed__16 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ScpInterfaces _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
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
		public _003CHideDecayedPopup_003Ed__16(int _003C_003E1__state)
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

	public static ScpInterfaces singleton;

	public TextMeshProUGUI Scp049_decayedPopup;

	public Image Scp049_loading;

	public Image Scp049_cooldown;

	public TextMeshProUGUI remainingTargets;

	public Image Scp173EyeIndicator;

	public Image Scp173TantrumCooldown;

	public Image Scp173BreakneckCooldown;

	public TextMeshProUGUI Scp173BlinkTimer;

	public GameObject Scp106_eq;

	public GameObject Scp049_eq;

	public GameObject Scp096_eq;

	public GameObject Scp173InterfaceObj;

	public static int remTargs;

	private void Awake()
	{
	}

	public void DecayedPopup()
	{
	}

	[IteratorStateMachine(typeof(_003CHideDecayedPopup_003Ed__16))]
	private IEnumerator HideDecayedPopup()
	{
		return null;
	}
}
