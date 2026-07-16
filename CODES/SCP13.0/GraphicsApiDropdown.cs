using UnityEngine;

public class GraphicsApiDropdown : MonoBehaviour
{
	private enum GraphicsApi
	{
		Auto = 0,
		DirectX11 = 1,
		DirectX12 = 2,
		Vulkan = 3
	}

	private const string GraphicsApiLauncher = "-launcher-graphicsapi";

	private const string ForceArgument = "-force";

	private const string ForceDirectX11 = "-force-d3d11";

	private const string ForceDirectX12 = "-force-d3d12";

	private const string ForceVulkan = "-force-vulkan";

	[SerializeField]
	private GameObject _forcedWarning;

	private void Awake()
	{
	}

	private static GraphicsApi GetGraphicsApi(out bool forced)
	{
		forced = default(bool);
		return default(GraphicsApi);
	}
}
