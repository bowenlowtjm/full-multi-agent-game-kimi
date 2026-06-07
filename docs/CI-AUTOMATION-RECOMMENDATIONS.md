# Automated CI Checks Recommendation — Pully QA

**Purpose:** Define the automated checks that should run on every PR and milestone push to ensure QA gate efficiency.  
**Scope:** Error detection, test validation, and early playability signals.

---

## Current CI State

Existing `.github/workflows/ci.yml`:
- ✅ EditMode test runner (GameCI)
- ✅ PlayMode test runner (GameCI)
- ❌ No compile check on PR open
- ❌ No coverage enforcement
- ❌ No APK build verification
- ❌ No performance regression detection

---

## Recommended CI Pipeline

### Stage 1: Fast Feedback (Every PR Push)

Goal: Catch errors in <2 minutes, fail fast.

```yaml
# .github/workflows/pr-fast-check.yml
name: Fast Feedback
on:
  pull_request:
    paths:
      - 'Assets/**'
      - '**.cs'
      - 'Packages/**'

jobs:
  compile-check:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - name: Cache Library
        uses: actions/cache@v4
        with:
          path: Library
          key: Library-${{ hashFiles('Packages/manifest.json') }}
      - name: Compile Check
        uses: game-ci/unity-builder@v4
        env:
          UNITY_EMAIL: ${{ secrets.UNITY_EMAIL }}
          UNITY_PASSWORD: ${{ secrets.UNITY_PASSWORD }}
          UNITY_SERIAL: ${{ secrets.UNITY_SERIAL }}
        with:
          targetPlatform: Android
          buildMethod: UnityEditor.BuildPipeline.BuildPlayer
          # Fail on compile error only
```

### Stage 2: Test Validation (PR Ready for Review)

Goal: Ensure all tests pass, coverage maintained.

```yaml
# Enhanced ci.yml additions
jobs:
  editmode-tests:
    # ... existing ...
    
  playmode-tests:
    # ... existing ...
    
  coverage-report:
    needs: [editmode-tests, playmode-tests]
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - name: Generate Coverage
        uses: game-ci/unity-test-runner@v4
        with:
          testMode: editmode
          coverageOptions: 'generateAdditionalMetrics;generateHtmlReport'
          # Fail if coverage < 80%
      - name: Check Coverage Threshold
        run: |
          COVERAGE=$(cat artifacts/EditMode/TestResults/coverage.json | jq '.summary.lineCoveragePercent')
          if (( $(echo "$COVERAGE < 80" | bc -l) )); then
            echo "Coverage $COVERAGE% below 80% threshold"
            exit 1
          fi
```

### Stage 3: Build Verification (Milestone Exit)

Goal: Ensure APK builds, no link errors.

```yaml
# .github/workflows/build-check.yml
name: Build Verification
on:
  push:
    branches: [main, develop]
  pull_request:
    branches: [main]

jobs:
  android-build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - name: Build Android APK
        uses: game-ci/unity-builder@v4
        env:
          UNITY_EMAIL: ${{ secrets.UNITY_EMAIL }}
          UNITY_PASSWORD: ${{ secrets.UNITY_PASSWORD }}
          UNITY_SERIAL: ${{ secrets.UNITY_SERIAL }}
        with:
          targetPlatform: Android
          androidAppBundle: false
          buildName: pully-m1-test
      - name: Upload APK
        uses: actions/upload-artifact@v4
        with:
          name: android-apk
          path: build/Android/*.apk
```

### Stage 4: Performance Baseline (M2+)

Goal: Catch FPS regressions, memory leaks.

```yaml
# .github/workflows/perf-check.yml (M2+)
name: Performance Baseline
on:
  push:
    branches: [main]
  workflow_dispatch:

jobs:
  performance-test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - name: Run PlayMode with Profiler
        uses: game-ci/unity-test-runner@v4
        with:
          testMode: playmode
          customParameters: -profiler-enable -profiler-log-file profiler.log
      - name: Analyze FPS
        run: |
          # Parse profiler.log for FPS drops below 60
          # Fail if >1% frames below threshold
      - name: Check Memory
        run: |
          # Parse for memory leaks between scene loads
          # Fail if unbounded growth detected
```

---

## Recommended Automated Checks Summary

### Must Have (M1 Minimum)

| Check | Trigger | Action on Fail |
|-------|---------|----------------|
| Compile (no CS errors) | Every PR push | Block merge |
| EditMode tests pass | PR ready for review | Block merge |
| PlayMode tests pass | PR ready for review | Block merge |
| No deleted tests | PR diff scan | Block merge |
| Coverage ≥80% | Post-test analysis | Block merge |

### Should Have (M1 Exit)

| Check | Trigger | Action on Fail |
|-------|---------|----------------|
| APK builds successfully | M1 exit / main push | Block release |
| All PRD verifications pass | PlayMode test | Block merge |
| No new warnings | CI log scan | Warn (not block) |

### Nice to Have (M2+)

| Check | Trigger | Action on Fail |
|-------|---------|----------------|
| FPS ≥60 on device | Bot harness | Block release |
| Memory leak detection | Perf tests | Warn |
| APK size budget (<50MB) | Post-build | Warn |
| Determinism check (seeded RNG) | Unit test | Block merge |

---

## CI Enforcement Rules

### Branch Protection (Recommended Settings)

```
main branch:
  - Require status checks to pass before merging
    - Fast Feedback / compile-check
    - CI / editmode-tests
    - CI / playmode-tests
    - Coverage / coverage-report
  - Require branches to be up to date before merging
  - Require linear history
  - Include administrators (even orchestrator must pass QA)
```

### QA Gate Integration

```
Worker opens PR
      ↓
GitHub Actions runs automated checks
      ↓
All checks GREEN → PR gets "qa-ready" label
      ↓
qa agent runs manual verification
      ↓
qa agent posts PASS/FAIL as PR comment
      ↓
PASS: orchestrator may merge
FAIL: worker fixes, loop back
```

---

## Scripts for Local QA

### pre-push.sh (Worker runs before PR)
```bash
#!/bin/bash
# Run before opening PR
echo "=== Pre-Push QA Check ==="
echo "[1/4] Unity compile check..."
scripts/unity-check.sh || exit 1

echo "[2/4] EditMode tests (local)..."
# Requires Unity CLI
$UNITY_BIN -batchmode -runTests -testPlatform editmode || exit 1

echo "[3/4] PlayMode tests (local)..."
$UNITY_BIN -batchmode -runTests -testPlatform playmode || exit 1

echo "[4/4] Git status check..."
if git diff --name-only | grep -q "\.cs$"; then
  echo "C# changes detected - ensure tests updated"
fi

echo "=== Pre-Push PASS — Safe to open PR ==="
```

### qa-verify.sh (QA runs on PR)
```bash
#!/bin/bash
# QA gate verification script
PR_BRANCH=$1
BASE_BRANCH=${2:-main}

echo "=== QA Verification for $PR_BRANCH ==="
git checkout $PR_BRANCH

echo "[1/5] Compile check..."
scripts/unity-check.sh || { echo "FAIL: Compile errors"; exit 1; }

echo "[2/5] Test count check..."
BEFORE=$(git show $BASE_BRANCH:Assets/Tests --name-only | wc -l)
AFTER=$(find Assets/Tests -name "*.cs" | wc -l)
if [ $AFTER -lt $BEFORE ]; then
  echo "WARNING: Tests may have been deleted"
fi

echo "[3/5] PRD verification markers..."
grep -r "V01\|V02\|V03\|V04" Assets/Tests/ || echo "WARNING: No PRD verification tests found"

echo "[4/5] Check for debug code..."
if grep -r "Debug.Log\|print(" Assets/_Game/Scripts/; then
  echo "WARNING: Debug statements in production code"
fi

echo "[5/5] Documentation updates..."
git diff --name-only | grep -E "docs/|\.md$" || echo "NOTE: No doc updates"

echo "=== QA Verification Complete ==="
```

---

## Implementation Priority

### Week 1 (M1 Start)
1. ✅ Keep existing ci.yml (EditMode + PlayMode)
2. 🔲 Add compile check to PR workflow
3. 🔲 Add coverage threshold (80%)

### Week 2 (M1 Mid)
1. 🔲 Add APK build verification
2. 🔲 Add test count regression check
3. 🔲 Add branch protection rules

### Week 3 (M2 Start)
1. 🔷 Add performance baseline job
2. 🔷 Add FPS regression detection
3. 🔷 Add memory leak check

### Week 4 (M3)
1. 🔷 Add bot player harness to CI
2. 🔷 Add determinism validation

---

## Success Metrics

| Metric | Target | Measurement |
|--------|--------|-------------|
| Time to compile feedback | <2 min | CI pipeline duration |
| Time to full test results | <10 min | CI pipeline duration |
| False positive rate | <5% | QA override frequency |
| Coverage regression caught | 100% | Blocked PRs with coverage drop |
| Post-merge reverts | 0 | Main branch stability |

---

*Recommendations version: 1.0*  
*Created by: qa agent*  
*Reference: qa.SKILL.md, GameCI documentation*
