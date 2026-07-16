using UnityEngine;
using UnityEngine.UI;

public class InterfaceColorAdjuster : MonoBehaviour
{
	public Graphic[] graphicsToChange;

    private void Awake()
    {
        PlayerList.ica = this;
    }

    private void ChangeColor(ReferenceHub hub, Color color)
	{
	}
}
