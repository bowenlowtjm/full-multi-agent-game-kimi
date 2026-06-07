# Pre-M1 QA Checklist — Core Loop Verification

**Milestone:** M1 — Core Loop  
**Status:** 🔴 Not Started (awaiting implementation)  
**QA Role:** Independent verification gate — I did not write this code  
**Authority:** L3 autonomous merge gate

---

## M1 Exit Criteria Summary

Per TASK-INDEX.md, M1 consists of 7 tasks:
- M1-001: Gesture Recognizer (5 gestures)
- M1-002: Ruleset Definition (SO-based data)
- M1-003: Scoring System (combo, lives, persistence)
- M1-004: Target Spawner (seeded RNG)
- M1-005: Input Manager (touch/mouse → target)
- M1-006: EditMode Tests (unit tests)
- M1-007: PlayMode Test (integration test)

**M1 is complete when:** All 7 tasks done, CI green, QA sign-off.

---

## Pre-M1 QA Verification Checklist

### A. Build & Compilation (Gate: Must Pass)

| # | Check | Method | Pass Criteria | Status |
|---|-------|--------|---------------|--------|
| A1 | Headless compile | `scripts/unity-check.sh` | Exit 0, "CLEAN" output | ⬜ |
| A2 | No console errors | CI log scan | Zero CS errors | ⬜ |
| A3 | No new warnings | CI log scan | No warning spam introduced | ⬜ |
| A4 | CI workflow green | GitHub Actions | Both EditMode + PlayMode pass | ⬜ |
| A5 | No deleted tests | `git diff` | Test count ≥ previous | ⬜ |
| A6 | Coverage maintained | CI artifacts | ≥80% core logic classes | ⬜ |

### B. Gesture Recognition (M1-001)

| # | Check | Test Method | Pass Criteria | Status |
|---|-------|-------------|---------------|--------|
| B1 | Single tap detected | `GestureRecognizerTests` | Event fired, position correct | ⬜ |
| B2 | Double tap detected | `GestureRecognizerTests` | Two taps <300ms detected | ⬜ |
| B3 | Long press detected | `GestureRecognizerTests` | Hold >500ms triggers | ⬜ |
| B4 | Swipe-tap detected | `GestureRecognizerTests` | Flick movement recognized | ⬜ |
| B5 | Two-finger tap detected | `GestureRecognizerTests` | Multi-touch event fires | ⬜ |
| B6 | No false positives | Unit test | Wrong gesture near threshold = no event | ⬜ |
| B7 | Touch + mouse both work | PlayMode test | Input works in editor and mobile | ⬜ |
| B8 | Performance | Profiler/metrics | <0.5ms per frame processing | ⬜ |

### C. Ruleset Definition (M1-002)

| # | Check | Test Method | Pass Criteria | Status |
|---|-------|-------------|---------------|--------|
| C1 | SO loads at runtime | `RulesetDefinitionTests` | Asset not null, fields populated | ⬜ |
| C2 | Shape enum exists | Code inspection | Circle, Square, Triangle, Star defined | ⬜ |
| C3 | Color palette defined | Visual/inspector | 4 distinct colors mapped | ⬜ |
| C4 | Mapping table correct | Unit test | Matches PRD spec (see below) | ⬜ |
| C5 | Spawn rates configured | Inspector/asset | Per-type probabilities set | ⬜ |
| C6 | Seeded RNG | Unit test | Same seed → identical sequence | ⬜ |
| C7 | Tooltips/descriptions | Inspector check | All fields documented | ⬜ |

**Expected Mapping (from M1-002 task):**
| Shape | Color | Gesture | Base Score |
|-------|-------|---------|------------|
| Circle | Blue | Single tap | 100 |
| Square | Gold | Double tap | 200 |
| Triangle | Green | Long press | 150 |
| Star | Purple | Swipe-tap | 250 |

### D. Scoring System (M1-003)

| # | Check | Test Method | Pass Criteria | Status |
|---|-------|-------------|---------------|--------|
| D1 | Base × combo math | `ScoreCalculatorTests` | Formula correct per hit | ⬜ |
| D2 | Combo increments | Unit test | +1 per consecutive correct | ⬜ |
| D3 | Combo multiplier | Unit test | ×1.1 per level, capped at ×5 | ⬜ |
| D4 | Miss resets combo | Unit test | Wrong gesture → combo = 0 | ⬜ |
| D5 | Lives decrement | Unit test | Miss → lives-- | ⬜ |
| D6 | Game over at 0 lives | Unit test | lives=0 triggers GAME_OVER | ⬜ |
| D7 | High score persists | Unit test | PlayerPrefs/file storage works | ⬜ |
| D8 | Score events fire | Event inspection | OnScoreChanged, OnComboChanged work | ⬜ |

### E. Target Spawner (M1-004)

| # | Check | Test Method | Pass Criteria | Status |
|---|-------|-------------|---------------|--------|
| E1 | Spawn interval works | PlayMode test | Targets spawn at configured rate | ⬜ |
| E2 | Seeded RNG deterministic | Unit test | Same seed = same sequence | ⬜ |
| E3 | Weighted probabilities | Unit test | Spawn rates match ruleset | ⬜ |
| E4 | Expiry timer | Visual/test | Targets disappear after timeout | ⬜ |
| E5 | Expiry triggers miss | Integration test | Miss penalty on expiry | ⬜ |
| E6 | Max concurrent limit | Load test | No more than N targets at once | ⬜ |
| E7 | Safe spawn zone | Visual/test | Targets within 5%-95% bounds | ⬜ |
| E8 | No GC spikes | Profiler | Object pooling or efficient instancing | ⬜ |

### F. Input Manager (M1-005)

| # | Check | Test Method | Pass Criteria | Status |
|---|-------|-------------|---------------|--------|
| F1 | Raycast finds target | Unit/mock | Screen point → target hit | ⬜ |
| F2 | Gesture validation | Integration test | Correct gesture → hit | ⬜ |
| F3 | Wrong gesture = miss | Integration test | Wrong gesture → miss penalty | ⬜ |
| F4 | Miss (no target) | PlayMode test | Touch nowhere → miss | ⬜ |
| F5 | Touch works | Device test | Android touch functional | ⬜ |
| F6 | Mouse works | Editor test | Mouse input functional | ⬜ |
| F7 | No input lag | Perception/frame | Immediate response to input | ⬜ |

### G. EditMode Tests (M1-006)

| # | Check | Method | Pass Criteria | Status |
|---|-------|--------|---------------|--------|
| G1 | ScoreCalculatorTests | Test runner | All combo scenarios covered | ⬜ |
| G2 | GestureRecognizerTests | Test runner | All 5 gestures validated | ⬜ |
| G3 | RulesetDefinitionTests | Test runner | Data loading validated | ⬜ |
| G4 | TargetTests | Test runner | Lifecycle/expiry tested | ⬜ |
| G5 | Coverage ≥80% | CI report | Core classes hit threshold | ⬜ |
| G6 | All tests deterministic | Re-run | Same results every run | ⬜ |
| G7 | No test-only code | Code review | Clean separation | ⬜ |

### H. PlayMode Integration (M1-007)

| # | Check | Method | Pass Criteria | Status |
|---|-------|--------|---------------|--------|
| H1 | Test scene loads | `[UnityTest]` | Scene loads with all managers | ⬜ |
| H2 | Spawn target with known props | Test script | Target spawned, properties set | ⬜ |
| H3 | Simulate correct gesture | Mock input | Score increases, combo up | ⬜ |
| H4 | Simulate wrong gesture | Mock input | Combo reset, lives down | ⬜ |
| H5 | Headless CI execution | GitHub Actions | Runs without GUI | ⬜ |
| H6 | Test completes <30s | Timing | Fast execution | ⬜ |
| H7 | Scene cleanup | Post-test | No persisted modifications | ⬜ |

### I. PRD Verification (From PRD Section 7)

| # | Verification | Method | Pass Criteria | Status |
|---|--------------|--------|---------------|--------|
| I1 | V01: Tap on Charge breaks combo | PlayMode test | Wrong gesture → combo reset | ⬜ |
| I2 | V04: High score persists | Unit/PlayMode test | Score survives app restart | ⬜ |

**Note:** V02 and V03 require M2 features (Trash bin zone, dense spawning)

---

## Sign-Off

M1 CANNOT EXIT until all checkboxes above are ✅.

| Reviewer | Date | Decision | Notes |
|----------|------|----------|-------|
| qa agent | [date] | ⬜ PASS / ⬜ FAIL | [findings] |

---

## Post-M1 QA Actions

Upon PASS:
1. Update TASK-INDEX.md: M1 status → ✅ Complete
2. Append QA report to `docs/run-log.md`
3. Post summary to Discord #qa channel
4. Unblock M2 tasks for assignment

Upon FAIL:
1. Document specific failing check(s) above
2. Create bounce-back report with repro steps
3. Assign to owning worker
4. Update TASK-INDEX.md with blocker note
5. Promote any new trap to `GOTCHAS.md`

---

*Checklist version: 1.0*  
*Created by: qa agent*  
*Reference: qa.SKILL.md, PRD.md Section 7, TASK-INDEX.md*
