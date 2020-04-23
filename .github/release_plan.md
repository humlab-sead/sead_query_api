# Prerequisites


# Release candidate (Monday, XXX XX)

- [ ] Update master for the release
  - [ ] Create a branch against `master` for a pull request
  - [ ] Run `pipenv lock` to make sure [`Pipfile.lock`](https://github.com/humlab-sead/sead_query_api/blob/master/package.json) is up-to-date (🤖)
  - [ ] Update [`CHANGELOG.md`](https://github.com/humlab-sead/sead_query_api/blob/master/CHANGELOG.md) (🤖)
  - [ ] Create a pull request against `master` (🤖)
  - [ ] Merge pull request into `master`
- [ ] GDPR bookkeeping (🤖; see team notes)

# Final (Monday, XXX XX)

## Preparation

- [ ] final updates to the `release` branch
  - [ ] Create a branch against `release` for a pull request
  - [ ] Update the version in [`package.json`](https://github.com/humlab-sead/sead_query_api/blob/master/package.json) (🤖)
  - [ ] Update [`CHANGELOG.md`](https://github.com/humlab-sead/sead_query_api/blob/master/CHANGELOG.md) (🤖)
  - [ ] Create pull request against `release` (🤖)
  - [ ] Merge pull request into `release`
- [ ] Make sure component governance is happy

## Release

- [ ] Publish the release via Azure DevOps
  - [ ] Make sure [CI](https://github.com/humlab-sead/sead_query_api/blob/master/CONTRIBUTING.md) is passing
  - [ ] Make sure the "Upload" stage on the release page succeeded
  - [ ] Make sure no extraneous files are being included in the `.vsix` file (make sure to check for hidden files)
  - [ ] Deploy the "Publish" stage
- [ ] Publish [documentation changes](https://github.com/humlab-sead/vscode-docs/pulls?q=is%3Apr+is%3Aopen+label%3Apython)
- [ ] Merge `release` back into `master` (🤖)

## Clean up after _this_ release


## Prep for the _next_ release
