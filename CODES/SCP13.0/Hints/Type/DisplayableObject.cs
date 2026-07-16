using Mirror;
using UnityEngine;

namespace Hints
{
    public abstract class DisplayableObject<TData> : NetworkObject<TData>
    {
        public float DurationScalar { get; private set; }
        protected new TData Data { get; private set; }
        protected bool IsAlive { get; private set; }

        protected DisplayableObject(float durationScalar = 1f)
        {
            this.DurationScalar = durationScalar;
        }

        public override void Deserialize(NetworkReader reader)
        {
            float duration = reader.ReadFloat();
            this.DurationScalar = duration;
            this.UpdateProgress(duration);
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.WriteFloat(this.DurationScalar);
        }

        public bool TryAwake(TData data, float rawTime)
        {
            float progress = this.UpdateProgress(rawTime);
            if (progress > 0f)
            {
                this.Data = data;
                this.IsAlive = true;
                this.Start();
                return true;
            }
            return false;
        }

        public float Update(float rawTime)
        {
            if (!this.IsAlive)
            {
                return 0f;
            }

            float progress = this.UpdateProgress(rawTime);

            if (progress > 0f)
            {
                this.UpdateState(progress);
                return progress;
            }

            this.Destroy();
            this.IsAlive = false;
            return 0f;
        }

        protected abstract float UpdateProgress(float rawTime);
        protected abstract void UpdateState(float progress);
    }
}