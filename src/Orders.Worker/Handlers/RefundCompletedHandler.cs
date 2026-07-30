using Orders.Worker.Audit;

namespace Orders.Worker.Handlers;

public sealed class RefundCompletedHandler(IAuditSink auditSink)
{
    public async Task HandleAsync(
        string orderId,
        string refundId,
        decimal amount,
        string providerReference,
        CancellationToken cancellationToken)
    {
        await auditSink.WriteAsync(
            new AuditEvent("refund.completed", orderId, new { refundId, amount, providerReference }),
            cancellationToken);
        await auditSink.WriteAsync(
            new AuditEvent("payment.refunded", orderId, new { refundId, amount }),
            cancellationToken);
    }
}