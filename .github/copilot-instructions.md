# Repository instructions

This is a .NET 9 order-processing worker.

- Preserve event names and audit payload fields during migrations.
- Pass `CancellationToken` through asynchronous operations.
- Do not remove compatibility code until no production callers remain.
- Run the complete test suite before declaring a migration complete.