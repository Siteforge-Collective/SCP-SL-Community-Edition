using System.Collections.Generic;
using System.Text;
using RemoteAdmin.Elements;
using TMPro;
using Tooltips;
using UnityEngine;

namespace RemoteAdmin.Menus
{
	public class RaCommandMenu : MonoBehaviour, ITooltipHolder
	{
		[field: Tooltip("The format on which our command will be sent.\n{0} = Command.\n{1} = Selected Player.\n{2} = Selected Values.\n{3} = Input Field.")]
		[field: SerializeField]
		protected string DefaultFormat { get; set; }

		[field: SerializeField]
		[field: Header("-- Required parameters --")]
		public List<ValueButton> Options { get; set; }

		[field: SerializeField]
		[field: Header("-- Optional parameters --")]
		protected TMP_InputField InputFieldText { get; set; }

		[field: SerializeField]
		public TooltipData[] StoredInfo { get; set; }

		[field: SerializeField]
		protected TooltipManager TooltipManager { get; set; }

		public virtual void SendCommand(string command, string format = "")
		{
		}

		protected virtual string BuildCommand(string command, string format)
		{
			return null;
		}

		protected virtual string GetSelectedPlayers(StringBuilder builder)
		{
			return null;
		}

		protected virtual string GetSelectedOptions(StringBuilder builder)
		{
			return null;
		}

		protected virtual string GetInputFieldText()
		{
			return null;
		}

		protected virtual void OnStart()
		{
		}

		protected virtual void OnUpdate()
		{
		}

		private void Update()
		{
		}

		private void Start()
		{
		}
	}
}
