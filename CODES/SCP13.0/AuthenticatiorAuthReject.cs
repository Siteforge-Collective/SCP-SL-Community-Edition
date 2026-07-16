using System;
using Utf8Json;

public readonly struct AuthenticatiorAuthReject : IEquatable<AuthenticatiorAuthReject>, IJsonSerializable
{
	public readonly string Id;

	public readonly string Reason;

	[SerializationConstructor]
	public AuthenticatiorAuthReject(string id, string reason)
	{
		Id = null;
		Reason = null;
	}

	public bool Equals(AuthenticatiorAuthReject other)
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

	public static bool operator ==(AuthenticatiorAuthReject left, AuthenticatiorAuthReject right)
	{
		return false;
	}

	public static bool operator !=(AuthenticatiorAuthReject left, AuthenticatiorAuthReject right)
	{
		return false;
	}
}
