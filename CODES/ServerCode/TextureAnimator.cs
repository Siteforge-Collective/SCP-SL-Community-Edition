public class TextureAnimator : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Material[] textures;

	public global::UnityEngine.Renderer targetRenderer;

	public float cooldown;

	public global::UnityEngine.Light optionalLight;

	public int lightRange;

	private void Start()
	{
		global::MEC.Timing.RunCoroutine(_Animate(), global::MEC.Segment.FixedUpdate);
	}

	private global::System.Collections.Generic.IEnumerator<float> _Animate()
	{
		while (this != null)
		{
			for (int i = 0; i < textures.Length; i++)
			{
				optionalLight.enabled = i < lightRange;
				targetRenderer.material = textures[i];
				for (int x = 0; (float)x < 50f * cooldown; x++)
				{
					yield return 0f;
				}
			}
		}
	}
}
