# Releasing

This repository follows the same release process as the other RadicalFx repositories: [Radical](https://github.com/RadicalFx/Radical/blob/master/RELEASING.md), [Radical.Windows](https://github.com/RadicalFx/Radical.Windows/blob/master/RELEASING.md), and [Radical.Avalonia](https://github.com/RadicalFx/Radical.Avalonia/blob/main/RELEASING.md).

## Branches and versions

Versions are calculated by [MinVer](https://github.com/adamralph/minver) from git tags. Nothing else sets a version number.

- **`master`** is where development happens, and it always builds the **next major** that hasn't been branched yet. Its `MinVerMinimumMajorMinor`, in `src/Radical/Radical.csproj`, is set to that major, so untagged builds are versioned `N.0.0-alpha.0.<height>`.
- **`release-X.Y`** branches stabilize and patch an `X.Y` release. Betas, release candidates, the final release, and patch releases are all tagged on the release branch, never on `master`.

To see which release branches exist, and what each branch builds:

```shell
git branch -r --list 'origin/release-*'
dotnet build src/Radical/Radical.csproj -t:MinVer -getProperty:MinVerVersion
```

Until a repository ships its first stable major, it may have no release branch yet. In that case pre-releases are tagged on `master`.

## Where changes go

1. Every change goes to `master` first, through a pull request.
2. If a change also has to ship in a release that's being prepared, cherry-pick it onto a branch created from `release-X.Y` and open a pull request against `release-X.Y`:

   ```shell
   git switch -c backport-<topic>-to-X.Y origin/release-X.Y
   git cherry-pick -x <commit>...
   ```

   `-x` records the original commit in the message. Include any earlier commits the change depends on.
3. Don't backport:
   - Renovate dependency updates, unless the release needs them: release branches keep their own dependency set.
   - Workflows that only run from the default branch, such as the project board one.

Anything merged to `master` after a release branch is created is **not** in that release unless it's backported.

## Starting a new major

To start stabilizing major `X` (for example, the first `X.0.0-beta.1`):

1. On `master`, `MinVerMinimumMajorMinor` should already be `X`.
2. Create `release-X.0` from `master` and push it.
3. Open a pull request on `master` that sets `MinVerMinimumMajorMinor` to `X+1`, so `master` moves on to the next major.
4. Tag the first pre-release on `release-X.0`, as described below.

Don't change `MinVerMinimumMajorMinor` on `master` to get a release out of it: releases come from the release branch.

## Tagging a release

1. Make sure everything that should ship is merged into `release-X.Y`, including backports, and that its CI build is green.
2. Check the dependencies on other RadicalFx packages. A stable release must not depend on a pre-release, and NuGet warns about it when packing (NU5104). If needed, raise the lower bound of the version range to the stable version first.
3. Tag the release branch and push the tag:

   ```shell
   git switch release-X.Y
   git pull
   git tag X.Y.Z            # or X.Y.Z-beta.N, X.Y.Z-rc.N
   git push origin X.Y.Z
   ```

   Tags have no `v` prefix: the CI workflow only runs on tags matching `[0-9]+.[0-9]+.[0-9]+` and `[0-9]+.[0-9]+.[0-9]+-*`.
4. The tag build runs the tests and publishes the packages:
   - pre-release tags (`X.Y.Z-*`) go to the [Feedz.io pre-releases feed](https://f.feedz.io/radical-framework/pre-releases/nuget/index.json)
   - stable tags (`X.Y.Z`) go to [NuGet.org](https://www.nuget.org/profiles/radical)
5. Create the GitHub release from the tag, with notes generated since the previous release:

   ```shell
   gh release create X.Y.Z --verify-tag --generate-notes --notes-start-tag <previous-tag>
   ```

   Add `--prerelease` for pre-release tags. Call out breaking changes, such as a new minimum .NET version, at the top of the notes.

## Releasing across repositories

Radical.Windows and Radical.Avalonia depend on Radical through a version range, for example `[3.0.0-beta.1, 4.0.0)`:

1. Release Radical first.
2. Then update the range in the UI libraries if needed: raise the lower bound when they depend on a new Radical release, and widen the upper bound when Radical ships a new major.
3. Then release the UI libraries.
