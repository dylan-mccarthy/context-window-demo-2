# Audit migration plan

## Scope

Found 14 `LegacyAudit.Write` calls across 8 production files.

## Handlers

- [x] `OrderCreatedHandler.cs` - 2 call sites
- [x] `PaymentAcceptedHandler.cs` - 2 call sites
- [ ] `PaymentRejectedHandler.cs` - 1 call site
- [ ] `OrderPackedHandler.cs` - 2 call sites
- [ ] `OrderShippedHandler.cs` - 2 call sites
- [ ] `OrderCancelledHandler.cs` - 2 call sites
- [ ] `RefundRequestedHandler.cs` - 1 call site
- [ ] `RefundCompletedHandler.cs` - 2 call sites

## Supporting changes

- [x] Confirm `IAuditSink` registration
- [ ] Update constructor tests as handlers migrate (2 of 8 complete)
- [ ] Search production code for remaining `LegacyAudit.Write` calls
- [ ] Run complete test suite

## Validation

- `dotnet test tests/Orders.Worker.Tests/Orders.Worker.Tests.csproj`
- `dotnet test Orders.sln`
- `rg "LegacyAudit\.Write" src/Orders.Worker/Handlers`

## Progress log

- Migrated `OrderCreatedHandler.cs` and `PaymentAcceptedHandler.cs`; event names, correlation IDs, payload fields, and cancellation tokens are preserved.
- Updated handler tests to capture events from both the new sink and the legacy compatibility API.
- Focused test project passed: 3 tests succeeded; 10 legacy calls remain in 6 production files.
- Next unchecked item: `PaymentRejectedHandler.cs`.