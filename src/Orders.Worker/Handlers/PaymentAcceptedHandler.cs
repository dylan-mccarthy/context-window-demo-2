using Orders.Worker.Audit;

namespace Orders.Worker.Handlers;

public sealed class PaymentAcceptedHandler
{
    public Task HandleAsync(
        string orderId,
        string paymentId,
        decimal amount,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        LegacyAudit.Write("payment.accepted", orderId, new { paymentId, amount });
        LegacyAudit.Write("order.paid", orderId, new { paymentId });
        return Task.CompletedTask;
    }
}