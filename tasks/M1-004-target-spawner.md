# M1-004: Target Spawner

**Status:** Backlog  
**Priority:** P0  
**Assigned:** —  
**Estimated:** 5h  
**Milestone:** M1 — Core Loop

## Description

Implement the target spawning system. Spawns targets at intervals using seeded RNG, manages active target lifecycle, and handles expiry.

## Acceptance Criteria

- [ ] Spawns targets at configurable interval
- [ ] Uses seeded RNG from RulesetDefinition (deterministic)
- [ ] Shape/color selection weighted by spawn probabilities
- [ ] Targets have expiry timer (shrink/flash before disappearing)
- [ ] Maximum concurrent targets enforced
- [ ] On expiry: trigger miss penalty via ScoreManager
- [ ] Targets can be queried by position (for input raycasting)

## Technical Notes

- `SpawnerManager` singleton (already exists)
- Target prefab instantiated from pool (consider object pooling)
- Position: random within spawn bounds (screen-relative)
- Expiry visual feedback: scale down + flash red over last 2 seconds

## Dependencies

- M1-002: RulesetDefinition for spawn rates
- M1-003: ScoreManager for miss penalty

## Artifacts Required

- `Assets/_Game/Scripts/SpawnerManager.cs` (exists, needs completion)
- `Assets/_Game/Prefabs/Target.prefab` (needs creation)
- `Assets/_Game/Scripts/Target.cs` (exists, needs completion)

## QA Checklist

- [ ] Same seed produces identical spawn sequences
- [ ] Expiry triggers miss penalty
- [ ] Max concurrent targets respected
- [ ] Spawn area respects safe margins
- [ ] Performance: no GC spikes from instantiation

---

**Next:** M1-005-input-manager (uses target queries)
