using System;
using Utf8Json;

public readonly struct CreditsList : IEquatable<CreditsList>, IJsonSerializable
{
	public readonly CreditsListCategory[] credits;

	[SerializationConstructor]
	public CreditsList(CreditsListCategory[] credits)
	{
		this.credits = null;
	}

	public bool Equals(CreditsList other)
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

	public static bool operator ==(CreditsList left, CreditsList right)
	{
		return false;
	}

	public static bool operator !=(CreditsList left, CreditsList right)
	{
		return false;
	}
}
