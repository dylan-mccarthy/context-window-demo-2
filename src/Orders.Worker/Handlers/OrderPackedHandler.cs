using Orders.Worker.Audit;

namespace Orders.Worker.Handlers;

public sealed class OrderPackedHandler(IAuditSink auditSink)
{
    public async Task HandleAsync(
        string orderId,
        string packageId,
        int itemCount,
        CancellationToken cancellationToken)
    {
        await auditSink.WriteAsync(
            new AuditEvent("order.packed", orderId, new { packageId, itemCount }),
            cancellationToken);
        await auditSink.WriteAsync(
            new AuditEvent("package.labelled", orderId, new { packageId }),
            cancellationToken);
    }
}