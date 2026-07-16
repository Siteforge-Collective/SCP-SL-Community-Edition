using Discord.Basic;
using TMPro;
using UnityEngine;

namespace Discord.Modules
{
	public class RequestableJoinModule : DiscordModuleBase
	{
		private static readonly int Requested;

		private const string RequestedAnimKey = "Requested";

		[SerializeField]
		private Animator _joinAnimator;

		[SerializeField]
		private TMP_Text _joinText;

		private DiscordUser _joinRequest;

		public override bool IsEnabled
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public bool RequestAvailable { get; set; }

		public void SendResponse(Reply reply)
		{
		}

		protected override void OnUpdateModule()
		{
		}

		protected override void OnDestroy()
		{
		}

		private void OnReceivedRequest(ref DiscordUser request)
		{
		}

		private void OnRequestAccepted(string secret)
		{
		}

		private void Start()
		{
		}

		private void Update()
		{
		}
	}
}
