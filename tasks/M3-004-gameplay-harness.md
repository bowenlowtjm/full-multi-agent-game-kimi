# M3-004: Gameplay Harness

**Status:** Backlog
**Priority:** P2
**Assigned:** —
**Estimated:** 4h
**Milestone:** M3 — Balance & Robustness

## Description

Automated gameplay quality checks: score distribution, FPS stability, and performance metrics.

## Acceptance Criteria

- [ ] Automated score distribution check
- [ ] FPS monitoring (must stay ≥60)
- [ ] Memory usage tracking
- [ ] GC hitch detection
- [ ] Report generation after bot runs
- [ ] Performance regression detection

## Technical Notes

- Unity Profiler API for metrics
- Custom profiler markers for key systems
- Automated comparison to baseline

## Dependencies

- M3-001: Bot player for data collection

## Artifacts Required

- `Assets/Editor/GameplayHarness.cs`
- Performance reports in `docs/perf/`

## QA Checklist

- [ ] FPS ≥60 throughout
- [ ] GC hitches <5 over 10 sessions
- [ ] Memory stable (no leaks)

---

**Milestone M3 Exit:**
- Bot runs complete
- Determinism verified
- Balance tuned
- Performance acceptable

**Next:** M4-001-splash-screen
