using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Mirror;
using UnityEngine;

public static class Escape
{
	private enum EscapeScenarioType
	{
		None = 0,
		ClassD = 1,
		CuffedClassD = 2,
		Scientist = 3,
		CuffedScientist = 4
	}

	private readonly struct EscapeScenarioText
	{
		private readonly int _id;

		private readonly string _def;

		public string Text => null;

		public EscapeScenarioText(int translationKey, string defaultText)
		{
			_id = 0;
			_def = null;
		}
	}

	public struct EscapeMessage : NetworkMessage
	{
		public byte ScenarioId;

		public ushort EscapeTime;
	}

	[CompilerGenerated]
	private sealed class _003CPlayEscapeAnim_003Ed__14 : IEnumerator<float>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private float _003C_003E2__current;

		public string txt;

		public int seconds;

		private CanvasRenderer _003Ccr_003E5__2;

		private int _003Ct_003E5__3;

		private byte _003Ci_003E5__4;

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
		public _003CPlayEscapeAnim_003Ed__14(int _003C_003E1__state)
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

	private static readonly Dictionary<EscapeScenarioType, EscapeScenarioText> Scenarios;

	private static readonly Vector3 WorldPos;

	private const float RadiusSqr = 156.5f;

	private const float MinAliveTime = 10f;

	private const string TranslationKey = "Facility";

	private const float InsurgencyEscapeReward = 4f;

	private const float FoundationEscapeReward = 3f;

	public static event Action<ReferenceHub> OnServerPlayerEscape
	{
		[CompilerGenerated]
		add
		{
		}
		[CompilerGenerated]
		remove
		{
		}
	}

	[RuntimeInitializeOnLoadMethod]
	private static void Init()
	{
	}

	private static void ServerHandlePlayer(ReferenceHub hub)
	{
	}

	private static EscapeScenarioType ServerGetScenario(ReferenceHub hub)
	{
		return default(EscapeScenarioType);
	}

	private static void ClientReceiveMessage(EscapeMessage msg)
	{
	}

	[IteratorStateMachine(typeof(_003CPlayEscapeAnim_003Ed__14))]
	private static IEnumerator<float> PlayEscapeAnim(string txt, int seconds)
	{
		return null;
	}
}
