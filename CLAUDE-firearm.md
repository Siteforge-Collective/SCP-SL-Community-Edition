<!-- Reference companion to CLAUDE.md. Read when working on the weapon/inventory/animation system. -->
# CLAUDE-firearm.md — Firearm System Reference

This file documents the complete weapon system architecture, all bug fixes applied across sessions 2–6, and key invariants discovered. **Always read this before touching any firearm, inventory, or first-person animation code.**

---

## Firearm Class Hierarchy

```
ItemBase
└── Firearm (NetworkBehaviour, abstract)
    ├── AutomaticFirearm          — standard rifles/pistols (most weapons)
    ├── Disruptor                 — SCP-127 / special disruptor
    └── (other firearm types)
```

Every Firearm has **modules** set in `OnAdded()`:
- `IAmmoManagerModule` — manages reload/unload state (`AutomaticAmmoManager`, `TubularMagazineAmmoManager`, `ClipLoadedInternalMagAmmoManager`)
- `IEquipperModule` — controls when shots are allowed after equip (`EventBasedEquipper`, `ChambercheckEquipper`)
- `IActionModule` — handles trigger logic, shot authorization (`AutomaticAction`, `PumpAction`, `DoubleAction`)
- `IInspectorModule` — inspect animation (`SimpleInspector`)
- `IAdsModule` — aim-down-sights (`StandardAds`)
- `IHitregModule` — hit registration (`SingleBulletHitreg`, `MultiShotHitreg`, `BuckshotHitreg`)

---

## Key Files

| File | Purpose |
|---|---|
| `InventorySystem/Items/Firearms/Firearm.cs` | Base class, `HasViewmodel` cache, `EquipUpdate()` → `UpdateAnims()` every frame, `AnimSetFloat/Bool/Int/Trigger` extension via `FirearmExtensions.cs` |
| `InventorySystem/Items/Firearms/AutomaticFirearm.cs` | Most weapons. `OnAdded()` creates all modules. `UpdateAnims()` sets all animator params. |
| `InventorySystem/Items/Firearms/FirearmExtensions.cs` | Static extensions: `AnimSetFloat`, `AnimSetBool`, `AnimSetInt`, `AnimSetTrigger` — route to server animator AND viewmodel animator |
| `InventorySystem/Items/Firearms/FirearmAnimatorHashes.cs` | All animator param hashes (DrawSpeedMultiplier, Fire, Reload, etc.) |
| `InventorySystem/Items/Firearms/FirearmBasicMessagesHandler.cs` | Shot/reload network message routing |
| `InventorySystem/Items/Firearms/AutomaticFirearmAnimatorEvents.cs` | Animation events: `MarkAsEquipped`, `InsertMagazine`, `RemoveMagazine`, `UseChargingHandle`, `SetGripBlendSpeed` |
| `InventorySystem/Items/Firearms/ShotgunAnimatorEvents.cs` | Animation events: `MarkAsEquipped`, `InsertShell`, `Pump`, `UnloadShells` |
| `InventorySystem/Items/Firearms/DisruptorAnimatorEvents.cs` | Animation events: `MarkAsEquipped`, `SetMagazine`, `SetCocked`, `SetChambered` |
| `InventorySystem/Items/AnimatedViewmodelBase.cs` | Base for all animated viewmodels. Holds `_animator` (SerializeField), mirrors all anim calls to SharedHandsController.Hands |
| `InventorySystem/Items/Firearms/AnimatedFirearmViewmodel.cs` | Firearm viewmodel base. `InitAny()` wires events. `OnEquipped()` → `UpdateAnims()`. `InitSpectator()` complex setup. |
| `InventorySystem/Items/Firearms/ClosedBoltFirearmViewmodel.cs` | CrossVEC viewmodel. `LateUpdate()` drives BoltCatch/NoMag/Trigger/FullAuto layer weights. |
| `InventorySystem/Items/SharedHandsController.cs` | Scene singleton. `Hands` Animator (Hands Rig). `UpdateInstance()` sets avatar/controller at runtime. `SetRoleGloves()` swaps arm materials/meshes by role. |
| `InventorySystem/Items/Firearms/Modules/EventBasedEquipper.cs` | Equip readiness: `OnEquipped()` resets, `Equip()` (called from animation event) starts 0.1s timer, `Standby` checks it |
| `InventorySystem/Items/Firearms/Modules/ChambercheckEquipper.cs` | Time-based equip readiness using a configurable draw time |
| `InventorySystem/Items/Firearms/Modules/AutomaticAmmoManager.cs` | Reload management for standard magazines |
| `InventorySystem/Items/Firearms/Modules/TubularMagazineAmmoManager.cs` | Shotgun shell-by-shell reload |
| `InventorySystem/Items/Firearms/Modules/ClipLoadedInternalMagAmmoManager.cs` | Internal-mag weapons |
| `InventorySystem/Items/Firearms/Modules/AutomaticAction.cs` | Full/semi auto trigger logic |
| `InventorySystem/Items/Firearms/Modules/PumpAction.cs` | Shotgun pump logic, chambered rounds tracking |
| `InventorySystem/Items/Firearms/Modules/DoubleAction.cs` | Revolver double-action logic, hammer cock |
| `InventorySystem/Items/Firearms/Modules/SingleBulletHitreg.cs` | Standard hitreg |
| `InventorySystem/Items/Firearms/Modules/BuckshotHitreg.cs` | Shotgun spread hitreg |
| `InventorySystem/Inventory.cs` | `CurInstance` setter: activates viewmodel, calls `UpdateInstance`, calls `OnEquipped`. `CreateItemInstance()` instantiates item + viewmodel. `EquipUpdate()` called every frame for equipped items. |
| `InventorySystem/Items/Firearms/Attachments/AttachmentsUtils.cs` | `AttachmentsValue()` — reads attachment params. `DrawSpeedMultiplier` uses `ParameterMixingMode.Percent` → `DefaultValue = 1.0` (not 0!). |

---

## Shot Authorization Flow (full chain)

```
Client presses fire key
→ Firearm.EquipUpdate() → UpdateKeys() → ActionModule.DoClientsideAction(triggered)
→ ActionModuleResponse.Shoot
→ HitregModule.ClientCalculateHit(out ShotMessage)
→ NetworkClient.Send(ShotMessage)
→ OnWeaponShot() → OnShotCalled event → AnimatedFirearmViewmodel.OnShot() → AnimatorSetTrigger(Fire)

Server receives ShotMessage
→ FirearmBasicMessagesHandler.ServerReceiveShot()
→ ActionModule.ServerAuthorizeShot() checks:
   - EquipperModule.Standby (must be true)
   - AmmoManagerModule.Standby (must be true)
   - Ammo > 0
→ HitregModule.ServerProcessHit()
→ DealDamage on victims
→ ServerSendAudioMessage(clipId)
→ firearm.ServerSideAnimator.Play(Fire)
```

**Local player shots are BLOCKED until `EquipperModule.Standby = true`.**
For `EventBasedEquipper`: `Standby` becomes true 0.1s after `Equip()` is called from the `MarkAsEquipped` animation event.

---

## Equip Flow (CurInstance setter in Inventory.cs)

```csharp
// 1. Activate viewmodel
_curInstance.ViewModel.gameObject.SetActive(true);
// 2. Wire Hands Rig to viewmodel's avatar/controller
SharedHandsController.UpdateInstance(_curInstance.ViewModel);
// 3. Viewmodel-side equip (sets DrawSpeedMultiplier, initializes sway, UpdateAttachments)
_curInstance.ViewModel.OnEquipped();
// 4. Firearm-side equip (UpdateAnims again if IsLocalPlayer, resets trigger state)
_curInstance.OnEquipped();
// 5. Mark as equipped → EquipUpdate() called every frame from now on
_curInstance.IsEquipped = true;
```

`CreateItemInstance(id, updateViewmodel: true)`:
1. Instantiates Firearm prefab
2. Instantiates viewmodel prefab as child of Firearm
3. Calls `viewmodel.InitLocal(itemBase)` → `InitAny()` (wires `_fa`, subscribes events, sets `_fa.ViewModel = this`)
4. Calls `viewmodel.gameObject.SetActive(false)` — viewmodel starts INACTIVE

---

## Animator Architecture

### Two Animators Run In Sync

| Animator | Location | Drives |
|---|---|---|
| Viewmodel Animator | On "Anims" child inside CrossvecViewmodel prefab | Weapon bone transforms (MeshRenderer parts on weapon) |
| Hands Rig Animator | "Hands Rig" GameObject in Facility scene | Arm/sleeve bone transforms → SkinnedMeshRenderers |

**`AnimatedViewmodelBase.AnimatorSetFloat/Bool/Trigger/etc.`** always mirrors calls to BOTH animators simultaneously.

### SharedHandsController.UpdateInstance() Call Order
```csharp
Hands.gameObject.SetActive(true);
Hands.avatar = animatedIvb.AnimatorAvatar;           // From viewmodel's _animator
Hands.runtimeAnimatorController = animatedIvb.AnimatorRuntimeController;
Singleton._trackedPosition = animatedIvb.AnimatorTransform;  // "Anims" transform
Hands.Rebind();  // RESETS all parameters to defaults
// → then OnEquipped() must set DrawSpeedMultiplier = 1.0
```

### CrossvecAnimator.controller — Key Parameters
| Parameter | Type | Default | Used For |
|---|---|---|---|
| `DrawSpeedMultiplier` | float | **0** | draw_stock state speed (0 = frozen, 1 = plays at 1.2x) |
| `ReloadSpeedMultiplier` | float | 1 | reload animation speed |
| `Fire` | trigger | — | fire animation |
| `Reload` | trigger | — | reload animation |
| `Unload` | trigger | — | unload animation |
| `IsMagInserted` | bool | — | NoMag layer weight |
| `IsChambered` | bool | — | BoltCatch layer weight |
| `IsCocked` | bool | — | cock state display |
| `Ammo` | int | — | ammo display |
| `GripBlend` | float | — | grip style blend |
| `AdsCurrent` | float | — | ADS blend |

**Critical:** `draw_stock` state has `m_SpeedParameterActive: 1` → controlled by `DrawSpeedMultiplier`. At default=0, animation is FROZEN. `UpdateAnims()` sets it to 1.0 via `AttachmentsUtils.AttachmentsValue(DrawSpeedMultiplier)` which starts from `DefaultValue = 1`.

### CrossvecViewmodel Prefab Hierarchy
```
CrossvecViewmodel (ClosedBoltFirearmViewmodel, _animator → Anims)
└── PivotPoint
    └── Anims (Animator: CrossvecAnimator + CROSSVEC_RIGAvatar, AutomaticFirearmAnimatorEvents)
        └── Armature (rotation -90°, scale 100x)
            ├── camera
            ├── hand.L (+ finger bones)
            ├── hand.R (+ finger bones)
            ├── pole.L
            ├── pole.R
            └── root
                ├── arm.L → forearm.L
                ├── arm.R → forearm.R
                └── main (weapon body + weapon attachments)
```
No SkinnedMeshRenderer in this prefab. Weapon parts are regular MeshRenderers on bone transforms.

### Hands Rig Hierarchy (Facility scene)
```
Hands Rig (Animator: no avatar/controller in Inspector, set at runtime)
├── Armature (rotation -90°, scale 100x) — matches CrossvecViewmodel Armature
│   ├── camera
│   ├── hand.L ... hand.R
│   ├── pole.L ... pole.R
│   └── root
│       ├── arm.L → forearm.L
│       ├── arm.R → forearm.R
│       └── main
└── Chaos Hands (inactive by default)
    ├── Hands Chaos L (SkinnedMeshRenderer, bones = Hands Rig bones)
    ├── Hands Chaos R (SkinnedMeshRenderer)
    ├── Glove Chaos L
    └── Glove Chaos R
```
**SkinnedMeshRenderers reference Hands Rig bone transforms directly.**
`SharedHandsController._sleevesRenderers` = array of Renderer refs for arm mesh (material-only swap).
`SharedHandsController._glovesForRole` = role-specific glove GameObjects (SetActive by role).
`SharedHandsController._defaultGloves` = shown when no role-specific match.

---

## All Bug Fixes Applied (Sessions 2–6)

### DO NOT RE-APPLY — already in code

#### Session 2–3 (AutomaticAction, AutomaticAmmoManager, TubularMagazineAmmoManager)
- `AutomaticAction.cs` — various shot authorization and trigger logic fixes
- `TubularMagazineAmmoManager.cs` — reload shell-by-shell fixes
- `AutomaticAmmoManager.cs` — general reload/unload flow fixes
- `AmmoPickup.cs`, `AmmoSearchCompletor.cs` — pickup quantity fixes
- `FirearmBasicMessagesHandler.cs` — message routing fixes
- `BuckshotHitreg.cs` — shotgun spread pattern fixes

#### Session 3–4 (Pickup system)
- `SearchSessionPipe.cs`, `ItemSearchCompletor.cs`, `ArmorSearchCompletor.cs`
- `Scp244SearchCompletor.cs`, `Scp330SearchCompletor.cs`
- `CollisionDetectionPickup.cs`, `SearchCoordinator.cs`, `SearchCompletor.cs`

#### Session 4 (Jailbird, MicroHID)
- `JailbirdHitreg.cs` — infinite loop fix in hit detection
- `JailbirdItem.cs` — AttackPerformed charge/melee logic
- `MicroHIDHandler.cs` — wrong State sync field

#### Session 5 (Animation system — CRITICAL)
- `AnimatedViewmodelBase.cs`:
  - `SkipEquipTime` → use `Hub.inventory.LastItemSwitch` (not `CurrentRole.ActiveTime`)
  - `InitAny()` → fallback `GetComponent<Animator>()` + LogError if null
  - `AnimatorForceUpdate()` no-args → `fastMode=true`
- `AutomaticFirearmAnimatorEvents.cs`, `ShotgunAnimatorEvents.cs`, `DisruptorAnimatorEvents.cs`:
  - `MarkAsEquipped()` → `if (firearm.IsLocalPlayer)` only (was wrong inverse condition that blocked Equip() for local player)
- `EventBasedEquipper.cs`:
  - `Standby` → `if (!_firearm.IsLocalPlayer) return true` at top (remote players always ready on server)
- `AnimatedFirearmViewmodel.cs`:
  - `InitAny()` → `GetComponentInChildren<FirearmAnimatorEventsBase>(true).InitializeFirearm(_fa)`

#### Session 6 (Busy reset, spectator double-init)
- `AutomaticAmmoManager.cs` → `EquipUpdate()`: local player branch now `shouldReset = true` (no animator state check)
- `ClipLoadedInternalMagAmmoManager.cs` → `EquipUpdate()`: same fix, `isIdle = true` for local player
- `AnimatedFirearmViewmodel.cs` → `InitSpectator()`: removed redundant `InitAny()` call (was called twice → double event subscriptions → double sounds for spectators)

---

## Key Invariants

### HasViewmodel Caching
`Firearm.HasViewmodel` caches `_hasViewmodel` on first access. It's safe because `InitAny()` → `_fa.ViewModel = this` runs inside `CreateItemInstance()` before any code checks `HasViewmodel`.

### DoubleAction Audio Offset
`DoubleAction.ClientPlaySound(int index)` uses `AudioClips[index + 2]`. **INTENTIONAL design:**
- `AudioClips[0,1]` = main shot sounds (sent server-side via `ServerSendAudioMessage`)
- `AudioClips[2+]` = local client sounds (trigger pull, mechanical clicks, hammer cock)

### DrawSpeedMultiplier Default
AnimatorController has `DrawSpeedMultiplier` default = **0** (animation frozen), but `AttachmentsUtils.AttachmentsValue(DrawSpeedMultiplier)` uses `ParameterMixingMode.Percent` → `DefaultValue = 1.0`. So `UpdateAnims()` correctly sets it to 1.0. Both animators get this value via `AnimatorSetFloat`.

### Server vs Client Animator Separation
- `fa.ServerSideAnimator` (`Firearm._animator` SerializeField) = on Firearm prefab, used server-side
- `fa.ClientViewmodel.ViewmodelAnimator` = instantiated viewmodel's Animator on "Anims" child
- `SharedHandsController.Hands` = scene Animator that mirrors viewmodel params
- `FirearmExtensions.AnimSetFloat/etc.` routes to BOTH server animator (if `NetworkServer.active`) AND viewmodel (if `HasViewmodel`)

### FirearmAnimatorEventsBase.IsServerController
`IsServerController` is set by `InitializeFirearm(firearm)` call: `true` when `NetworkServer.active && firearm.ServerSideAnimator == GetComponent<Animator>()`. Viewmodel animator events have `IsServerController = false` — they only trigger client-side effects (sounds, sway). Server animator events drive actual game state (ammo counts, flags).

---

## T-Pose Issue — Status (Session 6)

**Symptom:** Arms and weapon visible but all animations static (equip/fire/reload don't play).

**Exhaustive analysis found NO code bug.** Everything is correctly wired:
- Animation clips (`VEC_draw_stock.anim`) have full bone data for arm.L, arm.R, hand.L, hand.R, forearm.L, main ✓
- Hands Rig bone hierarchy exactly matches animation clip paths (`Armature/root/arm.L` etc.) ✓
- SkinnedMeshRenderers reference Hands Rig bones ✓
- `DrawSpeedMultiplier` set to 1.0 on BOTH animators via `UpdateAnims()` in `OnEquipped()` ✓
- `Firearm.EquipUpdate()` → `UpdateAnims()` runs EVERY FRAME while equipped ✓
- No avatar mask on any AnimatorController layer ✓

**Likely cause:** Unity 6 runtime behavior difference when setting `runtimeAnimatorController` at runtime. The Hands Rig animator gets its controller assigned via `UpdateInstance()` — may need one extra frame to settle.

**Debugging steps if T-pose persists:**
1. Check Unity Console for `[AnimatedViewmodelBase] No Animator found` error
2. Add temporary: `Debug.Log($"HandsSpeed={SharedHandsController.Singleton.Hands.GetFloat(\"DrawSpeedMultiplier\")}")` after `OnEquipped()` to confirm parameter is applied
3. Check `SharedHandsController.SetRoleGloves()` is called for the player's role — arms invisible if wrong role
4. In Unity Editor: Select "Hands Rig" in Hierarchy during Play mode → verify Animator shows current state and DrawSpeedMultiplier = 1.0

---

## Attachment System

### AttachmentParam Enum → AttachmentParameterDefinition
Each param has a `ParameterMixingMode`:
- `Additive`: starts at 0, enabled attachments ADD their values
- `Percent`: starts at 1.0, enabled attachments MULTIPLY
- `Override`: takes the value of the last enabled attachment that specifies it

`DrawSpeedMultiplier` = Percent (default=1.0) → without attachments, always returns 1.0.
`MagazineCapacityModifier` = Additive → without attachments, returns 0.
`ShotClipIdOverride` = Override → returns 0 by default (first shot sound clip).

### Attachment Code (uint bitmask)
Each attachment is a bit in a `uint`. `GetCurrentAttachmentsCode()` encodes which are enabled. `ApplyAttachmentsCode(code)` toggles them. `ValidateAttachmentsCode(code)` ensures only one per slot is enabled.

---

## Audio System

`Firearm.AudioClips` = `FirearmAudioClip[]` (array, indexed by byte clip ID).
`ServerSendAudioMessage(byte clipId)` → server sends to all clients → each client plays `AudioClips[clipId]`.
`ClientPlaySound(int index)` in DoubleAction uses `AudioClips[index + 2]` for local trigger sounds.

Audio is played via `AudioSourcePoolManager.PlaySound(clip, position, maxDist, volume, falloff, channel)`.
Weapons use `AudioMixerChannelType.Weapons` channel.

---

## Firearm Status Flags

`FirearmStatusFlags` (byte flags):
- `MagazineInserted` — mag is in the gun
- `Chambered` — round in chamber
- `Cocked` — hammer is cocked
- `FlashlightEnabled` — flashlight on (if flashlight attachment active)

Status is a `FirearmStatus` struct: `{byte Ammo, FirearmStatusFlags Flags, uint Attachments}`.
Status is a `[SyncVar]` on the Firearm NetworkBehaviour — synced server→all clients automatically.
