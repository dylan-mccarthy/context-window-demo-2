# Durable audit migration demo

This .NET 9 order-processing worker is the starting state for **Demo 2: Files are continuity**. Eight handlers contain 14 calls to a deprecated static audit API. A complete asynchronous replacement already exists and is registered with dependency injection.

The exercise is coordination rather than API design: invoke `/migrate-audit`, interrupt the migration after useful state appears in `MIGRATION_PLAN.md`, then continue from a fresh chat using only that file.

## Baseline

```bash
dotnet test Orders.sln
rg "LegacyAudit\.Write" src/Orders.Worker/Handlers
```

The baseline should report 14 legacy production calls and a passing test suite. `MIGRATION_PLAN.md` is deliberately absent until the migration agent creates it.

See [docs/audit-migration.md](docs/audit-migration.md) for the live demonstration flow and prepared Git states.