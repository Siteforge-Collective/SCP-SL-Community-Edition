using System;

public static class TimeBehaviour
{
	public static long CurrentUnixTimestamp => 0L;

	public static long CurrentTimestamp()
	{
		return 0L;
	}

	public static long GetBanExpirationTime(uint seconds)
	{
		return 0L;
	}

	public static bool ValidateTimestamp(long timestampentry, long timestampexit, long limit)
	{
		return false;
	}

	public static string Rfc3339Time()
	{
		return null;
	}

	public static string Rfc3339Time(DateTimeOffset date)
	{
		return null;
	}

	public static string FormatTime(string format)
	{
		return null;
	}

	public static string FormatTime(string format, DateTimeOffset date)
	{
		return null;
	}
}
