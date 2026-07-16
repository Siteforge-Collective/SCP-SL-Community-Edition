using System;
using Utf8Json;

public readonly struct NewsList : IEquatable<NewsList>, IJsonSerializable
{
	public readonly int appid;

	public readonly NewsListItem[] newsitems;

	public readonly int count;

	[SerializationConstructor]
	public NewsList(int appid, NewsListItem[] newsitems, int count)
	{
		this.appid = 0;
		this.newsitems = null;
		this.count = 0;
	}

	public bool Equals(NewsList other)
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

	public static bool operator ==(NewsList left, NewsList right)
	{
		return false;
	}

	public static bool operator !=(NewsList left, NewsList right)
	{
		return false;
	}
}
