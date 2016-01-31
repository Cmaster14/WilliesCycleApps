# Willy's Motorcycles

## Software Process
### Pre-Commit Checklist
#### General Items
* Builds on your local machine
* Debugging smoke test passes
* Passes all local unit tests
* Passes all local UI tests
* If applicable, unit and/or UI test were written for the code added in this commit

#### Structure and Form
* Conforms to established coding standards
* No unneeded or uncalled methods
* All variables, methods, and classes are descriptively named
* Most common cases are first in if-then loops
* Nullable data is checked before using it

#### Documentation
* Code is written in a self-documenting manner
* [XML documentation](https://msdn.microsoft.com/en-us/library/vstudio/b2s063f7(v=vs.100).aspx) is added above every class or method you have written
   * It's not necessary to compile your XML docs after individual commits. We will do this at the end of each cycle.

### Bugs & Enhancements
#### Bugs
When you encouter a bug, file an issue on the GitHub repository for the issue. Include a descriptive title, relevant tags, as well as how to reproduce as well as a test case, if applicable. If you suspect you know what the issue is, include this in the issue as well. Even if the issue is a fairly trivial fix, we should file a bug on it. When you commit a fix, say "Fix #" followed by the issue number. Example: "Fix #1".

#### Enhancement
If you suspect you have room for an enhancement (such as a performance improvement or feature idea), file an enhancement issue on the GitHub repository. Include a descriptive title, relevant tags, as well as a description of the enhancement, along with possible implementation strategies. When the enhancement is fully completed (not 1/2, 3/4, but all the way), commit "Fix #" followed by the issue number, along with the feature or enhancement's name. Example: "Fix #1 - Account System".

### Coding Standards
* [.NET Naming Guidelines](https://msdn.microsoft.com/en-us/library/ms229002(v=vs.110).aspx)
* [C# Coding Conventions](https://msdn.microsoft.com/en-us/library/ff926074.aspx)
