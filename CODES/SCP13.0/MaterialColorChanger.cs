using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MaterialColorChanger : MonoBehaviour
{
	[SerializeField]
	[ColorUsage(false, true)]
	private Color _color;

	[SerializeField]
	private int _materialIndex;

	[SerializeField]
	private string _shaderColorPropertyName;

	private void Awake()
	{
	}
}
