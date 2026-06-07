# M2-009: Integration Tests

**Status:** Backlog
**Priority:** P1
**Assigned:** —
**Estimated:** 4h
**Milestone:** M2 — Menus + Art + Build

## Description

Write PlayMode integration tests for menus, lives, timer, and game-over flows.

## Acceptance Criteria

- [ ] Test: Menu → Game scene transition
- [ ] Test: Lives decrement to 0 → game over
- [ ] Test: Timer reaches 0 → game over
- [ ] Test: Pause → Resume flow
- [ ] Test: High score persistence
- [ ] All tests pass in CI

## Test Scenarios

```
1. Load MainMenu
2. Click Play
3. Assert: Game scene loaded

1. Start game
2. Mock 3 misses
3. Assert: GameOver scene loaded
4. Assert: Final score displayed

1. Start game
2. Wait 60 seconds
3. Assert: GameOver scene loaded
```

## Dependencies

- All M2 features implemented

## Artifacts Required

- `Assets/Tests/PlayMode/MenuFlowTests.cs`
- `Assets/Tests/PlayMode/GameFlowTests.cs`

## QA Checklist

- [ ] All tests pass locally
- [ ] All tests pass in CI
- [ ] Tests cover critical user paths

---

**Milestone M2 Exit:**
- APK builds and installs
- All screens functional
- Art integrated
- QA sign-off

**Next:** M3-001-bot-player
