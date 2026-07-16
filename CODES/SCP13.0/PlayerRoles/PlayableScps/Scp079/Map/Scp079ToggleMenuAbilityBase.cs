using System.Diagnostics;
using Mirror;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.Map
{
	public abstract class Scp079ToggleMenuAbilityBase<T> : Scp079KeyAbilityBase where T : Scp079ToggleMenuAbilityBase<T>
	{
		[SerializeField]
		private float _cooldown;

		[SerializeField]
		private AudioClip _soundOpen;

		[SerializeField]
		private AudioClip _soundClose;

		private string _openTxt;

		private string _closeTxt;

		private static bool _trackedInstanceSet;

		private static Scp079ToggleMenuAbilityBase<T> _trackedInstance;

		private static readonly Stopwatch CooldownSw;

		public override bool IsReady => false;

		public override string AbilityName => null;

		public override string FailMessage => null;

		protected bool SyncState { get; set; }

		protected abstract Scp079HudTranslation OpenTranslation { get; }

		protected abstract Scp079HudTranslation CloseTranslation { get; }

		public static bool IsOpen
		{
			get
			{
				return false;
			}
			internal set
			{
			}
		}

		public static bool Visible => false;

		private void OnSpectatorTargetChanged()
		{
		}

		private void PlaySound()
		{
		}

		protected override void Trigger()
		{
		}

		protected override void Start()
		{
		}

		public override void ClientWriteCmd(NetworkWriter writer)
		{
		}

		public override void ServerProcessCmd(NetworkReader reader)
		{
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}
	}
}
