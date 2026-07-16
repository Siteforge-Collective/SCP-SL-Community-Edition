namespace MapGeneration
{
	public class SeedSynchronizer : global::Mirror.NetworkBehaviour
	{
		private const string SeedConfigKey = "map_seed";

		private const string DebugLogChannel = "MAPGEN";

		private const string WarningLogFormat = "<color=orange>Warning:</color> {0}";

		private const string ErrorLogFormat = "<color=red>Fatal Error:</color> {0}";

		private static readonly string[] MapAliases = new string[3] { "LCZ", "HCZ", "EZ" };

		public static bool MapGenerated;

		[global::Mirror.SyncVar]
		private int _syncSeed;

		private static global::MapGeneration.SeedSynchronizer _singleton;

		private static readonly global::System.Diagnostics.Stopwatch _stopwatch;

		public static float TimeSinceMapGeneration => (float)_stopwatch.Elapsed.TotalSeconds;

		public static int Seed
		{
			get
			{
				if (!(_singleton == null))
				{
					return _singleton._syncSeed;
				}
				return 0;
			}
		}

		public int Network_syncSeed
		{
			get
			{
				return _syncSeed;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref _syncSeed))
				{
					int syncSeed = _syncSeed;
					SetSyncVar(value, ref _syncSeed, 1uL);
				}
			}
		}

		public static event global::System.Action OnMapGenerated;

		private void Awake()
		{
			_singleton = this;
		}

		private void OnDestroy()
		{
			MapGenerated = false;
		}

		private void Start()
		{
			if (global::Mirror.NetworkServer.active)
			{
				int num = global::GameCore.ConfigFile.ServerConfig.GetInt("map_seed", -1);
				if (num < 1)
				{
					num = global::UnityEngine.Random.Range(1, int.MaxValue);
					DebugInfo("Server has successfully generated a random seed: " + num, MessageImportance.Normal);
				}
				else
				{
					DebugInfo("Server has successfully loaded a seed from config: " + num, MessageImportance.Normal);
				}
				Network_syncSeed = global::UnityEngine.Mathf.Clamp(num, 1, int.MaxValue);
			}
		}

		private void Update()
		{
			if (Seed <= 0 || MapGenerated || ReferenceHub.LocalHub == null)
			{
				return;
			}
			GenerateLevel();
			global::System.Collections.Generic.HashSet<global::MapGeneration.RoomIdentifier> hashSet = new global::System.Collections.Generic.HashSet<global::MapGeneration.RoomIdentifier>();
			foreach (global::MapGeneration.RoomIdentifier allRoomIdentifier in global::MapGeneration.RoomIdentifier.AllRoomIdentifiers)
			{
				if (allRoomIdentifier == null || !allRoomIdentifier.TryAssignId())
				{
					hashSet.Add(allRoomIdentifier);
				}
			}
			foreach (global::MapGeneration.RoomIdentifier item in hashSet)
			{
				global::MapGeneration.RoomIdentifier.AllRoomIdentifiers.Remove(item);
			}
			global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.MapGenerated);
			try
			{
				global::MapGeneration.SeedSynchronizer.OnMapGenerated();
			}
			catch (global::System.Exception ex)
			{
				global::UnityEngine.Debug.LogError("Failed to call the OnMapGenerated event, error: " + ex.Message);
				global::UnityEngine.Debug.LogError("List of methods that can cause this issue:");
				global::System.Delegate[] invocationList = global::MapGeneration.SeedSynchronizer.OnMapGenerated.GetInvocationList();
				foreach (global::System.Delegate obj in invocationList)
				{
					try
					{
						obj.Method.Invoke(obj.Target, null);
					}
					catch
					{
						global::UnityEngine.Debug.LogError("- " + obj.Method.Name);
					}
				}
			}
			MapGenerated = true;
			_stopwatch.Restart();
		}

		private void GenerateLevel()
		{
			for (int i = 0; i < global::MapGeneration.ImageGenerator.ZoneGenerators.Length; i++)
			{
				string text = MapAliases[i];
				if (global::MapGeneration.ImageGenerator.ZoneGenerators[i].GenerateMap(_syncSeed - i, text, out var blackbox))
				{
					DebugInfo(text + " generator tasks completed, no fatal errors to report.", MessageImportance.LessImportant);
				}
				else
				{
					DebugError(isFatal: true, text + " generator tasks failed, blackbox message: " + blackbox);
				}
			}
			DebugInfo("Sequence of procedural level generation completed.", MessageImportance.Normal);
			if (global::Mirror.NetworkServer.active)
			{
				global::MapGeneration.DoorSpawnpoint.SetupAllDoors();
			}
		}

		internal static void DebugInfo(string txt, MessageImportance importance, bool nospace = false)
		{
			global::GameCore.Console.AddDebugLog("MAPGEN", txt, importance, nospace);
		}

		internal static void DebugError(bool isFatal, string txt)
		{
			DebugInfo(string.Format(isFatal ? "<color=red>Fatal Error:</color> {0}" : "<color=orange>Warning:</color> {0}", txt), MessageImportance.MostImportant);
			global::UnityEngine.Debug.LogError("Map generation error for seed " + Seed + ": " + txt);
		}

		static SeedSynchronizer()
		{
			global::MapGeneration.SeedSynchronizer.OnMapGenerated = delegate
			{
			};
			_stopwatch = new global::System.Diagnostics.Stopwatch();
		}

		private void MirrorProcessed()
		{
		}

		public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
		{
			bool result = base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				global::Mirror.NetworkWriterExtensions.WriteInt32(writer, _syncSeed);
				return true;
			}
			global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 1L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteInt32(writer, _syncSeed);
				result = true;
			}
			return result;
		}

		public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				int syncSeed = _syncSeed;
				Network_syncSeed = global::Mirror.NetworkReaderExtensions.ReadInt32(reader);
				return;
			}
			long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
			if ((num & 1L) != 0L)
			{
				int syncSeed2 = _syncSeed;
				Network_syncSeed = global::Mirror.NetworkReaderExtensions.ReadInt32(reader);
			}
		}
	}
}
