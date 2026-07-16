using UnityEngine;

namespace CustomRendering
{
    public class FogSetting : MonoBehaviour
    {
        public float _weight;

        public FogType FogType;

        public int Priority;

        public float StartDistance;

        public float EndDistance = 200f;

        [ColorUsage(false, true)]
        public Color Color = Color.black;

        public bool _isDirty;

        public bool _state;

        public bool IsEnabled
        {
            get
            {
                return _state;
            }
            set
            {
                if (_state != value)
                {
                    _state = value;
                    _isDirty = true;
                }
            }
        }

        public float BlendTime { get; set; }

        public float Weight
        {
            get
            {
                if (!FogController.Singleton.ForcedFog.HasValue)
                {
                    return _weight;
                }

                if (FogController.Singleton.ForcedFog.Value != FogType)
                {
                    return 0f;
                }

                return 1f;
            }
            set
            {
                _weight = value;
            }
        }

        public void UpdateWeight()
        {
            if (!_isDirty)
            {
                return;
            }

            if (BlendTime <= 0f)
            {
                Weight = (IsEnabled ? 1 : 0);
                _isDirty = false;
                return;
            }

            float num = 1f / BlendTime;
            Weight += Time.deltaTime * (IsEnabled ? num : (0f - num));
            if (Weight < 0f || Weight > 1f)
            {
                Weight = Mathf.Clamp01(Weight);
                _isDirty = false;
            }
        }
    }
}
