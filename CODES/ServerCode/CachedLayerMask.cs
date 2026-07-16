public class CachedLayerMask
{
	private int _cachedMask;

	private readonly string[] _layers;

	public global::UnityEngine.LayerMask Mask
	{
		get
		{
			if (_cachedMask == 0)
			{
				_cachedMask = global::UnityEngine.LayerMask.GetMask(_layers);
			}
			return _cachedMask;
		}
	}

	public CachedLayerMask(params string[] layers)
	{
		_layers = layers;
	}

	public static implicit operator int(CachedLayerMask mask)
	{
		return mask.Mask;
	}
}
