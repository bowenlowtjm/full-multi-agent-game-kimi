# Task Index — Pully Multi-Agent Run

**Run ID:** pully-B-local-20260608  
**Config:** B (Hermes role-team)  
**Rung:** L3 (spec-only)  
**Status:** In Progress

## Quick Status

| Milestone | Status | Assigned | Blocking |
|-----------|--------|----------|----------|
| M1 | 🔴 Not Started | — | — |
| M2 | 🔴 Not Started | — | M1 |
| M3 | 🔴 Not Started | — | M2 |
| M4 | 🔴 Not Started | — | M3 |

## Task Files

### M1 — Core Loop
- [M1-001-gesture-recognizer.md](M1-001-gesture-recognizer.md) — 5 gestures: tap, double-tap, long-press, swipe-tap, two-finger
- [M1-002-ruleset-definition.md](M1-002-ruleset-definition.md) — ScriptableObject for shape×color→gesture mapping
- [M1-003-scoring-system.md](M1-003-scoring-system.md) — Base reward × combo multiplier, miss penalty
- [M1-004-target-spawner.md](M1-004-target-spawner.md) — Seeded RNG spawn system
- [M1-005-input-manager.md](M1-005-input-manager.md) — Touch/pointer input, raycasting, gesture dispatch
- [M1-006-editmode-tests.md](M1-006-editmode-tests.md) — Unit tests for scoring/combo/gesture
- [M1-007-playmode-test.md](M1-007-playmode-test.md) — Integration test: input → score

### M2 — Menus + Art + Build
- [M2-001-main-menu.md](M2-001-main-menu.md) — Play, best score, Settings buttons
- [M2-002-settings-screen.md](M2-002-settings-screen.md) — Audio/haptics toggles, tutorial replay
- [M2-003-game-scene-hud.md](M2-003-game-scene-hud.md) — Score, combo, lives, timer UI
- [M2-004-pause-flow.md](M2-004-pause-flow.md) — Resume, restart, settings, quit
- [M2-005-game-over-flow.md](M2-005-game-over-flow.md) — Score display, best score, retry/menu
- [M2-006-sprite-generation.md](M2-006-sprite-generation.md) — 4 target shapes + UI/FX sprites
- [M2-007-sprite-atlas.md](M2-007-sprite-atlas.md) — Pack sprites, set import settings
- [M2-008-ci-apk-build.md](M2-008-ci-apk-build.md) — APK builds and installs
- [M2-009-integration-tests.md](M2-009-integration-tests.md) — Lives/timer/game-over flow tests

### M3 — Balance & Robustness
- [M3-001-bot-player.md](M3-001-bot-player.md) — Automated playtest bot
- [M3-002-determinism-test.md](M3-003-determinism-test.md) — Same seed + input = identical replay
- [M3-003-balance-tuning.md](M3-003-balance-tuning.md) — Spawn rates, difficulty curve
- [M3-004-gameplay-harness.md](M3-004-gameplay-harness.md) — Score distribution, FPS stability checks

### M4 — Release Polish
- [M4-001-splash-screen.md](M4-001-splash-screen.md) — Boot + branded splash
- [M4-002-how-to-play.md](M4-002-how-to-play.md) — Skippable tutorial for 5 gestures
- [M4-003-music-loop.md](M4-003-music-loop.md) — Background music with mute
- [M4-004-sfx-set.md](M4-004-sfx-set.md) — Hit, miss, combo, UI, game-over sounds
- [M4-005-haptics.md](M4-005-haptics.md) — Mobile haptics on hit/miss
- [M4-006-juice-polish.md](M4-006-juice-polish.md) — Particles, screen shake, animated transitions
- [M4-007-performance-pass.md](M4-007-performance-pass.md) — 60fps on mid-range Android
- [M4-008-colorblind-mode.md](M4-008-colorblind-mode.md) — Accessibility option
- [M4-009-uninstall-test.md](M4-009-uninstall-test.md) — ≥8/10 gameplay quality gate

## Roles

| Role | Agent | Current Task |
|------|-------|--------------|
| Orchestrator | hermes-main | Coordinating, task dispatch |
| Game PM | game-pm | Creates tasks, accepts work, quality gate |
| Game Art | game-art | Sprite generation, atlas packing |
| QA | qa | Pre-merge verification, smoke tests |

## File Naming Convention

```
{MILESTONE}-{TASK_NUM}-{short-name}.md
└── M1/M2/M3/M4
    └── XXX-sequential number
```

## Task State Transitions

```
Backlog → Assigned → In Progress → Awaiting Review → QA → Done
   ↑______________________________________________|
   (if rejected)
```

---

*Last updated: 2026-06-08*
