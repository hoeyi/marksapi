# Contributing to <em>markets-apiclient</em> #

* [Commit Message Guidelines](#commit-message-guidelines)
* [Object Naming Conventions](#object-naming-conventions)

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

* **build**: Changes that affect the build system or external dependencies
* **feat**: A new feature
* **fix**: A bug fix
* **perf**: A code change that improves performance
* **refactor**: A code change that neither fixes a bug nor adds a feature
* **revert**: Reverts commit `<hash>`.
* **style**: Changes that do not affect the meaning of the code 
(white-space, formatting, missing semi-colons, etc)
* **test**: Adding missing tests or correcting existing tests

### Scope ###
Use the `scope` component of the commit subject to descript the endpoint the change applies to.

Example: 
```
feat(eod): implement methods for /eod enpoint
```

Scope should also declare which API library it impacts.

Example:
```
feat(massive/eod): implement methods for massive/eod endpoint
```

Use the type **docs** for changes to internal and external documentation.

Example:
```
docs(marksapi): add method documentation for '{method}'
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

Example:
```
BREAKING CHANGE: Ends support for [NAME] API

Resolves #42 (where #42 is the GitHub issue no.)
```
