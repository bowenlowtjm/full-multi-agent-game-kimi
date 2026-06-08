# M4-005: Haptics

**Status:** ✅ Done
**Priority:** P2
**Assigned:** orchestrator
**Estimated:** 2h
**Started:** 2026-06-08
**Completed:** 2026-06-08
**Milestone:** M4 — Release Polish

## Completed Work

- [x] `HapticsManager.cs` with Android/iOS support
- [x] Hit haptic (light)
- [x] Miss haptic (strong)
- [x] Combo haptic (escalating)
- [x] Settings toggle integration
- [x] Fallback to vibration

## Description

Mobile haptic feedback on hit and miss.

## Acceptance Criteria

- [ ] Haptic on correct hit (light tap)
- [ ] Haptic on miss (distinct rumble)
- [ ] Toggle in Settings
- [ ] Android haptic API
- [ ] No crash on devices without haptics

## Technical Notes

- Use Unity iOS/Android haptic APIs
- Fallback to vibration if haptics unavailable
- Respect device do-not-disturb

## Artifacts Required

- Haptic integration in InputManager
- Settings toggle

## QA Checklist

- [ ] Hit haptic feels good
- [ ] Miss haptic distinct
- [ ] Toggle works
- [ ] No crash on old devices

---

**Next:** M4-006-juice-polish
