# M1-001: Gesture Recognizer

**Status:** Backlog  
**Priority:** P0 (blocks all gameplay)  
**Assigned:** —  
**Estimated:** 4h  
**Milestone:** M1 — Core Loop

## Description

Implement a gesture recognition system that can detect 5 distinct gestures from touch/pointer input and emit events for game logic to consume.

## Acceptance Criteria

- [ ] Single tap: quick press and release < 200ms, single touch
- [ ] Double tap: two taps within 300ms window
- [ ] Long press: hold > 500ms before release
- [ ] Swipe-tap: tap followed by directional flick before release
- [ ] Two-finger tap: two simultaneous touches, quick release
- [ ] Events: `OnGestureDetected(GestureType type, Vector2 position, Vector2? direction)`
- [ ] Works with both touch (mobile) and mouse (editor)
- [ ] EditMode unit test: each gesture type validates correctly

## Technical Notes

- Use Unity Input System (already configured in `Arcade.Game.asmdef`)
- Store gesture thresholds in `RulesetDefinition` or constants class
- Handle multi-touch properly (don't conflict with two-finger)
- Consider gesture timeouts (don't leave pending gestures hanging)

## Dependencies

- None (can start immediately)

## Artifacts Required

- `Assets/_Game/Scripts/GestureRecognizer.cs` (exists, needs completion)
- `Assets/Tests/EditMode/GestureRecognizerTests.cs` (update existing)
- CI green on gesture tests

## QA Checklist

- [ ] All 5 gestures detected in test suite
- [ ] No false positives between similar gestures
- [ ] Touch and mouse both work
- [ ] Performance: <0.5ms per frame

---

**Next:** M1-002-ruleset-definition (uses gesture types)
