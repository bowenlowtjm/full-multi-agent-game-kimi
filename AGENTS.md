# AGENTS.md — Arcade Gesture Game

This repo is **one run** of the multi-agent game experiment using the PRD-based approach.

## What this project is
A Unity 6 (3D project, 2D-sprite gameplay) Android arcade game per the PRD at `PRD.md`.

## Target Types (from PRD)
- **TG-01 Pop** - Blue Sphere, Single Tap, +10 pts
- **TG-02 Heavy** - Gold Square, Double Tap, +25 pts  
- **TG-03 Charge** - Green Anchor, Long Press (1.5s), +50 pts
- **TG-04 Trash** - Red jagged line, Drag to Trash Bin, +40 pts

## State Machine
INIT → MAIN_MENU → GAMEPLAY → PAUSE → GAME_OVER

## Roles (native Hermes role-agents)

Orchestrator drives the loop over these agents:
- **game-pm**: Reads PRD → creates Linear SAA epic + tickets per PRD sections
- **game-art**: Generates sprites for 4 target types + HUD + Trash Bin + FX
- **game-logic**: SpawnerManager, ScoreManager, GestureRecognizer, TargetDefinition SO
- **unity-scene**: Scenes, prefabs, HUD wiring, Trash Bin Zone UI
- **build-ci**: Builder.cs, batchmode APK, GameCI
- **test-author**: EditMode + PlayMode tests per PRD Section 7
- **qa**: Independent gate — runs PRD Verification 01-04

## Conventions
- All game code+assets under `Assets/_Game/`. One asmdef: `Arcade.Game`.
- Target definitions are **data-driven** via ScriptableObject — never hardcode.
- **After every C# edit, run `scripts/unity-check.sh`** until CLEAN.
- Tests under `Assets/Tests/` (EditMode + PlayMode).
- Every significant change → `docs/run-log.md` + Discord.

## Memory
- **OpenViking** (`viking://runs/arcade-B-L3-20250607/`) for shared project memory
- `DESIGN.md` = art taste (Blue Sphere, Gold Square, Green Anchor, Red Line styles)

## Linear Workflow
1. **Game PM** reads PRD → creates Linear epic + 24 tickets (see RUN-PARAMETERS.md)
2. **Orchestrator** pulls tickets, assigns to workers
3. **Workers** claim → In Progress → PR → QA gate
4. **QA** runs PRD Verification 01-04 + compile/CI checks
5. **Merge** on QA PASS + green CI

## PRD Verification Checklist (QA Gate)
- [ ] Verification 01: Tap on Charge breaks combo
- [ ] Verification 02: Trash drag outside bin penalizes
- [ ] Verification 03: 60 FPS with 10+ targets
- [ ] Verification 04: High score persists across restarts
