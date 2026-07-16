using UnityEngine;

namespace InventorySystem.Items.Firearms.Attachments.Components
{
    public abstract class Attachment : MonoBehaviour
    {
        private bool _parentCached;

        private Firearm _parentFirearm;

        private bool[] _activeParameters;

        private float[] _parameterValues;

        private bool _initialized;

        private const string AttNamesFilename = "AttachmentNames";

        public abstract AttachmentName Name { get; }

        public abstract AttachmentSlot Slot { get; }

        public abstract float Weight { get; }

        public abstract float Length { get; }

        public abstract AttachmentDescriptiveAdvantages DescriptivePros { get; }

        public abstract AttachmentDescriptiveDownsides DescriptiveCons { get; }

        public virtual bool IsEnabled { get; set; }

        public int AttachmentId { get; internal set; }

        private void SetupArray()
        {
            if (!_initialized)
            {
                Initialize();
                _initialized = true;
            }
        }

        protected virtual void Initialize()
        {
            int totalNumberOfParams = AttachmentsUtils.TotalNumberOfParams;
            _parameterValues = new float[totalNumberOfParams];
            _activeParameters = new bool[totalNumberOfParams];
        }

        protected void SetParameterValue(AttachmentParameterValuePair pair)
        {
            SetParameterValue(pair.Parameter, pair.Value);
        }

        protected void SetParameterValue(AttachmentParam param, float val)
        {
            SetParameterValue((int)param, val);
        }

        protected void SetParameterValue(int param, float val)
        {
            SetupArray();
            _parameterValues[param] = val;
            _activeParameters[param] = true;
        }

        protected void ResetParameter(AttachmentParam param)
        {
            SetupArray();
            _activeParameters[(int)param] = false;
        }

        public bool TryGetValue(int param, out float val)
        {
            SetupArray();
            val = _parameterValues[param];
            return _activeParameters[param];
        }

        public bool TryGetValue(AttachmentParam param, out float val)
        {
            return TryGetValue((int)param, out val);
        }

        public bool TryGetParentFirearm(out Firearm firearm)
        {
            if (!_parentCached)
            {
                if (!base.transform.parent.TryGetComponent<Firearm>(out var component))
                {
                    firearm = null;
                    return false;
                }
                _parentFirearm = component;
                _parentCached = true;
            }
            firearm = _parentFirearm;
            return true;
        }

        public void GetNameAndDescription(out string n, out string d)
        {
            if (TranslationReader.TryGet("AttachmentNames", (int)Name, out var val))
            {
                string[] array = val.Split('~');
                n = array[0];
                d = ((array.Length == 1) ? string.Empty : array[1]);
            }
            else
            {
                n = Name.ToString();
                d = string.Empty;
            }
        }

        public void GetActiveParamsNonAlloc(AttachmentParam[] activeParams, out int len)
        {
            SetupArray();
            len = 0;
            for (int i = 0; i < AttachmentsUtils.TotalNumberOfParams; i++)
            {
                if (_activeParameters[i])
                {
                    activeParams[len] = (AttachmentParam)i;
                    len++;
                }
            }
        }
    }
}
