public class BloodEffectsSystem : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		global::UnityEngine.Object.Destroy(GetComponent<global::UnityEngine.Rendering.PostProcessing.PostProcessVolume>());
	}
}
