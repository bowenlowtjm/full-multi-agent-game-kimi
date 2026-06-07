# M4-007: Performance Pass

**Status:** Backlog
**Priority:** P1
**Assigned:** —
**Estimated:** 4h
**Milestone:** M4 — Release Polish

## Description

Optimize for 60fps on mid-range Android devices.

## Acceptance Criteria

- [ ] Stable 60 FPS throughout gameplay
- [ ] Cold start <3 seconds
- [ ] No GC hitches
- [ ] Reasonable memory usage (<150MB)
- [ ] Battery efficient
- [ ] Tested on mid-range device

## Optimizations

- Sprite atlases (M2-007)
- Object pooling for targets
- Minimize garbage generation
- Efficient UI updates
- Shader optimization

## Artifacts Required

- Performance report
- Optimized code changes

## QA Checklist

- [ ] FPS ≥60 sustained
- [ ] No frame drops on busy scenes
- [ ] Memory stable
- [ ] Device doesn't overheat

---

**Next:** M4-008-colorblind-mode
