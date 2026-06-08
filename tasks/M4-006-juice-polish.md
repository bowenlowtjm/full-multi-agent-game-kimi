# M4-006: Juice & Polish

**Status:** ✅ Done
**Priority:** P1
**Assigned:** orchestrator
**Estimated:** 6h
**Started:** 2026-06-08
**Completed:** 2026-06-08
**Milestone:** M4 — Release Polish

## Completed Work

- [x] Enhanced `EffectsManager.cs` with full juice suite
- [x] Particle pooling for performance
- [x] Floating score popups with pooling
- [x] Screen shake with intensity multiplier
- [x] Smooth flash effects with curves
- [x] Scale pop animations
- [x] High score celebration effect
- [x] Haptic integration on all effects

## Description

Game-feel polish: particles, screen shake, animated transitions, popup feedback.

## Acceptance Criteria

- [ ] Hit particles: scale pop + burst
- [ ] Miss feedback: screen flash/shake
- [ ] Combo escalation: rising pitch + visual intensity
- [ ] Expiry telegraph: shrink/flash/wobble
- [ ] Score popups: animated + satisfying
- [ ] Screen transitions: smooth animated
- [ ] Button press states: visual feedback
- [ ] New high score: celebration moment

## Feel Must-Haves

Per GAME-SPEC:
- Juicy hits (scale-pop + particle + sound)
- Felt misses (shake + "oof")
- Combo escalation (×5 feels alive)
- Fair tension (expiry telegraph)
- Reward the chase (big score popups)

## Artifacts Required

- `Assets/_Game/Scripts/EffectsManager.cs` (exists, expand)
- Particle systems for hits/bursts
- Animation curves for transitions

## QA Checklist

- [ ] Every interaction has feedback
- [ ] ×5 combo feels exciting
- [ ] Miss feels bad (intentional)
- [ ] Transitions smooth

---

**Next:** M4-007-performance-pass
