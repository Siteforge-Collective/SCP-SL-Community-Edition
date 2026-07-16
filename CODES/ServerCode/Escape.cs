public static class Escape
{
	private enum EscapeScenarioType
	{
		None = 0,
		ClassD = 1,
		CuffedClassD = 2,
		Scientist = 3,
		CuffedScientist = 4
	}

	private readonly struct EscapeScenarioText
	{
		private readonly int _id;

		private readonly string _def;

		public string Text => TranslationReader.Get("Facility", _id, _def);

		public EscapeScenarioText(int translationKey, string defaultText)
		{
			_id = translationKey;
			_def = defaultText;
		}
	}

	public struct EscapeMessage : global::Mirror.NetworkMessage
	{
		public byte ScenarioId;

		public ushort EscapeTime;
	}

	private static readonly global::System.Collections.Generic.Dictionary<Escape.EscapeScenarioType, Escape.EscapeScenarioText> Scenarios = new global::System.Collections.Generic.Dictionary<Escape.EscapeScenarioType, Escape.EscapeScenarioText>
	{
		[Escape.EscapeScenarioType.ClassD] = new Escape.EscapeScenarioText(30, "You escaped as a Class D and joined the Chaos Insurgency."),
		[Escape.EscapeScenarioType.CuffedClassD] = new Escape.EscapeScenarioText(36, "You were recaptured by the Nine-Tailed Fox.\nWith one less threat in the facility, they were able to reinforce."),
		[Escape.EscapeScenarioType.Scientist] = new Escape.EscapeScenarioText(29, "You escaped as a Scientist and joined the MTF units."),
		[Escape.EscapeScenarioType.CuffedScientist] = new Escape.EscapeScenarioText(37, "You were taken prisoner as a scientist by the Chaos Insurgency.\nThey were able to gain an advantage from the information you gave them.")
	};

	private static readonly global::UnityEngine.Vector3 WorldPos = new global::UnityEngine.Vector3(124f, 989f, 31f);

	private const float RadiusSqr = 156.5f;

	private const float MinAliveTime = 10f;

	private const string TranslationKey = "Facility";

	private const float InsurgencyEscapeReward = 4f;

	private const float FoundationEscapeReward = 3f;

	public static event global::System.Action<ReferenceHub> OnServerPlayerEscape;

	[global::UnityEngine.RuntimeInitializeOnLoadMethod]
	private static void Init()
	{
		StaticUnityMethods.OnUpdate += delegate
		{
			if (global::Mirror.NetworkServer.active)
			{
				global::Utils.NonAllocLINQ.HashsetExtensions.ForEach(ReferenceHub.AllHubs, delegate(ReferenceHub x)
				{
					ServerHandlePlayer(x);
				});
			}
		};
	}

	private static void ServerHandlePlayer(ReferenceHub hub)
	{
		global::PlayerRoles.RoleTypeId roleTypeId = global::PlayerRoles.RoleTypeId.None;
		Escape.EscapeScenarioType escapeScenarioType = ServerGetScenario(hub);
		switch (escapeScenarioType)
		{
		case Escape.EscapeScenarioType.None:
			return;
		case Escape.EscapeScenarioType.ClassD:
		case Escape.EscapeScenarioType.CuffedScientist:
			roleTypeId = global::PlayerRoles.RoleTypeId.ChaosConscript;
			global::Respawning.RespawnTokensManager.GrantTokens(global::Respawning.SpawnableTeamType.ChaosInsurgency, 4f);
			break;
		case Escape.EscapeScenarioType.CuffedClassD:
			roleTypeId = global::PlayerRoles.RoleTypeId.NtfPrivate;
			global::Respawning.RespawnTokensManager.GrantTokens(global::Respawning.SpawnableTeamType.NineTailedFox, 3f);
			break;
		case Escape.EscapeScenarioType.Scientist:
			roleTypeId = global::PlayerRoles.RoleTypeId.NtfSpecialist;
			global::Respawning.RespawnTokensManager.GrantTokens(global::Respawning.SpawnableTeamType.NineTailedFox, 3f);
			break;
		}
		if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerEscape, hub, roleTypeId))
		{
			hub.connectionToClient.Send(new Escape.EscapeMessage
			{
				ScenarioId = (byte)escapeScenarioType,
				EscapeTime = (ushort)global::UnityEngine.Mathf.CeilToInt(hub.roleManager.CurrentRole.ActiveTime)
			});
			Escape.OnServerPlayerEscape(hub);
			hub.roleManager.ServerSetRole(roleTypeId, global::PlayerRoles.RoleChangeReason.Escaped);
		}
	}

	private static Escape.EscapeScenarioType ServerGetScenario(ReferenceHub hub)
	{
		if (!(hub.roleManager.CurrentRole is global::PlayerRoles.HumanRole humanRole))
		{
			return Escape.EscapeScenarioType.None;
		}
		if ((humanRole.FpcModule.Position - WorldPos).sqrMagnitude > 156.5f)
		{
			return Escape.EscapeScenarioType.None;
		}
		if (humanRole.ActiveTime < 10f)
		{
			return Escape.EscapeScenarioType.None;
		}
		bool flag = global::InventorySystem.Disarming.DisarmedPlayers.IsDisarmed(hub.inventory);
		if (flag && !CharacterClassManager.CuffedChangeTeam)
		{
			return Escape.EscapeScenarioType.None;
		}
		switch (humanRole.RoleTypeId)
		{
		case global::PlayerRoles.RoleTypeId.Scientist:
			if (!flag)
			{
				return Escape.EscapeScenarioType.Scientist;
			}
			return Escape.EscapeScenarioType.CuffedScientist;
		case global::PlayerRoles.RoleTypeId.ClassD:
			if (!flag)
			{
				return Escape.EscapeScenarioType.ClassD;
			}
			return Escape.EscapeScenarioType.CuffedClassD;
		default:
			return Escape.EscapeScenarioType.None;
		}
	}

	private static void ClientReceiveMessage(Escape.EscapeMessage msg)
	{
	}
}
