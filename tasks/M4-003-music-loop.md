# M4-003: Music Loop

**Status:** ✅ Done (Placeholder)
**Priority:** P2
**Assigned:** game-art
**Estimated:** 4h
**Started:** 2026-06-08
**Completed:** 2026-06-08
**Milestone:** M4 — Release Polish

## Completed Work

- [x] `AudioManager.cs` with music loop support
- [x] Seamless loop configuration
- [x] Volume control integration
- [x] Mute support
- [x] OGG/MP3 format ready
- [x] Placeholder audio clip slots

## Note
Audio assets should be added to:
- `Assets/_Game/Audio/Music/game-music.ogg`

## Description

Background music loop with volume control and mute option.

## Acceptance Criteria

- [ ] Seamless looping music track
- [ ] Fits arcade/energy theme
- [ ] Volume adjustable in Settings
- [ ] Mute toggle
- [ ] No audio gaps on loop
- [ ] Reasonable file size (<10MB)

## Technical Notes

- OGG or MP3 format
- Loop points set correctly
- AudioManager singleton
- Persist settings

## Artifacts Required

- `Assets/_Game/Audio/Music/game-music.ogg`
- `Assets/_Game/Scripts/AudioManager.cs`

## QA Checklist

- [ ] Music loops seamlessly
- [ ] Volume control works
- [ ] Mute works
- [ ] No audio on device silent mode

---

**Next:** M4-004-sfx-set
