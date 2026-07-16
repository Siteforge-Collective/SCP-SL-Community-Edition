using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Subtitles
{
	public class SubtitleController : MonoBehaviour
	{
		public static SubtitleController Singleton;

		[SerializeField]
		private Subtitle[] subtitles;

		[SerializeField]
		private SubtitleCategory[] subtitleCategories;

		private Dictionary<SubtitleType, Subtitle> Subtitles;

		public void SetupSubtitle(SubtitlePart[] subtitles)
		{
		}

		public void ClearSubtitles(CassieAnnouncementType type)
		{
		}

		public void AddSubtitle(string message, float duration, Subtitle masterSubtitle)
		{
		}

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		[RuntimeInitializeOnLoadMethod]
		private static void SetDefaultPrefs()
		{
		}

		private void UpdateEnabled(bool b)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private string ReplaceInfo(string message, string[] optionalData, bool convertNumbers)
		{
			return null;
		}

		private string GetTranslation(Subtitle subtitle)
		{
			return null;
		}
	}
}
