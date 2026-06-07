# M2-006: Sprite Generation

**Status:** Backlog
**Priority:** P2
**Assigned:** game-art
**Estimated:** 6h
**Milestone:** M2 — Menus + Art + Build

## Description

Generate 2D sprites for all target shapes, UI elements, and FX. Create consistent style per DESIGN.md.

## Acceptance Criteria

- [ ] 4 target sprites (Circle, Square, Triangle, Star)
- [ ] Each shape in its ruleset color
- [ ] Distinct silhouette for each shape (accessibility)
- [ ] UI: buttons, frames, icons (hearts, stars)
- [ ] FX: hit burst, miss flash, combo popup
- [ ] Consistent art style across all sprites
- [ ] High contrast, readable at small sizes

## Art Style

Theme: **"Critters"** — each shape is a little creature with:
- Idle blink animation (frames)
- Panic expression as expiry approaches
- Happy reaction on hit
- Squash when tapped

## Technical Notes

- PNG format with transparency
- Power-of-two dimensions where possible
- Consistent pixels-per-unit (recommend 100)
- Save source files (AI/PSD) in `Assets/_Game/Sprites/Source/`

## Dependencies

- M1-002: Know the 4 shapes and colors
- DESIGN.md: Style guide

## Artifacts Required

- `Assets/_Game/Sprites/Targets/` — 4 shape sprites + color variants
- `Assets/_Game/Sprites/UI/` — buttons, frames, icons
- `Assets/_Game/Sprites/FX/` — particles, flashes
- Updated `DESIGN.md` with style rationale

## QA Checklist

- [ ] All shapes distinguishable by silhouette alone
- [ ] Colors match ruleset
- [ ] No placeholder primitives remain
- [ ] Style consistent across all assets

---

**Next:** M2-007-sprite-atlas
