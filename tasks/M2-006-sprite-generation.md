# M2-006: Sprite Generation

**Status:** In Progress  
**Priority:** P2  
**Assigned:** game-art  
**Estimated:** 6h (3h remaining for atlas + import)  
**Milestone:** M2 — Menus + Art + Build

## Description

Generate 2D sprites for all target shapes, UI elements, and FX. Create consistent style per DESIGN.md.

## Acceptance Criteria

- [x] 4 target sprites (Circle, Square, Triangle, Star)
- [x] Each shape in its ruleset color (Green, Red, Blue, Yellow, Purple)
- [x] Multiple expressions per shape (idle, panic, hit, squash)
- [x] UI: buttons, frames, icons (hearts, stars)
- [x] FX: hit burst, particle sprites
- [x] Consistent art style across all sprites
- [x] High contrast, readable at small sizes
- [ ] Import into Unity with correct settings
- [ ] Verify silhouette distinguishability

## Sprite Asset List (Complete)

### Targets: 25 sprites
- Circle (Green): idle_01, idle_02, panic, hit, squash
- Circle (Red): idle_01, idle_02, panic, hit, squash
- Square (Blue): idle_01, idle_02, panic, hit, squash
- Triangle (Yellow): idle_01, idle_02, panic, hit, squash
- Star (Purple): idle_01, idle_02, panic, hit, squash

### UI: 5 sprites
- button_play.png, button_settings.png, icon_heart_full.png, icon_heart_empty.png, frame_menu.png

### FX: 3 sprites
- particle_hit.png, particle_star.png, flash_hit.png

**Total: 33 sprites generated in `Assets/_Game/Sprites/...`**

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
