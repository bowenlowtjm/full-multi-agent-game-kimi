# M2-008: CI APK Build

**Status:** Backlog
**Priority:** P0
**Assigned:** —
**Estimated:** 4h
**Milestone:** M2 — Menus + Art + Build

## Description

Configure CI/CD to build debug APK automatically on push to main.

## Acceptance Criteria

- [ ] GitHub Actions workflow for Android build
- [ ] Builds on every push to main
- [ ] APK artifact uploaded
- [ ] Build succeeds with no errors
- [ ] APK installs on Android device
- [ ] Build time <15 minutes

## Technical Notes

- Use GameCI Docker images
- Unity license secrets required
- Build target: Android
- Configuration: Debug (not Release)

## Dependencies

- All prior M2 tasks

## Artifacts Required

- `.github/workflows/build.yml`
- Working CI pipeline

## QA Checklist

- [ ] CI green on main
- [ ] APK downloadable from Actions
- [ ] APK installs and runs on device
- [ ] Build logs clean

---

**Next:** M2-009-integration-tests
