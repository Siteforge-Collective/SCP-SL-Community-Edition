using UnityEngine;
using UnityEngine.UI;

public class ParticleMenu : MonoBehaviour
{
	public ParticleExamples[] particleSystems;

	public GameObject gunGameObject;

	private int currentIndex;

	private GameObject currentGO;

	public Transform spawnLocation;

	public Text title;

	public Text description;

	public Text navigationDetails;

	private void Start()
	{
	}

	public void Navigate(int i)
	{
	}
}
