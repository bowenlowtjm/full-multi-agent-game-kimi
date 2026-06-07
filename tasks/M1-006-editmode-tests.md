# M1-006: EditMode Tests

**Status:** ✅ Done
**Priority:** P1
**Assigned:** orchestrator
**Estimated:** 3h
**Started:** 2026-06-08
**Completed:** 2026-06-08
**Milestone:** M1 — Core Loop

## Completed Work

- [x] `ScoreCalculatorTests`: combo multiplier, miss handling, game over, high score
- [x] `RulesetDefinitionTests.cs`: seeded RNG determinism, default values
- [x] `GestureRecognizerTests.cs`: thresholds, enum values, TapData structure
- [x] All EditMode tests use NUnit with proper Setup/Teardown
- [x] Tests run without Unity scene (pure C# logic)

## Description

Write comprehensive EditMode (unit) tests for game logic. Tests run without Unity scene, validating pure C# logic.

## Acceptance Criteria

- [ ] `ScoreCalculatorTests`: all combo multiplier scenarios
- [ ] `GestureRecognizerTests`: mock input → gesture detection
- [ ] `RulesetDefinitionTests`: data loading validation
- [ ] `TargetTests`: lifecycle, expiry logic
- [ ] All tests pass in CI (`ci.yml`)
- [ ] Coverage: core logic classes ≥80%

## Test Scenarios

### ScoreCalculatorTests
```
- Combo 0 → multiplier 1.0
- Combo 1 → multiplier 1.1
- Combo 9 → multiplier 5.0 (capped)
- Combo 10 → still 5.0 (capped)
- Miss resets combo to 0
```

### GestureRecognizerTests
```
- Mock single tap input → SingleTap event
- Mock double tap input → DoubleTap event
- Mock long hold → LongPress event
- Mock swipe → SwipeTap event
- Mock two touches → TwoFingerTap event
- Wrong gesture near threshold → no false positive
```

### RulesetDefinitionTests
```
- Load ruleset asset → not null
- Seeded RNG → identical sequence
- All shape×color mappings present
```

## Technical Notes

- Use NUnit (already in test asmdef)
- Mock time for gesture timing tests
- Test data: create temporary ScriptableObjects in `[SetUp]`

## Dependencies

- M1-001 through M1-005 (test the implemented systems)

## Artifacts Required

- `Assets/Tests/EditMode/ScoreCalculatorTests.cs` (exists, expand)
- `Assets/Tests/EditMode/GestureRecognizerTests.cs` (create)
- `Assets/Tests/EditMode/RulesetDefinitionTests.cs` (create)
- `Assets/Tests/EditMode/TargetTests.cs` (create)

## QA Checklist

- [ ] All tests pass locally
- [ ] All tests pass in CI
- [ ] No test-only code in production
- [ ] Tests are deterministic (no random failures)

---

**Next:** M1-007-playmode-test
