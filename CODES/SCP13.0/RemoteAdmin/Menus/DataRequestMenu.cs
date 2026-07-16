using UnityEngine;
using UnityEngine.UI;

namespace RemoteAdmin.Menus
{
	public class DataRequestMenu : RaCommandMenu
	{
		public enum RequestType
		{
			Info = 0,
			ShortInfo = 1,
			Auth = 2,
			ExternalLookup = 3
		}

		[SerializeField]
		private Button _externalLookup;

		public void Query(RequestType requestType)
		{
		}

		public void Query(string requestType)
		{
		}

		protected override void OnStart()
		{
		}
	}
}
