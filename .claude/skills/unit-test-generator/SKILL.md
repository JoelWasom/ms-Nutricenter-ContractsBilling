---
name: Unit Test Generator
description: "Trigger: generate unit test, write test for class or method, add test coverage, testear. Emit an xUnit+Moq test per project/unit-test-rules-1.md."
license: Apache-2.0
metadata:
  author: "JoelWasom"
  version: "1.0"
---

## Activation Contract

Load this skill whenever the user asks to write, add, or generate a unit test in this repository. It covers domain types and application command handlers tested from `src/Nutricenter.ContractsBilling.Test`.

## Hard Rules

- Read `project/unit-test-rules-1.md` from the repository root BEFORE writing anything. It is the normative source; this skill never restates or overrides it.
- If a rule there is marked `mandatory` and you cannot satisfy it, STOP and report the conflict. Do not silently pick a side.
- Never modify production code to make a test pass. Report the required production change and wait for authorization.
- One Act per test. `[Fact]` is the default; `[Theory]` only when one behavior repeats over scalar inputs.
- Build real domain objects; mock ports only. Success asserts `Times.Once` on `CreateAsync`/`CommitAsync`, failure asserts `Times.Never`.
- Never add `using Xunit;` — it is a global import in the test project.
- Test code, identifiers, and comments are written in English.

## Decision Gates

| Target | Action |
|---|---|
| Domain aggregate, entity, or value object | `Domain/<Type>Test.cs`; cover both branches of every guard |
| Application command handler | `Application/Commands/<Area>/<Handler>Test.cs`; success and not-found always, invalid-state only when an actual guard exists in the handler |
| Infrastructure, controllers, real I/O, or the database | Not a unit test — say so and stop |
| Type under test not found or ambiguous | Ask for the path; never guess |

## Execution Steps

1. Read `project/unit-test-rules-1.md`.
2. Read the type under test plus one sibling test file to match the local arrangement pattern.
3. Pick the matching decision row and target file; create the file if it does not exist.
4. Add `private static` factory helpers for repeated arrange data instead of inline duplication.
5. Run `dotnet test src/Nutricenter.ContractsBilling.Test/Nutricenter.ContractsBilling.Test.csproj --nologo` and capture the observed result.

## Output Contract

Return the target file path, the test method names created, the literal `dotnet test` result, and any rule you could not satisfy with its reason. Never claim a pass you did not observe.

## References

- `project/unit-test-rules-1.md` — normative checklist (path relative to the repository root).
- `project/unit-test-rules.md` — supporting conventions.
