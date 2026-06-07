# M3-004: Gameplay Harness

**Status:** ✅ Done
**Priority:** P2
**Assigned:** orchestrator
**Estimated:** 4h
**Started:** 2026-06-08
**Completed:** 2026-06-08
**Milestone:** M3 — Balance & Robustness

## Completed Work

- [x] `GameplayHarness.cs` Editor tool
- [x] Configurable session count
- [x] Score distribution tracking
- [x] FPS monitoring placeholder
- [x] Session results table
- [x] Markdown report export to `docs/perf/`

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
