# M4-008: Colorblind Mode

**Status:** Backlog
**Priority:** P3
**Assigned:** game-art
**Estimated:** 3h
**Milestone:** M4 — Release Polish

## Description

Accessibility option for color vision deficiencies. Patterns/symbols distinguish targets, not just color.

## Acceptance Criteria

- [ ] Toggle in Settings
- [ ] Each shape has distinct internal pattern
- [ ] Patterns visible regardless of color
- [ ] Works for deuteranopia, protanopia, tritanopia
- [ ] Symbols don't hurt readability

## Implementation

- Add pattern overlay to sprites
- Or: replace sprites when mode enabled
- Patterns: dots, stripes, checkers, solid

## Artifacts Required

- `Assets/_Game/Sprites/Targets/Colorblind/` variants
- Settings toggle

## QA Checklist

- [ ] All shapes distinguishable
- [ ] Patterns don't hurt readability
- [ ] Toggle works

---

**Next:** M4-009-uninstall-test
