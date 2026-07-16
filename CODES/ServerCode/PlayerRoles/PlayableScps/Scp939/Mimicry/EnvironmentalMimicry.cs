namespace PlayerRoles.PlayableScps.Scp939.Mimicry
{
	public class EnvironmentalMimicry : global::PlayerRoles.PlayableScps.Subroutines.ScpStandardSubroutine<global::PlayerRoles.PlayableScps.Scp939.Scp939Role>
	{
		[global::System.Serializable]
		public struct EnvMimicryCategory
		{
			public global::PlayerRoles.PlayableScps.Scp939.Scp939HudTranslation Name;

			public global::PlayerRoles.PlayableScps.Scp939.Mimicry.EnvMimicryOption[] Options;
		}

		private byte _syncCat;

		private byte _syncSound;

		private global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicPointController _mimicPoint;

		[global::UnityEngine.SerializeField]
		private float _activationCooldown;

		public readonly global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown Cooldown = new global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown();

		[field: global::UnityEngine.SerializeField]
		public global::PlayerRoles.PlayableScps.Scp939.Mimicry.EnvironmentalMimicry.EnvMimicryCategory[] Categories { get; private set; }

		public string CooldownText
		{
			get
			{
				if (Cooldown.IsReady)
				{
					return string.Empty;
				}
				return string.Format(Translations.Get(global::PlayerRoles.PlayableScps.Scp939.Scp939HudTranslation.EnvMimicryCooldown), ((float)global::UnityEngine.Mathf.RoundToInt(Cooldown.Remaining * 10f) / 10f).ToString("0.0"));
			}
		}

		public event global::System.Action OnSoundPlayed;

		protected override void Awake()
		{
			base.Awake();
			GetSubroutine<global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicPointController>(out _mimicPoint);
		}

		public override void ResetObject()
		{
			base.ResetObject();
			Cooldown.Clear();
		}

		public void ClientPlay(int category, int sound)
		{
			_syncCat = (byte)category;
			_syncSound = (byte)sound;
			ClientSendCmd();
		}

		public override void ClientWriteCmd(global::Mirror.NetworkWriter writer)
		{
			base.ClientWriteCmd(writer);
			writer.WriteByte(_syncCat);
			writer.WriteByte(_syncSound);
		}

		public override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			base.ServerProcessCmd(reader);
			if (Cooldown.IsReady)
			{
				_syncCat = reader.ReadByte();
				_syncSound = reader.ReadByte();
				Cooldown.Trigger(_activationCooldown);
				ServerSendRpc(toAll: true);
			}
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			writer.WriteByte(_syncCat);
			writer.WriteByte(_syncSound);
			Cooldown.WriteCooldown(writer);
			writer.WriteByte((byte)global::UnityEngine.Random.Range(0, 255));
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			_syncCat = (byte)(reader.ReadByte() % Categories.Length);
			_syncSound = (byte)(reader.ReadByte() % Categories[_syncCat].Options.Length);
			Cooldown.ReadCooldown(reader);
			Categories[_syncCat].Options[_syncSound].Play(reader.ReadByte());
			this.OnSoundPlayed?.Invoke();
		}

		private void Update()
		{
			global::PlayerRoles.PlayableScps.Scp939.Mimicry.EnvironmentalMimicry.EnvMimicryCategory[] categories = Categories;
			for (int i = 0; i < categories.Length; i++)
			{
				categories[i].Options.ForEach(delegate(global::PlayerRoles.PlayableScps.Scp939.Mimicry.EnvMimicryOption x)
				{
					x.UpdateSequence(_mimicPoint);
				});
			}
		}
	}
}
