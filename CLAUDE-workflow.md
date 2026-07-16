# CLAUDE-workflow.md — How to Work on This Project (Mandatory Method)

This is the working method that consistently finds and fixes bugs in this codebase.
Follow it step by step. Do not skip steps to "save time" — skipped steps are where
wrong fixes come from. Written for ALL assistants working here, including smaller
models: when in doubt, follow the checklist literally.

**The single most important rule: the code in `Assets/Scripts/` is a RECONSTRUCTION
from a broken decompiler. It lies. Never debug it by reading it alone, never "fix"
it by guessing what looks logical. Always compare against ground truth first.**

---

## 0. Before touching anything

1. Read the header comments of `CLAUDE.md` — they list companion docs. Open the one
   for your system (firearms → `CLAUDE-firearm.md`, scenes/UI → `CLAUDE-scenes.md`,
   prefabs → `CLAUDE-prefabs-*.md`, binary reading → `CLAUDE-binary-recovery.md`).
2. Check the memory index (`MEMORY.md` in the Claude memory dir) for the same
   symptom — many bugs were already fixed once. **Never re-apply a fix marked ✅.**
3. If Unity is open, MCP tools are available (`mcp__mcp-unity__*`, host port 8090,
   ParrelSync clone 8091): console logs, live GameObject inspection, recompile.

## 1. The debugging loop (repeat until root cause is PROVEN)

```
symptom
  → find the code path (Grep; follow the call chain, not your intuition)
  → find the same method in GROUND TRUTH and compare LINE BY LINE:
      a) CODES/ClientCode/cpp2il_out/<Type>_metadata.txt   — field offsets, method addresses
      b) CODES/ClientCode/CPP2IL_ISIL_Client/IsilDump/<Type>.txt — real v12 logic (per instruction)
      c) Tools/ReadGameAssembly.ps1 -Address 0x18XXXXXXX   — real constants from the binary
      d) the .prefab/.unity YAML — what fields actually point to
      e) CODES/ServerCode/ — server-side variants; CODES/SCP13.0/ — clean C# STRUCTURE
         (v13 constants may DIFFER from v12 — e.g. SustainTime 2 vs 3 — trust only ISIL/binary)
  → the bug is the DIFFERENCE between the project code and the ground truth
  → fix minimally, keeping project null-guards and style
  → recompile (mcp__mcp-unity__recompile_scripts), confirm 0 errors
  → record the root cause in memory; mark ✅ only after the USER confirms in runtime
```

Rules inside the loop:
- **Never guess a number.** Every float/int the decompiler lost is still in
  `D:\AssetToBaseStart12.0.2\Client\GameAssembly.dll` — read it (see
  `CLAUDE-binary-recovery.md`). Reading takes one command; guessing costs a session.
- **Count the side effects.** If your method has more `SetActive`/`Destroy`/RPC
  calls than the ISIL shows — the rip INVENTED them. Delete them. If it has fewer —
  the rip LOST them. Restore them.
- **Check jump polarity.** For every `if` in restored code, trace the ISIL
  `JumpIfEqual/JumpIfNotEqual` target and verify the condition isn't inverted.
- **State the root cause before editing.** If you cannot say "line X does Y but
  ground truth does Z, which causes the symptom because …", you have not found it
  yet — keep digging instead of patching symptoms.

## 2. Catalog of known rip failure patterns (check these FIRST)

Every one of these has actually happened in this project, most more than once:

| # | Pattern | Example |
|---|---------|---------|
| 1 | **Inverted condition / ternary** | PumpAction auto-pump `Ammo==0` vs `!=0`; popup fade 1→0 |
| 2 | **Invented side-effect calls** | `ImgBackground.gameObject.SetActive(hasRole)` hid every PlayerList row |
| 3 | **Lost loop over children** | `SetLights` didn't iterate lamps — lights never turned off |
| 4 | **Lost field initializer** | `_playerInfoToShow` flags = 0 → empty nickname hover |
| 5 | **Wrong/zeroed constants** | Badge x 370/0 instead of 412/370; sensitivity ×2 |
| 6 | **Curve/lerp semantics flipped** | Light disable curve = brightness, rip lerped the other way → lights re-lit |
| 7 | **Stubbed getter (`return Color.clear/false/null`)** | `ServerRoles.GetColor()` → invisible nicknames |
| 8 | **Lost recursion gate → StackOverflow** | `ValueButton.SetState` without `if (isSelected)` → native crash, EMPTY managed stack |
| 9 | **Serialized field points at the PREFAB ROOT** | `SetActive(false)` on it kills the whole instance — check fileID → m_GameObject in YAML |
| 10 | **Host-vs-client predicate inverted** | `x => !x.isLocalPlayer` excludes the HOST from broadcasts |
| 11 | **SyncVar hook swallowed** | Mirror calls hooks on host only if netId already in `NetworkClient.spawned`; assignments in the spawn frame are lost — call server-side hooks manually |
| 12 | **`Instantiate` inherits disabled state** | Scene UI templates are stored disabled — clone needs explicit `SetActive(true)` |
| 13 | **Server-only init in a dedicated-only path** | Permissions loaded only in `CreateMatch` → NRE on listen-host; put init in `OnStartServer` |
| 14 | **Client handler class replaced by server variant** | `GameConsoleCommandHandler` had server commands; ClientCode ≠ ServerCode for `*Handler` |
| 15 | **Double state-machine call** | `CheckRateLimit()` called twice — first call restarts the stopwatch, second always fails |
| 16 | **Argument order / wrong overload** | `BezierQuadratic(t)` args shuffled; `ServerOverridePosition(pos, POS)` instead of `(pos, zero)` |
| 17 | **Unity fake-null vs C# null** | `?.` on destroyed UnityEngine.Object bypasses the lifetime check — use `!= null` |
| 18 | **Pickup state written AFTER `NetworkServer.Spawn`** | SyncVars must be set BEFORE Spawn or the first payload ships defaults |

Symptom heuristics:
- "UI element invisible / empty" → patterns 7, 4, 12, 9.
- "It happens, then immediately un-happens" → something re-runs every frame:
  patterns 2, 6, 15 — find WHO re-applies the state (log or ISIL), not where it's set.
- "Works on host, broken on client" (or vice versa) → patterns 10, 11, 13, 14.
- "Native crash, empty managed stack" → pattern 8 (StackOverflow, look for lost gates).
- "Input goes to the wrong key" → stale `%AppData%` keybinding.txt, not code.

## 3. Editing rules

- Minimal diffs. Fix the found difference; don't refactor around it.
- Keep the project's defensive additions (null-guards, `TryGet…` fallbacks) — they
  are allowed by the restoration philosophy and have saved many NREs.
- v13 (`CODES/SCP13.0/`) is good for ARCHITECTURE and naming; v12 ISIL/binary is the
  only authority for BEHAVIOUR and NUMBERS.
- Do not break Mirror contracts: SyncVar/Command/RPC signatures, read/write order in
  `ServerWriteRpc`/`ClientProcessRpc` pairs must stay symmetric.
- Comment only non-obvious facts the code can't show (e.g. "second arg is a zero
  rotation delta, NOT the position" or "curve VALUE = brightness"). No noise.

## 4. After the fix

1. `mcp__mcp-unity__recompile_scripts` → must be 0 errors (≈400 legacy warnings are normal).
2. Tell the user exactly WHAT to test in runtime. Do not claim it works — it is
   "fixed, awaiting runtime test" until the user confirms.
3. Write the root cause + the lesson into memory (one file per topic, update the
   index line). Mark ✅ only after user confirmation.
4. Commit only when the user asks. Message: what + why, English, concise.

## 5. Where ground truth lives (quick map)

| Need | Source |
|------|--------|
| Field offsets, method File/Ram offsets | `CODES/ClientCode/cpp2il_out/<Type>_metadata.txt` |
| Real v12 client logic | `CODES/ClientCode/CPP2IL_ISIL_Client/IsilDump/<Type>.txt` |
| Quick (lossy!) pseudocode | `CODES/ClientCode/cpp2il_out/method_dumps/` |
| Numeric constants | `Tools/ReadGameAssembly.ps1` on `D:\AssetToBaseStart12.0.2\Client\GameAssembly.dll` |
| Server-side logic | `CODES/ServerCode/` |
| Clean C# structure (bodies stripped) | `CODES/SCP13.0/` |
| Real serialized values / prefab layout | `Projects/Client/ExportedProject/Assets` (match by NAME, GUIDs differ) |
| Original assets | `D:\AssetToBaseStart12.0.2\Client\SCPSL_Data` (UnityPy) |
| Which GameObject a component sits on | the `.prefab`/`.unity` YAML itself (fileID → m_GameObject) |
| Live scene state / console / recompile | Unity MCP (`mcp__mcp-unity__*`) |

**Final sanity check before you say "fixed": can you point to the exact ground-truth
line that proves your change? If not — you guessed. Go back to step 1.**
