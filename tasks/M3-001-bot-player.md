# M3-001: Bot Player

**Status:** Backlog
**Priority:** P2
**Assigned:** —
**Estimated:** 5h
**Milestone:** M3 — Balance & Robustness

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
