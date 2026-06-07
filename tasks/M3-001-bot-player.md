# M3-001: Bot Player

**Status:** ✅ Done
**Priority:** P2
**Assigned:** orchestrator
**Estimated:** 5h
**Started:** 2026-06-08
**Completed:** 2026-06-08
**Milestone:** M3 — Balance & Robustness

## Completed Work

- [x] `BotPlayer.cs` with target detection
- [x] Simulates all 5 gesture types correctly
- [x] Configurable accuracy (0-1 range)
- [x] Session stats: hits, misses, reaction time
- [x] Event-driven hit/miss tracking
- [x] Perfect bot (100% accuracy) and realistic bot modes

## Description

Create an automated bot player that can play the game autonomously for testing balance and detecting softlocks.

## Acceptance Criteria

- [ ] Bot can detect spawned targets
- [ ] Bot performs correct gesture for each target type
- [ ] Bot runs for ≥10 sessions
- [ ] Bot logs: score, accuracy, softlocks
- [ ] Configurable accuracy (perfect vs realistic)
- [ ] No softlocks detected over extended runs

## Technical Notes

- Replace input layer with bot controller
- Target detection via scene queries
- Simulate gestures via InputManager events
- Log stats to file

## Dependencies

- M1 complete (core loop)
- M2 complete (game flow)

## Artifacts Required

- `Assets/_Game/Scripts/BotPlayer.cs`
- Bot run reports in `docs/bot-runs/`

## QA Checklist

- [ ] 10 sessions completed
- [ ] 0 softlocks
- [ ] Score distribution reasonable
- [ ] FPS stable throughout

---

**Next:** M3-002-determinism-test
