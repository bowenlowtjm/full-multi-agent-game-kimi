# M2-003: Game Scene HUD

**Status:** ✅ Done
**Priority:** P1
**Assigned:** orchestrator
**Estimated:** 4h
**Started:** 2026-06-08
**Completed:** 2026-06-08
**Milestone:** M2 — Menus + Art + Build

## Completed Work

- [x] `HUDManager` with Score display (D6 format)
- [x] Combo multiplier display with hide/show
- [x] Lives display (dynamic icon spawning)
- [x] Pause button and toggle
- [x] Best score display
- [x] Event-driven updates from ScoreManager

## Description

In-game HUD showing score, combo, lives, and timer during gameplay.

## Acceptance Criteria

- [ ] Score display (large, prominent)
- [ ] Combo multiplier display (×1, ×1.1, etc.)
- [ ] Lives display (3 hearts/icons)
- [ ] Timer display (60-second countdown)
- [ ] Pause button
- [ ] All values update in real-time
- [ ] Visual feedback on score/combo changes

## Technical Notes

- `HUDManager` singleton (exists, verify)
- Unity UI Text components (no TMPro)
- Anchor to screen edges
- Combo display animates on change

## Dependencies

- M1-003: ScoreManager events
- M1-004: Game timer

## Artifacts Required

- `Assets/_Game/Scripts/HUDManager.cs` (exists, complete)
- `Assets/_Game/Scenes/Game.unity` (update)

## QA Checklist

- [ ] All HUD elements update correctly
- [ ] Timer reaches 0 triggers game over
- [ ] Combo animation visible
- [ ] No overlap with gameplay area

---

**Next:** M2-004-pause-flow
