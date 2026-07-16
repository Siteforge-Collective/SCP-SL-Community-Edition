public readonly struct DiscordEmbedField : global::System.IEquatable<DiscordEmbedField>, IJsonSerializable
{
	public readonly string name;

	public readonly string value;

	public readonly bool inline;

	[global::Utf8Json.SerializationConstructor]
	public DiscordEmbedField(string name, string value, bool inline)
	{
		this.name = name;
		this.value = value;
		this.inline = inline;
	}

	public bool Equals(DiscordEmbedField other)
	{
		if (name == other.name && value == other.value)
		{
			return inline == other.inline;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is DiscordEmbedField other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (((((name != null) ? name.GetHashCode() : 0) * 397) ^ ((value != null) ? value.GetHashCode() : 0)) * 397) ^ inline.GetHashCode();
	}

	public static bool operator ==(DiscordEmbedField left, DiscordEmbedField right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(DiscordEmbedField left, DiscordEmbedField right)
	{
		return !left.Equals(right);
	}
}
