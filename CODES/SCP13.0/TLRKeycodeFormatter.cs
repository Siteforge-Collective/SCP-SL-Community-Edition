using System;
using UnityEngine;

public sealed class TLRKeycodeFormatter : UniversalTextModifier
{
	[Serializable]
	private struct Key
	{
		public ActionName Action;

		public int FormatIndex;

		public string AdditionalFormat;
	}

	[SerializeField]
	private Key[] _keys;

	[SerializeField]
	private int _maxChars;

	[SerializeField]
	private string _missingFormat;

	private TextLanguageReplacer _cachedtlr;

	private bool _cacheSet;

	private TextLanguageReplacer TextLangReplacer => null;

	protected override void Awake()
	{
	}

	private void Format()
	{
	}
}
