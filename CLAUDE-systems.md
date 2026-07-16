<!-- Reference companion to CLAUDE.md. Read specific sections as needed. -->
# CLAUDE-systems.md — Detailed System Reference

## Unity Packages — Full Descriptions

| Package | Version | Purpose |
|---|---|---|
| `com.unity.ai.navigation` | 2.0.12 | NavMesh-based NPC/SCP navigation |
| `com.unity.nuget.newtonsoft-json` | 3.2.2 | JSON serialization |
| `com.unity.postprocessing` | 3.5.4 | Camera post-processing effects |
| `com.unity.ugui` | 2.0.0 | Legacy UI system (Canvas/Text) |
| `com.unity.multiplayer.center` | 1.0.1 | Multiplayer tools hub |
| `com.unity.ide.visualstudio` | 2.0.27 | Visual Studio IDE integration |

---

## Assembly Definitions — Full Descriptions

| Assembly | Location | Purpose |
|---|---|---|
| `Mirror` | `Assets/Mirror/Core/` | Core Mirror networking runtime |
| `Mirror.Transports` | `Assets/Mirror/Transports/` | All transports (KCP, Telepathy, SimpleWeb, Encryption) |
| `Mirror.Components` | `Assets/Mirror/Components/` | Mirror built-in components |
| `Mirror.Editor` | `Assets/Mirror/Editor/` | Mirror editor tooling |
| `Unity.Mirror.CodeGen` | `Assets/Mirror/Editor/Weaver/` | Mirror IL weaver (code generation) |
| `Mirror.CompilerSymbols` | `Assets/Mirror/CompilerSymbols/` | Compiler define symbols |
| `Mirror.Examples` | — | Mirror example scenes |
| `KCP` | `Assets/Mirror/Transports/KCP/kcp2k/` | KCP UDP transport protocol |
| `Telepathy` | `Assets/Mirror/Transports/Telepathy/` | TCP transport |
| `SimpleWebTransport` | `Assets/Mirror/Transports/SimpleWeb/` | WebSocket transport |
| `Edgegap` | `Assets/Mirror/Hosting/Edgegap/` | Edgegap cloud hosting integration |
| `EncryptionTransportEditor` | `Assets/Mirror/Transports/Encryption/Editor/` | Encryption transport editor |
| `sc.posteffects.runtime` | `Assets/SC Post Effects/Runtime/` | Post effects runtime |
| `sc.posteffects.editor` | `Assets/SC Post Effects/Editor/` | Post effects editor |
| `projectCloner` | `Assets/ParrelSync/` | ParrelSync multi-instance cloning |

---

## Third-Party Libraries — Full Descriptions

### DLLs in `Assets/Plugins/`
| Library | Purpose |
|---|---|
| `AmplifyBloom.dll` | High-quality bloom post-processing |
| `BouncyCastle.Crypto.dll` | RSA/ECDSA/AES/ECDH cryptography for authentication |
| `Caress.dll` | Unknown utility (likely audio or input) |
| `CommandSystem.Core.dll` | SCP:SL command system core |
| `Facepunch.Steamworks.Win64.dll` | Steamworks.NET — Steam lobby, authentication, tickets |
| `NorthwoodLib.dll` | Northwood utility library (string utils, pools, etc.) |
| `YamlDotNet.dll` | YAML config file parsing |
| `zxing.unity.dll` | QR code generation (used in Remote Admin player info) |

### In-Source Libraries — Full Details
- **MEC (More Effective Coroutines)** — In `Assets/Plugins/Assembly-CSharp-firstpass/MEC/` — used throughout for coroutines (`Timing.RunCoroutine`, `Timing.CallDelayed`)
- **Utf8Json** — In `Assets/Scripts/Utf8Json/` — high-performance JSON serialization
- **Discord SDK** — In `Assets/Plugins/Assembly-CSharp-firstpass/Discord/` — Discord integration (partially implemented)
- **CameraFilterPack** — In `CODES/ClientCode/` — extensive camera shader/filter collection (200+ effects)
- **UBER Shader** — In `Assets/UBER/` — advanced surface shaders

---

## Core Systems — Detailed Descriptions

### 1. Network Manager — `CustomNetworkManager`
**File:** `Assets/Scripts/Assembly-CSharp/CustomNetworkManager.cs`
Extends `LiteNetLib4MirrorNetworkManager`. Responsibilities:
- Server startup sequence (`CreateMatch()`, `_CreateLobby()` coroutine)
- Client connect/disconnect handling with localized error messages
- Rate limiting windows for IP, UserID, preauth challenges
- Config loading (`ConfigFile.ReloadGameConfigs()`)
- Fast restart support (`EnableFastRestart`, `FastRestartDelay`)
- Reconnection logic
- Static singleton: `CustomNetworkManager.TypedSingleton`

### 2. Player Hub — `ReferenceHub`
**File:** `Assets/Scripts/Assembly-CSharp/ReferenceHub.cs`
The "god object" for players. Every player GameObject has this component. Aggregates:
- `CharacterClassManager` — authentication, instance mode, user ID sync
- `PlayerRoleManager` — current role management
- `PlayerStats` — health, stamina, AHP, hume shield stats
- `Inventory` — item management
- `ServerRoles` — permissions and badges
- `QueryProcessor` — Remote Admin queries
- `NicknameSync` — player name sync
- `PlayerInteract` — interaction system
- `PlayerEffectsController` — status effects
- `HintDisplay` — HUD hints
- `FriendlyFireHandler` — FF tracking

### 3. Role System — `PlayerRoleManager` / `PlayerRoleBase`
**Files:** `Assets/Scripts/Assembly-CSharp/PlayerRoles/`
- `PlayerRoleManager` (NetworkBehaviour) manages the player's current role
- `PlayerRoleBase` — abstract base for all roles
- `HumanRole` — base for all human roles
- `RoleTypeId` enum — all role identifiers
- Events: `OnServerRoleSet`, `RoleChanged`

**Playable SCP Roles:**
| SCP | Key Classes |
|---|---|
| SCP-049 | `Scp049Role`, `Scp049AttackAbility`, `Scp049ResurrectAbility`, `Scp049SenseAbility`, `Scp049CallAbility` |
| SCP-079 | `Scp079Role`, `Scp079AuxManager`, `Scp079TierManager`, camera/door/tesla/speaker/elevator abilities |
| SCP-096 | `Scp096Role`, `Scp096RageManager`, `Scp096ChargeAbility`, `Scp096AttackAbility`, `Scp096TargetsTracker` |
| SCP-106 | `Scp106Role`, `Scp106StalkAbility`, `Scp106HuntersAtlasAbility`, `Scp106SinkholeController` |
| SCP-173 | `Scp173Role`, `Scp173SnapAbility`, `Scp173BreakneckSpeedsAbility`, `Scp173TantrumAbility`, `Scp173ObserversTracker` |
| SCP-939 | `Scp939Role`, `Scp939LungeAbility`, `Scp939ClawAbility`, `Scp939AmnesticCloudAbility`, `Scp939FocusAbility` |

### 4. Stats System — `PlayerStats`
**Files:** `Assets/Scripts/Assembly-CSharp/PlayerStatsSystem/`
Modular stat system with defined stat modules:
- `HealthStat` — HP
- `AhpStat` — Artificial HP (temporary shield)
- `StaminaStat` — Sprint stamina
- `AdminFlagsStat` — Server-side admin flags
- `HumeShieldStat` — SCP hume shield

Damage handlers per damage type: `FirearmDamageHandler`, `ExplosionDamageHandler`, `Scp049DamageHandler`, `Scp096DamageHandler`, `WarheadDamageHandler`, `UniversalDamageHandler`, etc.

Events: `PlayerStats.OnAnyPlayerDamaged`, `PlayerStats.OnAnyPlayerDied`

### 5. Inventory System — `Inventory`
**Files:** `Assets/Scripts/Assembly-CSharp/InventorySystem/`
- `Inventory` (NetworkBehaviour) — per-player inventory
- `ItemBase` — base class for all items
- `ItemType` enum — all item identifiers
- Item categories: Firearms, Armor, Keycards, Usables (Medkits, SCP items), Throwables, Jailbird, MicroHID, Radio, Coin, Flashlight

**Firearms subsystem** (`InventorySystem/Items/Firearms/`):
- `Firearm` base class with modular attachments system
- Specific implementations: `AutomaticFirearm`, `Revolver`, `Shotgun`, `Com45`, `ParticleDisruptor`
- Audio: `FirearmAudioManager`
- Viewmodels: per-weapon animated viewmodel classes

### 6. Authentication — `CentralAuthManager` / `CharacterClassManager`
**Files:** `Assets/Scripts/Assembly-CSharp/CentralAuth*.cs`, `CharacterClassManager.cs`
- Runs on a background thread (`_authThread`)
- Supports Steam and Discord distribution platforms (`DistributionPlatform`)
- ECDSA/RSA key-based token authentication with the Northwood central server
- `CentralAuthPreauthToken` — token sent during connection preauth
- Challenge-response system in `CustomLiteNetLib4MirrorTransport`
- `CharacterClassManager` holds `UserId`, `AuthToken`, `RequestIp`, instance mode

### 7. Map Generation — `MapGeneration`
**Files:** `Assets/Scripts/Assembly-CSharp/MapGeneration/`
- `SeedSynchronizer` (NetworkBehaviour) — syncs the map seed across clients (`[SyncVar] int _syncSeed`)
- `RoomIdentifier` — tags each room with `RoomName`, `FacilityZone`, `RoomShape`
- `RoomName` and `FacilityZone` enums define all rooms (LCZ, HCZ, EZ)
- `SeedSynchronizer.OnMapGenerated` event fires when generation completes

### 8. Round Management
- `RoundStart` (NetworkBehaviour) — lobby timer, lobby lock, player count display. `RoundStart.singleton`
- `RoundSummary` (NetworkBehaviour) — end-of-round tracking, win condition (`LeadingTeam` enum: FacilityForces, ChaosInsurgency, Anomalies, Draw), class counts
- `RoundSummary.RoundLock` — prevents round from ending

### 9. Respawn System — `RespawnManager`
**Files:** `Assets/Scripts/Assembly-CSharp/Respawning/`
- `RespawnManager` (MonoBehaviour singleton) — manages wave respawn sequences
- Phases: `RespawnCooldown → SelectingTeam → PlayingEntryAnimations → SpawningSelectedTeam`
- Two spawnable teams: `ChaosInsurgency` (handled by `ChaosInsurgencySpawnHandler`) and `NineTailedFox` (handled by `NineTailedFoxSpawnHandler`)
- Token system: `RespawnTokensManager` tracks respawn points

### 10. Alpha Warhead — `AlphaWarheadController`
**File:** `Assets/Scripts/Assembly-CSharp/AlphaWarheadController.cs`
NetworkBehaviour. Manages the facility nuke: armed state, countdown, detonation, blast doors, auto-detonate. Uses `AlphaWarheadSyncInfo` SyncVar.

### 11. Decontamination — `DecontaminationController`
**Files:** `Assets/Scripts/Assembly-CSharp/LightContainmentZoneDecontamination/`
LCZ decontamination sequence controller with gas, doors, and client timers.

### 12. SCP-914 — `Scp914Controller`
**Files:** `Assets/Scripts/Assembly-CSharp/Scp914/`
Item upgrading machine. Knob settings and upgrade processors per item type.

### 13. Interactables / Doors — `Interactables`
**Files:** `Assets/Scripts/Assembly-CSharp/Interactables/`
- `IInteractable`, `IClientInteractable`, `IServerInteractable` — interaction interfaces
- `InteractionCoordinator` — per-player interaction coordinator
- Door types: `BasicDoor`, `BreakableDoor`, `CheckpointDoor`, `ElevatorDoor`, `PryableDoor`, `AirlockController`
- `ElevatorManager`, `ElevatorChamber`, `ElevatorPanel` — elevator system

### 14. Player Effects — `PlayerEffectsController` / `CustomPlayerEffects`
**Files:** `Assets/Scripts/Assembly-CSharp/CustomPlayerEffects/`
Status effect system with individual effect classes: `Bleeding`, `Blinded`, `Burned`, `Concussed`, `Corroding`, `Deafened`, `Decontaminating`, `Asphyxiated`, `CardiacArrest`, `AmnesiaVision`, `AmnesiaItems`, etc.

### 15. Voice Chat — `VoiceChat`
**Files:** `Assets/Scripts/Assembly-CSharp/VoiceChat/`
- `VoiceChatMicCapture` — microphone capture
- `VoiceChatChannel` — channel types (proximity, intercom, SCP, radio, etc.)
- `VcMuteFlags`, `VcPrivacyFlags` — mute/privacy state
- Codec and playback subsystems

### 16. Remote Admin — `RemoteAdmin`
**Files:** `Assets/Scripts/Assembly-CSharp/RemoteAdmin/`
- `QueryProcessor` — per-player RA query handler
- `CommandProcessor` — command dispatch
- `RemoteAdminCryptographicManager` — encrypted RA communication
- `TextBasedRemoteAdmin` — text-based RA interface
- `PlayerCommandSender`, `ServerConsoleSender`, `ConsoleCommandSender` — ICommandSender implementations

### 17. Command System
**Files:** `Assets/Scripts/Assembly-CSharp/CommandSystem/`
- `ClientCommandHandler`, `RemoteAdminCommandHandler`, `GameConsoleCommandHandler`
- Commands are in `CommandSystem/Commands/`
- `CommandSystem.Core.dll` (precompiled) provides the base framework

### 18. Config System — `ConfigFile` / `YamlConfig`
**Files:** `Assets/Scripts/Assembly-CSharp/GameCore/ConfigFile.cs`, `YamlConfig.cs`
- YAML-based configuration using `YamlDotNet`
- Key config files: `config_gameplay.txt`, `config_remoteadmin.txt`, `hoster_policy.txt`
- `ConfigFile.ServerConfig` — main server config
- `ConfigFile.HosterPolicy` — hosting provider policy overrides
- `ConfigSharing` — cross-server config sharing

### 19. Security / Rate Limiting — `Security`
**Files:** `Assets/Scripts/Assembly-CSharp/Security/`
- `RateLimit`, `DummyRateLimit`, `ServerRateLimit` — connection and command rate limiting
- `PlayerRateLimitHandler` — per-player rate limit enforcement
- `RateLimitCreator` — loads rate limit configs

### 20. Cryptography — `Cryptography`
**Files:** `Assets/Scripts/Assembly-CSharp/Cryptography/`
Wrappers around BouncyCastle: `AES`, `ECDH`, `ECDSA`, `RSA`, `Sha`, `Md`, `PBKDF2`
Used for authentication token signing and Remote Admin encrypted communication.

### 21. Friendly Fire System
- `FriendlyFireHandler` — per-player FF tracking
- `FriendlyFireDetector` (and subclasses: `LifeFriendlyFireDetector`, `WindowFriendlyFireDetector`, `RoundFriendlyFireDetector`, `RespawnFriendlyFireDetector`) — detect and log FF incidents
- `FriendlyFireConfig` — FF configuration

### 22. Hazards
**Files:** `Assets/Scripts/Assembly-CSharp/Hazards/`
- `EnvironmentalHazard` — base class
- `TantrumEnvironmentalHazard` — SCP-173 tantrum hazard
- `SinkholeEnvironmentalHazard` — SCP-106 sinkhole

### 23. GameObjectPools
**Files:** `Assets/Scripts/Assembly-CSharp/GameObjectPools/`
- `PoolManager`, `Pool<T>` — generic object pool system
- `IPoolResettable`, `IPoolSpawnable` — pool interfaces

### 24. Server Console — `ServerConsole`
**File:** `Assets/Scripts/Assembly-CSharp/ServerConsole.cs`
Static singleton MonoBehaviour. Manages:
- Server registration with Northwood central servers
- Public key fetching and caching
- Log output, whitelist, friendly fire config flags
- `ServerConsole.Ip`, `ServerConsole.PublicKey`

---

## Key MonoBehaviour Classes — Full Role Descriptions

| Class | Type | Role |
|---|---|---|
| `CustomNetworkManager` | NetworkManager | Server/client lifecycle |
| `ReferenceHub` | NetworkBehaviour | Central player component aggregator |
| `CharacterClassManager` | NetworkBehaviour | Player authentication and instance mode |
| `PlayerRoleManager` | NetworkBehaviour | Player role assignment |
| `PlayerStats` | NetworkBehaviour | Player health/stats |
| `Inventory` | NetworkBehaviour | Player inventory |
| `RoundStart` | NetworkBehaviour | Lobby/round start timer |
| `RoundSummary` | NetworkBehaviour | Win condition tracking |
| `AlphaWarheadController` | NetworkBehaviour | Nuke system |
| `RespawnManager` | MonoBehaviour | Team wave respawning |
| `DecontaminationController` | NetworkBehaviour | LCZ decontamination |
| `Scp914Controller` | NetworkBehaviour | SCP-914 upgrade machine |
| `TeslaGateController` | NetworkBehaviour | Tesla gate management |
| `SeedSynchronizer` | NetworkBehaviour | Map generation seed sync |
| `ServerConsole` | MonoBehaviour | Server management singleton |
| `SteamLobby` | MonoBehaviour | Steam lobby management |

---

## Asset-Driven Data — Full Details

The project does **not** use ScriptableObjects for core gameplay data (weapons, SCPs, spawning). All per-role and per-weapon numeric values are hardcoded as `const` or `[SerializeField]` fields on MonoBehaviour/NetworkBehaviour components. However, several ScriptableObjects drive visual, audio, and configuration concerns:

| Class | File Path | Configures |
|---|---|---|
| `FirearmGlobalSettingsPreset` | `InventorySystem/Items/Firearms/FirearmGlobalSettingsPreset.cs` | Global firearm accuracy curves: running inaccuracy multiplier, jump inaccuracy, ADS movement speed curve, weight-to-stamina and weight-to-movement-speed curves |
| `ReflexSightReticlePack` | `InventorySystem/Items/Firearms/Attachments/Components/ReflexSightReticlePack.cs` | Array of reticle textures for reflex sight attachments on firearms |
| `CharacterControllerSettingsPreset` | `PlayerRoles/FirstPersonControl/CharacterControllerSettingsPreset.cs` | `CharacterController` physics parameters (slope limit, step offset, skin width, radius, height) applied per-role |
| `ModelSharedSettings` | `PlayerRoles/FirstPersonControl/Thirdperson/ModelSharedSettings.cs` | Head-bob animation curves (walk/strafe horizontal and vertical), landing animation curve, fall damage sound |
| `SpawnablesDistributorSettings` | `MapGeneration/Distributors/SpawnablesDistributorSettings.cs` | Map spawner parameters: spawner delay, unfreeze delay, arrays of `SpawnableItem` and `SpawnableStructure`; loaded at runtime via `Resources.LoadAll<SpawnablesDistributorSettings>()` |
| `DoorAudioSettings` | `DoorAudioSettings.cs` | Per-door-type audio clips: opening sounds, closing sounds, access-denied, access-granted |
| `PanelVisualSettings` | `Interactables/Interobjects/DoorUtils/PanelVisualSettings.cs` | Door keypad panel materials (open, closed, moving, error, denied) and localisation translation IDs for panel text |
| `RadialMenuSettings` | `RadialMenus/RadialMenuSettings.cs` | Radial menu UI sprites: main ring sprites and highlight template sprites |
| `ToggleColorPreset` | `RemoteAdmin/Presets/ToggleColorPreset.cs` | Remote Admin UI toggle button `ColorBlock` for selected/unselected states |
| `HeadlessRuntime` | `HeadlessRuntime.cs` | Dedicated server headless profile: profile name, framerate limit value, bool flags for camera and console |

**Non-ScriptableObject data-driven systems:**

- **YAML config** — all gameplay tuning (lobby time, respawn intervals, max wave sizes, decontamination enable/disable, map seed) is read at runtime from `config_gameplay.txt` via `ConfigFile.ServerConfig.GetBool/GetInt/GetFloat/GetString`. Parsed by `YamlDotNet`.
- **Respawn wave sizes** — hardcoded in `RespawnManager.SpawnableTeams` dictionary: CI default max 15, initial tokens 18; MTF default max 15, initial tokens 24. Config keys `maximum_CI_respawn_amount` / `maximum_MTF_respawn_amount` and `respawn_tickets_ci_initial_count` / `respawn_tickets_mtf_initial_count` can override these.
- **Damage values** — constants on ability classes (e.g., `Scp096AttackAbility.HumanDamage = 85f`, `DoorDamage = 250f`, `WindowDamage = 500`) — not asset-driven.
- **Decontamination phases** — configured as a `DecontaminationPhase[]` array on the `DecontaminationController` prefab in the Inspector (not a ScriptableObject), with `AudioClip` references and `PhaseFunction` enum values per phase.
