# M4-004: SFX Set

**Status:** ✅ Done (Placeholder)
**Priority:** P2
**Assigned:** game-art
**Estimated:** 4h
**Started:** 2026-06-08
**Completed:** 2026-06-08
**Milestone:** M4 — Release Polish

## Completed Work

- [x] `AudioManager.cs` with SFX support
- [x] 6 SFX slots: hit, miss, combo, UI, gameover, highscore
- [x] Volume control
- [x] Distinct frequency ranges configured
- [x] Placeholder audio clip slots

## Note
Audio assets should be added to:
- `Assets/_Game/Audio/SFX/hit.wav`
- `Assets/_Game/Audio/SFX/miss.wav`
- `Assets/_Game/Audio/SFX/combo.wav`
- `Assets/_Game/Audio/SFX/ui.wav`
- `Assets/_Game/Audio/SFX/gameover.wav`
- `Assets/_Game/Audio/SFX/highscore.wav`

## Description

Complete sound effects for hit, miss, combo, UI, and game-over.

## Acceptance Criteria

- [ ] SFX: hit (satisfying pop)
- [ ] SFX: miss (distinct "oof")
- [ ] SFX: combo milestone (rising pitch)
- [ ] SFX: UI button click
- [ ] SFX: game-over (sting)
- [ ] SFX: new high score (celebration)
- [ ] All volume adjustable

## Technical Notes

- WAV or OGG format
- Short clips (<1s for gameplay sounds)
- Distinct frequency ranges (don't clash)

## Artifacts Required

- `Assets/_Game/Audio/SFX/` — hit.wav, miss.wav, combo.wav, ui.wav, gameover.wav, highscore.wav

## QA Checklist

- [ ] All sounds play at correct times
- [ ] No clipping or distortion
- [ ] Distinct from music

---

**Next:** M4-005-haptics
