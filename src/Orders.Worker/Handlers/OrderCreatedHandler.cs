using Orders.Worker.Audit;

namespace Orders.Worker.Handlers;

public sealed class OrderCreatedHandler(IAuditSink auditSink)
{
    public async Task HandleAsync(
        string orderId,
        string customerId,
        string salesChannel,
        CancellationToken cancellationToken)
    {
        await auditSink.WriteAsync(
            new AuditEvent("order.created", orderId, new { customerId, salesChannel }),
            cancellationToken);
        await auditSink.WriteAsync(
            new AuditEvent("order.received", orderId, new { customerId }),
            cancellationToken);
    }
}