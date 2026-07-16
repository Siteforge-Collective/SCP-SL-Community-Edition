using Mirror;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Utils.Networking;

namespace Respawning.NamingRules
{
	public static class UnitNameMessageHandler
	{
        public static Dictionary<SpawnableTeamType, List<string>> ReceivedNames = new Dictionary<SpawnableTeamType, List<string>>();

        private static readonly NetworkWriter SendHistory = new NetworkWriter();

        private static readonly SpawnableTeamType[] PregeneratedNameTeams = new SpawnableTeamType[1] { SpawnableTeamType.NineTailedFox };

        public static event Action<SpawnableTeamType, string, int> OnNameAdded;

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            CustomNetworkManager.OnClientReady += delegate
            {
                SendHistory.Reset();
                ReceivedNames.Clear();
                NetworkClient.ReplaceHandler<UnitNameMessage>(ProcessMessage);
                if (NetworkServer.active)
                {
                    SpawnableTeamType[] pregeneratedNameTeams = PregeneratedNameTeams;
                    foreach (SpawnableTeamType spawnableTeamType in pregeneratedNameTeams)
                    {
                        if (UnitNamingRule.TryGetNamingRule(spawnableTeamType, out var rule))
                        {
                            SendNew(spawnableTeamType, rule);
                        }
                    }
                }
            };
            ReferenceHub.OnPlayerAdded = (Action<ReferenceHub>)Delegate.Combine(ReferenceHub.OnPlayerAdded, (Action<ReferenceHub>)delegate (ReferenceHub hub)
            {
                if (NetworkServer.active && !hub.isLocalPlayer)
                {
                    NetworkReaderPooled networkReaderPooled = NetworkReaderPool.Get(new ArraySegment<byte>(SendHistory.buffer, 0, SendHistory.Position));
                    while (networkReaderPooled.Remaining > 0)
                    {
                        SpawnableTeamType spawnableTeamType = (SpawnableTeamType)networkReaderPooled.ReadByte();
                        if (!UnitNamingRule.TryGetNamingRule(spawnableTeamType, out var rule))
                        {
                            break;
                        }
                        hub.connectionToClient.Send(new UnitNameMessage
                        {
                            Data = networkReaderPooled,
                            NamingRule = rule,
                            Team = spawnableTeamType
                        });
                    }
                    networkReaderPooled.Dispose();
                }
            });
        }

        private static void ProcessMessage(UnitNameMessage msg)
        {
            if (ReceivedNames.TryGetValue(msg.Team, out var value))
            {
                value.Add(msg.UnitName);
            }
            else
            {
                value = new List<string> { msg.UnitName };
                ReceivedNames.Add(msg.Team, value);
            }
            int num = value.Count - 1;
            UnitNameMessageHandler.OnNameAdded?.Invoke(msg.Team, msg.UnitName, num);
            if (NetworkServer.active && (!PregeneratedNameTeams.Contains(msg.Team) || num != 0))
            {
                msg.NamingRule.PlayEntranceAnnouncement(msg.UnitName);
            }
        }


        public static string GetReceived(SpawnableTeamType teamType, int unitNameId)
        {
            if (!ReceivedNames.TryGetValue(teamType, out var value))
            {
                return string.Empty;
            }
            int count = value.Count;
            if (count != 0 && unitNameId < count)
            {
                return value[unitNameId];
            }
            return string.Empty;
        }

        public static void SendNew(SpawnableTeamType team, UnitNamingRule rule)
        {
            new UnitNameMessage
            {
                Team = team,
                NamingRule = rule
            }.SendToHubsConditionally((ReferenceHub x) => true);
        }

        public static UnitNameMessage ReadUnitName(this NetworkReader reader)
        {
            SpawnableTeamType spawnableTeamType = (SpawnableTeamType)reader.ReadByte();
            if (!UnitNamingRule.TryGetNamingRule(spawnableTeamType, out var rule))
            {
                throw new InvalidOperationException($"No compatible decoder detected to read the name of spawnable team: {spawnableTeamType}.");
            }
            return new UnitNameMessage
            {
                Team = spawnableTeamType,
                NamingRule = rule,
                UnitName = rule.ReadName(reader)
            };
        }

        public static void WriteUnitName(this NetworkWriter writer, UnitNameMessage msg)
        {
            byte team = (byte)msg.Team;
            writer.WriteByte(team);
            if (msg.Data == null)
            {
                int position = writer.Position;
                msg.NamingRule.GenerateNew(writer);
                SendHistory.WriteByte(team);
                SendHistory.WriteBytes(writer.buffer, position, writer.Position - position);
            }
            else
            {
                int position2 = msg.Data.Position;
                msg.NamingRule.ReadName(msg.Data);
                writer.WriteBytes(msg.Data.buffer.Array, position2, msg.Data.Position - position2);
            }
        }
    }
}
