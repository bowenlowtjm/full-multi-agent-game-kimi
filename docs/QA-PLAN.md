# QA Plan — Pully Arcade Gesture Game

**Role:** Independent QA Gate (qa agent)  
**Config:** B (Hermes role-team)  
**Rung:** L3 (autonomous merge gate - no human oversight required)  
**Authority:** I am the merge gate. A FAIL blocks merge until resolved.

---

## 1. QA Workflow

### 1.1 When QA Runs

| Trigger | Action |
|---------|--------|
| PR to `main` | Full QA verification before merge approval |
| Milestone push (M1/M2/M3/M4 exit) | Complete milestone sign-off |
| Worker sets task `in-review` | Pre-merge verification for that task's scope |
| `qa-required` label on PR | Priority QA run |

### 1.2 QA Process Flow

```
Worker marks task in-review / opens PR
              ↓
    QA receives notification
              ↓
    ┌─────────────────────┐
    │   VERIFY CHECKS     │
    │  ├─ Compile check   │
    │  ├─ CI green        │
    │  ├─ EditMode tests  │
    │  ├─ PlayMode tests  │
    │  └─ Playability*    │
    └─────────────────────┘
              ↓
    ┌─────────────────────┐
    │   WRITE REPORT      │
    │  Append to run-log  │
    │  Post to Discord    │
    └─────────────────────┘
              ↓
    ┌─────────────────────┐
    │   DECISION          │
    │  ✅ PASS → Merge OK │
    │  ❌ FAIL → Bounce   │
    └─────────────────────┘
```

*Playability checks apply from M2 onwards; M1 focuses on core loop correctness.

### 1.3 Evidence Requirements

Every QA report MUST include:
- [ ] Unity check result (CLEAN or error output)
- [ ] CI status (URL to workflow run)
- [ ] Test counts (passed/failed/skipped)
- [ ] Coverage % (if regression)
- [ ] Playability notes (from M2)
- [ ] Specific repro steps for any failure

---

## 2. Verification Tiers

### Tier 1: Error-Free (M1 Minimum)
| Check | Method | Pass Criteria |
|-------|--------|---------------|
| Headless Compile | `scripts/unity-check.sh` | Exit 0, no CS errors |
| Console Clean | CI logs scan | No errors, no new warnings |
| CI Green | GitHub Actions | EditMode + PlayMode pass |
| Test Coverage | CI artifacts | ≥80% core logic, no regression |
| No Skipped Tests | Test output grep | No `[Ignore]` or `[Skip]` found |

### Tier 2: Functional Correctness (M1 Exit)
| Check | Method | Pass Criteria |
|-------|--------|---------------|
| 5 Gestures Work | EditMode tests | All gesture types validate |
| Ruleset Loading | Unit test | SO loads, data matches spec |
| Scoring Math | Unit test | Combo ×1.1, capped at ×5 |
| Seeded RNG | Unit test | Same seed → identical sequence |
| Input→Score Pipeline | PlayMode test | Full integration validates |
| PRD V01 | PlayMode test | Wrong gesture breaks combo |
| PRD V04 | Unit test | High score persists |

### Tier 3: Playability (M2+ Milestones)
| Check | Method | Pass Criteria |
|-------|--------|---------------|
| Smoke Flow | Manual/bot | Menu → Game → Game Over → Menu |
| HUD Updates | Visual verify | Score/Combo/Lives reflect state |
| All Gestures Mapped | Bot test | Each gesture hits correct target |
| No Softlocks | Bot 10× sessions | 0 hangs, can always reach Game Over |
| FPS Stable | Bot harness | ≥60 FPS with 10+ targets (V03) |
| Release Polish | Checklist per spec/GAME-SPEC.md | No placeholders, full flow |

---

## 3. QA Report Template

```markdown
## QA Report — [Date] — [Commit SHA]

### Scope
- Milestone: M[X]
- PR: #[number] (if applicable)
- Changed: [files/components]

### Checks Performed

#### 1. Compile Check
```bash
$ scripts/unity-check.sh
[unity-check] live Editor on :8090 — refreshing
[unity-check] CLEAN
```
Result: ✅ PASS

#### 2. CI Status
- URL: [GitHub Actions link]
- EditMode: [X] passed, [0] failed
- PlayMode: [X] passed, [0] failed
Result: ✅ PASS

#### 3. Test Coverage
- Core logic: [X]%
- Previous: [X]%
- Regression: [No/Yes - details]
Result: ✅ PASS

#### 4. Functional Verification
| Test | Expected | Actual | Status |
|------|----------|--------|--------|
| [test name] | [expected] | [actual] | ✅/❌ |

#### 5. Playability (if applicable)
[Bot results or manual notes]

### Decision
- [ ] **PASS** — Merge approved
- [ ] **FAIL** — Blocked, see below

### Blockers (if FAIL)
1. [Issue description]
   - Repro: [steps]
   - Severity: [Blocker/High/Medium]
   - Owner: [worker to fix]

### New GOTCHAs
[Any new trap discovered promoted to GOTCHAS.md]
```

---

## 4. Anti-Patterns to Prevent

QA exists to catch these specific failures:

1. **Rubber-stamping** — Approving without running checks
2. **Red CI acceptance** — Never merge on red; demand fix or justification
3. **Deleted/skipped tests** — Coverage regression is a FAIL
4. **Happy-path only** — Must test wrong gestures, misses, expiry edges
5. **"It compiles" = playable** — A build with no juice or softlocks FAILS
6. **No evidence** — Reports without artifacts/URLs are invalid
7. **L3 override** — At L3, I cannot escalate to human; unresolved blockers = autonomy failure

---

## 5. Communication

- **PASS reports:** Append to `docs/run-log.md`, brief Discord post
- **FAIL reports:** Immediate Discord ping to worker + orchestrator
- **Blockers >4 hours:** Escalation protocol (L3 = log as autonomy failure)

---

*Plan version: 1.0*  
*Created by: qa agent*  
*Updated: [auto-updated per run]*
