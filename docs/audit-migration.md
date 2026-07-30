# Audit migration demo

This repository demonstrates that files, not chat history, provide durable state for a multi-step agent task. The migration prompt requires the agent to create and maintain `MIGRATION_PLAN.md` as an explicit protocol.

## Live flow

1. Start from the `demo/start` tag and run `/migrate-audit`.
2. Stop after the plan exists and two or three handlers are complete.
3. Open a fresh chat and say: `Read MIGRATION_PLAN.md and continue the migration from its current state. Do not repeat completed work. Confirm the next unchecked item before editing.`
4. Let the fresh session complete the checklist and validation.

The chat is gone. The work state is not.

Chat is interaction. Files are continuity.

## Prepared states

The intended Git refs are:

| Ref | State |
| --- | --- |
| `demo/start` | All handlers use `LegacyAudit`; no plan exists. |
| `demo/checkpoint` | A plan exists; two or three handlers are migrated; tests pass. |
| `demo/complete` | All handlers are migrated; no production legacy calls remain; tests pass. |

A checklist preserves continuity. It does not make an agent diligent by itself, so the protocol also defines completion criteria and deterministic validation commands.