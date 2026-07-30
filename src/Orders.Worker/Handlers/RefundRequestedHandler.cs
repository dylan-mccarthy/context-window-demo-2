using Orders.Worker.Audit;

namespace Orders.Worker.Handlers;

public sealed class RefundRequestedHandler(IAuditSink auditSink)
{
    public async Task HandleAsync(
        string orderId,
        string refundId,
        decimal amount,
        string reason,
        CancellationToken cancellationToken)
    {
        await auditSink.WriteAsync(
            new AuditEvent("refund.requested", orderId, new { refundId, amount, reason }),
            cancellationToken);
    }
}