using System;
using Utf8Json;

public readonly struct DiscordWebhook : IEquatable<DiscordWebhook>, IJsonSerializable
{
	public readonly string content;

	public readonly string username;

	public readonly string avatar_url;

	public readonly bool tts;

	public readonly DiscordEmbed[] embeds;

	[SerializationConstructor]
	public DiscordWebhook(string content, string username, string avatar_url, bool tts, DiscordEmbed[] embeds)
	{
		this.content = null;
		this.username = null;
		this.avatar_url = null;
		this.tts = false;
		this.embeds = null;
	}

	public bool Equals(DiscordWebhook other)
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

	public static bool operator ==(DiscordWebhook left, DiscordWebhook right)
	{
		return false;
	}

	public static bool operator !=(DiscordWebhook left, DiscordWebhook right)
	{
		return false;
	}
}
