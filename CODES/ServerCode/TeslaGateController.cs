public class TeslaGateController : global::Mirror.NetworkBehaviour
{
	public global::System.Collections.Generic.List<TeslaGate> TeslaGates = new global::System.Collections.Generic.List<TeslaGate>();

	public static TeslaGateController Singleton { get; private set; }

	private static void ServerReceiveMessage(global::Mirror.NetworkConnection conn, TeslaHitMsg msg)
	{
		if (ReferenceHub.TryGetHubNetID(conn.identity.netId, out var hub))
		{
			if (msg.TeslaGateId < 0 || msg.TeslaGateId >= Singleton.TeslaGates.Count)
			{
				hub.gameConsoleTransmission.SendToClient($"Received invalid tesla gate id {msg.TeslaGateId}!", "red");
				return;
			}
			if (global::UnityEngine.Vector3.Distance(Singleton.TeslaGates[msg.TeslaGateId].transform.position, hub.transform.position) > Singleton.TeslaGates[msg.TeslaGateId].sizeOfTrigger * 2.2f)
			{
				hub.gameConsoleTransmission.SendToClient("You are too far from a tesla gate!", "red");
				return;
			}
			global::PlayerStatsSystem.DamageHandlerBase.CassieAnnouncement cassieAnnouncement = new global::PlayerStatsSystem.DamageHandlerBase.CassieAnnouncement();
			cassieAnnouncement.Announcement = "SUCCESSFULLY TERMINATED BY AUTOMATIC SECURITY SYSTEM";
			cassieAnnouncement.SubtitleParts = new global::Subtitles.SubtitlePart[1]
			{
				new global::Subtitles.SubtitlePart(global::Subtitles.SubtitleType.TerminatedBySecuritySystem, (string[])null)
			};
			global::PlayerStatsSystem.DamageHandlerBase.CassieAnnouncement cassieAnnouncement2 = cassieAnnouncement;
			hub.playerStats.DealDamage(new global::PlayerStatsSystem.UniversalDamageHandler(global::UnityEngine.Random.Range(200, 300), global::PlayerStatsSystem.DeathTranslations.Tesla, cassieAnnouncement2));
		}
	}

	private void Awake()
	{
		Singleton = this;
	}

	private void Start()
	{
		global::MEC.Timing.RunCoroutine(DelayedStopIdleParticles());
		global::Mirror.NetworkServer.ReplaceHandler<TeslaHitMsg>(ServerReceiveMessage);
	}

	private global::System.Collections.Generic.IEnumerator<float> DelayedStopIdleParticles()
	{
		for (int i = 0; i < 15; i++)
		{
			yield return float.NegativeInfinity;
		}
		foreach (TeslaGate teslaGate in TeslaGates)
		{
			if (teslaGate == null || teslaGate.windupParticles == null)
			{
				continue;
			}
			global::UnityEngine.ParticleSystem[] windupParticles = teslaGate.windupParticles;
			foreach (global::UnityEngine.ParticleSystem particleSystem in windupParticles)
			{
				if (!(particleSystem == null))
				{
					particleSystem.Stop();
				}
			}
		}
	}

	public void FixedUpdate()
	{
		if (global::Mirror.NetworkServer.active)
		{
			foreach (TeslaGate teslaGate in TeslaGates)
			{
				if (teslaGate.isActiveAndEnabled)
				{
					if (teslaGate.InactiveTime > 0f)
					{
						teslaGate.NetworkInactiveTime = global::UnityEngine.Mathf.Max(0f, teslaGate.InactiveTime - global::UnityEngine.Time.fixedDeltaTime);
					}
					else
					{
						bool flag = false;
						bool flag2 = false;
						foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
						{
							if (global::PlayerRoles.PlayerRolesUtils.IsAlive(allHub))
							{
								if (!flag)
								{
									flag = teslaGate.PlayerInIdleRange(allHub);
								}
								if (!flag2 && teslaGate.PlayerInRange(allHub) && !teslaGate.InProgress)
								{
									flag2 = true;
								}
							}
						}
						if (flag2)
						{
							teslaGate.ServerSideCode();
						}
						if (flag != teslaGate.isIdling)
						{
							teslaGate.ServerSideIdle(flag);
						}
					}
				}
			}
			return;
		}
		foreach (TeslaGate teslaGate2 in TeslaGates)
		{
			teslaGate2.ClientSideCode();
		}
	}

	private void MirrorProcessed()
	{
	}
}
