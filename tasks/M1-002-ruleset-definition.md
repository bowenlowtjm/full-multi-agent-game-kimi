# M1-002: Ruleset Definition

**Status:** ✅ Done
**Priority:** P0
**Assigned:** orchestrator
**Estimated:** 3h
**Started:** 2026-06-08
**Completed:** 2026-06-08
**Milestone:** M1 — Core Loop

## Description

Create a data-driven ruleset system using ScriptableObjects. The ruleset defines:
- Which shape + color combinations map to which required gesture
- Base reward values per target type
- Spawn probabilities and timing
- Combo multipliers
- Seeded RNG configuration

## Acceptance Criteria

- [ ] `RulesetDefinition` ScriptableObject exists with all required fields
- [ ] Shape enum: Circle, Square, Triangle, Star (4 types)
- [ ] Color palette defined (4 distinct colors matching gesture types)
- [ ] Mapping: shape×color → gesture + base score (see spec/RULESET.md)
| Shape | Color | Gesture | Base Score |
|-------|-------|---------|------------|
| Circle | Blue | Single tap | 100 |
| Square | Gold | Double tap | 200 |
| Triangle | Green | Long press | 150 |
| Star | Purple | Swipe-tap | 250 |
- [ ] Spawn rate, expiry time per target type
- [ ] Seeded RNG: deterministic replay support
- [ ] At least one `.asset` file created under `Assets/_Game/ScriptableObjects/`

## Technical Notes

- Already exists: `Assets/_Game/Scripts/RulesetDefinition.cs` (namespace fixed to Arcade.Game)
- Use Unity's ScriptableObject pattern
- Consider making reward data serializable for balancing
- Seed should be configurable per session

## Dependencies

- M1-001: needs GestureType enum defined

## Artifacts Required

- `Assets/_Game/ScriptableObjects/DefaultRuleset.asset`
- Sample ruleset data populated from spec/RULESET.md

## QA Checklist

- [ ] Ruleset can be loaded at runtime
- [ ] Data matches spec exactly (no hardcoded deviations)
- [ ] Seeded RNG produces identical sequences with same seed
- [ ] All SO fields have tooltips/descriptions

---

**Next:** M1-003-scoring-system (uses ruleset data)
