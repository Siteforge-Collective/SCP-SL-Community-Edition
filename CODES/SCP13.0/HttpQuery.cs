using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using UnityEngine;

public static class HttpQuery
{
	private const int SleepTime = 12;

	private static readonly HttpClient _client;

	private static readonly bool LockHttpMode;

	private static readonly HttpQueryMode HttpMode;

	static HttpQuery()
	{
	}

	public static string Get(string url)
	{
		return null;
	}

	public static string Get(string url, out bool success)
	{
		success = default(bool);
		return null;
	}

	public static string Get(string url, out bool success, out HttpStatusCode code)
	{
		success = default(bool);
		code = default(HttpStatusCode);
		return null;
	}

	public static string Post(string url, string data)
	{
		return null;
	}

	public static string Post(string url, string data, out bool success)
	{
		success = default(bool);
		return null;
	}

	public static string Post(string url, string data, out bool success, out HttpStatusCode code)
	{
		success = default(bool);
		code = default(HttpStatusCode);
		return null;
	}

	public static string ToPostArgs(IEnumerable<string> data)
	{
		return null;
	}

	public static WWWForm ToUnityForm(string data)
	{
		return null;
	}
}
