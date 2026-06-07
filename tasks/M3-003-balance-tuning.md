# M3-003: Balance Tuning

**Status:** Backlog
**Priority:** P2
**Assigned:** game-pm
**Estimated:** 4h
**Milestone:** M3 — Balance & Robustness

## Description

Tune spawn rates, difficulty curve, and target distribution for optimal gameplay feel.

## Acceptance Criteria

- [ ] Spawn rate escalates over 60 seconds
- [ ] Difficulty feels "fair but challenging"
- [ ] Score distribution: reasonable min/max
- [ ] No target type dominates
- [ ] Combo achievable but not trivial
- [ ] Player can reach ×5 combo with skill

## Metrics

- Average score range: 2000-8000
- Combo ×5 achievable in 30% of sessions
- Miss rate: 10-30% (varies by skill)
- No "impossible" target sequences

## Dependencies

- M3-001: Bot data for analysis

## Artifacts Required

- Updated `RulesetDefinition` with tuned values
- Balance analysis in `docs/balance.md`

## QA Checklist

- [ ] Bot scores within target range
- [ ] No degenerate spawn patterns
- [ ] Difficulty curve feels right

---

**Next:** M3-004-gameplay-harness
