using System;
using Utf8Json;

public readonly struct CreditsListMember : IEquatable<CreditsListMember>, IJsonSerializable
{
	public readonly string name;

	public readonly string title;

	public readonly string color;

	[SerializationConstructor]
	public CreditsListMember(string name, string title, string color)
	{
		this.name = null;
		this.title = null;
		this.color = null;
	}

	public bool Equals(CreditsListMember other)
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

	public static bool operator ==(CreditsListMember left, CreditsListMember right)
	{
		return false;
	}

	public static bool operator !=(CreditsListMember left, CreditsListMember right)
	{
		return false;
	}
}
