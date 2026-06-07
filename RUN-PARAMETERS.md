# RUN PARAMETERS — Multi-Agent Arcade Game (Config B)

**Filled per RUN-PROTOCOL.md for this run.**

## Run Parameters Block

| Parameter | Value |
|-----------|-------|
| **SPEC_REPO** | `/Users/bowenlow/Documents/Shared_Notes/Dev-Hermes-Pully` |
| **RUN_ID** | `arcade-B-L3-20250607` |
| **CONFIG** | B (Hermes role-team) |
| **RUNG** | L3 (Spec-only) |
| **MODELS** | orchestrator=KimiK2.5 / workers=KimiK2.5 |
| **MEMORY** | OpenViking |
| **LINEAR_TEAM** | SAA |
| **LINEAR_LABEL** | run:arcade-B-L3-20250607 |
| **LINEAR_PROJECT** | https://linear.app/saaavvvy-dev-space/project/multi-agent-game-kimi-7517a3ed1179/overview |
| **DISCORD_TARGET** | `#hermes-updates` (post-only webhook) |
| **ART_STYLE** | flat-vector (L3: baked into params) |
| **REPO_LOCATION** | `/Users/bowenlow/Documents/agent-game-explore/full-multi-agent-game-kimi` |
| **UNITY_MCP_PORT** | 6401 |
| **PULLY_REFRESH_PORT** | 8090 |
| **VIKING_NAMESPACE** | `viking://runs/arcade-B-L3-20250607/` |

## Project PRD

**Source:** `/Users/bowenlow/Documents/Shared_Notes/Dev-Hermes-Pully/Game Design PRD.md`  
**Local Copy:** `/Users/bowenlow/Documents/agent-game-explore/full-multi-agent-game-kimi/PRD.md`

### Game Overview
- **Name:** Arcade Gesture Game
- **Type:** Single-Player Casual Arcade Mobile
- **Core:** Multi-gesture input (Tap, Double Tap, Long Press, Drag)
- **Platform:** Unity 6 LTS, Android Portrait

### Target Types (from PRD)
| ID | Visual | Gesture | Score |
|----|--------|---------|-------|
| TG-01 (Pop) | Blue Sphere | Single Tap | +10 |
| TG-02 (Heavy) | Gold Square | Double Tap | +25 |
| TG-03 (Charge) | Green Anchor | Long Press (1.5s) | +50 |
| TG-04 (Trash) | Red jagged line | Drag to Trash Bin | +40 |

### State Machine
`INIT` → `MAIN_MENU` → `GAMEPLAY` → `PAUSE` → `GAME_OVER`

### Key Systems
1. **SpawnerManager** - Safe-zone spawning, collision avoidance
2. **ScoreManager** - Base score × multiplier, combo every 5 hits
3. **GestureRecognizer** - Tap, Double Tap (300ms), Long Press (1.5s), Drag (>10px threshold)
4. **Trash Bin Zone** - Drag targets must end here
5. **Local Persistence** - High score via PlayerPrefs

## Agent Roles

Config B — Hermes role-team with parallel workers:

| Role | Responsibility | Linear Integration |
|------|---------------|-------------------|
| **orchestrator** | Loop driver, coordination | Sprint planning, issue management |
| **game-pm** | Product brain, PRD → tickets | Creates SAA epic + issues from PRD |
| **game-art** | Sprite generation (4 targets + UI + FX) | Art tasks via Linear |
| **game-logic** | C# systems - scoring, combo, gesture recognition | Logic tasks, Ruleset SO |
| **unity-scene** | Scenes, prefabs, HUD, Trash Bin Zone | Scene composition tasks |
| **build-ci** | Builder.cs, batchmode APK, GameCI | CI/CD tasks |
| **test-author** | EditMode + PlayMode tests, QA harness | Test tasks |
| **qa** | Independent verification gate | Blocks/approves PRs |

## Linear Ticket Structure (PM Creates)

Based on PRD Section 4+7:

**Epic:** `SAA-XXX` Arcade Gesture Game Implementation

**Milestone 1: Core Loop & Foundation**
- SAA-001: Implement GameStateMachine (INIT, MAIN_MENU, GAMEPLAY, PAUSE, GAME_OVER)
- SAA-002: Implement SpawnerManager with safe-zone bounds and collision detection
- SAA-003: Implement ScoreManager with combo multiplier (every 5 hits)
- SAA-004: Implement local persistence for high scores (PlayerPrefs)
- SAA-005: Create TargetDefinition ScriptableObject system
- SAA-006: Implement basic gesture recognition (Tap, Double Tap, Long Press, Drag)
- SAA-007: Create Main Menu scene
- SAA-008: Create Gameplay scene with HUD
- SAA-009: Create Game Over scene with retry

**Milestone 2: Targets & Feedback**
- SAA-010: Implement TG-01 Pop (Blue Sphere, Single Tap)
- SAA-011: Implement TG-02 Heavy (Gold Square, Double Tap, 300ms)
- SAA-012: Implement TG-03 Charge (Green Anchor, Long Press, 1.5s)
- SAA-013: Implement TG-04 Trash (Red Line, Drag to Trash Bin Zone)
- SAA-014: Generate sprite assets for 4 target types
- SAA-015: Create Trash Bin Zone UI and drag target
- SAA-016: Implement hit/miss feedback (particles, SFX)
- SAA-017: Implement combo escalation visuals

**Milestone 3: Polish & QA**
- SAA-018: Balance spawn rates (1.5s → 0.4s over time)
- SAA-019: Implement pause functionality
- SAA-020: QA Verification 01: Tap on Charge breaks combo
- SAA-021: QA Verification 02: Trash drag outside bin penalizes
- SAA-022: QA Verification 03: 60 FPS performance test
- SAA-023: QA Verification 04: High score persistence test
- SAA-024: Build APK artifact via GameCI

## Memory Structure (OpenViking)

```
viking://runs/arcade-B-L3-20250607/
├── project/
│   ├── conventions/          # Coding standards
│   ├── decisions/            # Key decisions from PRD
│   ├── gotchas/              # Known traps
│   └── run-log/              # Execution log
├── taste/
│   └── design/               # Art style from PRD
├── agents/
│   ├── game-pm/              # PM memory
│   ├── game-art/
│   ├── game-logic/
│   ├── unity-scene/
│   ├── build-ci/
│   ├── test-author/
│   └── qa/
└── shared/
    └── context/              # Cross-agent shared
```

## Discord Communication (L3)

Post-only webhook feed for:
- Ticket creation/completion
- CI status changes
- QA PASS/FAIL results
- APK build completion

No human reply expected — team self-transitions based on PRD spec.
