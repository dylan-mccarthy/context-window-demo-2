---
name: migrate-audit
description: Migrate legacy audit calls using a durable work plan
agent: agent
---

Migrate all production uses of `LegacyAudit.Write` to `IAuditSink.WriteAsync`.

Before editing production code:

1. Search the repository for every `LegacyAudit.Write` call.
2. Create `MIGRATION_PLAN.md`.
3. Record every affected production file as an unchecked checklist item.
4. Record the total number of call sites found.
5. Record the required validation commands.

During the migration:

1. Work through the checklist one file at a time.
2. Mark a file complete only after all legacy calls in that file are migrated.
3. Preserve event names, correlation identifiers and payload fields.
4. Pass the existing cancellation token to `WriteAsync`.
5. Update tests when constructor dependencies change.
6. Update `MIGRATION_PLAN.md` whenever progress or scope changes.
7. Run focused tests after each logical batch.

Before declaring completion:

1. Re-read `MIGRATION_PLAN.md`.
2. Search again for remaining production uses of `LegacyAudit.Write`.
3. Run the complete test suite.
4. Record the final validation results in the plan.
5. Do not say the task is complete while unchecked items remain.

Do not delete `LegacyAudit` until all production callers have been removed.