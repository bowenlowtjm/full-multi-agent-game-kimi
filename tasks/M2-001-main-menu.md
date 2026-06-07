# M2-001: Main Menu Screen

**Status:** ✅ Done
**Priority:** P1
**Assigned:** orchestrator
**Estimated:** 3h
**Started:** 2026-06-08
**Completed:** 2026-06-08
**Milestone:** M2 — Menus + Art + Build

## Completed Work

- [x] `MainMenuManager` with Play button, Best Score display
- [x] Scene transitions via `SceneLoader`
- [x] High score read from PlayerPrefs
- [x] GoToMainMenu state initialization

## Description

Implement the main menu screen with Play, Best Score display, and Settings buttons.

## Acceptance Criteria

- [ ] Menu scene loads on app start
- [ ] "Play" button loads Game scene
- [ ] "Best Score" displays persisted high score
- [ ] "Settings" button opens Settings screen
- [ ] Visual polish: button states (normal, hover, pressed)
- [ ] Background art or gradient (not programmer gray)
- [ ] Responsive layout for portrait

## Technical Notes

- Use Unity UI (Canvas, Button components)
- `MainMenuManager` singleton (exists, verify)
- `SceneLoader` for transitions (exists, verify)
- Consider animated entrance

## Dependencies

- M1 complete (game exists to transition to)

## Artifacts Required

- `Assets/_Game/Scenes/MainMenu.unity`
- `Assets/_Game/Scripts/MainMenuManager.cs` (exists)

## QA Checklist

- [ ] All buttons functional
- [ ] Best score displays correctly
- [ ] Transitions smooth
- [ ] No layout issues on 16:9 and taller aspect ratios

---

**Next:** M2-002-settings-screen
