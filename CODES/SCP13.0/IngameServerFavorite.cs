using UnityEngine;
using UnityEngine.UI;

public class IngameServerFavorite : MonoBehaviour
{
	public Sprite FullStar;

	public Sprite EmptyStar;

	public Image StarImage;

	[SerializeField]
	private Button _favoriteButton;

	private bool _favorited;

	private void Start()
	{
	}

	private void SetStar(bool isFullStar)
	{
	}

	public void OnButtonClick()
	{
	}
}
