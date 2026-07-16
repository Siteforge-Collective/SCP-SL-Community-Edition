using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using PlayerRoles;
using PlayerStatsSystem;
using UnityEngine;
using UnityEngine.UI;

public class YouWereKilled : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003C_Play_003Ed__8 : IEnumerator<float>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private float _003C_003E2__current;

		public YouWereKilled _003C_003E4__this;

		public string reason;

		public string attacker;

		public string attackerRole;

		private CanvasRenderer[] _003Crenderers_003E5__2;

		private float _003Ctime_003E5__3;

		private int _003Ci_003E5__4;

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
		public _003C_Play_003Ed__8(int _003C_003E1__state)
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

	public static YouWereKilled Singleton;

	[Space]
	public GameObject _root;

	public Text _info;

	private const float DeathBellVolume = 0.85f;

	[SerializeField]
	private AudioClip _deathBell;

	private void Awake()
	{
	}

	public void PlayRegular(DamageHandlerBase hitInfo)
	{
	}

	public void PlayAttacker(string nickname, RoleTypeId role)
	{
	}

	[IteratorStateMachine(typeof(_003C_Play_003Ed__8))]
	private IEnumerator<float> _Play(string reason, string attacker, string attackerRole)
	{
		return null;
	}
}
