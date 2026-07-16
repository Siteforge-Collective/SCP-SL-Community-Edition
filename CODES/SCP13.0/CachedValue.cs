using System;

public class CachedValue<T>
{
	private bool _cacheSet;

	private T _cachedValue;

	private readonly Func<T> _factory;

	private readonly Func<bool> _updateChecker;

	private readonly bool _usesChecker;

	public T Value => default(T);

	public bool IsDirty => false;

	public CachedValue(Func<T> factory)
	{
	}

	public CachedValue(Func<T> factory, Func<bool> checker)
	{
	}

	public void RefreshValue()
	{
	}

	public void SetDirty()
	{
	}
}
