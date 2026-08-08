# Contributing to <em>marksapi</em> #

Use these guide when making contributions to this project.

## Commit Message Guidelines ##

The structure of these guidelines are based on the [Angular convetion](
https://github.com/angular/angular/blob/22b96b9/CONTRIBUTING.md#commit) and 
[Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0-beta.2/).

Commit messages should follow the format:
```
<type>[optional scope]: <description>
[optional body]
[optional footer]
```

### Type ###
Must be one of the following:
* **docs** for changes to internal and external documentation
* **build**: Changes that affect the build system or external dependencies
* **feat**: A new feature
* **fix**: A bug fix
* **perf**: A code change that improves performance
* **refactor**: A code change that neither fixes a bug nor adds a feature
* **revert**: Reverts commit `<hash>`.
* **style**: Changes that do not affect the meaning of the code 
(white-space, formatting, missing semi-colons, etc)
* **test**: Adding missing tests or correcting existing tests

**Example:** Add missing in-code documentation
```
docs({library}): add XML documentation for {method}
```

**Example:** Add a feature to a library
```
feat({library}): add coverage for {endpoint} in {library}
```

### Scope ###
Use the `scope` component of the commit subject line to denote the library impacted.

**Example:** Update the version of a package
```
build({library 1}, {library 2}): bump {package} to v2.0
```

### Subject ###
The subject contains a succinct description of the change:

* Use the imperative, present tense
* Don't capitalize the first letter
* Don't include punctuation

### Body ###
The body contains the detail of why the change was made:
* Use the imperative, present tense
* Include the motivation for the change with contrast to prior behavior

### Footer ###
The footer contains information on breaking changes. Start with the phrase 
`BREAKING CHANGE:`. Also use this space to reference closing GitHub issues. 

**Example(s):**
```
BREAKING CHANGE: Ends support for [NAME] API
```
```
Resolves #42 (where #42 is the GitHub issue no.)
```
