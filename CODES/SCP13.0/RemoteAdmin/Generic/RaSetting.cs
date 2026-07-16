using System;

namespace RemoteAdmin.Generic
{
	[Serializable]
	public abstract class RaSetting<T>
	{
		private T _value;

		public virtual T Value
		{
			get
			{
				return default(T);
			}
			set
			{
			}
		}

		public virtual T DefaultValue { get; }

		public abstract string Path { get; }

		public RaSetting()
		{
		}

		public void Load()
		{
		}

		public void Save()
		{
		}

		public virtual void Reset()
		{
		}

		protected abstract void OnLoad();

		protected abstract void OnSave();

		~RaSetting()
		{
		}
	}
}
