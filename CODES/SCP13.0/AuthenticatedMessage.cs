using System.Collections.Generic;

public class AuthenticatedMessage
{
	public readonly string Message;

	public readonly bool Administrator;

	public AuthenticatedMessage(string m, bool a)
	{
	}

	public static string GenerateAuthenticatedMessage(string message, long timestamp, string password)
	{
		return null;
	}

	public static string GenerateNonAuthenticatedMessage(string message)
	{
		return null;
	}

	public static AuthenticatedMessage AuthenticateMessage(string message, long timestamp, string password)
	{
		return null;
	}

	public static byte[] Encode(byte[] data)
	{
		return null;
	}

	public static List<byte[]> Decode(byte[] data)
	{
		return null;
	}
}
