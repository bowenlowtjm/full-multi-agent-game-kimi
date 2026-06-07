# M4-005: Haptics

**Status:** Backlog
**Priority:** P2
**Assigned:** —
**Estimated:** 2h
**Milestone:** M4 — Release Polish

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
