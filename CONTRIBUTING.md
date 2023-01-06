# Contribution guidelines

This document describes the contribution guidelines for bicepdocs.

## 💻 &nbsp; Branching

We try to use branch folders for different type of changes

- `feature/` - is used when adding a new feature or changing existing features
- `bugfix/` - is used when fixing a bug
- `docs/` - is used when only adding, updating or deleting documentation

## Pull Requests

All changes, no matter how small should go through pull requests.

## ✅ &nbsp; Versioning

Versioning of the tool is determined manually and the sources are tagged. The release process starts when the tag is created. All versions below `1.0.0` may contain breaking changes on minor and patch adjustments. While below `1.0.0` the tools is considered to be in beta.

### 🎯 &nbsp; Versioning standard

The tools is versioned and follows the [Semantic Versioning 2.0.0 standard](https://semver.org/spec/v2.0.0.html):

Given a version number MAJOR.MINOR.PATCH, increment the:

- MAJOR version when you make incompatible API changes to public APIs
- MINOR version when you add functionality in a backwards compatible manner
- PATCH version when you make backwards compatible bug fixes
