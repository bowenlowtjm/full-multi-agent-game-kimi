# M2-007: Sprite Atlas

**Status:** Ready to Start  
**Priority:** P2  
**Assigned:** game-art  
**Estimated:** 2h  
**Milestone:** M2 — Menus + Art + Build

## Description

Pack all sprites into a Unity Sprite Atlas for optimal draw calls and memory.

## Acceptance Criteria

- [ ] Sprite Atlas created
- [ ] All target sprites packed
- [ ] UI sprites packed
- [ ] FX sprites packed
- [ ] Atlas is power-of-two dimensions
- [ ] Padding configured (4px recommended)
- [ ] Import settings correct (filter, PPU)
- [ ] No atlas warnings in Unity

## Technical Notes

- Use Unity's built-in Sprite Atlas (2D Sprite package)
- Filter mode: Point for pixel art, Bilinear for vector
- PPU: 100 (match sprite design)
- Compression: RGBA Compressed

## Dependencies

- M2-006: Sprites generated

## Artifacts Required

- `Assets/_Game/Sprites/Atlas.spriteatlas`

## QA Checklist

- [ ] All sprites in atlas
- [ ] Draw calls reduced (check Frame Debugger)
- [ ] No visual artifacts
- [ ] Memory usage reasonable

---

**Next:** M2-008-ci-apk-build
