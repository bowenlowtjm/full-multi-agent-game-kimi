# M4-003: Music Loop

**Status:** Backlog
**Priority:** P2
**Assigned:** game-art
**Estimated:** 4h
**Milestone:** M4 — Release Polish

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
