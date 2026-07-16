using System.Text;
using TMPro;
using UnityEngine;

public class DevelopmentWatermark : MonoBehaviour
{
	private const int WatermarkLineLenght = 100;

	private const byte WatermarkLineCount = 40;

	[SerializeField]
	private TextMeshProUGUI _watermarkDisplay;

	private bool _isAuthenticated;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void FillRectWithRepeatingText(string id)
	{
	}

	private static int AppendAndCount(StringBuilder sb, string text)
	{
		return 0;
	}
}
