namespace FacilitySoundtrack
{
	public class SoundtrackManager : global::UnityEngine.MonoBehaviour
	{
		[global::UnityEngine.SerializeField]
		private global::FacilitySoundtrack.SoundtrackLayerBase[] _layers;

		private void Update()
		{
			int num = _layers.Length - 1;
			float num2 = 0f;
			for (int num3 = num; num3 >= 0; num3--)
			{
				float num4 = global::UnityEngine.Mathf.Clamp01(_layers[num3].Weight);
				float volumeScale = global::UnityEngine.Mathf.Max(1f - num2, 0f) * num4;
				if (!_layers[num3].Additive)
				{
					num2 += num4;
				}
				_layers[num3].UpdateVolume(volumeScale);
			}
		}
	}
}
