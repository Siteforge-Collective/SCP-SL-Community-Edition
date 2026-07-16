using UnityEngine;

namespace Discord.Modules
{
	public abstract class DiscordModuleBase : MonoBehaviour
	{
		[SerializeField]
		private bool _isEnabled;

		public virtual bool IsEnabled
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public void UpdateModule()
		{
		}

		protected virtual void OnUpdateModule()
		{
		}

		protected virtual void OnDestroy()
		{
		}
	}
}
