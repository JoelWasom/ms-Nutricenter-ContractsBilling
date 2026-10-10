# Feature: contract-tests — Unit tests for Contract domain + handlers

## Objective
Cover the Contract aggregate (post-fix behavior) and all 7 Contract command handlers with xunit + Moq unit tests.

## Problem / Why
`ContractTest.cs` still encodes the old inverted `Activate()` guard the user just fixed, and the 7 Contract handlers have zero coverage.

## Scope
- Edit: `src/Nutricenter.ContractsBilling.Test/Domain/ContractTest.cs` (2 Activate tests only)
- Create under `src/Nutricenter.ContractsBilling.Test/Application/Commands/Contract/`:
  `ContractCreateCommandHandlerTest.cs`, `ContractAddServiceCommandHandlerTest.cs`,
  `ContractRemoveServiceCommandHandlerTest.cs`, `ContractActivateCommandHandlerTest.cs`,
  `ContractSuspendedCommandHandlerTest.cs`, `ContractTerminatedCommandHandlerTest.cs`,
  `ContractCancelCommandHandlerTest.cs`
- `src/` stays read-only. No skill registry exists; no project-specific standards applied.

## Constraints
- Follow the existing `ServiceTypeCreateCommandHandlerTest` Arrange/Act/Assert + Moq pattern.
- TDD mode: unknown (not configured); ordinary check is `dotnet test`. Test runner: `dotnet test`.

## Authorized scope
Test project + this document only. No production-code changes.

## Checklist
- [ ] T1 Update the 2 Activate domain tests to the fixed guard (empty→throws, with-service→Active)
- [ ] T2 `ContractCreateCommandHandler` tests (success + generator/repository/commit verification)
- [ ] T3 `ContractAddServiceCommandHandler` tests (success + contract-not-found)
- [ ] T4 `ContractRemoveServiceCommandHandler` tests (success + contract-not-found)
- [ ] T5 `ContractActivateCommandHandler` tests (success + not-found + empty-contract invalid)
- [ ] T6 `Suspended` / `Terminated` / `Cancel` handler tests (success + not-found + invalid-state where guarded)
- [ ] T7 Full `dotnet test` green + work-unit commit on feature branch (tests green; commit blocked: no git identity configured)

## Acceptance criteria
- `dotnet test` passes 100% (domain + application suites).
- Every Contract handler has at least a success test and a not-found test.
- Route per task: direct inline (delegated mapper unavailable in this runtime).

## Progress
- T1–T6 done. 30/30 tests green (14 domain + 14 new handler + ServiceType + placeholder).
- T7 partial: branch `feature/contract-tests` created, 9 files staged; commit blocked (see below).

## Verification evidence
- `dotnet test src/Nutricenter.ContractsBilling.Test`: 30 passed, 0 failed, 0 skipped.
- Only pre-existing CS8618/CS8602 warnings in Domain (untouched production code); no new warnings from tests.

## Next step
- Fix-forward any red tests, then branch + work-unit commit.
