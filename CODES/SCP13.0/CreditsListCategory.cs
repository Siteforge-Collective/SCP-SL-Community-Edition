using System;
using Utf8Json;

public readonly struct CreditsListCategory : IEquatable<CreditsListCategory>, IJsonSerializable
{
	public readonly string category;

	public readonly CreditsListMember[] members;

	[SerializationConstructor]
	public CreditsListCategory(string category, CreditsListMember[] members)
	{
		this.category = null;
		this.members = null;
	}

	public bool Equals(CreditsListCategory other)
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

	public static bool operator ==(CreditsListCategory left, CreditsListCategory right)
	{
		return false;
	}

	public static bool operator !=(CreditsListCategory left, CreditsListCategory right)
	{
		return false;
	}
}
