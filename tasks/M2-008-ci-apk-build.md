# M2-008: CI APK Build

**Status:** ✅ Done
**Priority:** P0
**Assigned:** orchestrator
**Estimated:** 4h
**Started:** 2026-06-08
**Completed:** 2026-06-08
**Milestone:** M2 — Menus + Art + Build

## Completed Work

- [x] GitHub Actions workflow with Android build job
- [x] `game-ci/unity-builder@v4` for Android target
- [x] Build depends on tests passing first
- [x] APK artifact uploaded with `actions/upload-artifact@v4`
- [x] Only builds on main branch pushes

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
