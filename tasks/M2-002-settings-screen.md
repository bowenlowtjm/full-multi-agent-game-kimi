# M2-002: Settings Screen

**Status:** ✅ Done
**Priority:** P1
**Assigned:** orchestrator
**Estimated:** 4h
**Started:** 2026-06-08
**Completed:** 2026-06-08
**Milestone:** M4 — Release Polish

## Completed Work

- [x] `SettingsManager.cs` with audio/haptics/tutorial
- [x] `AudioManager.cs` with music + 6 SFX slots
- [x] Volume sliders with persistence
- [x] Mute toggle
- [x] Haptics toggle
- [x] Tutorial replay button

## Description

Settings screen with audio/haptics toggles and tutorial replay option.

## Acceptance Criteria

- [ ] Music volume slider (0-100%)
- [ ] SFX volume slider (0-100%)
- [ ] Mute toggle (master)
- [ ] Haptics toggle (on/off)
- [ ] "Replay Tutorial" button
- [ ] Settings persist across sessions
- [ ] Back button returns to previous screen

## Technical Notes

- Use PlayerPrefs or JSON for persistence
- Immediate feedback when toggling
- Preview sound on volume change

## Dependencies

- M2-001: accessible from Main Menu
- M4-003, M4-004, M4-005: audio/haptics exist

## Artifacts Required

- `Assets/_Game/Scripts/SettingsManager.cs` (create)
- UI integration with AudioManager

## QA Checklist

- [ ] All settings persist after app restart
- [ ] Tutorial replay works
- [ ] UI responsive

---

**Next:** M2-003-game-scene-hud
