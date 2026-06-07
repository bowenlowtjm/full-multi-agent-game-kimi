# M2-004: Pause Flow

**Status:** Backlog
**Priority:** P1
**Assigned:** —
**Estimated:** 3h
**Milestone:** M2 — Menus + Art + Build

## Description

Pause menu overlay with resume, restart, settings, and quit options.

## Acceptance Criteria

- [ ] Pause button pauses game time
- [ ] Pause menu overlay appears
- [ ] "Resume" continues game
- [ ] "Restart" resets current game
- [ ] "Settings" opens settings overlay
- [ ] "Quit to Menu" returns to Main Menu
- [ ] Game state persists during pause

## Technical Notes

- `Time.timeScale = 0` for pause
- Modal overlay with dimmed background
- Handle Android back button as pause

## Dependencies

- M2-002: Settings accessible
- M2-003: Pause button in HUD

## Artifacts Required

- Pause UI overlay prefab
- Pause state management in GameStateManager

## QA Checklist

- [ ] Pause works mid-game
- [ ] Resume continues from exact state
- [ ] Restart clears and restarts
- [ ] Android back button works

---

**Next:** M2-005-game-over-flow
