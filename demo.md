# Demo 2: Files are continuity

## What this demo proves

An agent does not reliably create durable state merely because it can write files. Durable state needs a prescribed protocol: what file to create, what it must contain, when to update it, and how completion is verified.

This demo uses a multi-file audit API migration to show that a fresh chat can continue accurately from repository state alone.

> The chat is gone. The work state is not.
>
> Chat is interaction. Files are continuity.

## Before the demo

Open this repository in VS Code and make sure the terminal is at the repository root.

Reset to the prepared starting state:

```bash
git switch main
git status --short
```

Expected result: no output from `git status --short`. The working tree is clean.

Confirm the baseline:

```bash
dotnet test Orders.sln
rg "LegacyAudit\.Write" src/Orders.Worker/Handlers
test ! -e MIGRATION_PLAN.md && echo "MIGRATION_PLAN.md is absent"
```

Expected result:

- All 3 tests pass.
- The search reports 14 calls across 8 handlers.
- `MIGRATION_PLAN.md` is absent.
- Obsolete API warnings are expected in the starting state.

## Set the scene

Show the audience these files:

- `.github/copilot-instructions.md`: short repository-wide constraints.
- `.github/prompts/migrate-audit.prompt.md`: the reusable migration protocol.
- `src/Orders.Worker/Audit/LegacyAudit.cs`: the deprecated compatibility API.
- `src/Orders.Worker/Audit/IAuditSink.cs`: the replacement abstraction.
- One or two files under `src/Orders.Worker/Handlers`: representative legacy callers.

Explain that the replacement API is already implemented and registered. The challenge is coordinating 14 edits across 8 handlers, updating tests, preserving behavior, and proving completion.

Point out the critical properties of the prompt:

1. It names the durable state file: `MIGRATION_PLAN.md`.
2. It defines the required checklist contents.
3. It says when the state must be updated.
4. It defines deterministic completion checks.

## Phase 1: Start the migration

Open Copilot Chat in Agent mode and run:

```text
/migrate-audit
```

Let the agent search the repository, create `MIGRATION_PLAN.md`, and migrate one or two handlers.

As soon as the plan contains useful state, open it beside the chat. Highlight:

- The recorded total of 14 call sites across 8 files.
- Checked and unchecked handler items.
- The supporting work checklist.
- The validation commands.
- The next remaining item.

Do not wait for the entire migration. Stop the agent after two or three handlers are complete and the focused tests pass.

The important artifact is not the chat transcript. It is the visibly changing plan in the repository.

## Deterministic checkpoint fallback

If the live run is slow, finishes too quickly, or leaves the repository in an awkward state, switch to the prepared checkpoint:

```bash
git reset --hard
git clean -f MIGRATION_PLAN.md
git switch demo/checkpoint
dotnet test Orders.sln
rg "LegacyAudit\.Write" src/Orders.Worker/Handlers
```

Expected checkpoint state:

- `MIGRATION_PLAN.md` exists.
- `OrderCreatedHandler.cs` and `PaymentAcceptedHandler.cs` are checked off.
- 10 legacy calls remain across 6 handlers.
- `PaymentRejectedHandler.cs` is the next unchecked item.
- All 3 tests pass.

Use `git reset --hard` and `git clean` here only as presenter resets when you intentionally want to discard the live demo edits. The targeted `git clean` removes the untracked plan created on `main` so Git can switch to the checkpoint where that file is tracked.

## Phase 2: Interrupt continuity

Close the current chat and open a completely new chat. Do not summarize the previous conversation.

Send exactly this:

```text
Read MIGRATION_PLAN.md and continue the migration from its current state.

Do not repeat completed work.
Confirm the next unchecked item before editing.
```

Pause when the new agent identifies `PaymentRejectedHandler.cs` as the next item. Emphasize that it learned this from the file, not from the previous chat.

Then let it continue the migration.

Land the first key line:

> The chat is gone. The work state is not.

Then land the summary:

> Chat is interaction. Files are continuity.

## Phase 3: Show completion discipline

Before the agent declares completion, keep `MIGRATION_PLAN.md` visible. The protocol requires it to:

1. Re-read the plan.
2. Search production code again for `LegacyAudit.Write`.
3. Run the complete test suite.
4. Record final validation results.
5. Resolve every unchecked item.

The expected final validation commands are:

```bash
rg "LegacyAudit\.Write" src/Orders.Worker/Handlers
dotnet test Orders.sln
rg -- "- \[ \]" MIGRATION_PLAN.md
```

Expected result:

- The legacy-call search returns no matches.
- All 3 tests pass.
- The unchecked-item search returns no matches.
- `LegacyAudit.cs` still exists as compatibility code.

Make the qualification explicit:

> A checklist preserves continuity. It does not magically make the agent diligent. That is why completion criteria and verification still matter.

## Deterministic completion fallback

If needed, show the prepared completed state:

```bash
git reset --hard
git clean -f MIGRATION_PLAN.md
git switch demo/complete
rg "LegacyAudit\.Write" src/Orders.Worker/Handlers || true
dotnet test Orders.sln
rg -- "- \[ \]" MIGRATION_PLAN.md || true
```

Expected result: zero production legacy calls, zero unchecked plan items, and all tests passing.

## Reset for the next presentation

Return to the clean starting state:

```bash
git switch main
git status --short
```

If live edits remain, discard them only when you are certain they are disposable presentation changes:

```bash
git reset --hard
git clean -f MIGRATION_PLAN.md
git switch main
```

Verify the reset:

```bash
rg "LegacyAudit\.Write" src/Orders.Worker/Handlers | wc -l
test ! -e MIGRATION_PLAN.md && echo "ready"
```

Expected result: 14 matching lines and `ready`.

## Presenter notes

- Keep the plan visible during both chats. The audience should see state changing in a file.
- Do not over-focus on the C# syntax. The migration is the workload; continuity is the subject.
- Interrupt only after the plan contains enough information for a stranger to continue.
- Use `demo/checkpoint` freely. Its purpose is to make the continuity moment reliable.
- End on the completion checks, not merely on the last code edit.
