using System;
using System.Net;
using System.Runtime.InteropServices;

internal static class HttpWorkaround
{
	private class HttpProxyException : Exception
	{
		public HttpProxyException(string message)
		{
		}
	}

	public static readonly bool Enabled;

	private const string HttpProxy = "HttpProxy";

	private const CallingConvention Convention = CallingConvention.StdCall;

	private const CharSet Encoding = CharSet.Unicode;

	[PreserveSig]
	private static extern bool Initialize(string ptr, out IntPtr message);

	[PreserveSig]
	private static extern IntPtr Get(string url, out bool success, out int code, out IntPtr exception);

	[PreserveSig]
	private static extern IntPtr Post(string url, string data, out bool success, out int code, out IntPtr exception);

	[PreserveSig]
	private static extern void Free(IntPtr ptr);

	static HttpWorkaround()
	{
	}

	internal static string Get(string url, out bool success, out HttpStatusCode code)
	{
		success = default(bool);
		code = default(HttpStatusCode);
		return null;
	}

	internal static string Post(string url, string data, out bool success, out HttpStatusCode code)
	{
		success = default(bool);
		code = default(HttpStatusCode);
		return null;
	}
}
