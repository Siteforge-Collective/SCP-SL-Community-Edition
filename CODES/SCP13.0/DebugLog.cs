using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public static class DebugLog
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Log(string text)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Log(object text)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void LogWarning(string text)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void LogWarning(object text)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void LogError(string text)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void LogError(object text)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void LogException(Exception exception)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Conditional("UNITY_EDITOR")]
	public static void LogEditor(string text)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Conditional("UNITY_EDITOR")]
	public static void LogEditor(object text)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Conditional("UNITY_EDITOR")]
	public static void LogWarningEditor(string text)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Conditional("UNITY_EDITOR")]
	public static void LogWarningEditor(object text)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Conditional("UNITY_EDITOR")]
	public static void LogErrorEditor(string text)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Conditional("UNITY_EDITOR")]
	public static void LogErrorEditor(object text)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Conditional("UNITY_EDITOR")]
	public static void LogExceptionEditor(Exception exception)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void LogBuild(string text)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void LogBuild(object text)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void LogWarningBuild(string text)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void LogWarningBuild(object text)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void LogErrorBuild(string text)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void LogErrorBuild(object text)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void LogExceptionBuild(Exception exception)
	{
	}
}
