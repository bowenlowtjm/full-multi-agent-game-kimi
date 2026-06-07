# M1-003: Scoring System

**Status:** ✅ Done
**Priority:** P0
**Assigned:** orchestrator
**Estimated:** 4h
**Started:** 2026-06-08
**Completed:** 2026-06-08
**Milestone:** M1 — Core Loop

## Completed Work

- [x] `ScoreManager` integrated with `RulesetDefinition`
- [x] Combo multiplier uses ruleset.comboCap
- [x] Lives uses ruleset.lives
- [x] `RegisterHit(baseReward)` overload for ruleset-driven scoring
- [x] `SetRuleset()` method for runtime configuration

## Description

Implement the scoring and combo system. Track player performance, calculate multipliers, handle misses/penalties, and persist high scores.

## Acceptance Criteria

- [ ] Base reward × combo multiplier calculation
- [ ] Combo: ×1.1 per consecutive correct, capped at ×5
- [ ] Miss/wrong gesture: combo resets to ×1
- [ ] Lives mode: lose 1 life per miss (max 3 lives)
- [ ] Game over when lives = 0
- [ ] High score persistence (PlayerPrefs or file)
- [ ] Score events: `OnScoreChanged`, `OnComboChanged`, `OnLivesChanged`

## Technical Notes

- `ScoreManager` singleton pattern (already exists)
- Thread-safe if using events
- Consider floating-point precision for combo math
- Persist high score immediately on game over

## Dependencies

- M1-002: uses RulesetDefinition for base values

## Artifacts Required

- `Assets/_Game/Scripts/ScoreManager.cs` (exists, needs completion)
- `Assets/_Game/Scripts/ScoreCalculator.cs` (exists, verify logic)
- `Assets/Tests/EditMode/ScoreCalculatorTests.cs` (exists, expand coverage)

## QA Checklist

- [ ] Unit tests: combo math correct at all levels
- [ ] Unit tests: miss resets combo
- [ ] Unit tests: lives decrement correctly
- [ ] Unit tests: high score persists across sessions
- [ ] EditMode tests: 90%+ coverage

---

**Next:** M1-004-target-spawner (uses scoring events)
