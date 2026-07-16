using UnityEngine;

namespace RemoteAdmin.Elements
{
	public class SendButton : CustomButton
	{
		public string CustomCommand;

		[Tooltip("The format on which our command will be sent.\n{0} = Command.\n{1} = Selected Player.\n{2} = Selected Values.\n{3} = Input Field.")]
		public string CustomFormat;

		public override void Select()
		{
		}

		protected virtual void SendCommand(string command, string format)
		{
		}
	}
}
