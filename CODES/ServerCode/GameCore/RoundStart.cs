namespace GameCore
{
	public class RoundStart : global::Mirror.NetworkBehaviour
	{
		public static global::GameCore.RoundStart singleton;

		public static bool LobbyLock;

		private static bool _singletonSet;

		[global::Mirror.SyncVar]
		public short Timer = -2;

		internal static readonly global::System.Diagnostics.Stopwatch RoundStartTimer;

		public static bool RoundStarted
		{
			get
			{
				if (_singletonSet)
				{
					return singleton.Timer == -1;
				}
				return false;
			}
		}

		public static global::System.TimeSpan RoundLength => RoundStartTimer.Elapsed;

		public short NetworkTimer
		{
			get
			{
				return Timer;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref Timer))
				{
					short timer = Timer;
					SetSyncVar(value, ref Timer, 1uL);
				}
			}
		}

		static RoundStart()
		{
			RoundStartTimer = new global::System.Diagnostics.Stopwatch();
			global::UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
		}

		private static void OnSceneLoaded(global::UnityEngine.SceneManagement.Scene scene, global::UnityEngine.SceneManagement.LoadSceneMode mode)
		{
			RoundStartTimer.Reset();
		}

		private void Start()
		{
			GetComponent<global::UnityEngine.RectTransform>().localPosition = global::UnityEngine.Vector3.zero;
		}

		private void Awake()
		{
			singleton = this;
			_singletonSet = true;
		}

		private void OnDestroy()
		{
			_singletonSet = false;
		}

		private void Update()
		{
		}

		private void MirrorProcessed()
		{
		}

		public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
		{
			bool result = base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				global::Mirror.NetworkWriterExtensions.WriteInt16(writer, Timer);
				return true;
			}
			global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 1L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteInt16(writer, Timer);
				result = true;
			}
			return result;
		}

		public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				short timer = Timer;
				NetworkTimer = global::Mirror.NetworkReaderExtensions.ReadInt16(reader);
				return;
			}
			long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
			if ((num & 1L) != 0L)
			{
				short timer2 = Timer;
				NetworkTimer = global::Mirror.NetworkReaderExtensions.ReadInt16(reader);
			}
		}
	}
}
