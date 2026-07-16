using System.Collections.Generic;
using Discord.Elements;
using PlayerRoles;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Discord.Modules
{
	public class RichPresenceModule : DiscordModuleBase
	{
		public static readonly Dictionary<DiscordGameStatus, DiscordGameStateElement> GameState;

		public static readonly Dictionary<RoleTypeId, DiscordRoleElement> PlayableRoles;

		public static readonly Dictionary<Team, DiscordTeamElement> PlayableTeams;

		private static bool _dictionariesPopulated;

		[SerializeField]
		private DiscordElementBase[] _elements;

		private DiscordElementBase _elementBase;

		private long _timeSinceJoin;

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

		private string ServerAddress => null;

		public void RefreshPresence()
		{
		}

		public void SetStatus(DiscordGameStatus status, bool updatePresence = true)
		{
		}

		public void SetStatus(RoleTypeId roleTypeId, bool refreshPresence = true)
		{
		}

		public void SetStatus(Team team, bool updatePresence = true)
		{
		}

		public void ResetTimer(bool updatePresence = true)
		{
		}

		protected override void OnDestroy()
		{
		}

		private void Start()
		{
		}

		private void Awake()
		{
		}

		private void ClearPresence()
		{
		}

		private long GenerateUnixTime()
		{
			return 0L;
		}

		private void OnPlayerAdded(ReferenceHub _)
		{
		}

		private void OnRoleChanged(ReferenceHub userHub, PlayerRoleBase prevRole, PlayerRoleBase newRole)
		{
		}

		private void OnLevelFinishedLoading(Scene scene, LoadSceneMode _)
		{
		}

		private void PopulateDictionaries()
		{
		}
	}
}
