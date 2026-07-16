using System;
using Utf8Json;

public readonly struct DiscordEmbedField : IEquatable<DiscordEmbedField>, IJsonSerializable
{
	public readonly string name;

	public readonly string value;

	public readonly bool inline;

	[SerializationConstructor]
	public DiscordEmbedField(string name, string value, bool inline)
	{
		this.name = null;
		this.value = null;
		this.inline = false;
	}

	public bool Equals(DiscordEmbedField other)
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

	public static bool operator ==(DiscordEmbedField left, DiscordEmbedField right)
	{
		return false;
	}

	public static bool operator !=(DiscordEmbedField left, DiscordEmbedField right)
	{
		return false;
	}
}
