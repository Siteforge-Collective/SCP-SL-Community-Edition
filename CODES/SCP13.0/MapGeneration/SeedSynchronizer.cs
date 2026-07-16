using System;
using System.Collections.Generic;
using System.Diagnostics;
using GameCore;
using GameObjectPools;
using Mirror;
using PluginAPI.Events;
using UnityEngine;

namespace MapGeneration
{
    public class SeedSynchronizer : NetworkBehaviour
    {
        private const string SeedConfigKey = "map_seed";
        private const string DebugLogChannel = "MAPGEN";
        private const string WarningLogFormat = "<color=orange>Warning:</color> {0}";
        private const string ErrorLogFormat = "<color=red>Fatal Error:</color> {0}";

        private static readonly string[] MapAliases = new string[3] { "LCZ", "HCZ", "EZ" };

        public static bool MapGenerated { get; private set; }

        [SyncVar]
        private int _syncSeed;

        private static SeedSynchronizer _singleton;
        private static readonly Stopwatch _stopwatch = new Stopwatch();

        public static float TimeSinceMapGeneration => (float)_stopwatch.Elapsed.TotalSeconds;

        public static int Seed
        {
            get
            {
                if (_singleton != null)
                {
                    return _singleton._syncSeed;
                }
                return 0;
            }
        }

        public static event Action OnMapGenerated;

        private void Awake()
        {
            _singleton = this;
        }

        private void OnDestroy()
        {
            _singleton = null;
        }

        private void Start()
        {
            int configSeed = ConfigFile.ServerConfig.GetInt(SeedConfigKey, -1);
            int randomSeed = UnityEngine.Random.Range(1, int.MaxValue);

            DebugInfo("Server has successfully generated a random seed: " + randomSeed.ToString(),
                      MessageImportance.Normal);

            if (NetworkServer.active)
            {
                _syncSeed = (configSeed != -1) ? configSeed : randomSeed;
            }
        }

        private void Update()
        {
            if (Seed <= 0 || MapGenerated || ReferenceHub.LocalHub == null)
            {
                return;
            }

            GenerateLevel();

            HashSet<RoomIdentifier> hashSet = new HashSet<RoomIdentifier>();
            foreach (RoomIdentifier allRoomIdentifier in RoomIdentifier.AllRoomIdentifiers)
            {
                if (allRoomIdentifier == null || !allRoomIdentifier.TryAssignId())
                {
                    hashSet.Add(allRoomIdentifier);
                }
            }
            foreach (RoomIdentifier item in hashSet)
            {
                RoomIdentifier.AllRoomIdentifiers.Remove(item);
            }

            EventManager.ExecuteEvent(new MapGeneratedEvent());

            try
            {
                PoolManager.Singleton.RestartRound();

                if (OnMapGenerated != null)
                {
                    Delegate[] invocationList = OnMapGenerated.GetInvocationList();
                    foreach (Delegate dlg in invocationList)
                    {
                        try
                        {
                            dlg.Method.Invoke(dlg.Target, null);
                        }
                        catch (Exception innerEx)
                        {
                            UnityEngine.Debug.LogError("- " + dlg.Method.Name);
                            UnityEngine.Debug.LogError(innerEx.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError("Failed to call the OnMapGenerated event, error: " + ex.Message);
                UnityEngine.Debug.LogError(ex.StackTrace);
            }

            MapGenerated = true;
            _stopwatch.Restart();
        }

        private void GenerateLevel()
        {
            for (int i = 0; i < ImageGenerator.ZoneGenerators.Length; i++)
            {
                string text = MapAliases[i];
                if (ImageGenerator.ZoneGenerators[i].GenerateMap(_syncSeed - i, text, out var blackbox))
                {
                    DebugInfo(text + " generator tasks completed, no fatal errors to report.", MessageImportance.LessImportant);
                }
                else
                {
                    DebugError(isFatal: true, text + " generator tasks failed, blackbox message: " + blackbox);
                }
            }

            DebugInfo("Sequence of procedural level generation completed.", MessageImportance.Normal);

            if (SteamLobby.singleton != null)
            {
                SteamLobby.singleton.CreateLobby(PlayButton.maxPlayers, false);
            }

            DoorSpawnpoint.SetupAllDoors();
        }

        internal static void DebugInfo(string txt, MessageImportance importance, bool nospace = false)
        {
            GameCore.Console.AddDebugLog(DebugLogChannel, txt, importance, nospace);
        }

        internal static void DebugError(bool isFatal, string txt)
        {
            string format = isFatal ? ErrorLogFormat : WarningLogFormat;
            DebugInfo(string.Format(format, txt), MessageImportance.MostImportant);
            UnityEngine.Debug.LogError($"Map generation error for seed {Seed}: {txt}");
        }

        public static void ResetStatus()
        {
            MapGenerated = false;
            _stopwatch.Reset();
        }
    }
}