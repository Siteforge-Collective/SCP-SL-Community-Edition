public static class DebugLog
{
	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	public static void Log(string text)
	{
		global::UnityEngine.Debug.Log(text);
	}

	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	public static void Log(object text)
	{
		global::UnityEngine.Debug.Log(text);
	}

	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	public static void LogWarning(string text)
	{
		global::UnityEngine.Debug.LogWarning(text);
	}

	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	public static void LogWarning(object text)
	{
		global::UnityEngine.Debug.LogWarning(text);
	}

	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	public static void LogError(string text)
	{
		global::UnityEngine.Debug.LogError(text);
	}

	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	public static void LogError(object text)
	{
		global::UnityEngine.Debug.LogError(text);
	}

	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	public static void LogException(global::System.Exception exception)
	{
		global::UnityEngine.Debug.LogException(exception);
	}

	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	[global::System.Diagnostics.Conditional("UNITY_EDITOR")]
	public static void LogEditor(string text)
	{
		global::UnityEngine.Debug.Log(text);
	}

	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	[global::System.Diagnostics.Conditional("UNITY_EDITOR")]
	public static void LogEditor(object text)
	{
		global::UnityEngine.Debug.Log(text);
	}

	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	[global::System.Diagnostics.Conditional("UNITY_EDITOR")]
	public static void LogWarningEditor(string text)
	{
		global::UnityEngine.Debug.LogWarning(text);
	}

	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	[global::System.Diagnostics.Conditional("UNITY_EDITOR")]
	public static void LogWarningEditor(object text)
	{
		global::UnityEngine.Debug.LogWarning(text);
	}

	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	[global::System.Diagnostics.Conditional("UNITY_EDITOR")]
	public static void LogErrorEditor(string text)
	{
		global::UnityEngine.Debug.LogError(text);
	}

	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	[global::System.Diagnostics.Conditional("UNITY_EDITOR")]
	public static void LogErrorEditor(object text)
	{
		global::UnityEngine.Debug.LogError(text);
	}

	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	[global::System.Diagnostics.Conditional("UNITY_EDITOR")]
	public static void LogExceptionEditor(global::System.Exception exception)
	{
		global::UnityEngine.Debug.LogException(exception);
	}

	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	public static void LogBuild(string text)
	{
		global::UnityEngine.Debug.Log(text);
	}

	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	public static void LogBuild(object text)
	{
		global::UnityEngine.Debug.Log(text);
	}

	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	public static void LogWarningBuild(string text)
	{
		global::UnityEngine.Debug.LogWarning(text);
	}

	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	public static void LogWarningBuild(object text)
	{
		global::UnityEngine.Debug.LogWarning(text);
	}

	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	public static void LogErrorBuild(string text)
	{
		global::UnityEngine.Debug.LogError(text);
	}

	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	public static void LogErrorBuild(object text)
	{
		global::UnityEngine.Debug.LogError(text);
	}

	[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	public static void LogExceptionBuild(global::System.Exception exception)
	{
		global::UnityEngine.Debug.LogException(exception);
	}
}
