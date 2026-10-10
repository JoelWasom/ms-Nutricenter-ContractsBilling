---
name: Unit Test Verifier
description: "Trigger: verify unit test, check generated test, audit test compliance, validate test. Audit tests against project/unit-test-rules-1.md and run the suite."
license: Apache-2.0
metadata:
  author: "JoelWasom"
  version: "1.0"
---

## Activation Contract

Load this skill when asked to verify, audit, or check generated tests. It normally runs right after `unit-test-generator`, which creates test files but never executes them — this skill owns both the rules audit and the execution.

## Hard Rules

- Read `project/unit-test-rules-1.md` from the repository root FIRST. It is the normative source; every verdict must cite the rule number it came from.
- Never edit the test, the production code, or the rules file. This skill reports only — fixing is a separate, authorized step.
- Never mark a check `PASS` you did not observe. Evidence that cannot be gathered is `UNKNOWN`, not pass.
- Run the suite exactly once. No retries, no `--filter` to isolate failures, no skipped tests, no suppressing warnings.
- Report failure output verbatim. Attribute a warning to the target only by its source file path; never compare warning counts between runs, because incremental builds make them incomparable.

## Decision Gates

| Finding | Action |
|---|---|
| Test file missing or path ambiguous | Ask for the path; never guess |
| Mandatory rule violated | Blocker — report rule number, evidence, and the required change |
| Rule satisfied but not statically verifiable | `UNKNOWN` with the reason |
| Handler has fewer branches than a rule assumes | Fewer cases is correct — never invent a test to reach a count |
| Suite green and zero blockers | `PASS` |

## Execution Steps

1. Read `project/unit-test-rules-1.md`.
2. Locate the target test file; when none is named, take every file under `src/Nutricenter.ContractsBilling.Test`.
3. Audit every applicable rule in that file. The usual hot spots are file and method naming, folder placement, one Act, `[Fact]` default, factory helpers, no `using Xunit;`, real domain objects with mocked ports, `Times.Once` on success and `Times.Never` on failure, and guard coverage on both branches. This list is a starting point, not the full rule set — the rules file governs. Verify the persistence call the handler actually makes, which is not always `CreateAsync`.
4. Run `dotnet test src/Nutricenter.ContractsBilling.Test/Nutricenter.ContractsBilling.Test.csproj --nologo` once and capture the literal result line.
5. Classify the outcome and hand back every violation found.

## Output Contract

Return an overall verdict of `PASS`, `FAIL`, or `UNKNOWN`; a rule-by-rule table with rule number, status (`PASS`, `FAIL`, `UNKNOWN`, or `N/A` when the rule does not apply to this target), and evidence; the literal `dotnet test` result line; the warnings attributed to the target's project or `none`; and for each blocker the file, rule number, and the change required. Never return `PASS` while any mandatory rule is violated or any required check is unrun.

## References

- `project/unit-test-rules-1.md` — normative checklist (path relative to the repository root).
- `project/unit-test-rules.md` — supporting conventions.
