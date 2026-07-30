using Orders.Worker.Audit;

namespace Orders.Worker.Handlers;

public sealed class PaymentRejectedHandler(IAuditSink auditSink)
{
    public async Task HandleAsync(
        string orderId,
        string paymentId,
        string reason,
        CancellationToken cancellationToken)
    {
        await auditSink.WriteAsync(
            new AuditEvent("payment.rejected", orderId, new { paymentId, reason }),
            cancellationToken);
    }
}