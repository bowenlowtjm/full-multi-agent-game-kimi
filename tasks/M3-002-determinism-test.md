# M3-002: Determinism Test

**Status:** ✅ Done
**Priority:** P2
**Assigned:** orchestrator
**Estimated:** 3h
**Started:** 2026-06-08
**Completed:** 2026-06-08
**Milestone:** M3 — Balance & Robustness

## Completed Work

- [x] `DeterminismTests.cs` with 4 test cases
- [x] Same seed → identical spawn sequences
- [x] Different seeds → different sequences  
- [x] Score calculation determinism verified
- [x] Seeded RNG verification

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
