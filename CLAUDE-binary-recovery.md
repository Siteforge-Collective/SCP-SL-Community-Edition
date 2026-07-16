# CLAUDE-binary-recovery.md — Recovering Ground Truth from the Original Game Build

**Rule: NEVER guess numeric constants, field semantics, or logic order when restoring code.
Everything can be read from the original v12.0.2 build. This doc is the recipe.**

The original (untouched) client build lives at:

```
D:\AssetToBaseStart12.0.2\Client\               ← original game root
D:\AssetToBaseStart12.0.2\Client\GameAssembly.dll   ← all game code (IL2CPP)
D:\AssetToBaseStart12.0.2\Client\SCPSL_Data\        ← all assets (extract with UnityPy)
```

---

## The 4 layers of reference (use them IN THIS ORDER)

### Layer 1 — Type metadata: field offsets + method addresses (ALWAYS START HERE)

```
CODES/ClientCode/cpp2il_out/<TypeName>_metadata.txt
```

Contains for every type:
- **Every field with its exact runtime offset** (`Offset in Defining Type: 0x18`).
- **Every method with `File Offset` and `Ram Offset`** into GameAssembly.dll, plus parameters.

This is the decoder ring for ISIL. Example (`PlayerListElement_metadata.txt`):

```
Field: instance        Offset 0x18 (24)
Field: TextNick        Offset 0x20 (32)
Field: TextBadge       Offset 0x28 (40)
Field: ImgVerified     Offset 0x30 (48)
Field: ImgBackground   Offset 0x38 (56)
Field: ToggleMute      Offset 0x40 (64)
Field: OpenProfile     Offset 0x48 (72)
```

So when ISIL says `Move rbx, [rsi+48]` and `rsi` holds a `PlayerListElement`,
that read is **ImgVerified** — no guessing.

Rule of thumb if metadata is missing for a type: IL2CPP object header = 16 bytes
(klass + monitor); `MonoBehaviour` adds `m_CachedPtr` at +16, so script fields start
at **+24 (0x18)** in declaration order (8 bytes per reference/8-aligned value).
Plain (non-Unity) classes start fields at **+16 (0x10)**.

### Layer 2 — ISIL instruction dumps: the real logic

```
CODES/ClientCode/CPP2IL_ISIL_Client/IsilDump/<TypeName>.txt
```

Each method has raw x64 `Disassembly:` and lifted `ISIL:` (numbered instructions).
**The ISIL is ground truth for v12 client behaviour** — the pseudocode in
`cpp2il_out/method_dumps/*_methods.txt` is often corrupt/incomplete (it silently
DROPS calls and invents none — but the rip in `Assets/Scripts` was written from it,
so the rip both loses calls AND sometimes invents extra ones. Verify against ISIL.)

How to read ISIL:
- `Move rbx, [rsi+40]` — field read; resolve offset via Layer 1 metadata.
- Named calls (`Call ServerRoles.GetColor`) are resolved for you.
- `Call 0, rcx, rdx, r8, ...` after `Move r8, [rax+680]` = **virtual call through
  vtable slot** `+680`. Identify it by context: what object is in `rcx`, what argument
  type is passed. Empirically confirmed in this build:
  `Graphic vtable +664 = get_color`, `+680 = set_color`, `TMP_Text +1368 = set_text`,
  `Component vtable via Component.get_gameObject` is usually named.
- `cmove eax, r13d` in disassembly = conditional select (`x = cond ? a : b`) —
  the ISIL sometimes renders this poorly; check the raw disassembly for `cmov`.
- String literals appear inline: `Move rdx, "["`.

### Layer 3 — Constants from the binary itself (floats, magic numbers)

ISIL references data constants by absolute VA:

```
movss xmm1, dword ptr [181EA337Ch]     ← some float in .rdata
```

Read the actual value with the ready-made tool (NO manual PE math needed):

```
powershell -File Tools\ReadGameAssembly.ps1 -Address 0x181EA337C
# → VA 0x181EA337C -> .rdata -> file offset 0x1EA117C
# → float : 1
```

It accepts absolute VAs from ISIL (base 0x180000000 auto-subtracted), bare RVAs,
and raw file offsets (`-IsFileOffset`, e.g. a method's `File Offset` from metadata
to dump its bytes). `-Count N` to read more bytes.

Real example from 2026-07-04: the rip had invented badge x-offsets `370 : 0`.
Disassembly showed `mov eax,19Ch / cmove eax,r13d` (r13=0x172) and an alpha constant
at `[0x181EA337C]`. Reading the binary gave **412 / 370** and **alpha = 1.0**. No guessing.

### Layer 4 — Assets from the original build (prefabs, values in Inspector)

- `reference_original_build` memory: UnityPy recipe to extract any asset from
  `Client\SCPSL_Data` (controllers/avatars/CRC32 path-hash).
- `Projects/Client/ExportedProject/Assets` — AssetRipper export of the clean client:
  real prefabs/materials/meshes and **serialized field values** (method bodies are
  stubs there — logic comes from Layers 1–3). Match assets by NAME (GUIDs differ).

---

## Mandatory cross-checks before writing restored code

1. **Field → GameObject mapping in prefabs.** A serialized component reference may
   point to a component ON THE PREFAB ROOT. Then `field.gameObject.SetActive(false)`
   kills the whole prefab instance. Check the `.prefab` YAML: find the field's
   `fileID`, find which `GameObject` that component's `m_GameObject` points to.
   (This exact trap: `PlayerListElement.ImgBackground` is the Image on the row root;
   the rip's invented `SetActive(hasRole)` hid every player row.)
2. **Count the calls.** If your restored method has MORE `SetActive`/side-effect calls
   than the ISIL shows — you (or the rip) invented them. Delete.
3. **Check conditions' polarity.** Rip frequently inverts `if` conditions and ternaries.
   Trace the actual jump targets in ISIL (`JumpIfEqual {N}` → what runs at N).

## Where things are

| What | Where |
|---|---|
| Field offsets, method File/Ram offsets | `CODES/ClientCode/cpp2il_out/<Type>_metadata.txt` |
| Real v12 client logic (per instruction) | `CODES/ClientCode/CPP2IL_ISIL_Client/IsilDump/<Type>.txt` |
| Quick (but lossy) pseudocode | `CODES/ClientCode/cpp2il_out/method_dumps/<Type>_methods.txt` |
| Binary constants reader | `Tools/ReadGameAssembly.ps1` |
| Original binary | `D:\AssetToBaseStart12.0.2\Client\GameAssembly.dll` |
| Original assets | `D:\AssetToBaseStart12.0.2\Client\SCPSL_Data` (UnityPy) |
| Ripped-but-real serialized values | `Projects/Client/ExportedProject/Assets` |
| Server-side reference | `CODES/ServerCode/` |
| v13 clean architecture reference | `CODES/SCP13.0/` |
