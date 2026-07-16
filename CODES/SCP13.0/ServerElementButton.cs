using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ServerElementButton : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003C_Show_003Ed__24 : IEnumerator<float>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private float _003C_003E2__current;

		public ServerElementButton _003C_003E4__this;

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
		public _003C_Show_003Ed__24(int _003C_003E1__state)
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

	public static string HighlightedIp;

	public GameObject[] icons;

	public Image FavorImage;

	public Sprite[] FavorSprites;

	public string ServerName;

	public string IpAddress;

	public int Players;

	public uint ServerID;

	public int MaxPlayers;

	private NewMainMenu newMainMenu;

	private NewServerBrowser newServerBrowser;

	private RectTransform serverInfo;

	private ReportServer reportServer;

	private TextMeshProUGUI infoText;

	private Image bgColor;

	private void Awake()
	{
	}

	private void OnEnable()
	{
	}

	public void SetValues()
	{
	}

	public void SwitchFavorIcon(bool favored)
	{
	}

	public void PlayButton()
	{
	}

	public void ShowInfo()
	{
	}

	private void LateUpdate()
	{
	}

	public void MarkAsFavorite()
	{
	}

	public void ReportForm()
	{
	}

	[IteratorStateMachine(typeof(_003C_Show_003Ed__24))]
	private IEnumerator<float> _Show(string id)
	{
		return null;
	}
}
