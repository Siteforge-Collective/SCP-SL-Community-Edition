using Decals;
using UnityEngine;

public class BulletHoleController : MonoBehaviour
{
	[SerializeField]
	private Material[] _materials;

	[SerializeField]
	private float _minScale;

	[SerializeField]
	private float _maxScale;

	private Decal _decal;

	private Transform _transform;

	private void Awake()
	{
	}

	public void SetupDecal(RaycastHit hit)
	{
	}
}
