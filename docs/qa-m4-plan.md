# M4 QA Plan — Quality Assurance Strategy

**Run ID:** pully-B-L4-20260608  
**Role:** qa  
**Focus:** M4-007, M4-009 (Final Quality Gates)  
**Last Updated:** 2026-06-08

---

## M4 QA Scope

### Tasks Under QA
| Task | QA Focus | Method |
|------|----------|--------|
| M4-007 Performance Pass | 60fps across devices | Automated + manual |
| M4-009 Uninstall Test | ≥8/10 gameplay quality | Structured playtest |

### Regressions to Check
- M1: Gesture recognition still accurate
- M2: Menus, pause, game-over flows intact
- M3: Bot player still passes determinism tests
- M2: All 33 sprites display correctly

---

## Entry/Exit Criteria

### Entry (Before M4 Work)
- [ ] M1-M3 QA complete and passed
- [ ] No P0/P1 bugs open
- [ ] CI build green
- [ ] Smoke test passes

### Per-Task QA Gates

| Task | Entry Criteria | QA Checklist | Exit Criteria |
|------|---------------|--------------|---------------|
| M4-006 | FX Assets ready | FX trigger correctly, no console errors, 60fps maintained | All FX reviewed |
| M4-004 | SFX assets ready | All sounds play, volume balanced, no distortion | Audio QA pass |
| M4-003 | Music asset ready | Loops seamless, mute toggle works | Music QA pass |
| M4-005 | Tap targets work | Haptics fire on hit/miss, respect settings | Haptics verified on device |
| M4-002 | Tutorial built | All 5 gestures explained, skippable | Tutorial QA pass |
| M4-001 | Splash scene ready | Loads <3s, transitions correctly | Splash QA pass |
| M4-007 | M4-006 complete | Profiling results, device matrix tested | 60fps on target devices |
| M4-008 | Visual polish done | Patterns visible, colorblind sim tested | Accessibility QA pass |

---

## M4-007: Performance Pass QA

### Test Matrix

| Device | Spec | Target | Min Accepted |
|--------|------|--------|--------------|
| Samsung Galaxy A52 | Mid-range Android | 60fps | 55fps avg |
| Pixel 7 | High-end Android | 60fps | 58fps avg |
| Simulator | Editor / CI | 60fps | 60fps |

### Profiling Checklist
- [ ] CPU usage <50% during 10-target spawn
- [ ] GPU time <16ms per frame
- [ ] No GC spikes during gameplay
- [ ] Memory usage stable (no leaks)
- [ ] Battery drain acceptable (<10% per 15min)

### Performance Scenarios
1. Menu idle: 60fps, low CPU
2. Gameplay start: 60fps maintained
3. Max targets (10+): 60fps maintained
4. Heavy FX (×5 combo + particles): 60fps maintained
5. Scene transitions: No stutter

---

## M4-009: Uninstall Test QA

### The Quality Gate
**Criterion:** ≥8/10 average rating from playtesters

### Playtest Protocol

#### Phase 1: First Launch (5 min)
- Cold start time measurement
- Splash impression
- Tutorial clarity
- Menu navigation ease

#### Phase 2: First Game (5 min)
- Gesture learning curve
- Feedback clarity (hit/miss understanding)
- Difficulty feeling appropriate
- No confusion about UI

#### Phase 3: Regular Play (10 min)
- Flow state achieved
- Combo mechanics satisfying
- Determinism test (same run twice)
- Score feels meaningful

#### Phase 4: Exit Interview
1. Rate 1-10: Would you keep this game installed?
2. Best part?
3. Most confusing part?
4. Suggested improvements?

### Scoring Rubric

| Aspect | Points | Criteria |
|--------|--------|----------|
| Tutorial | 1 | Explains all 5 gestures clearly |
| Feel | 2 | Hit/miss feedback is satisfying |
| Flow | 2 | Can reach "in the zone" state |
| Fair | 2 | Deaths feel fair, not random |
| Polish | 2 | No jank, smooth 60fps |
| Want More | 1 | Player wants to play again |

**Total: 10 points → Need ≥8 average across 3+ testers**

---

## Automated QA Tests

### CI Tests (Run on every build)
```
✓ All EditMode tests pass
✓ All PlayMode tests pass
✓ Bot player completes game without errors
✓ Determinism test passes
✓ Compilation error-free
```

### Device Tests (Manual or automated)
```
✓ Splash → Menu transition
✓ Menu → Game transition
✓ Pause/Resume flow
✓ Game Over → Retry flow
✓ Settings toggles persist
✓ Audio mute/unmute
✓ Haptics on/off
✓ Background music loops
```

---

## Regression Checklist

Verify these still work after M4 changes:

- [ ] M1-001: All 5 gestures recognized
- [ ] M1-002: Ruleset loads correctly
- [ ] M1-003: Scoring/combo works
- [ ] M1-004: Seeded spawns are deterministic
- [ ] M2-001: Main menu displays
- [ ] M2-003: HUD shows score/combo/lives
- [ ] M2-004: Pause/resume functional
- [ ] M2-005: Game over/retry functional
- [ ] M2-006: All 33 sprites visible
- [ ] M3-001: Bot player runs
- [ ] M3-002: Determinism test passes

---

## QA Sign-off

Milestone M4 accepted when:
- [ ] All M4 tasks QA verified
- [ ] Regression tests pass
- [ ] M4-007: 60fps on target devices
- [ ] M4-009: ≥8/10 uninstall test score
- [ ] No P0/P1 bugs open
- [ ] CI green
- [ ] APK builds successfully

---

*Role: qa | Status: Standing by for M4 work submissions*
