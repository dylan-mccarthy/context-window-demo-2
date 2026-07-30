using Orders.Worker.Audit;

namespace Orders.Worker.Handlers;

public sealed class OrderCancelledHandler(IAuditSink auditSink)
{
    public async Task HandleAsync(
        string orderId,
        string reason,
        string cancelledBy,
        CancellationToken cancellationToken)
    {
        await auditSink.WriteAsync(
            new AuditEvent("order.cancelled", orderId, new { reason, cancelledBy }),
            cancellationToken);
        await auditSink.WriteAsync(
            new AuditEvent("fulfilment.stopped", orderId, new { reason }),
            cancellationToken);
    }
}