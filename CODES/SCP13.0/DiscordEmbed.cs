using System;
using Utf8Json;

public readonly struct DiscordEmbed : IEquatable<DiscordEmbed>, IJsonSerializable
{
	public readonly string title;

	public readonly string type;

	public readonly string description;

	public readonly int color;

	public readonly DiscordEmbedField[] fields;

	[SerializationConstructor]
	public DiscordEmbed(string title, string type, string description, int color, DiscordEmbedField[] fields)
	{
		this.title = null;
		this.type = null;
		this.description = null;
		this.color = 0;
		this.fields = null;
	}

	public bool Equals(DiscordEmbed other)
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

	public static bool operator ==(DiscordEmbed left, DiscordEmbed right)
	{
		return false;
	}

	public static bool operator !=(DiscordEmbed left, DiscordEmbed right)
	{
		return false;
	}
}
