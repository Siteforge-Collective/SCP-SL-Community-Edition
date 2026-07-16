using System;
using Utf8Json;

public readonly struct AuthenticatorResponse : IEquatable<AuthenticatorResponse>, IJsonSerializable
{
	public readonly bool success;

	public readonly bool verified;

	public readonly string error;

	public readonly string token;

	public readonly string[] messages;

	public readonly string[] actions;

	public readonly string[] authAccepted;

	public readonly AuthenticatiorAuthReject[] authRejected;

	public readonly string verificationChallenge;

	public readonly string verificationResponse;

	[SerializationConstructor]
	public AuthenticatorResponse(bool success, bool verified, string error, string token, string[] messages, string[] actions, string[] authAccepted, AuthenticatiorAuthReject[] authRejected, string verificationChallenge, string verificationResponse)
	{
		this.success = false;
		this.verified = false;
		this.error = null;
		this.token = null;
		this.messages = null;
		this.actions = null;
		this.authAccepted = null;
		this.authRejected = null;
		this.verificationChallenge = null;
		this.verificationResponse = null;
	}

	public bool Equals(AuthenticatorResponse other)
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

	public static bool operator ==(AuthenticatorResponse left, AuthenticatorResponse right)
	{
		return false;
	}

	public static bool operator !=(AuthenticatorResponse left, AuthenticatorResponse right)
	{
		return false;
	}
}
