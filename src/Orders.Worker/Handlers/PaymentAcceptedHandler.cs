using Orders.Worker.Audit;

namespace Orders.Worker.Handlers;

public sealed class PaymentAcceptedHandler(IAuditSink auditSink)
{
    public async Task HandleAsync(
        string orderId,
        string paymentId,
        decimal amount,
        CancellationToken cancellationToken)
    {
        await auditSink.WriteAsync(
            new AuditEvent("payment.accepted", orderId, new { paymentId, amount }),
            cancellationToken);
        await auditSink.WriteAsync(
            new AuditEvent("order.paid", orderId, new { paymentId }),
            cancellationToken);
    }
}