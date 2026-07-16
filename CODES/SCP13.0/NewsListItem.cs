using System;
using Utf8Json;

public readonly struct NewsListItem : IEquatable<NewsListItem>, IJsonSerializable
{
	public readonly string gid;

	public readonly string title;

	public readonly string url;

	public readonly bool is_external_url;

	public readonly string author;

	public readonly string contents;

	public readonly string feedlabel;

	public readonly long date;

	public readonly string feedname;

	public readonly int feedtype;

	[SerializationConstructor]
	public NewsListItem(string gid, string title, string url, bool is_external_url, string author, string contents, string feedlabel, long date, string feedname, int feedtype)
	{
		this.gid = null;
		this.title = null;
		this.url = null;
		this.is_external_url = false;
		this.author = null;
		this.contents = null;
		this.feedlabel = null;
		this.date = 0L;
		this.feedname = null;
		this.feedtype = 0;
	}

	public bool Equals(NewsListItem other)
	{
		return false;
	}

	public override bool Equals(object obj)
	{
		return false;
	}

	public override int GetHashCode()
	{
		return 0;
	}

	public static bool operator ==(NewsListItem left, NewsListItem right)
	{
		return false;
	}

	public static bool operator !=(NewsListItem left, NewsListItem right)
	{
		return false;
	}
}
