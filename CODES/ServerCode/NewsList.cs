public readonly struct NewsList : global::System.IEquatable<NewsList>, IJsonSerializable
{
	public readonly int appid;

	public readonly NewsListItem[] newsitems;

	public readonly int count;

	[global::Utf8Json.SerializationConstructor]
	public NewsList(int appid, NewsListItem[] newsitems, int count)
	{
		this.appid = appid;
		this.newsitems = newsitems;
		this.count = count;
	}

	public bool Equals(NewsList other)
	{
		return appid == other.appid;
	}

	public override bool Equals(object obj)
	{
		if (obj is NewsList other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return appid.GetHashCode();
	}

	public static bool operator ==(NewsList left, NewsList right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(NewsList left, NewsList right)
	{
		return !left.Equals(right);
	}
}
