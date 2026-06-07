# M1-005: Input Manager

**Status:** ✅ Done
**Priority:** P0
**Assigned:** orchestrator
**Estimated:** 4h
**Started:** 2026-06-08
**Completed:** 2026-06-08
**Milestone:** M1 — Core Loop

## Completed Work

- [x] `InputManager` raycast-based target selection
- [x] Touch + mouse input support
- [x] Double-tap detection
- [x] Long-press detection for Charge targets
- [x] Drag-to-trash for Trash targets
- [x] Trash bin zone integration
- [x] Gesture routing to `Target` callbacks

## Description

Connect input detection to target interaction. Raycast to find touched targets, validate gestures against target requirements, and route to scoring.

## Acceptance Criteria

- [ ] Raycast from touch/mouse position to find hit target
- [ ] Query target for required gesture type
- [ ] Compare detected gesture to required gesture
- [ ] Correct gesture: trigger hit → ScoreManager → destroy target
- [ ] Wrong gesture: trigger miss → ScoreManager (target stays)
- [ ] Miss (no target hit): trigger miss penalty
- [ ] Touch input works on Android
- [ ] Mouse input works in Editor

## Technical Notes

- `InputManager` singleton (exists, needs completion)
- Use Camera.main.ScreenPointToRay
- Layer mask for targets only
- Consider drag gesture for "swipe-tap" (needs movement threshold)

## Dependencies

- M1-001: GestureRecognizer for gesture events
- M1-004: Target.cs with `RequiredGesture` property
- M1-003: ScoreManager for hit/miss

## Artifacts Required

- `Assets/_Game/Scripts/InputManager.cs` (exists, verify completion)
- Integration with GestureRecognizer events

## QA Checklist

- [ ] Correct gesture on target = score
- [ ] Wrong gesture on target = miss
- [ ] Gesture with no target = miss
- [ ] Touch and mouse both functional
- [ ] No input lag or dropped touches

---

**Next:** M1-006-editmode-tests (tests this integration)
