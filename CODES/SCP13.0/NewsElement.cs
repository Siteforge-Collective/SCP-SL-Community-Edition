using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewsElement : MonoBehaviour
{
	public TextMeshProUGUI Title;

	public TextMeshProUGUI Date;

	public TextMeshProUGUI Content;

	public Image Background;

	public int Id;

	private NewsLoader _loader;

	private void Start()
	{
	}

	public void OnClick()
	{
	}
}
