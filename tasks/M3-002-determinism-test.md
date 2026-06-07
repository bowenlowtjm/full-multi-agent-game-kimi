# M3-002: Determinism Test

**Status:** Backlog
**Priority:** P2
**Assigned:** —
**Estimated:** 3h
**Milestone:** M3 — Balance & Robustness

## Description

Verify that the same seed + input timeline produces identical game results every time.

## Acceptance Criteria

- [ ] Record input timeline during bot play
- [ ] Replay with same seed and recorded inputs
- [ ] Verify: identical score at end
- [ ] Verify: identical combo history
- [ ] Verify: identical spawn sequence

## Technical Notes

- Determinism requires:
  - Seeded RNG (no unseeded Random calls)
  - Fixed timestep
  - No frame-dependent logic
  - Ordered event processing

## Dependencies

- M3-001: Bot player for input recording

## Artifacts Required

- `Assets/Tests/PlayMode/DeterminismTests.cs`
- Input recording format

## QA Checklist

- [ ] Replay produces identical results
- [ ] 5 runs with same seed = identical
- [ ] Test passes in CI

---

**Next:** M3-003-balance-tuning
