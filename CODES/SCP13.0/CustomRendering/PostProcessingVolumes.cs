using UnityEngine;
using UnityEngine.Rendering;

namespace CustomRendering
{
    public class PostProcessingVolumes : MonoBehaviour
    {
        private static PostProcessingVolumes _singleton;

        private void Awake()
        {
            if (_singleton != null)
            {
                Destroy(this);
                return;
            }
            _singleton = this;
        }

        public static Volume SafeGetVolume(int layer, float priority, params VolumeComponent[] settings)
        {
            GameObject go = new GameObject("TemporaryPostProcessVolume");
            go.transform.parent = _singleton.transform;
            Volume volume = go.AddComponent<Volume>();
            volume.isGlobal = false;
            volume.blendDistance = 1f;
            go.layer = layer;
            volume.priority = priority;
            VolumeProfile profile = ScriptableObject.CreateInstance<VolumeProfile>();
            profile.components.AddRange(settings);
            volume.profile = profile;
            return volume;
        }

        public static void DestroyVolume(Volume volume)
        {
            if (volume != null)
            {
                Destroy(volume.gameObject);
            }
        }
    }
}
