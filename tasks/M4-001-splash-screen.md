# M4-001: Splash Screen

**Status:** ✅ Done
**Priority:** P2
**Assigned:** orchestrator
**Estimated:** 2h
**Started:** 2026-06-08
**Completed:** 2026-06-08
**Milestone:** M4 — Release Polish

## Completed Work

- [x] `SplashScreen.cs` with async scene loading
- [x] Fade in/out animations
- [x] Loading bar progress
- [x] Minimum display time (2s)
- [x] Transitions to MainMenu

## Description

Branded splash screen with fast load into menu.

## Acceptance Criteria

- [ ] Splash screen on app launch
- [ ] Logo/brand visible
- [ ] Loading indicator
- [ ] Transitions to Main Menu
- [ ] Cold start <3 seconds
- [ ] App icon included

## Technical Notes

- First scene in build settings
- Async load next scene
- Consider animated logo

## Artifacts Required

- `Assets/_Game/Scenes/Splash.unity`
- App icon: `Assets/_Game/Sprites/icon.png`

## QA Checklist

- [ ] Splash displays
- [ ] Load time <3s
- [ ] Icon visible on device

---

**Next:** M4-002-how-to-play
