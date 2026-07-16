namespace Hazards
{
	public class SinkholeEnvironmentalHazard : global::Hazards.EnvironmentalHazard
	{
		public override void OnEnter(ReferenceHub player)
		{
			if (IsActive && !global::PlayerRoles.PlayerRolesUtils.IsSCP(player))
			{
				base.OnEnter(player);
				player.playerEffectsController.EnableEffect<global::CustomPlayerEffects.Sinkhole>(1f);
			}
		}

		public override void OnStay(ReferenceHub player)
		{
			player.playerEffectsController.EnableEffect<global::CustomPlayerEffects.Sinkhole>(1f);
		}

		public override void OnExit(ReferenceHub player)
		{
			base.OnExit(player);
			player.playerEffectsController.EnableEffect<global::CustomPlayerEffects.Sinkhole>(1f);
		}

		protected override void Start()
		{
			if (global::Mirror.NetworkServer.active && global::GameCore.ConfigFile.ServerConfig.GetFloat("sinkhole_spawn_chance") < (float)global::UnityEngine.Random.Range(1, 100))
			{
				if (base.netId == 0)
				{
					global::UnityEngine.Object.Destroy(base.gameObject);
				}
				else
				{
					global::Mirror.NetworkServer.Destroy(base.gameObject);
				}
			}
			else
			{
				base.Start();
			}
		}

		private void MirrorProcessed()
		{
		}
	}
}
