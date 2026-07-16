using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NewsLoader : MonoBehaviour
{
	private class Announcement
	{
		public string Title;

		public string Content;

		public string Date;

		public string Link;

		public NewsElement Thumbnail;

		public Announcement(string title, string content, string date, string link, NewsElement thumbnail)
		{
		}
	}

	[CompilerGenerated]
	private sealed class _003C_Request_003Ed__7 : IEnumerator<float>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private float _003C_003E2__current;

		public NewsLoader _003C_003E4__this;

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
		public _003C_Request_003Ed__7(int _003C_003E1__state)
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

	public TextMeshProUGUI ArticleText;

	public RectTransform ContentParent;

	public RectTransform Element;

	public Button OpenNewsUrlButton;

	private List<Announcement> _announcements;

	private string _curAnncUrl;

	private void Start()
	{
	}

	[IteratorStateMachine(typeof(_003C_Request_003Ed__7))]
	private IEnumerator<float> _Request()
	{
		return null;
	}

	private void DetectCustomLinks(ref string content)
	{
	}

	private void DetectImages(ref string content)
	{
	}

	private void DetectYoutubePreview(ref string content)
	{
	}

	private void TextProcessor(string text)
	{
	}

	internal void OpenAnnouncementUrl()
	{
	}

	internal void ShowAnnouncement(int id)
	{
	}
}
