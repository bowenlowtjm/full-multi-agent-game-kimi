# M4 Art Requirements — game-art Plan

**Run ID:** pully-B-L4-20260608  
**Role:** game-art  
**Task Focus:** M4-001, M4-003, M4-004, M4-006, M4-008  
**Last Updated:** 2026-06-08

---

## Art Deliverables Summary

| Task | Art Needed | Format | Est. Time |
|------|------------|--------|-----------|
| M4-001 Splash | Logo, loading spinner | PNG/SVG | 2h |
| M4-003 Music | BGM loop track | MP3/OGG | 4h |
| M4-004 SFX | 8-10 sound effects | WAV/MP3 | 3h |
| M4-006 Juice | Particle textures, anim curves | PNG + code | 4h |
| M4-008 Colorblind | Pattern overlays, simulator test | PNG/Code | 2h |

---

## M4-001: Splash Screen Art

### Requirements
- **Logo:** "PULLY" wordmark, arcade style
- **Resolution:** 1024x1024 for app icon, 1920x1080 for splash
- **Style:** Match existing target art (flat neon, arcade aesthetic)
- **Colors:** Dark background (#1A1A2E), neon accent (#00D4FF)
- **Loading Indicator:** Animated spinner or progress bar

### Assets to Produce
- `Assets/_Game/Sprites/icon.png` (1024x1024, app icon)
- `Assets/_Game/Sprites/splash_logo.png` (1920x1080)
- `Assets/_Game/Sprites/loading_spinner.png` (128x128, animated frames)

---

## M4-003: Music Loop

### Requirements
- Tempo: 120-140 BPM (energetic arcade)
- Duration: 30-60 second loop
- Mood: Upbeat, tense enough for arcade action
- Layers: Base loop + optional intensity layer for high combo

### Technical
- Format: OGG Vorbis (Unity preferred for streaming)
- Loop: Seamless
- Memory: <5MB compressed
- Pulling from PRD: No specific music requirements, use arcade-style synth

---

## M4-004: SFX Set

### Required Sounds (8 sounds)
| Event | Sound | Priority |
|-------|-------|----------|
| Target Hit | Bright pop/chime | P0 |
| Combo Up | Rising arpeggio | P0 |
| Combo Broken | Descending tone | P0 |
| Miss | Dull thud/whoosh | P0 |
| Target Expired | Warning tick | P1 |
| Button Press | UI click | P1 |
| Game Over | Big chord/swell | P1 |
| New High Score | Victory fanfare | P2 |

### Technical
- Format: WAV (short sounds) / MP3 (music)
- Samples: 44.1kHz, 16-bit
- Max duration: Button=0.1s, Music=60s

---

## M4-006: Juice & Polish FX

### Particle Effects
| FX | Description | Implementation |
|----|-------------|----------------|
| Hit Burst | Scale-pop + particle burst at target | Shuriken + Sprite |
| Combo Lines | Rising spark trail | Trail renderer |
| Expiry Telegraph | Pulsing red warning | Animation + Shader |
| Score Popup | Floating + rising text | Text mesh + animation |
| Screen Shake | Subtle on miss | Camera animation |
| Transition Swipe | Between scenes | UI mask + animation |

### EffectsManager.cs Extensions
Per GAME-SPEC feel requirements:
```
- Juicy hits: scale-pop + burst particle + sound
- Felt misses: screen shake + "oof"
- Combo escalation: ×5 feels ALIVE (particles + pitch rise)
- Fair tension: expiry telegraph (pulsing red)
- Reward the chase: big animated score popups
```

### Art Assets Needed
- `particle_burst.png` — 256x256 burst pattern
- `particle_spark.png` — 64x64 spark
- `hit_ring.png` — 128x128 expanding ring

---

## M4-008: Colorblind Mode

### Implementation
- Pattern overlays on targets (not just color)
- TG-01 POP: Circle shape + dots pattern
- TG-02 HEAVY: Square shape + stripes pattern
- TG-03 CHARGE: Anchor shape + waves pattern
- TG-04 TRASH: Zigzag shape + crosshatch pattern

### Patterns to Generate
- `pattern_dots.png` (overlay for POP)
- `pattern_stripes.png` (overlay for HEAVY)
- `pattern_waves.png` (overlay for CHARGE)
- `pattern_crosshatch.png` (overlay for TRASH)

---

## DESIGN.md Reference

Color Palette (must maintain consistency):
- POP (Blue Sphere): #00D4FF, #0077B6
- HEAVY (Gold Square): #FFD700, #FFA500
- CHARGE (Green Anchor): #00FF88, #2E8B57
- TRASH (Red Line): #FF4444, #CC0000

---

## Integration Notes for Unity

1. Audio: Add AudioMixer for music/SFX separation
2. Particles: Use Shuriken system withobject pooling
3. Animations: Use DOTween or Unity Animation (decided in code)
4. Patterns: Use sprite overlay or shader-based

---

*Role: game-art | Status: Ready to support M4-006-VFX implementation*
