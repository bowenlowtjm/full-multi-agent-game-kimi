# M2-005: Game Over Flow

**Status:** Backlog
**Priority:** P1
**Assigned:** —
**Estimated:** 3h
**Milestone:** M2 — Menus + Art + Build

## Description

Game over screen with final score, best score comparison, and retry/menu options.

## Acceptance Criteria

- [ ] Appears when lives = 0 or time = 0
- [ ] Displays final score
- [ ] Displays best score
- [ ] "New High Score!" celebration if applicable
- [ ] "Retry" starts new game
- [ ] "Menu" returns to Main Menu
- [ ] High score saved if beaten

## Technical Notes

- `GameOverManager` singleton (exists, verify)
- Celebration: particle burst, sound, animation
- Best score read from PlayerPrefs

## Dependencies

- M1-003: ScoreManager for final score
- M2-001: Main Menu to return to

## Artifacts Required

- `Assets/_Game/Scripts/GameOverManager.cs` (exists)
- `Assets/_Game/Scenes/GameOver.unity` or overlay

## QA Checklist

- [ ] Game over triggers correctly
- [ ] High score persists
- [ ] Celebration plays for new best
- [ ] Buttons functional

---

**Next:** M2-006-sprite-generation
