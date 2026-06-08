# M4 Polish Milestone — PM Execution Plan

**Run ID:** pully-B-L4-20260608  
**Role:** game-pm  
**Status:** ACTIVE — M4 Execution  
**Last Updated:** 2026-06-08

---

## M4 Task Overview

9 tasks across 4 workstreams. Grouped by dependencies and integration points.

### Critical Path (Blocking)
| Task | Priority | Hours | Dependencies |
|------|----------|-------|--------------|
| M4-006 Juice & Polish | P1 | 6h | — |
| M4-004 SFX Set | P2 | 4h | M4-006 (VFX sync) |
| M4-005 Haptics | P2 | 3h | M4-006 |
| M4-009 Uninstall Test | P0 | 2h | ALL M4 tasks |

### Parallel Workstreams

#### Stream A: Visual Polish (game-art orchestrator coordination)
- **M4-006 Juice & Polish** — particles, screen shake, anims
- **M4-008 Colorblind Mode** — accessibility overlay

#### Stream B: Audio (game-art / audio specialist)
- **M4-003 Music Loop** — BGM with mute toggle
- **M4-004 SFX Set** — hit/miss/combo/UI sounds

#### Stream C: Player Experience
- **M4-001 Splash Screen** — branded boot
- **M4-002 How-to-Play** — skippable tutorial
- **M4-007 Performance Pass** — 60fps optimization
- **M4-005 Haptics** — mobile vibration

---

## Recommended Execution Order

### Phase 1: Foundation (Day 1)
1. **M4-006 Juice & Polish** — Paves way for all FX-dependent work
2. **M4-007 Performance Pass** — Establish baseline before adding FX overhead

### Phase 2: Audio Integration (Day 1-2)
3. **M4-004 SFX Set** — Sync with VFX events
4. **M4-003 Music Loop** — Layer under gameplay

### Phase 3: UX Enhancement (Day 2)
5. **M4-002 How-to-Play** — Tutorial flow
6. **M4-005 Haptics** — Mobile feel
7. **M4-008 Colorblind Mode** — Accessibility

### Phase 4: Entry Polish (Day 2)
8. **M4-001 Splash Screen** — First impressions
9. **M4-009 Uninstall Test** — Final quality gate

---

## Dependencies & Blockers

```
M4-006 Juice & Polish
  ├─→ M4-004 SFX (sync events)
  ├─→ M4-005 Haptics (sync events)
  └─→ M4-002 How-to-Play (tutorial animations)

M4-007 Performance Pass
  └─→ ALL (sets optimization baseline)

M4-003 Music Loop
  └─→ M4-004 SFX (audio mixing)

ALL → M4-009 Uninstall Test (final gate)
```

---

## Risk Assessment

| Risk | Likelihood | Impact | Mitigation |
|------|------------|--------|------------|
| Particle perf hit | Medium | High | Test on mid-range Android early |
| Audio latency on Android | Medium | Medium | Use simple AudioSource, not complex mixing |
| Splash adds 3s+ to launch | Low | Medium | Keep animation <2s, async load |
| Colorblind mode changes balance | Low | Medium | Test with colorblind simulator |

---

## Definition of Done for M4

- [ ] All P1-P2 tasks complete and QA verified
- [ ] App launches with splash + <3s cold start
- [ ] Audio: music + all SFX with mute toggles
- [ ] FX: every interaction has visual/haptic feedback
- [ ] Perf: 60fps sustained on mid-range Android
- [ ] Tutorial: new player can learn 5 gestures
- [ ] **M4-009 Uninstall Test: ≥8/10 average rating**

---

## QA Integration Points

- After M4-006: Visual FX QA checkpoint
- After M4-004: Audio sync QA checkpoint
- After M4-007: Performance baseline QA checkpoint
- Final: Full regression + M4-009 uninstall test

---

*Role: game-pm | Next: Awaiting orchestrator to begin M4-006*
