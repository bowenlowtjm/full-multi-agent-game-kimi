# M1-007: PlayMode Integration Test

**Status:** Backlog  
**Priority:** P1  
**Assigned:** —  
**Estimated:** 3h  
**Milestone:** M1 — Core Loop

## Description

Write a PlayMode (integration) test that validates the full input → gesture → target → score pipeline in a real Unity scene.

## Acceptance Criteria

- [ ] Test scene loads with all managers
- [ ] Spawn a target with known properties
- [ ] Simulate correct gesture input
- [ ] Verify: target destroyed, score increased, combo increased
- [ ] Simulate wrong gesture
- [ ] Verify: target remains, miss penalty applied, combo reset
- [ ] Test runs headless in CI

## Test Script

```
1. Load "TestScene"
2. Mock InputManager to inject gesture events
3. Wait for target spawn
4. Inject "SingleTap" at target position
5. Assert: score = base reward, combo = 1
6. Spawn second target
7. Inject "LongPress" (wrong gesture)
8. Assert: combo = 0, lives = 2
```

## Technical Notes

- Use `UnityEngine.TestTools` for scene loading
- `[UnityTest]` attribute for coroutine tests
- Mock time to control spawn timing
- Clean up scene after test

## Dependencies

- M1-001 through M1-006

## Artifacts Required

- `Assets/Tests/PlayMode/SmokePlayModeTests.cs` (exists, expand)
- `Assets/Tests/PlayMode/CoreLoopTests.cs` (create)
- Test scene: `Assets/Tests/PlayMode/TestScene.unity`

## QA Checklist

- [ ] Test passes locally in PlayMode
- [ ] Test passes in CI (headless)
- [ ] Test completes in <30 seconds
- [ ] No scene modifications persisted

---

**Milestone M1 Exit Criteria:**
- All tasks complete
- CI green
- QA sign-off
- Code review (if applicable)

**Next:** M2-001-main-menu (menus milestone)
