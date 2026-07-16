using UnityEngine;

public class GunAim : MonoBehaviour
{
	public int borderLeft;

	public int borderRight;

	public int borderTop;

	public int borderBottom;

	private Camera parentCamera;

	private bool isOutOfBounds;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public bool GetIsOutOfBounds()
	{
		return false;
	}
}
