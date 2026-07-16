namespace PlayerRoles.FirstPersonControl
{
	public class FpcNoclip
	{
		public static float CurSpeed = 10f;

		private const float DefaultNoclipSpeed = 10f;

		private const float MinNoclipSpeed = 0.1f;

		private const float MaxNoclipSpeed = 250f;

		private const float NoclipLerp = 16f;

		private const float NoclipMaxDiffSqrt = 25f;

		private const float RecentTimeThreshold = 2.5f;

		private const string AxisName = "Mouse ScrollWheel";

		private bool _wasEnabled;

		private readonly ReferenceHub _hub;

		private readonly global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule _fpmm;

		private readonly global::PlayerStatsSystem.AdminFlagsStat _stats;

		private readonly global::System.Diagnostics.Stopwatch _lastNcSw;

		private static readonly global::System.Collections.Generic.HashSet<uint> PermittedPlayers = new global::System.Collections.Generic.HashSet<uint>();

		private static global::UnityEngine.KeyCode _keyFwd;

		private static global::UnityEngine.KeyCode _keyBwd;

		private static global::UnityEngine.KeyCode _keyLft;

		private static global::UnityEngine.KeyCode _keyRgt;

		private static global::UnityEngine.KeyCode _keyUpw;

		private static global::UnityEngine.KeyCode _keyDnw;

		private static global::UnityEngine.KeyCode _keyToggle;

		private static global::UnityEngine.KeyCode _keyFog;

		public bool IsActive
		{
			get
			{
				return _stats.HasFlag(global::PlayerStatsSystem.AdminFlags.Noclip);
			}
			set
			{
				_stats.SetFlag(global::PlayerStatsSystem.AdminFlags.Noclip, value);
			}
		}

		public bool RecentlyActive
		{
			get
			{
				if (_lastNcSw.IsRunning)
				{
					return _lastNcSw.Elapsed.TotalSeconds < 2.5;
				}
				return false;
			}
		}

		public FpcNoclip(ReferenceHub hub, global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule fpmm)
		{
			_hub = hub;
			_fpmm = fpmm;
			_stats = hub.playerStats.GetModule<global::PlayerStatsSystem.AdminFlagsStat>();
			_lastNcSw = new global::System.Diagnostics.Stopwatch();
			if (_hub.isLocalPlayer)
			{
				ReloadInputConfigs();
			}
		}

		public void UpdateNoclip()
		{
			if (_hub.isLocalPlayer && global::UnityEngine.Input.GetKeyDown(_keyToggle))
			{
				global::Mirror.NetworkClient.Send(default(global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcNoclipToggleMessage));
			}
			if (!_stats.HasFlag(global::PlayerStatsSystem.AdminFlags.Noclip))
			{
				if (_wasEnabled)
				{
					DisableNoclipClientside();
				}
				_wasEnabled = false;
				return;
			}
			_wasEnabled = true;
			_lastNcSw.Restart();
			if (global::Mirror.NetworkServer.active)
			{
				_fpmm.Motor.ResetFallDamageCooldown();
			}
			if (!_hub.isLocalPlayer)
			{
				global::UnityEngine.Vector3 position = _fpmm.Motor.ReceivedPosition.Position;
				float t = (((position - _fpmm.Position).sqrMagnitude > 25f) ? 1f : (16f * global::UnityEngine.Time.deltaTime));
				_fpmm.Position = global::UnityEngine.Vector3.Lerp(_fpmm.Position, position, t);
			}
		}

		public void ShutdownModule()
		{
			if (global::Mirror.NetworkServer.active)
			{
				IsActive = false;
			}
			DisableNoclipClientside();
		}

		private void DisableNoclipClientside()
		{
		}

		public static bool IsPermitted(ReferenceHub ply)
		{
			if (ply != null)
			{
				return PermittedPlayers.Contains(ply.netId);
			}
			return false;
		}

		public static void PermitPlayer(ReferenceHub ply)
		{
			if (!(ply == null))
			{
				PermittedPlayers.Add(ply.netId);
				ply.gameConsoleTransmission.SendToClient("Noclip is now permitted.", "green");
			}
		}

		public static void UnpermitPlayer(ReferenceHub ply)
		{
			if (!(ply == null))
			{
				PermittedPlayers.Remove(ply.netId);
				ply.playerStats.GetModule<global::PlayerStatsSystem.AdminFlagsStat>().SetFlag(global::PlayerStatsSystem.AdminFlags.Noclip, status: false);
				ply.gameConsoleTransmission.SendToClient("Noclip permission revoked.", "yellow");
			}
		}

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			global::InventorySystem.Inventory.OnServerStarted += PermittedPlayers.Clear;
		}

		private static void ReloadInputConfigs()
		{
		}
	}
}
