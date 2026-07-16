namespace FacilitySoundtrack
{
	public abstract class SoundtrackLayerBase : global::UnityEngine.MonoBehaviour
	{
		public abstract float Weight { get; }

		public abstract bool Additive { get; }

		public abstract void UpdateVolume(float volumeScale);
	}
}
