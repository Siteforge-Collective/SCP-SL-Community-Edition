using UnityEngine;

public class LCZ_Label : MonoBehaviour
{
	public MeshRenderer chRend;

	public MeshRenderer numRend;

	public void Refresh(Material ch, Material num)
	{
		chRend.material = ch;
        numRend.material = num;
	}
}
