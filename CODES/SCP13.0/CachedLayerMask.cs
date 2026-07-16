using UnityEngine;

public class CachedLayerMask
{
	private int _cachedMask;

	private readonly string[] _layers;

	public LayerMask Mask => default(LayerMask);

	public CachedLayerMask(params string[] layers)
	{
	}

	public static implicit operator int(CachedLayerMask mask)
	{
		return 0;
	}
}
